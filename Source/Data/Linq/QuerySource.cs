using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using BLToolkit.Reflection.Extension;

namespace BLToolkit.Data.Linq
{
	using Data.Sql;
	using Mapping;
	using Reflection;

	abstract class QuerySource : QueryField
	{
		#region Table

		public class Table : QuerySource
		{
			public Table(MappingSchema mappingSchema, SqlQuery sqlQuery, LambdaInfo lambda)
				: base(sqlQuery, lambda)
			{
				var type = TypeHelper.GetGenericType(typeof(IQueryable<>), lambda.Body.Expr.Type);

				OriginalType = type == null ? lambda.Body.Expr.Type : type.GetGenericArguments()[0];
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
				var type = association.MemberAccessor.MemberInfo.MemberType == MemberTypes.Property ?
					((PropertyInfo)association.MemberAccessor.MemberInfo).PropertyType :
					((FieldInfo)   association.MemberAccessor.MemberInfo).FieldType;

				var left = association.CanBeNull;

				if (TypeHelper.IsSameOrParent(typeof(IList), type))
				{
					type = TypeHelper.GetListItemType(type);
					left = true;
				}

				OriginalType = type;
				_objectType  = GetObjectType(OriginalType, mappingSchema);
				SqlTable     = new SqlTable(mappingSchema, ObjectType);

				var psrc = sqlQuery.From[parent.SqlTable];
				var join = left ? SqlTable.WeakLeftJoin() : SqlTable.InnerJoin();

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
									var field = new SqlField(mm.MemberName, mm.Name, mm.MapMemberInfo.Nullable, int.MinValue);
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

			public override QueryField GetField(MemberInfo mi)
			{
				Column col;

				if (_columns.TryGetValue(mi.Name, out col))
					return col;

				return GetAssociation(mi);
			}

			private List<Column> _keyFields;
			public  List<Column>  KeyFields
			{
				get
				{
					if (_keyFields == null)
					{
						_keyFields = new List<Column>(
							from c in Fields.Select(f => (Column)f)
							where   c.Field.IsPrimaryKey
							orderby c.Field.PrimaryKeyOrder
							select c);

						if (_keyFields.Count == 0)
							_keyFields.AddRange(Fields.Select(f => (Column)f));
					}

					return _keyFields;
				}
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

			public override QueryField GetField(Expression expr)
			{
				if (expr.NodeType == ExpressionType.MemberAccess)
				{
					var ma = (MemberExpression)expr;

					if (ma.Expression != null)
					{
						if (ma.Expression.Type == ObjectType)
							return GetField(ma.Member);

						// Check for associations and 'InnerObject.Field' case.
						//
						var name = ma.Member.Name;
						var e    = ma.Expression;

						while (e != null)
						{
							if (e.NodeType == ExpressionType.MemberAccess)
							{
								ma   = (MemberExpression)e;
								name = ma.Member.Name + '.' + name;

								if (ma.Expression.Type == ObjectType)
								{
									Column col;
									if (_columns.TryGetValue(name, out col))
										return col;

									var tbl = GetAssociation(ma.Member);
									if (tbl != null)
										return tbl.GetField(expr);
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

				return objectMapper[((Column)field).Field.Name].MemberAccessor.MemberInfo;
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
				if (lambda.Body.Expr is NewExpression)
				{
					var body = lambda.Body.ConvertTo<NewExpression>();
					var ex   = body.Expr;

					if (ex.Members == null)
						return;

					for (var i = 0; i < ex.Members.Count; i++)
					{
						var member = ex.Members[i];

						if (member is MethodInfo)
							member = TypeHelper.GetPropertyByMethod((MethodInfo)member);

						var field = 
							GetBaseField(ex.Arguments[i]) ??
							new ExprColumn(this, body.Create(ex.Arguments[i], body.Index(ex.Arguments, New.Arguments, i)), member.Name);

						Fields.Add(field);
						Members.Add(member, field);
					}
				}
				else if (lambda.Body.Expr is MemberInitExpression)
				{
					var body = lambda.Body.ConvertTo<MemberInitExpression>();
					var ex   = body.Expr;

					for (var i = 0; i < ex.Bindings.Count; i++)
					{
						var binding = ex.Bindings[i];
						var member  = binding.Member;

						if (member is MethodInfo)
							member = TypeHelper.GetPropertyByMethod((MethodInfo)member);

						if (binding is MemberAssignment)
						{
							var ma = binding as MemberAssignment;

							var piBinding    = body.     Create(ma.Expression, body.Index(ex.Bindings, MemberInit.Bindings, i));
							var piAssign     = piBinding.Create(ma.Expression, piBinding.ConvertExpressionTo<MemberAssignment>());
							var piExpression = piAssign. Create(ma.Expression, piAssign.Property(MemberAssignmentBind.Expression));

							var field = GetBaseField(piExpression) ?? new ExprColumn(this, piExpression, member.Name);

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
				Members.TryGetValue(mi, out fld);
				return fld;
			}

			public override MemberInfo GetMember(QueryField field)
			{
				foreach (KeyValuePair<MemberInfo,QueryField> member in Members)
					if (member.Value == field)
						return member.Key;

				return null;
			}

			public override QueryField GetField(Expression expr)
			{
				switch (expr.NodeType)
				{
					case ExpressionType.Parameter:
						for (var i = 0; i < Lambda.Parameters.Length; i++)
							if (Lambda.Parameters[i] == expr)
								return Sources[i];

						throw new InvalidOperationException();

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

							var list = new List<MemberInfo>();

							while (expr != null)
							{
								switch (expr.NodeType)
								{
									case ExpressionType.MemberAccess:
										ma = (MemberExpression)expr;

										list.Insert(0, ma.Member);

										expr = ma.Expression;
										break;

									case ExpressionType.Parameter:
										expr = null;
										break;

									default:
										return null;
								}
							}

							var field = GetField(list, 0);

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
				: base(currentSql, null, baseQuery)
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

			public   SqlQuery                              SubSql;
			public   List<QuerySource>                     Unions = new List<QuerySource>();
			readonly Dictionary<QueryField,SubQueryColumn> _columns = new Dictionary<QueryField,SubQueryColumn>();

			public override QueryField EnsureField(QueryField field)
			{
				if (field == null)
					return null;

				SubQueryColumn col;

				if (!_columns.TryGetValue(field, out col))
				{
					col = new SubQueryColumn(this, field);

					Fields.Add(col);
					_columns.Add(field, col);
				}

				return col;
			}

			public override QueryField GetField(MemberInfo mi)
			{
				return EnsureField(BaseQuery.GetField(mi));
			}

			public override QueryField GetField(Expression expr)
			{
				if (expr.NodeType == ExpressionType.Parameter)
				{
					if (BaseQuery is Scalar)
						return EnsureField(BaseQuery.Fields[0]);

					if (BaseQuery is GroupBy)
						return BaseQuery;
				}

				return EnsureField(BaseQuery.GetField(expr));
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
				foreach (KeyValuePair<QueryField,SubQueryColumn> col in _columns)
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
			public  QueryField  CheckNullField
			{
				get
				{
					if (_checkNullField == null)
					{
						foreach (var f in Fields)
							if (!f.CanBeNull())
								return _checkNullField = f;

						var valueCol = new ExprColumn(BaseQuery, new SqlValue(1), null);
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
				_field = GetBaseField(lambda.Body) ?? new ExprColumn(this, lambda.Body, null);

				Fields.Add(_field);

				ParsingTracer.DecIndentLevel();
			}

			QueryField _field;

			public override QueryField GetField(MemberInfo mi)
			{
				throw new NotImplementedException();
			}

			public override MemberInfo GetMember(QueryField field)
			{
				return null;
			}

			public override QueryField GetField(Expression expr)
			{
				if (Lambda.Body.Expr is MemberExpression && expr is MemberExpression)
					if (((MemberExpression)expr).Member == ((MemberExpression)Lambda.Body.Expr).Member)
						return _field;

				return GetBaseField(expr);
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
				SqlQuery         sqlBilder,
				QuerySource      groupQuery,
				QuerySource      originalQuery,
				LambdaInfo       keySelector,
				QuerySource      elementSource,
				Type             groupingType,
				bool             isWrapped,
				ISqlExpression[] byExpressions)
				: base(sqlBilder, keySelector, groupQuery)
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

			public override QueryField GetBaseField(Expression expr)
			{
				return BaseQuery.GetBaseField(expr);
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

			GroupBy() {}

			protected override Expr CreateExpr(Dictionary<ICloneableElement,ICloneableElement> objectTree)
			{
				return new GroupBy { ElementSource = ElementSource };
			}
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

		public override string ToString()
		{
			var str = SqlQuery.ToString().Replace('\t', ' ').Replace('\n', ' ').Replace("\r", "");

			for (var len = str.Length; len != (str = str.Replace("  ", " ")).Length; len = str.Length)
			{
			}

			return str;
		}

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
		public virtual Type              ObjectType { get { return Lambda.Body.Expr.Type; }}

		public abstract QueryField GetField(Expression expr);
		public abstract QueryField GetField(MemberInfo mi);

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

			return ((QuerySource)field).GetField(members, currentMember + 1);
		}

		public virtual QueryField EnsureField(QueryField field)
		{
			foreach (var f in Fields)
				if (f == field)
					return field;

			throw new InvalidOperationException();
		}

		public virtual QueryField GetBaseField(Expression expr)
		{
			if (Sources.Length > 0)
			{
				if (expr.NodeType == ExpressionType.Parameter)
				{
					if (Sources.Length == 1)
						return BaseQuery;

					if (Sources.Length < Lambda.Parameters.Length)
						throw new InvalidOperationException();

					for (var i = 0; i < Sources.Length; i++)
						if (Lambda.Parameters[i].Expr == expr)
							return Sources[i];
				}

				foreach (var pq in Sources)
				{
					var field = pq.GetField(expr);
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

		public void Match(
			Action<Table>    tableAction,
			Action<Expr>     exprAction,
			Action<SubQuery> subQueryAction,
			Action<Scalar>   scalarAction,
			Action<GroupBy>  groupByAction)
		{
			if      (this is Table)    tableAction   (this as Table);
			else if (this is GroupBy)  groupByAction (this as GroupBy);
			else if (this is Expr)     exprAction    (this as Expr);
			else if (this is SubQuery) subQueryAction(this as SubQuery);
			else if (this is Scalar)   scalarAction  (this as Scalar);
			else
				throw new InvalidOperationException();
		}

		#endregion
	}
}
