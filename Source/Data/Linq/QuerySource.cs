using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace BLToolkit.Data.Linq
{
	using Data.Sql;
	using Mapping;
	using Reflection;
	using Reflection.Extension;

	abstract class QuerySource : QueryField
	{
		#region Table

		public class Table : QuerySource
		{
			public Table(MappingSchema mappingSchema, SqlQuery sqlQuery, LambdaInfo lambda)
				: base(sqlQuery, lambda)
			{
				var type = TypeHelper.GetGenericType(typeof(IEnumerable<>), lambda.Body.Type);

				OriginalType = type == null ? lambda.Body.Type : type.GetGenericArguments()[0];
				_objectType  = GetObjectType(OriginalType, mappingSchema);
				SqlTable     = new SqlTable(mappingSchema, _objectType);

				sqlQuery.From.Table(SqlTable);

				Init(mappingSchema);

				ParsingTracer.DecIndentLevel();
			}

			public Table(MappingSchema mappingSchema, SqlQuery sqlQuery, Type type)
				: base(sqlQuery, null)
			{
				OriginalType = type;
				_objectType  = GetObjectType(OriginalType, mappingSchema);
				SqlTable     = new SqlTable(mappingSchema, _objectType);

				sqlQuery.From.Table(SqlTable);

				Init(mappingSchema);

				ParsingTracer.DecIndentLevel();
			}

			Table(MappingSchema mappingSchema, SqlQuery sqlQuery, Association association, Table parent)
				: base(sqlQuery, null)
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
				_objectType  = GetObjectType(OriginalType, mappingSchema);
				SqlTable     = new SqlTable(mappingSchema, ObjectType);

				var psrc = sqlQuery.From[parent.SqlTable];
				var join = left ? SqlTable.WeakLeftJoin() : isList ? SqlTable.InnerJoin() : SqlTable.WeakInnerJoin();

				_parentAssociation     = parent;
				_parentAssociationJoin = join.JoinedTable;

				psrc.Joins.Add(join.JoinedTable);

				Init(mappingSchema);

				for (var i = 0; i < association.ThisKey.Length; i++)
				{
					Column col1, col2;

					if (!parent._columns.TryGetValue(association.ThisKey[i], out col1))
						throw new LinqException("Association key '{0}' not found for type '{1}.", association.ThisKey[i], parent.ObjectType);

					if (!_columns.TryGetValue(association.OtherKey[i], out col2))
						throw new LinqException("Association key '{0}' not found for type '{1}.", association.OtherKey[i], ObjectType);

					join.Field(col1.Field).Equal.Field(col2.Field);
				}

				ParsingTracer.DecIndentLevel();
			}

			void Init(MappingSchema mappingSchema)
			{
				_mappingSchema = mappingSchema;

				var objectMapper = mappingSchema.GetObjectMapper(ObjectType);

				foreach (var field in SqlTable.Fields)
				{
					var mapper = objectMapper[field.Value.PhysicalName];
					var column = new Column(this, field.Value, mapper);

					Fields.Add(column);
					_columns.Add(field.Value.Name, column);
				}

				foreach (var a in objectMapper.Associations)
					_associations.Add(a.MemberAccessor.Name, a);

				InheritanceMapping = objectMapper.InheritanceMapping;

				if (InheritanceMapping.Count > 0)
				{
					InheritanceDiscriminators = new List<string>(InheritanceMapping.Count);

					foreach (var mapping in InheritanceMapping)
					{
						string discriminator = null;

						foreach (MemberMapper mm in mappingSchema.GetObjectMapper(mapping.Type))
						{
							if (mm.MapMemberInfo.SqlIgnore == false)
							{
								if (!_columns.ContainsKey(mm.MemberName))
								{
									var field = new SqlField(mm.Type, mm.MemberName, mm.Name, mm.MapMemberInfo.Nullable, int.MinValue, null, mm);
									SqlTable.Fields.Add(field);

									var column = new Column(this, field, null);

									Fields.Add(column);
									_columns.Add(field.Name, column);
								}

								if (mm.MapMemberInfo.IsInheritanceDiscriminator)
									discriminator = mm.MapMemberInfo.Name;
							}
						}

						InheritanceDiscriminators.Add(discriminator);
					}

					var dname = InheritanceDiscriminators.FirstOrDefault(s => s != null);

					if (dname == null)
						throw new LinqException("Inheritance Discriminator is not defined for the '{0}' hierarchy.", ObjectType);

					for (var i = 0; i < InheritanceDiscriminators.Count; i++)
						if (InheritanceDiscriminators[i] == null)
							InheritanceDiscriminators[i] = dname;
				}
			}

			Type GetObjectType(Type originalType, MappingSchema mappingSchema)
			{
				for (var type = originalType.BaseType; type != null && type != typeof(object); type = type.BaseType)
				{
					var extension = TypeExtension.GetTypeExtension(type, mappingSchema.Extensions);
					var mapping   = mappingSchema.MetadataProvider.GetInheritanceMapping(type, extension);

					if (mapping.Length > 0)
						return type;
				}

				return OriginalType;
			}

			public SqlTable SqlTable;

			public          Type  OriginalType;
			private         Type _objectType;
			public override Type  ObjectType { get { return _objectType; } }

			readonly SqlQuery.JoinedTable _parentAssociationJoin;
			public   SqlQuery.JoinedTable  ParentAssociationJoin
			{
				get { return _parentAssociationJoin; }
			}

			readonly Table _parentAssociation;
			public   Table  ParentAssociation
			{
				get { return _parentAssociation; }
			}

			MappingSchema _mappingSchema;

			readonly Dictionary<string,Column> _columns = new Dictionary<string,Column>();
			public   Dictionary<string,Column>  Columns
			{
				get { return _columns; }
			}

			public List<InheritanceMappingAttribute> InheritanceMapping;
			public List<string>                      InheritanceDiscriminators;

			readonly Dictionary<string,Association> _associations = new Dictionary<string,Association>();

			readonly Dictionary<string,Table> _associatedTables = new Dictionary<string,Table>();
			public   Dictionary<string,Table>  AssociatedTables { get { return _associatedTables; }}

			QueryField GetField(string name)
			{
				Column col;

				if (_columns.TryGetValue(name, out col))
				{
					if (_parentAssociation != null && _parentAssociationJoin.JoinType == SqlQuery.JoinType.Inner && _parentAssociationJoin.IsWeak)
					{
						foreach (var cond in _parentAssociationJoin.Condition.Conditions)
						{
							var predicate = (SqlQuery.Predicate.ExprExpr)cond.Predicate;

							if (predicate.Expr2 == col.Field)
								return _parentAssociation.GetField(((SqlField)predicate.Expr1).Name);
						}
					}

					return col;
				}

				return null;
			}

			public override QueryField GetField(MemberInfo mi)
			{
				return GetField(mi.Name) ?? GetAssociation(mi);
			}

			List<QueryField> _keyFields;

			public override List<QueryField> GetKeyFields(bool allIfEmpty)
			{
				if (_keyFields == null)
				{
					_keyFields = (
						from c in Fields.Select(f => (Column)f)
						where   c.Field.IsPrimaryKey
						orderby c.Field.PrimaryKeyOrder
						select (QueryField)c
					).ToList();
				}

				return _keyFields.Count > 0 ? _keyFields : allIfEmpty ? Fields : _keyFields;
			}

			Table GetAssociation(MemberInfo mi)
			{
				Table tbl;

				if (_associatedTables.TryGetValue(mi.Name, out tbl))
					return tbl;

				Association a;

				if (_associations.TryGetValue(mi.Name, out a))
				{
					tbl = new Table(_mappingSchema, SqlQuery, a, this);
					_associatedTables.Add(mi.Name, tbl);

					return tbl;
				}

				return null;
			}

			public override QueryField GetField(LambdaInfo lambda, Expression expr, int currentMember)
			{
				if (expr.NodeType == ExpressionType.MemberAccess)
				{
					var ma = (MemberExpression)expr;

					if (ma.Expression != null)
					{
						if (ma.Expression.Type == ObjectType || currentMember > 0)
						{
							var list = GetMemberList(lambda, expr);

							if (list == null)
								return null;

							if (list.Count > 1)
							{
								var field = GetField(list, currentMember);
								if (field != null)
									return field;
							}

							return GetField(ma.Member);
						}

						// Check for associations and 'InnerObject.Field' case.
						//
						var name = ma.Member.Name;
						var e    = ma.Expression;

						while (e != null)
						{
							if (e.NodeType == ExpressionType.MemberAccess)
							{
								ma = (MemberExpression)e;

								if (ma.Expression == null)
									break;

								name = ma.Member.Name + '.' + name;

								if (ma.Expression.Type == ObjectType)
								{
									Column col;
									if (_columns.TryGetValue(name, out col))
										return col;

									var tbl = GetAssociation(ma.Member);
									if (tbl != null)
										return tbl.GetField(lambda, expr, currentMember + 1);
								}

								e = ma.Expression;
							}
							else
								break;
						}
					}
				}

				return null;
			}

			public override QueryField GetField(SqlField field)
			{
				QueryField col = Columns.Values.FirstOrDefault(f => f.Field == field);

				if (col == null)
				{
					foreach (var table in AssociatedTables.Values)
					{
						col = table.GetField(field);
						if (col != null)
							return col;
					}
				}

				return col;
			}

			public override MemberInfo GetMember(QueryField field)
			{
				var objectMapper = _mappingSchema.GetObjectMapper(ObjectType);

				if (field is SubQueryColumn)
					field = ((SubQueryColumn)field).Field;

				return objectMapper[((Column)field).Field.Name, true].MemberAccessor.MemberInfo;
			}

			public override ISqlExpression[] GetExpressions<T>(ExpressionParser<T> parser)
			{
				return new ISqlExpression[] { SqlTable };
			}

			Table() {}

			protected override QuerySource CloneInstance(Dictionary<ICloneableElement,ICloneableElement> objectTree, Predicate<ICloneableElement> doClone)
			{
				var table = new Table
				{
					_objectType = ObjectType,
					SqlTable    = (SqlTable)SqlTable.Clone(objectTree, doClone)
				};

				foreach (var c in _columns)
					table._columns.Add(c.Key, (Column)c.Value.Clone(objectTree, doClone));

				return table;
			}
		}

		#endregion

		#region Expr

		public class Expr : QuerySource
		{
			public Expr(SqlQuery sqlBilder, LambdaInfo lambda, params QuerySource[] baseQueries)
				: base(sqlBilder, lambda, baseQueries)
			{
				if (lambda.Body is NewExpression)
				{
					var ex = (NewExpression)lambda.Body;

					if (ex.Members == null)
						return;

					for (var i = 0; i < ex.Members.Count; i++)
					{
						var member = ex.Members[i];

						if (member is MethodInfo)
							member = TypeHelper.GetPropertyByMethod((MethodInfo)member);

						var field = 
							GetBaseField(lambda, ex.Arguments[i]) ??
							new ExprColumn(this, ex.Arguments[i], member.Name);

						Fields.Add(field);
						Members.Add(member, field);
					}
				}
				else if (lambda.Body is MemberInitExpression)
				{
					var ex = (MemberInitExpression)lambda.Body;

					for (var i = 0; i < ex.Bindings.Count; i++)
					{
						var binding = ex.Bindings[i];
						var member  = binding.Member;

						if (member is MethodInfo)
							member = TypeHelper.GetPropertyByMethod((MethodInfo)member);

						if (binding is MemberAssignment)
						{
							var ma    = binding as MemberAssignment;
							var field = GetBaseField(lambda, ma.Expression) ?? new ExprColumn(this, ma.Expression, member.Name);

							Fields.Add(field);
							Members.Add(member, field);
						}
						else
							throw new InvalidOperationException();
					}
				}

				ParsingTracer.DecIndentLevel();
			}

			protected readonly Dictionary<MemberInfo,QueryField> Members = new Dictionary<MemberInfo,QueryField>();

			public override QueryField GetField(MemberInfo mi)
			{
				QueryField fld;
				if (Members.TryGetValue(mi, out fld))
					return fld;

				fld = BaseQuery.GetField(mi) as QuerySource;

				if (fld != null)
					Members.Add(mi, fld);

				return fld;
			}

			public override List<QueryField> GetKeyFields(bool allIfEmpty)
			{
				return Fields;
			}

			public override MemberInfo GetMember(QueryField field)
			{
				foreach (var member in Members)
					if (member.Value == field)
						return member.Key;

				return null;
			}

			public override QueryField GetField(LambdaInfo lambda, Expression expr, int currentMember)
			{
				switch (expr.NodeType)
				{
					case ExpressionType.Parameter:
						for (var i = 0; i < Lambda.Parameters.Length; i++)
							if (Lambda.Parameters[i] == expr)
								return Sources[i];

						return null;
						//throw new InvalidOperationException();

					case ExpressionType.MemberAccess:
						{
							var ma = (MemberExpression)expr;

							if (ma.Expression != null)
							{
								if (ma.Expression.NodeType == ExpressionType.Parameter)
									return GetField(ma.Member);

								if (ma.Expression.NodeType == ExpressionType.Constant)
									break;
							}

							var list  = GetMemberList(lambda, expr);
							if (list == null || list.Count == 0)
								return null;

							var field = GetField(list, currentMember);

							if (field != null)
								return field;

							break;
						}
				}

				foreach (var field in Fields)
					if (field is ExprColumn && ((ExprColumn)field).Expr == expr)
						return field;

				return null;
			}

			protected Expr() {}

			protected virtual Expr CreateExpr(Dictionary<ICloneableElement,ICloneableElement> objectTree)
			{
				return new Expr();
			}

			protected override QuerySource CloneInstance(Dictionary<ICloneableElement,ICloneableElement> objectTree, Predicate<ICloneableElement> doClone)
			{
				var expr = CreateExpr(objectTree);

				foreach (var c in Members)
					expr.Members.Add(c.Key, (QueryField)c.Value.Clone(objectTree, doClone));

				return expr;
			}
		}

		#endregion

		#region SubQuery

		public class SubQuery : QuerySource
		{
			public SubQuery(SqlQuery currentSql, SqlQuery subSql, QuerySource baseQuery, bool addToSource)
				: base(currentSql, baseQuery.Lambda, baseQuery)
			{
				ParsingTracer.WriteLine(subSql);

				SubSql = subSql;

				if (addToSource)
					SqlQuery.From.Table(subSql);

				foreach (var field in baseQuery.Fields)
					EnsureField(field);

				ParsingTracer.DecIndentLevel();
			}

			public SubQuery(SqlQuery currentSql, SqlQuery subSql, QuerySource baseQuery)
				: this(currentSql, subSql, baseQuery, true)
			{
			}

			public   SqlQuery                          SubSql;
			public   QueryField                        CheckNullField;
			public   List<QuerySource>                 Unions   = new List<QuerySource>();
			readonly Dictionary<QueryField,QueryField> _columns = new Dictionary<QueryField,QueryField>();

			public override QueryField EnsureField(QueryField field)
			{
				if (field == null)
					return null;

				QueryField col;

				if (!_columns.TryGetValue(field, out col))
				{
					col = field is QuerySource ?
						(QueryField)new SubQuerySourceColumn(this, (QuerySource)field) :
						new SubQueryColumn(this, field);

					Fields.Add(col);
					_columns.Add(field, col);
				}

				return col;
			}

			public override QueryField GetField(MemberInfo mi)
			{
				return EnsureField(BaseQuery.GetField(mi));
			}

			public override List<QueryField> GetKeyFields(bool allIfEmpty)
			{
				return BaseQuery.GetKeyFields(allIfEmpty).Select(f => _columns[f]).ToList();
			}

			public override QueryField GetField(LambdaInfo lambda, Expression expr, int currentMember)
			{
				if (expr.NodeType == ExpressionType.Parameter)
				{
					if (BaseQuery is Scalar)
						return EnsureField(BaseQuery.Fields[0]);

					if (BaseQuery is GroupBy)
						return BaseQuery;
				}

				return EnsureField(BaseQuery.GetField(lambda, expr, currentMember));
			}

			protected override QueryField GetField(List<MemberInfo> members, int currentMember)
			{
				var field = GetField(members[currentMember]);

				if (field == null || currentMember + 1 == members.Count)
					 return field;

				if (!(field is SubQueryColumn))
					return ((QuerySource)field).GetField(members, currentMember + 1);

				field = BaseQuery.GetField(members, currentMember);

				return EnsureField(field);
			}

			public override QueryField GetField(SqlField field)
			{
				return EnsureField(base.GetField(field));
			}

			public override MemberInfo GetMember(QueryField field)
			{
				foreach (var col in _columns)
					if (col.Value == field)
						BaseQuery.GetMember(col.Value);

				return null;
			}

			public override Type ObjectType { get { return BaseQuery.ObjectType; } }

			protected SubQuery() {}

			protected virtual SubQuery CreateSubQuery(Dictionary<ICloneableElement,ICloneableElement> objectTree, Predicate<ICloneableElement> doClone)
			{
				return new SubQuery();
			}

			protected override QuerySource CloneInstance(Dictionary<ICloneableElement,ICloneableElement> objectTree, Predicate<ICloneableElement> doClone)
			{
				var sub = CreateSubQuery(objectTree, doClone);

				sub.SubSql = (SqlQuery)SubSql.Clone(objectTree, doClone);

				foreach (var c in _columns)
					sub._columns.Add(c.Key, (SubQueryColumn)c.Value.Clone(objectTree, doClone));

				return sub;
			}

			public override ISqlExpression[] GetExpressions<T>(ExpressionParser<T> parser)
			{
				var expr = base.GetExpressions(parser);

				return expr.Length == 1 ? expr : new ISqlExpression[] { SubSql };
			}
		}

		#endregion

		#region GroupJoin

		public class GroupJoin : SubQuery
		{
			public GroupJoin(SqlQuery currentSql, SqlQuery subSql, QuerySource baseQuery)
				: base(currentSql, subSql, baseQuery, false)
			{
			}

			public ExprColumn Counter;

			private QueryField _checkNullField;
			public new QueryField  CheckNullField
			{
				get
				{
					if (_checkNullField == null)
					{
						foreach (var f in Fields)
							if (!f.CanBeNull())
								return _checkNullField = f;

						var valueCol = new ExprColumn(BaseQuery, new SqlValue((int?)1), null);
						var subCol   = EnsureField(valueCol);

						_checkNullField = subCol;
					}

					return _checkNullField;
				}
			}

			GroupJoin() {}

			protected override SubQuery CreateSubQuery(Dictionary<ICloneableElement,ICloneableElement> objectTree, Predicate<ICloneableElement> doClone)
			{
				return new GroupJoin { Counter = (ExprColumn)Counter.Clone(objectTree, doClone) };
			}
		}

		#endregion

		#region Scalar

		public class Scalar : QuerySource
		{
			public Scalar(SqlQuery sqlBilder, LambdaInfo lambda, params QuerySource[] baseQueries)
				: base(sqlBilder, lambda, baseQueries)
			{
				_field = GetBaseField(lambda, lambda.Body) ?? new ExprColumn(this, lambda.Body, null);

				Fields.Add(_field);

				ParsingTracer.DecIndentLevel();
			}

			public Scalar(SqlQuery sqlBilder, LambdaInfo lambda, ISqlExpression field, params QuerySource[] baseQueries)
				: base(sqlBilder, lambda, baseQueries)
			{
				_field = new ExprColumn(this, field, null);

				Fields.Add(_field);

				ParsingTracer.DecIndentLevel();
			}

			QueryField _field;

			public override QueryField GetField(MemberInfo mi)
			{
				if (Lambda.Body.NodeType == ExpressionType.MemberAccess)
				{
					var ex = (MemberExpression)Lambda.Body;

					if (ex.Member == mi)
						return Fields[0];
				}

				return null;
			}

			public override List<QueryField> GetKeyFields(bool allIfEmpty)
			{
				throw new NotImplementedException();
			}

			public override MemberInfo GetMember(QueryField field)
			{
				return null;
			}

			public override QueryField GetField(LambdaInfo lambda, Expression expr, int currentMember)
			{
				if (Lambda.Body is MemberExpression && expr is MemberExpression)
					if (((MemberExpression)expr).Member == ((MemberExpression)Lambda.Body).Member)
						return _field;

				return GetBaseField(lambda, expr);
			}

			Scalar() {}

			protected override QuerySource CloneInstance(Dictionary<ICloneableElement,ICloneableElement> objectTree, Predicate<ICloneableElement> doClone)
			{
				return new Scalar { _field = (QueryField)_field.Clone(objectTree, doClone) };
			}
		}

		#endregion

		#region GroupBy

		public class GroupBy : Expr
		{
			public GroupBy(
				SqlQuery         sqlQuery,
				QuerySource      groupQuery,
				QuerySource      originalQuery,
				LambdaInfo       keySelector,
				QuerySource      elementSource,
				Type             groupingType,
				bool             isWrapped,
				ISqlExpression[] byExpressions)
				: base(sqlQuery, keySelector, groupQuery)
			{
				ParsingTracer.IncIndentLevel();

				OriginalQuery = originalQuery;
				ElementSource = elementSource;
				GroupingType  = groupingType;
				IsWrapped     = isWrapped;
				ByExpressions = byExpressions;

				var field = new GroupByColumn(this);

				Fields.Add(field);

				Members.Add(groupingType.GetProperty("Key"), field);

				ParsingTracer.DecIndentLevel();
			}

			public QuerySource      OriginalQuery;
			public QuerySource      ElementSource;
			public Type             GroupingType;
			public bool             IsWrapped;
			public ISqlExpression[] ByExpressions;

			public override QueryField GetBaseField(LambdaInfo lambda, Expression expr)
			{
				return BaseQuery.GetBaseField(lambda, expr);
			}

			public ExprColumn FindField(ExprColumn column)
			{
				foreach (var field in Fields)
					if (field is ExprColumn && ((ExprColumn)field).SqlExpression == column.SqlExpression)
						return (ExprColumn)field;

				Fields.Add(column);

				return column;
			}

			public override QueryField GetField(SqlField field)
			{
				return OriginalQuery.GetField(field);
			}

			public override QueryField GetField(LambdaInfo lambda, Expression expr, int currentMember)
			{
				var field = base.GetField(lambda, expr, currentMember);

				if (field != null && field is Column)
				{
					var f = ((Column)field).Field;

					if (SqlQuery.GroupBy.Items.Find(e => e == f) == null)
						SqlQuery.GroupBy.Expr(f);
				}

				return field;
			}

			GroupBy() {}

			protected override Expr CreateExpr(Dictionary<ICloneableElement,ICloneableElement> objectTree)
			{
				return new GroupBy { ElementSource = ElementSource };
			}
		}

		#endregion

		#region SubQuerySourceColumn

		public class SubQuerySourceColumn : QuerySource
		{
			public SubQuerySourceColumn(SubQuery querySource, QuerySource sourceColumn)
				: base(sourceColumn.SqlQuery, sourceColumn.Lambda, sourceColumn.Sources)
			{
				QuerySource  = querySource;
				SourceColumn = sourceColumn;

				ParsingTracer.WriteLine(sourceColumn);
			}

			public SubQuery    QuerySource;
			public QuerySource SourceColumn;

			public override QueryField GetField(LambdaInfo lambda, Expression expr, int currentMember)
			{
				if (expr == Lambda.Body)
					return null;

				var field = SourceColumn.GetField(lambda, expr, currentMember);
				return field == null ? null : QuerySource.EnsureField(field);
			}

			public override QueryField GetField(MemberInfo mi)
			{
				var field = SourceColumn.GetField(mi);
				return field == null ? null : QuerySource.EnsureField(field);
			}

			public override List<QueryField> GetKeyFields(bool allIfEmpty)
			{
				throw new NotImplementedException();
			}

			public override MemberInfo GetMember(QueryField field)
			{
				throw new NotImplementedException();
			}

			public override FieldIndex[] Select<T>(ExpressionParser<T> parser)
			{
				if (_indexes == null)
				{
					var idx = SourceColumn.Select(parser);

					var list = new List<FieldIndex>(idx.Length);

					for (var i = 0; i < idx.Length; i++)
						list.AddRange(QuerySource.EnsureField(idx[i].Field).Select(parser));

					_indexes = list.ToArray();
				}

				return _indexes;
			}

			/*
			void SetSubIndex<T>(ExpressionParser<T> parser)
			{
				if (_subIndex == null)
				{
					_subIndex = SourceColumn.Select(parser);

					if (QuerySource.SubSql.HasUnion)
					{
						var sub = QuerySource.BaseQuery;
						var idx = sub.Fields.IndexOf(SourceColumn);

						MemberInfo mi = sub.GetMember(SourceColumn);

						foreach (var union in QuerySource.Unions)
						{
							if (mi != null)
							{
								var f = union.GetField(mi);

								if (f != null)
								{
									f.Select(parser);
									continue;
								}

								if (union is QuerySource.Expr)
								{
									union.SqlQuery.Select.Add(new SqlValue(null));
									continue;
								}
							}

							union.Fields[idx].Select(parser);
						}
					}
				}
			}
			*/

			public override ISqlExpression[] GetExpressions<T>(ExpressionParser<T> parser)
			{
				throw new NotImplementedException();
				/*
				SetSubIndex(parser);

				if (_subIndex.Length != 1)
					throw new LinqException("Cannot convert '{0}' to SQL.", SourceColumn.GetExpressions(parser)[0]);

				return new [] { QuerySource.SubSql.Select.Columns[_subIndex[0].Index] };
				*/
			}

			public override bool CanBeNull()
			{
				return SourceColumn.CanBeNull();
			}

			SubQuerySourceColumn() {}

			protected override QuerySource CloneInstance(Dictionary<ICloneableElement, ICloneableElement> objectTree, Predicate<ICloneableElement> doClone)
			{
				return new SubQuerySourceColumn
				{
					QuerySource  = (SubQuery)   QuerySource. Clone(objectTree, doClone),
					SourceColumn = (QuerySource)SourceColumn.Clone(objectTree, doClone)
				};
			}

#if OVERRIDETOSTRING

			public override string ToString()
			{
				return SourceColumn.ToString();
			}

#endif
		}

		#endregion


		#region base

		protected QuerySource(SqlQuery sqlQuery, LambdaInfo lambda, params QuerySource[] baseQueries)
		{
			SqlQuery = sqlQuery;
			Lambda   = lambda;

			_sources = baseQueries;

#if DEBUG && TRACE_PARSING
			ParsingTracer.WriteLine(lambda);
			ParsingTracer.WriteLine(this);

			foreach (var query in baseQueries)
				ParsingTracer.WriteLine("base", query);

			foreach (var field in Fields)
				ParsingTracer.WriteLine("field ", field);

			ParsingTracer.IncIndentLevel();
#endif
		}

#if OVERRIDETOSTRING

		public override string ToString()
		{
			var str = SqlQuery.ToString().Replace('\t', ' ').Replace('\n', ' ').Replace("\r", "");

			for (var len = str.Length; len != (str = str.Replace("  ", " ")).Length; len = str.Length)
			{
			}

			return str;
		}

#endif

		protected QuerySource()
		{
		}

		protected abstract QuerySource CloneInstance(Dictionary<ICloneableElement,ICloneableElement> objectTree, Predicate<ICloneableElement> doClone);

		public override ICloneableElement Clone(Dictionary<ICloneableElement, ICloneableElement> objectTree, Predicate<ICloneableElement> doClone)
		{
			if (!doClone(this))
				return this;

			ICloneableElement clone;

			if (!objectTree.TryGetValue(this, out clone))
			{
				var qs = CloneInstance(objectTree, doClone);

				objectTree.Add(this, qs);

				qs.SqlQuery = (SqlQuery)SqlQuery.Clone(objectTree, doClone);
				qs.Lambda   = Lambda;
				qs._sources = Array. ConvertAll(Sources, q => (QuerySource)q.Clone(objectTree, doClone));
				qs._fields  = Fields.ConvertAll(f => (QueryField)f.Clone(objectTree, doClone));

				clone = qs;
			}

			return clone;
		}

		private         QuerySource[] _sources;
		public override QuerySource[]  Sources { get { return _sources; } }

		public QuerySource BaseQuery { get { return _sources[0]; } }

		public SqlQuery    SqlQuery;
		public LambdaInfo  Lambda;

		private        List<QueryField> _fields = new List<QueryField>();
		public virtual List<QueryField>  Fields     { get { return _fields; } }
		public virtual Type              ObjectType { get { return Lambda.Body.Type; }}

		public abstract QueryField       GetField    (LambdaInfo lambda, Expression expr, int currentMember);
		public abstract QueryField       GetField    (MemberInfo mi);
		public abstract List<QueryField> GetKeyFields(bool allIfEmpty);

		protected virtual QueryField GetField(List<MemberInfo> members, int currentMember)
		{
			var field = GetField(members[currentMember]);

			if (field == null || currentMember + 1 == members.Count)
				 return field;

			if (field is GroupByColumn)
				return ((GroupByColumn)field).GroupBySource.BaseQuery.GetField(members, currentMember + 1);

			if (field is SubQueryColumn)
			{
				var subQuery = ((SubQueryColumn)field).Field as QuerySource;

				if (subQuery == null)
					return null;

				var f = subQuery.GetField(members, currentMember + 1);

				return field.Sources[0].EnsureField(f);
			}

			if (field is QuerySource)
				return ((QuerySource)field).GetField(members, currentMember + 1);

			return field;
		}

		public virtual QueryField EnsureField(QueryField field)
		{
			foreach (var f in Fields)
				if (f == field)
					return field;

			throw new InvalidOperationException();
		}

		public virtual QueryField GetBaseField(LambdaInfo lambda, Expression expr)
		{
			if (Sources.Length > 0)
			{
				if (expr.NodeType == ExpressionType.Parameter)
				{
					if (Sources.Length == 1)
						return BaseQuery;

					if (Sources.Length < Lambda.Parameters.Length)
						throw new InvalidOperationException();

					for (var i = 0; i < Lambda.Parameters.Length; i++)
						if (Lambda.Parameters[i] == expr)
							return Sources[i];
				}

				foreach (var pq in Sources)
				{
					var field = pq.GetField(null, expr, 0);
					if (field != null)
						return field;
				}
			}

			return null;
		}

		public virtual QueryField GetField(SqlField field)
		{
			foreach (var source in Sources)
			{
				var f = source.GetField(field);
				if (f != null)
					return f;
			}

			return null;
		}

		protected List<MemberInfo> GetMemberList(LambdaInfo lambda, Expression expr)
		{
			var list = new List<MemberInfo>();

			while (expr != null)
			{
				switch (expr.NodeType)
				{
					case ExpressionType.MemberAccess:
						var ma = (MemberExpression)expr;

						list.Insert(0, ma.Member);

						expr = ma.Expression;
						break;

					case ExpressionType.Parameter :
						if (lambda != null)
						{
							foreach (var p in lambda.Parameters)
								if (expr == p)
									return list;

							return null;
						}

						return list;

					default :
						list.Clear();
						return list;
				}
			}

			return list;
		}

		public abstract MemberInfo GetMember(QueryField field);

		FieldIndex[] _indexes;

		public override FieldIndex[] Select<T>(ExpressionParser<T> parser)
		{
			ParsingTracer.WriteLine(this);
			ParsingTracer.IncIndentLevel();

			if (_indexes == null)
			{
				var indexes = new List<FieldIndex>(Fields.Count);

				foreach (var field in Fields)
					foreach (var idx in field.Select(parser))
						indexes.Add(new FieldIndex { Index = idx.Index, Field = field });

				_indexes = indexes.ToArray();
			}

			ParsingTracer.DecIndentLevel();
			return _indexes;
		}

		public override ISqlExpression[] GetExpressions<T>(ExpressionParser<T> parser)
		{
			if (Fields.Count == 1)
				return Fields[0].GetExpressions(parser);

			var exprs = new List<ISqlExpression>();

			foreach (var field in Fields)
				exprs.AddRange(field.GetExpressions(parser));

			return exprs.ToArray();
		}

		public override bool CanBeNull()
		{
			return true;
		}

		public bool Find(QuerySource query)
		{
			if (query == this)
				return true;

			foreach (var source in Sources)
				if (source.Find(query))
					return true;

			return false;
		}

		public void Match(
			Action<Table>                tableAction,
			Action<Expr>                 exprAction,
			Action<SubQuery>             subQueryAction,
			Action<Scalar>               scalarAction,
			Action<GroupBy>              groupByAction,
			Action<SubQuerySourceColumn> columnAction)
		{
			if      (this is Table)                tableAction   (this as Table);
			else if (this is GroupBy)              groupByAction (this as GroupBy);
			else if (this is Expr)                 exprAction    (this as Expr);
			else if (this is SubQuery)             subQueryAction(this as SubQuery);
			else if (this is Scalar)               scalarAction  (this as Scalar);
			else if (this is SubQuerySourceColumn) columnAction  (this as SubQuerySourceColumn);
			else
				throw new InvalidOperationException();
		}

		#endregion
	}
}
