using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace BLToolkit.Data.Linq
{
	using Mapping;
	using Data.Sql;
	using Reflection;

	abstract class QuerySource : QueryField
	{
		#region Table

		public class Table : QuerySource
		{
			public Table(MappingSchema mappingSchema, SqlBuilder sqlBuilder, LambdaInfo lambda)
				: base(sqlBuilder, lambda)
			{
				ObjectType = TypeHelper.GetGenericType(typeof(IQueryable<>), lambda.Body.Expr.Type).GetGenericArguments()[0];
				SqlTable   = new SqlTable(mappingSchema, ObjectType);

				sqlBuilder.From.Table(SqlTable);

				var objectMapper = mappingSchema.GetObjectMapper(ObjectType);

				foreach (var field in SqlTable.Fields)
				{
					var mapper = objectMapper[field.Value.PhysicalName];
					var column = new Column(this, field.Value, mapper);

					Fields.Add(column);
					_columns.Add(mapper.MemberAccessor.MemberInfo, column);
				}

				ParsingTracer.DecIndentLevel();
			}

			public Type     ObjectType;
			public SqlTable SqlTable;

			readonly Dictionary<MemberInfo,Column> _columns = new Dictionary<MemberInfo,Column>();

			public override QueryField GetField(MemberInfo mi)
			{
				Column col;
				_columns.TryGetValue(mi, out col);
				return col;
			}

			public override QueryField GetField(Expression expr)
			{
				if (expr.NodeType == ExpressionType.MemberAccess)
				{
					var ma = (MemberExpression)expr;

					if (ma.Expression != null && ma.Expression.Type == ObjectType)
						return GetField(ma.Member);
				}

				return null;
			}

			Table() {}

			protected override QuerySource CloneInstance(Dictionary<ICloneableElement,ICloneableElement> objectTree, Predicate<ICloneableElement> doClone)
			{
				if (!doClone(this))
					return this;

				var table = new Table
				{
					ObjectType = ObjectType,
					SqlTable   = (SqlTable)SqlTable.Clone(objectTree, doClone)
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
			public Expr(SqlBuilder sqlBilder, LambdaInfo lambda, params QuerySource[] parentQueries)
				: base(sqlBilder, lambda, parentQueries)
			{
				if (lambda.Body.Expr is NewExpression)
				{
					var ex = (NewExpression)lambda.Body.Expr;

					if (ex.Members == null)
						return;

					for (var i = 0; i < ex.Members.Count; i++)
					{
						var member = ex.Members[i];

						if (member is MethodInfo)
							member = TypeHelper.GetPropertyByMethod((MethodInfo)member);

						var field = 
							GetParentField(ex.Arguments[i]) ??
							new ExprColumn(this, lambda.Body.Create(ex.Arguments[i], lambda.Body.Index(ex.Arguments, New.Arguments, i)), member.Name);

						Fields.Add(field);
						Members.Add(member, field);
					}
				}
				else if (lambda.Body.Expr is MemberInitExpression)
				{
					var ex = (MemberInitExpression)lambda.Body.Expr;

					for (var i = 0; i < ex.Bindings.Count; i++)
					{
						var binding = ex.Bindings[i];
						var member  = binding.Member;

						if (member is MethodInfo)
							member = TypeHelper.GetPropertyByMethod((MethodInfo)member);

						if (binding is MemberAssignment)
						{
							var ma = binding as MemberAssignment;

							var piBinding    = lambda.Body.Create(ma.Expression, lambda.Body.Index(ex.Bindings, MemberInit.Bindings, i));
							var piAssign     = piBinding.  Create(ma.Expression, piBinding.ConvertExpressionTo<MemberAssignment>());
							var piExpression = piAssign.   Create(ma.Expression, piAssign.Property(MemberAssignmentBind.Expression));

							var field = GetParentField(piExpression) ?? new ExprColumn(this, piExpression, member.Name);

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

			public override QueryField GetField(Expression expr)
			{
				switch (expr.NodeType)
				{
					case ExpressionType.Parameter:
						throw new InvalidOperationException();
						//return this;

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
				if (!doClone(this))
					return this;

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
			public SubQuery(SqlBuilder currentSql, SqlBuilder subSql, QuerySource parentQuery, bool addToSource)
				: base(currentSql, null, parentQuery)
			{
				ParsingTracer.WriteLine(subSql);

				SubSql = subSql;

				if (addToSource)
					SqlBuilder.From.Table(subSql);

				foreach (var field in parentQuery.Fields)
					EnsureField(field);

				ParsingTracer.DecIndentLevel();
			}

			public SubQuery(SqlBuilder currentSql, SqlBuilder subSql, QuerySource parentQuery)
				: this(currentSql, subSql, parentQuery, true)
			{
			}

			public   SqlBuilder                            SubSql;
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
				return EnsureField(ParentQueries[0].GetField(mi));
			}

			public override QueryField GetField(Expression expr)
			{
				if (expr.NodeType == ExpressionType.Parameter && ParentQueries[0] is Scalar)
					return EnsureField(ParentQueries[0].Fields[0]);

				return EnsureField(ParentQueries[0].GetField(expr));
			}

			protected override QueryField GetField(List<MemberInfo> members, int currentMember)
			{
				var field = GetField(members[currentMember]);

				if (field == null || currentMember + 1 == members.Count)
					 return field;

				if (!(field is SubQueryColumn))
					return ((QuerySource)field).GetField(members, currentMember + 1);

				field = ParentQueries[0].GetField(members, currentMember);

				return EnsureField(field);
			}

			protected SubQuery() {}

			protected virtual SubQuery CreateSubQuery(Dictionary<ICloneableElement,ICloneableElement> objectTree, Predicate<ICloneableElement> doClone)
			{
				return new SubQuery();
			}

			protected override QuerySource CloneInstance(Dictionary<ICloneableElement,ICloneableElement> objectTree, Predicate<ICloneableElement> doClone)
			{
				if (!doClone(this))
					return this;

				var sub = CreateSubQuery(objectTree, doClone);

				sub.SubSql = (SqlBuilder)SubSql.Clone(objectTree, doClone);

				foreach (var c in _columns)
					sub._columns.Add(c.Key, (SubQueryColumn)c.Value.Clone(objectTree, doClone));

				return sub;
			}
		}

		#endregion

		#region GroupJoinQuery

		public class GroupJoinQuery : SubQuery
		{
			public GroupJoinQuery(SqlBuilder currentSql, SqlBuilder subSql, QuerySource parentQuery)
				: base(currentSql, subSql, parentQuery, false)
			{
			}

			public ExprColumn Counter;

			GroupJoinQuery() {}

			protected override SubQuery CreateSubQuery(Dictionary<ICloneableElement,ICloneableElement> objectTree, Predicate<ICloneableElement> doClone)
			{
				return new GroupJoinQuery { Counter = (ExprColumn)Counter.Clone(objectTree, doClone) };
			}
		}

		#endregion

		#region Scalar

		public class Scalar : QuerySource
		{
			public Scalar(SqlBuilder sqlBilder, LambdaInfo lambda, params QuerySource[] parentQueries)
				: base(sqlBilder, lambda, parentQueries)
			{
				_field = GetParentField(lambda.Body) ?? new ExprColumn(this, lambda.Body, null);

				Fields.Add(_field);

				ParsingTracer.DecIndentLevel();
			}

			QueryField _field;

			public override QueryField GetField(MemberInfo mi)
			{
				throw new NotImplementedException();
			}

			public override QueryField GetField(Expression expr)
			{
				if (Lambda.Body.Expr is MemberExpression && expr is MemberExpression)
					if (((MemberExpression)expr).Member == ((MemberExpression)Lambda.Body.Expr).Member)
						return _field;

				return GetParentField(expr);
			}

			Scalar() {}

			protected override QuerySource CloneInstance(Dictionary<ICloneableElement,ICloneableElement> objectTree, Predicate<ICloneableElement> doClone)
			{
				if (!doClone(this))
					return this;

				return new Scalar { _field = (QueryField)_field.Clone(objectTree, doClone) };
			}
		}

		#endregion

		#region GroupBy

		public class GroupBy : Expr
		{
			public GroupBy(
				SqlBuilder       sqlBilder,
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


			public override QueryField GetParentField(Expression expr)
			{
				return ParentQueries[0].GetParentField(expr);
			}

			GroupBy() {}

			protected override Expr CreateExpr(Dictionary<ICloneableElement,ICloneableElement> objectTree)
			{
				return new GroupBy { ElementSource = ElementSource };
			}
		}

		#endregion

		#region base

		protected QuerySource(SqlBuilder sqlBuilder, LambdaInfo lambda, params QuerySource[] parentQueries)
		{
			SqlBuilder    = sqlBuilder;
			Lambda        = lambda;
			ParentQueries = parentQueries;

#if TRACE_PARSING
			ParsingTracer.WriteLine(lambda);
			ParsingTracer.WriteLine(this);

			foreach (var parent in parentQueries)
				ParsingTracer.WriteLine("parent", parent);

			foreach (var field in Fields)
				ParsingTracer.WriteLine("field ", field);

			ParsingTracer.IncIndentLevel();
#endif
		}

		public override string ToString()
		{
			var str = SqlBuilder.ToString().Replace('\t', ' ').Replace('\n', ' ');

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

				qs.SqlBuilder    = (SqlBuilder)SqlBuilder.Clone(objectTree, doClone);
				qs.Lambda        = Lambda;
				qs.ParentQueries = Array. ConvertAll(ParentQueries, q => (QuerySource)q.Clone(objectTree, doClone));
				qs.Fields        = Fields.ConvertAll(f => (QueryField)f.Clone(objectTree, doClone));

				clone = qs;
			}

			return clone;
		}

		public override QuerySource[] Sources { get { return ParentQueries; } }

		public QuerySource[]    ParentQueries;
		public SqlBuilder       SqlBuilder;
		public LambdaInfo       Lambda;
		public List<QueryField> Fields = new List<QueryField>();

		public abstract QueryField GetField(Expression expr);
		public abstract QueryField GetField(MemberInfo mi);

		protected virtual  QueryField GetField(List<MemberInfo> members, int currentMember)
		{
			var field = GetField(members[currentMember]);

			if (field == null || currentMember + 1 == members.Count)
				 return field;

			if (field is GroupByColumn)
				return ((GroupByColumn)field).GroupBySource.ParentQueries[0].GetField(members, currentMember + 1);

			return ((QuerySource)field).GetField(members, currentMember + 1);
		}

		public virtual QueryField EnsureField(QueryField field)
		{
			foreach (var f in Fields)
				if (f == field)
					return field;

			throw new InvalidOperationException();
		}

		public virtual QueryField GetParentField(Expression expr)
		{
			if (ParentQueries.Length > 0)
			{
				if (expr.NodeType == ExpressionType.Parameter)
				{
					if (ParentQueries.Length == 1)
						return ParentQueries[0];

					if (ParentQueries.Length < Lambda.Parameters.Length)
						throw new InvalidOperationException();

					for (var i = 0; i < ParentQueries.Length; i++)
						if (Lambda.Parameters[i].Expr == expr)
							return ParentQueries[i];
				}

				foreach (var pq in ParentQueries)
				{
					var field = pq.GetField(expr);
					if (field != null)
						return field;
				}
			}

			return null;
		}

		FieldIndex[] _indexes;

		public override FieldIndex[] Select<T>(ExpressionParser<T> parser)
		{
			ParsingTracer.WriteLine(this);
			ParsingTracer.IncIndentLevel();

			if (_indexes == null)
			{
				_indexes = new FieldIndex[Fields.Count];

				var i = 0;

				foreach (var field in Fields)
				{
					var idx = field.Select(parser);

					if (idx.Length != 1)
						throw new InvalidOperationException();

					_indexes[i++] = new FieldIndex { Index = idx[0].Index, Field = field };
				}
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
