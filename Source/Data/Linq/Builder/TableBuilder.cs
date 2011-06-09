using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace BLToolkit.Data.Linq.Builder
{
	using BLToolkit.Linq;
	using Data.Sql;
	using Mapping;
	using Reflection;
	using Reflection.Extension;

	class TableBuilder : ISequenceBuilder
	{
		#region TableBuilder

		int ISequenceBuilder.BuildCounter { get; set; }

		static T Find<T>(ExpressionBuilder builder, BuildInfo buildInfo, Func<int,IBuildContext,T> action)
		{
			var expression = buildInfo.Expression;

			switch (expression.NodeType)
			{
				case ExpressionType.Constant:
					{
						var c = (ConstantExpression)expression;
						if (c.Value is IQueryable)
							return action(1, null);

						break;
					}

				case ExpressionType.Call:
					{
						var mc = (MethodCallExpression)expression;

						if (mc.Method.Name == "GetTable")
							if (expression.Type.IsGenericType && expression.Type.GetGenericTypeDefinition() == typeof(Table<>))
								return action(2, null);

						var attr = builder.GetTableFunctionAttribute(mc.Method);

						if (attr != null)
							return action(5, null);

						break;
					}

				case ExpressionType.MemberAccess:

					if (expression.Type.IsGenericType && expression.Type.GetGenericTypeDefinition() == typeof(Table<>))
						return action(3, null);

					// Looking for association.
					//
					if (buildInfo.IsSubQuery && buildInfo.SqlQuery.From.Tables.Count == 0)
					{
						var ctx = builder.GetContext(buildInfo.Parent, expression);
						if (ctx != null)
							return action(4, ctx);
					}

					break;

				case ExpressionType.Parameter:
					{
						if (buildInfo.IsSubQuery && buildInfo.SqlQuery.From.Tables.Count == 0)
						{
							var ctx = builder.GetContext(buildInfo.Parent, expression);
							if (ctx != null)
								return action(4, ctx);
						}

						break;
					}
			}

			return action(0, null);
		}

		public bool CanBuild(ExpressionBuilder builder, BuildInfo buildInfo)
		{
			return Find(builder, buildInfo, (n,_) => n > 0);
		}

		public IBuildContext BuildSequence(ExpressionBuilder builder, BuildInfo buildInfo)
		{
			return Find(builder, buildInfo, (n,ctx) =>
			{
				switch (n)
				{
					case 0 : return null;
					case 1 : return new TableContext(builder, buildInfo, ((IQueryable)((ConstantExpression)buildInfo.Expression).Value).ElementType);
					case 2 :
					case 3 : return new TableContext(builder, buildInfo, buildInfo.Expression.Type.GetGenericArguments()[0]);
					case 4 : return ctx.GetContext(buildInfo.Expression, 0, buildInfo);
					case 5 : return new TableContext(builder, buildInfo);
				}

				throw new InvalidOperationException();
			});
		}

		#endregion

		#region TableContext

		public class TableContext : IBuildContext
		{
			#region Properties

#if DEBUG
			public string _sqlQueryText { get { return SqlQuery == null ? "" : SqlQuery.SqlText; } }
#endif

			public ExpressionBuilder Builder     { get; private set; }
			public Expression       Expression { get; private set; }
			public SqlQuery         SqlQuery   { get; set; }
			public IBuildContext    Parent     { get; set; }

			protected Type         OriginalType;
			public    Type         ObjectType;
			protected ObjectMapper ObjectMapper;
			public    SqlTable     SqlTable;

			#endregion

			#region Init

			public TableContext(ExpressionBuilder builder, BuildInfo buildInfo, Type originalType)
			{
				Builder    = builder;
				Parent     = buildInfo.Parent;
				Expression = buildInfo.Expression;
				SqlQuery   = buildInfo.SqlQuery;

				OriginalType = originalType;
				ObjectType   = GetObjectType();
				SqlTable     = new SqlTable(builder.MappingSchema, ObjectType);
				ObjectMapper = Builder.MappingSchema.GetObjectMapper(ObjectType);

				SqlQuery.From.Table(SqlTable);
			}

			protected TableContext(ExpressionBuilder builder, SqlQuery sqlQuery)
			{
				Builder  = builder;
				SqlQuery = sqlQuery;
			}

			public TableContext(ExpressionBuilder builder, BuildInfo buildInfo)
			{
				Builder    = builder;
				Parent     = buildInfo.Parent;
				Expression = buildInfo.Expression;
				SqlQuery   = buildInfo.SqlQuery;

				var mc   = (MethodCallExpression)Expression;
				var attr = builder.GetTableFunctionAttribute(mc.Method);

				if (!mc.Method.ReturnType.IsGenericType || mc.Method.ReturnType.GetGenericTypeDefinition() != typeof(Table<>))
					throw new LinqException("Table function has to return Table<T>.");

				OriginalType = mc.Method.ReturnType.GetGenericArguments()[0];
				ObjectType   = GetObjectType();
				SqlTable     = new SqlTable(builder.MappingSchema, ObjectType);
				ObjectMapper = Builder.MappingSchema.GetObjectMapper(ObjectType);

				SqlQuery.From.Table(SqlTable);

				var args = mc.Arguments.Select(a => builder.ConvertToSql(this, a));

				attr.SetTable(SqlTable, mc.Method, mc.Arguments, args);
			}

			protected Type GetObjectType()
			{
				for (var type = OriginalType.BaseType; type != null && type != typeof(object); type = type.BaseType)
				{
					var extension = TypeExtension.GetTypeExtension(type, Builder.MappingSchema.Extensions);
					var mapping   = Builder.MappingSchema.MetadataProvider.GetInheritanceMapping(type, extension);

					if (mapping.Length > 0)
						return type;
				}

				return OriginalType;
			}

			#endregion

			#region BuildQuery

			class MappingData
			{
				public MappingSchema  MappingSchema;
				public ObjectMapper   ObjectMapper;
				public int[]          Index;
				public IValueMapper[] ValueMappers;
			}

			static object MapDataReaderToObject(IDataContext dataContext, IDataReader dataReader, MappingData data)
			{
				var source = data.MappingSchema.CreateDataReaderMapper(dataReader);

				var initContext = new InitContext
				{
					MappingSchema = data.MappingSchema,
					DataSource    = source,
					SourceObject  = dataReader,
					ObjectMapper  = data.ObjectMapper
				};

				var destObject = dataContext.CreateInstance(initContext) ?? data.ObjectMapper.CreateInstance(initContext);

				if (initContext.StopMapping)
					return destObject;

				var smDest = destObject as ISupportMapping;

				if (smDest != null)
				{
					smDest.BeginMapping(initContext);

					if (initContext.StopMapping)
						return destObject;
				}

				if (data.ValueMappers == null)
				{
					var mappers = new IValueMapper[data.Index.Length];

					for (var i = 0; i < data.Index.Length; i++)
					{
						var n = data.Index[i];

						if (n < 0)
							continue;

						if (!data.ObjectMapper.SupportsTypedValues(i))
						{
							mappers[i] = data.MappingSchema.DefaultValueMapper;
							continue;
						}

						var sourceType = source.           GetFieldType(n);
						var destType   = data.ObjectMapper.GetFieldType(i);

						if (sourceType == null) sourceType = typeof(object);
						if (destType   == null) destType   = typeof(object);

						IValueMapper t;

						if (sourceType == destType)
						{
							lock (data.MappingSchema.SameTypeMappers)
								if (!data.MappingSchema.SameTypeMappers.TryGetValue(sourceType, out t))
									data.MappingSchema.SameTypeMappers.Add(sourceType, t = data.MappingSchema.GetValueMapper(sourceType, destType));
						}
						else
						{
							var key = new KeyValuePair<Type,Type>(sourceType, destType);

							lock (data.MappingSchema.DifferentTypeMappers)
								if (!data.MappingSchema.DifferentTypeMappers.TryGetValue(key, out t))
									data.MappingSchema.DifferentTypeMappers.Add(key, t = data.MappingSchema.GetValueMapper(sourceType, destType));
						}

						mappers[i] = t;
					}

					data.ValueMappers = mappers;
				}

				var dest = data.ObjectMapper;
				var idx  = data.Index;
				var ms   = data.ValueMappers;

				for (var i = 0; i < idx.Length; i++)
				{
					var n = idx[i];

					if (n >= 0)
						ms[i].Map(source, dataReader, n, dest, destObject, i);
				}

				if (smDest != null)
					smDest.EndMapping(initContext);

				return destObject;
			}

			static readonly MethodInfo _mapperMethod = ReflectionHelper.Expressor<object>.MethodExpressor(_ => MapDataReaderToObject(null, null, null));

			Expression GetTableExpression(int[] index)
			{
				var data = new MappingData
				{
					MappingSchema = Builder.MappingSchema,
					ObjectMapper  = ObjectMapper,
					Index         = index
				};

				return Expression.Convert(
					Expression.Call(null, _mapperMethod,
						ExpressionBuilder.DataContextParam,
						ExpressionBuilder.DataReaderParam,
						Expression.Constant(data)),
					ObjectType);
			}

			Expression BuildQuery()
			{
				// Get indexes for all fields.
				//
				var info = ConvertToIndex(null, 0, ConvertFlags.All);

				// Convert to parent indexes.
				//
				var index = info.Select(idx => ConvertToParentIndex(idx.Index, null)).ToArray();

				// Build an expression.
				//
				return GetTableExpression(index);
			}

			public void BuildQuery<T>(Query<T> query, ParameterExpression queryParameter)
			{
				var expr = BuildExpression(null, 0);

				var mapper = Expression.Lambda<Func<QueryContext,IDataContext,IDataReader,Expression,object[],T>>(
					expr, new []
					{
						ExpressionBuilder.ContextParam,
						ExpressionBuilder.DataContextParam,
						ExpressionBuilder.DataReaderParam,
						ExpressionBuilder.ExpressionParam,
						ExpressionBuilder.ParametersParam,
					});

				query.SetQuery(mapper.Compile());
			}

			#endregion

			#region BuildExpression

			public Expression BuildExpression(Expression expression, int level)
			{
				// Build table.
				//
				var table = FindTable(expression, level);

				if (table.Field == null)
					return table.Table.BuildQuery();

				// Build field.
				//
				var info = ConvertToIndex(expression, level, ConvertFlags.Field).Single();
				var idx  = ConvertToParentIndex(info.Index, null);

				return Builder.BuildSql(expression.Type, idx);
			}

			#endregion

			#region ConvertToSql

			public SqlInfo[] ConvertToSql(Expression expression, int level, ConvertFlags flags)
			{
				switch (flags)
				{
					case ConvertFlags.All   :
						{
							var table = FindTable(expression, level);

							if (table.Field == null)
								return table.Table.SqlTable.Fields.Values
									.Select(_ => new SqlInfo { Sql = _, Member = _.MemberMapper.MemberAccessor.MemberInfo })
									.ToArray();

							break;
						}

					case ConvertFlags.Key   :
						{
							var table = FindTable(expression, level);

							if (table.Field == null)
							{
								var q =
									from f in table.Table.SqlTable.Fields.Values
									where f.IsPrimaryKey
									orderby f.PrimaryKeyOrder
									select new SqlInfo { Sql = f, Member = f.MemberMapper.MemberAccessor.MemberInfo };

								var key = q.ToArray();

								return key.Length != 0 ? key : ConvertToSql(expression, level, ConvertFlags.All);
							}

							break;
						}

					case ConvertFlags.Field :
						{
							var table = FindTable(expression, level);

							if (table.Field != null)
								return new[]
								{
									new SqlInfo
									{
										Sql    = table.Field,
										Member = table.Field.MemberMapper.MemberAccessor.MemberInfo
									}
								};

							break;
						}
				}

				throw new NotImplementedException();
			}

			#endregion

			#region ConvertToIndex

			readonly Dictionary<ISqlExpression,SqlInfo> _indexes = new Dictionary<ISqlExpression,SqlInfo>();

			public SqlInfo[] ConvertToIndex(Expression expression, int level, ConvertFlags flags)
			{
				switch (flags)
				{
					case ConvertFlags.Field :
					case ConvertFlags.Key   :
					case ConvertFlags.All   :

						var info = ConvertToSql(expression, level, flags);

						for (var i = 0; i < info.Length; i++)
						{
							var expr = info[i];
							
							SqlInfo n;

							if (_indexes.TryGetValue(expr.Sql, out n))
								info[i] = n;
							else
							{
								if (expr.Sql is SqlField)
								{
									var field = (SqlField)expr.Sql;
									expr.Index = SqlQuery.Select.Add(field, field.Alias);
								}
								else
								{
									expr.Index = SqlQuery.Select.Add(expr.Sql);
								}

								expr.Query = SqlQuery;

								_indexes.Add(expr.Sql, expr);
							}
						}

						return info;
				}

				throw new NotImplementedException();
			}

			#endregion

			#region IsExpression

			public bool IsExpression(Expression expression, int level, RequestFor requestFor)
			{
				switch (requestFor)
				{
					case RequestFor.Field      :
						{
							var table = FindTable(expression, level);
							return table != null && table.Field != null;
						}

					case RequestFor.Object      :
						{
							var table = FindTable(expression, level);
							return
								table       != null &&
								table.Field == null &&
								(expression == null || expression.GetLevelExpression(table.Level) == expression);
						}

					case RequestFor.Expression :
						{
							if (expression == null)
								return false;

							var levelExpression = expression.GetLevelExpression(level);

							switch (levelExpression.NodeType)
							{
								case ExpressionType.MemberAccess :
								case ExpressionType.Parameter    :
								case ExpressionType.Call         :

									var table = FindTable(expression, level);
									return table == null;
							}

							return true;
						}

					case RequestFor.Association      :
						{
							if (ObjectMapper.Associations.Count > 0)
							{
								var table = FindTable(expression, level);
								return
									table       != null &&
									table.Table is AssociatedTableContext &&
									table.Field == null &&
									(expression == null || expression.GetLevelExpression(table.Level) == expression);
							}

							return false;
						}
				}

				return false;
			}

			#endregion

			#region GetContext

			interface IAssociationHelper
			{
				Expression GetExpression(Expression parent, AssociatedTableContext association);
			}

			class AssociationHelper<T> : IAssociationHelper
				where T : class
			{
				public Expression GetExpression(Expression parent, AssociatedTableContext association)
				{
					Expression expr  = null;
					var        param = Expression.Parameter(typeof(T), "c");

					foreach (var cond in (association).ParentAssociationJoin.Condition.Conditions)
					{
						var p  = (SqlQuery.Predicate.ExprExpr)cond.Predicate;
						var e1 = Expression.MakeMemberAccess(parent, ((SqlField)p.Expr1).MemberMapper.MemberAccessor.MemberInfo);
						var e2 = Expression.MakeMemberAccess(param,  ((SqlField)p.Expr2).MemberMapper.MemberAccessor.MemberInfo) as Expression;

						while (e1.Type != e2.Type)
						{
							if (TypeHelper.IsNullableType(e1.Type))
							{
								e1 = Expression.PropertyOrField(e1, "Value");
								continue;
							}

							if (TypeHelper.IsNullableType(e2.Type))
							{
								e2 = Expression.PropertyOrField(e2, "Value");
								continue;
							}

							e2 = Expression.Convert(e2, e1.Type);
						}

						var ex = Expression.Equal(e1, e2);
							
						expr = expr == null ? ex : Expression.AndAlso(expr, ex);
					}

					var predicate = Expression.Lambda<Func<T,bool>>(expr, param);

					return Linq.Extensions.GetTable<T>(null).Where(predicate).Expression;
				}
			}

			public IBuildContext GetContext(Expression expression, int level, BuildInfo buildInfo)
			{
				if (expression == null)
				{
					if (buildInfo != null && buildInfo.IsSubQuery)
					{
						var table = new TableContext(
							Builder,
							new BuildInfo(Parent is SelectManyBuilder.SelectManyContext ? this : Parent, Expression, buildInfo.SqlQuery),
							SqlTable.ObjectType);

						return table;
					}

					return this;
				}

				if (ObjectMapper.Associations.Count > 0)
				{
					var levelExpression = expression.GetLevelExpression(level);

					if (buildInfo != null && buildInfo.IsSubQuery)
					{
						if (levelExpression == expression && expression.NodeType == ExpressionType.MemberAccess)
						{
							var association = (AssociatedTableContext)GetAssociation(expression, level).Table;

							if (association.IsList)
							{
								var ma     = (MemberExpression)buildInfo.Expression;
								var atype  = typeof(AssociationHelper<>).MakeGenericType(association.ObjectType);
								var helper = (IAssociationHelper)Activator.CreateInstance(atype);
								var expr   = helper.GetExpression(ma.Expression, association);

								buildInfo.IsAssociationBuilе = true;

								return Builder.BuildSequence(new BuildInfo(buildInfo, expr));
							}

							/*
							var table       = new TableContext(
								Builder,
								new BuildInfo(Parent is SelectManyBuilder.SelectManyContext ? this : Parent, Expression, buildInfo.SqlQuery),
								association.Table.ObjectType);

							foreach (var cond in ((AssociatedTableContext)association.Table).ParentAssociationJoin.Condition.Conditions)
							{
								var predicate = (SqlQuery.Predicate.ExprExpr)cond.Predicate;
								buildInfo.SqlQuery.Where
									.Expr(predicate.Expr1)
									.Equal
									.Field(table.SqlTable.Fields[((SqlField)predicate.Expr2).Name]);
							}

							return table;
							*/
						}
						else
						{
							var association = GetAssociation(levelExpression, level);
							((AssociatedTableContext)association.Table).ParentAssociationJoin.IsWeak = false;
							return association.Table.GetContext(expression, level + 1, buildInfo);
						}
					}
				}

				throw new InvalidOperationException();
			}

			#endregion

			#region ConvertToParentIndex

			public int ConvertToParentIndex(int index, IBuildContext context)
			{
				return Parent == null ? index : Parent.ConvertToParentIndex(index, this);
			}

			#endregion

			#region SetAlias

			public void SetAlias(string alias)
			{
				if (alias.Contains('<'))
					return;

				if (SqlTable.Alias == null)
					SqlTable.Alias = alias;
			}

			#endregion

			public ISqlExpression GetSubQuery(IBuildContext context)
			{
				return null;
			}

			#region Helpers

			SqlField GetField(Expression expression, int level)
			{
				if (expression.NodeType == ExpressionType.MemberAccess)
				{
					var memberExpression = (MemberExpression)expression;
					var levelExpression  = expression.GetLevelExpression(level);

					if (levelExpression.NodeType == ExpressionType.MemberAccess)
					{
						if (levelExpression != expression)
						{
							var levelMember = (MemberExpression)levelExpression;

							if (TypeHelper.IsNullableValueMember(memberExpression.Member) && memberExpression.Expression == levelExpression)
								memberExpression = levelMember;
							else
							{
								var sameType =
									levelMember.Member.ReflectedType == SqlTable.ObjectType ||
									levelMember.Member.DeclaringType == SqlTable.ObjectType;

								if (!sameType)
								{
									var mi = SqlTable.ObjectType.GetMember(levelMember.Member.Name, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
									sameType = mi.Any(_ => _.DeclaringType == levelMember.Member.DeclaringType);
								}

								if (sameType)
								{
									foreach (var field in SqlTable.Fields.Values)
									{
										if (field.MemberMapper is MemberMapper.ComplexMapper)
										{
											var name = levelMember.Member.Name;

											for (var ex = (MemberExpression)expression; ex != levelMember; ex = (MemberExpression)ex.Expression)
												name += "." + ex.Member.Name;

											if (field.MemberMapper.MemberName == name)
												return field;
										}
									}
								}
							}
						}

						if (levelExpression == memberExpression)
							foreach (var field in SqlTable.Fields.Values)
								if (TypeHelper.Equals(field.MemberMapper.MapMemberInfo.MemberAccessor.MemberInfo, memberExpression.Member))
									return field;
					}
				}

				return null;
			}

			[JetBrains.Annotations.NotNull]
			readonly Dictionary<MemberInfo,AssociatedTableContext> _associations = new Dictionary<MemberInfo,AssociatedTableContext>();

			class TableLevel
			{
				public TableContext Table;
				public SqlField     Field;
				public int          Level;
			}

			TableLevel FindTable(Expression expression, int level)
			{
				if (expression == null)
					return new TableLevel { Table = this };

				var levelExpression = expression.GetLevelExpression(level);

				switch (levelExpression.NodeType)
				{
					case ExpressionType.MemberAccess :
					case ExpressionType.Parameter    :
						{
							var field = GetField(expression, level);

							if (field != null || (level == 0 && levelExpression == expression))
								return new TableLevel { Table = this, Field = field, Level = level };

							return GetAssociation(expression, level);
						}
				}

				return null;
			}

			TableLevel GetAssociation(Expression expression, int level)
			{
				if (ObjectMapper.Associations.Count > 0)
				{
					var levelExpression = expression.GetLevelExpression(level);

					if (levelExpression.NodeType == ExpressionType.MemberAccess)
					{
						var memberExpression = (MemberExpression)levelExpression;

						AssociatedTableContext tableAssociation;

						if (!_associations.TryGetValue(memberExpression.Member, out tableAssociation))
						{
							var q =
								from a in ObjectMapper.Associations
								where TypeHelper.Equals(a.MemberAccessor.MemberInfo, memberExpression.Member)
								select new AssociatedTableContext(Builder, this, a) { Parent = Parent };

							tableAssociation = q.FirstOrDefault();

							_associations.Add(memberExpression.Member, tableAssociation);
						}

						if (tableAssociation != null)
						{
							if (levelExpression == expression)
								return new TableLevel { Table = tableAssociation, Level = level };

							var al = tableAssociation.GetAssociation(expression, level + 1);

							if (al != null)
								return al;

							var field = tableAssociation.GetField(expression, level + 1);

							return new TableLevel { Table = tableAssociation, Field = field, Level = field == null ? level : level + 1 };
						}
					}
				}

				return null;
			}

			#endregion
		}

		#endregion

		#region AssociatedTableContext

		class AssociatedTableContext : TableContext
		{
			private         TableContext         _parentAssociation;
			public readonly SqlQuery.JoinedTable  ParentAssociationJoin;
			public          bool                  IsList;

			public AssociatedTableContext(ExpressionBuilder builder, TableContext parent, Association association)
				: base(builder, parent.SqlQuery)
			{
				var type = TypeHelper.GetMemberType(association.MemberAccessor.MemberInfo);

				var left   = association.CanBeNull;

				if (TypeHelper.IsSameOrParent(typeof(IEnumerable), type))
				{
					var etypes = TypeHelper.GetGenericArguments(type, typeof(IEnumerable));
					type       = etypes != null && etypes.Length > 0 ? etypes[0] : TypeHelper.GetListItemType(type);
					IsList     = true;
				}

				OriginalType = type;
				ObjectType   = GetObjectType();
				ObjectMapper = Builder.MappingSchema.GetObjectMapper(ObjectType);
				SqlTable     = new SqlTable(builder.MappingSchema, ObjectType);

				var psrc = parent.SqlQuery.From[parent.SqlTable];
				var join = left ? SqlTable.WeakLeftJoin() : IsList ? SqlTable.InnerJoin() : SqlTable.WeakInnerJoin();

				_parentAssociation    = parent;
				ParentAssociationJoin = join.JoinedTable;

				psrc.Joins.Add(join.JoinedTable);

				//Init(mappingSchema);

				for (var i = 0; i < association.ThisKey.Length; i++)
				{
					SqlField field1;

					SqlField field2;

					if (!parent.SqlTable.Fields.TryGetValue(association.ThisKey[i], out field1))
						throw new LinqException("Association key '{0}' not found for type '{1}.", association.ThisKey[i], parent.ObjectType);

					if (!SqlTable.Fields.TryGetValue(association.OtherKey[i], out field2))
						throw new LinqException("Association key '{0}' not found for type '{1}.", association.OtherKey[i], ObjectType);

					join.Field(field1).Equal.Field(field2);
				}
			}
		}

		#endregion
	}
}
