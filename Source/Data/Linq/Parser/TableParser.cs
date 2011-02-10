using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace BLToolkit.Data.Linq.Parser
{
	using BLToolkit.Linq;
	using Data.Sql;
	using Mapping;
	using Reflection;
	using Reflection.Extension;

	class TableParser : ISequenceParser
	{
		int ISequenceParser.ParsingCounter { get; set; }

		public T Parse<T>(ExpressionParser parser, Expression expression, SqlQuery sqlQuery, Func<int,IParseContext,T> action)
		{
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

						break;
					}

				case ExpressionType.MemberAccess:
					if (expression.Type.IsGenericType && expression.Type.GetGenericTypeDefinition() == typeof(Table<>))
						return action(3, null);

					// Looking for association.
					//
					if (parser.IsSubQueryParsing && sqlQuery.From.Tables.Count == 0)
					{
						var ctx = parser.GetContext(null, expression);
						if (ctx != null && ctx.IsExpression(expression, 0, RequestFor.Association))
							return action(4, ctx);
					}

					break;

				case ExpressionType.Parameter:
					{
						break;
					}
			}

			return action(0, null);
		}

		public bool CanParse(ExpressionParser parser, Expression expression, SqlQuery sqlQuery)
		{
			return Parse(parser, expression, sqlQuery, (n,_) => n > 0);
		}

		public IParseContext ParseSequence(ExpressionParser parser, Expression expression, SqlQuery sqlQuery)
		{
			return Parse(parser, expression, sqlQuery, (n,ctx) =>
			{
				switch (n)
				{
					case 0 : return null;
					case 1 : return new TableContext(parser, expression, sqlQuery, ((IQueryable)((ConstantExpression)expression).Value).ElementType);
					case 2 :
					case 3 : return new TableContext(parser, expression, sqlQuery, expression.Type.GetGenericArguments()[0]);
					case 4 : return ctx.GetContext(expression, 0, sqlQuery);
				}

				throw new InvalidOperationException();
			});
		}

		class TableContext : IParseContext
		{
			protected Type         OriginalType;
			public    Type         ObjectType;
			protected ObjectMapper ObjectMapper;
			public    SqlTable     SqlTable;

			public ExpressionParser Parser     { get; private set; }
			public Expression       Expression { get; private set; }
			public SqlQuery         SqlQuery   { get; private set; }
			public IParseContext    Parent     { get; set; }

			public TableContext(ExpressionParser parser, Expression expression, SqlQuery sqlQuery, Type originalType)
			{
				Parser     = parser;
				Expression = expression;
				SqlQuery   = sqlQuery;

				OriginalType = originalType;
				ObjectType   = GetObjectType();
				SqlTable     = new SqlTable(parser.MappingSchema, ObjectType);
				ObjectMapper = Parser.MappingSchema.GetObjectMapper(ObjectType);

				SqlQuery.From.Table(SqlTable);
			}

			protected TableContext(ExpressionParser parser, SqlQuery sqlQuery)
			{
				Parser   = parser;
				SqlQuery = sqlQuery;
			}

			protected Type GetObjectType()
			{
				for (var type = OriginalType.BaseType; type != null && type != typeof(object); type = type.BaseType)
				{
					var extension = TypeExtension.GetTypeExtension(type, Parser.MappingSchema.Extensions);
					var mapping   = Parser.MappingSchema.MetadataProvider.GetInheritanceMapping(type, extension);

					if (mapping.Length > 0)
						return type;
				}

				return OriginalType;
			}

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
					MappingSchema = Parser.MappingSchema,
					ObjectMapper  = ObjectMapper,
					Index         = index
				};

				return Expression.Convert(
					Expression.Call(null, _mapperMethod,
						ExpressionParser.DataContextParam,
						ExpressionParser.DataReaderParam,
						Expression.Constant(data)),
					ObjectType);
			}

			Expression BuildQuery()
			{
				// Get indexes for all fields.
				//
				var index = this.ConvertToIndex(ConvertFlags.All);

				// Convert to parent indexes.
				//
				index = index.Select(idx => ConvertToParentIndex(idx, null)).ToArray();

				// Build an expression.
				//
				return GetTableExpression(index);
			}

			public void BuildQuery<T>(Query<T> query, ParameterExpression queryParameter)
			{
				var expr = this.BuildExpression();

				var mapper = Expression.Lambda<Func<QueryContext,IDataContext,IDataReader,Expression,object[],T>>(
					expr, new []
					{
						ExpressionParser.ContextParam,
						ExpressionParser.DataContextParam,
						ExpressionParser.DataReaderParam,
						ExpressionParser.ExpressionParam,
						ExpressionParser.ParametersParam,
					});

				query.SetQuery(mapper.Compile());
			}

			public Expression BuildExpression(Expression expression, int level)
			{
				// Build table.
				//
				var table = FindTable(expression, level);

				if (table.Field == null)
					return table.Table.BuildQuery();

				// Build field.
				//
				var idx = ConvertToIndex(expression, level, ConvertFlags.Field).Single();

				idx = ConvertToParentIndex(idx, null);

				return Parser.BuildSql(expression.Type, idx);
			}

			public ISqlExpression[] ConvertToSql(Expression expression, int level, ConvertFlags flags)
			{
				switch (flags)
				{
					case ConvertFlags.All   :
						{
							var table = FindTable(expression, level);

							if (table.Field == null)
								return table.Table.SqlTable.Fields.Values.ToArray();

							break;
						}

					case ConvertFlags.Key   :
						{
							var table = FindTable(expression, level);

							if (table.Field == null)
								return table.Table.SqlTable.GetKeys(true).ToArray();

							break;
						}

					case ConvertFlags.Field :
						{
							var table = FindTable(expression, level);

							if (table.Field != null)
								return new[] { table.Field };

							break;
						}
				}

				throw new NotImplementedException();
			}

			readonly Dictionary<ISqlExpression,int> _indexes = new Dictionary<ISqlExpression,int>();

			public int[] ConvertToIndex(Expression expression, int level, ConvertFlags flags)
			{
				switch (flags)
				{
					case ConvertFlags.Field :
					case ConvertFlags.Key   :
					case ConvertFlags.All   :

						return ConvertToSql(expression, level, flags)
							.Select(expr =>
							{
								int n;

								if (!_indexes.TryGetValue(expr, out n))
								{
									if (expr is SqlField)
									{
										var field = (SqlField)expr;
										n = SqlQuery.Select.Add(field, field.Alias);
									}
									else
									{
										n = SqlQuery.Select.Add(expr);
									}

									_indexes.Add(expr, n);
								}

								return n;
							})
							.ToArray();
				}

				throw new NotImplementedException();
			}

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

			public IParseContext GetContext(Expression expression, int level, SqlQuery currentSql)
			{
				if (ObjectMapper.Associations.Count > 0)
				{
					var levelExpression = expression.GetLevelExpression(level);

					if (levelExpression == expression && expression.NodeType == ExpressionType.MemberAccess)
					{
						if (Parser.IsSubQueryParsing)
						{
							var association = GetAssociation(expression, level);
							var table       = new TableContext(Parser, Expression, currentSql, association.Table.ObjectType) { Parent = Parent };

							foreach (var cond in ((AssociatedTableContext)association.Table).ParentAssociationJoin.Condition.Conditions)
							{
								var predicate = (SqlQuery.Predicate.ExprExpr)cond.Predicate;
								currentSql.Where
									.Expr(predicate.Expr1)
									.Equal
									.Field(table.SqlTable.Fields[((SqlField)predicate.Expr2).Name]);
							}

							return table;
						}

						throw new NotImplementedException();
					}
				}

				throw new InvalidOperationException();
			}

			public int ConvertToParentIndex(int index, IParseContext context)
			{
				return Parent == null ? index : Parent.ConvertToParentIndex(index, this);
			}

			public void SetAlias(string alias)
			{
				if (SqlTable.Alias == null)
					SqlTable.Alias = alias;
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
							if (TypeHelper.IsNullableValueMember(memberExpression.Member) && memberExpression.Expression == levelExpression)
								memberExpression = (MemberExpression)levelExpression;

						if (levelExpression == memberExpression)
							foreach (var field in SqlTable.Fields.Values)
								if (field.MemberMapper.MemberAccessor.MemberInfo == memberExpression.Member)
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
								where 
									a.MemberAccessor.MemberInfo.DeclaringType == memberExpression.Member.DeclaringType &&
									a.MemberAccessor.MemberInfo.Name          == memberExpression.Member.Name
								select new AssociatedTableContext(Parser, this, a) { Parent = Parent };

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

		class AssociatedTableContext : TableContext
		{
			private         TableContext         _parentAssociation;
			public readonly SqlQuery.JoinedTable  ParentAssociationJoin;

			public AssociatedTableContext(ExpressionParser parser, TableContext parent, Association association)
				: base(parser, parent.SqlQuery)
			{
				var type = TypeHelper.GetMemberType(association.MemberAccessor.MemberInfo);

				var left   = association.CanBeNull;
				var isList = false;

				if (TypeHelper.IsSameOrParent(typeof(IEnumerable), type))
				{
					var etypes = TypeHelper.GetGenericArguments(type, typeof(IEnumerable));
					type       = etypes != null && etypes.Length > 0 ? etypes[0] : TypeHelper.GetListItemType(type);
					isList     = true;
				}

				OriginalType = type;
				ObjectType   = GetObjectType();
				ObjectMapper = Parser.MappingSchema.GetObjectMapper(ObjectType);
				SqlTable     = new SqlTable(parser.MappingSchema, ObjectType);

				var psrc = parent.SqlQuery.From[parent.SqlTable];
				var join = left ? SqlTable.WeakLeftJoin() : isList ? SqlTable.InnerJoin() : SqlTable.WeakInnerJoin();

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
	}
}
