using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace BLToolkit.Data.Linq
{
	using Mapping;
	using Sql;
	using Reflection;

	abstract class QuerySource : QueryField
	{
		public class Table : QuerySource
		{
			public Table(MappingSchema mappingSchema, SqlBuilder sqlBuilder, ParseInfo<ConstantExpression> expr)
				: base(sqlBuilder, null, expr)
			{
				ObjectType = ((IQueryable)expr.Expr.Value).ElementType;
				SqlTable   = new SqlTable(mappingSchema, ObjectType);

				sqlBuilder.From.Table(SqlTable);

				var objectMapper = mappingSchema.GetObjectMapper(ObjectType);

				foreach (var field in SqlTable.Fields)
				{
					var mapper = objectMapper[field.Value.PhysicalName];

					Fields.Add(mapper.MemberAccessor.MemberInfo, new Column(this, field.Value, mapper));
				}
			}

			public Type     ObjectType;
			public SqlTable SqlTable;
		}

		public class Expr : QuerySource
		{
			public Expr(SqlBuilder sqlBilder, QuerySource parentQuery, ParseInfo<NewExpression> expr)
				: base(sqlBilder, parentQuery, expr)
			{
				var ex = expr.Expr;

				if (ex.Members == null)
					return;

				for (var i = 0; i < ex.Members.Count; i++)
				{
					var member = ex.Members[i];

					if (member is MethodInfo)
						member = TypeHelper.GetPropertyByMethod((MethodInfo)member);

					var field = parentQuery.GetField(ex.Arguments[i]);

					if (field != null)
					{
						Fields.Add(member, field);
					}
					else
					{
						var e = new ExprColumn(this, expr.Create(ex.Arguments[i], expr.Index(ex.Arguments, New.Arguments, i)), member.Name);
						Fields.Add(member, e);
					}
				}
			}

			public Expr(SqlBuilder sqlBilder, QuerySource parentQuery, ParseInfo<MemberInitExpression> expr)
				: base(sqlBilder, parentQuery, expr)
			{
				var ex = expr.Expr;

				for (var i = 0; i < ex.Bindings.Count; i++)
				{
					var binding = ex.Bindings[i];
					var member  = binding.Member;

					if (member is MethodInfo)
						member = TypeHelper.GetPropertyByMethod((MethodInfo)member);

					if (binding is MemberAssignment)
					{
						var ma = binding as MemberAssignment;

						var piBinding    = expr.     Create(ma.Expression, expr.Index(ex.Bindings, MemberInit.Bindings, i));
						var piAssign     = piBinding.Create(ma.Expression, piBinding.ConvertExpressionTo<MemberAssignment>());
						var piExpression = piAssign. Create(ma.Expression, piAssign.Property(MemberAssignmentBind.Expression));

						var field = parentQuery.GetField(piExpression);

						Fields.Add(member, field ?? new ExprColumn(this, piExpression, member.Name));
					}
					else
						throw new InvalidOperationException();
				}
			}
		}

		public class SubQuery : QuerySource
		{
			public SubQuery(SqlBuilder subSql, QuerySource parentQuery)
				: base(new SqlBuilder(), parentQuery, null)
			{
				SqlBuilder.From.Table(SubSql = subSql);

				foreach (var field in parentQuery.Fields)
					Fields.Add(field.Key, new SubQueryColumn(this, field.Value));
			}

			public SqlBuilder SubSql;

			public Dictionary<object,ISqlExpression> Columns = new Dictionary<object,ISqlExpression>();
		}

		public class MemberAccess : QuerySource
		{
			public MemberAccess(QuerySource parentQuery, ParseInfo<ParameterExpression> parameter, ParseInfo<MemberExpression> body)
				: base(parentQuery.SqlBuilder, parentQuery, body)
			{
				Parameter  = parameter;
				Body       = body;
				//Members    = (from b in body.Expr.Bindings select b.Member).ToList();
			}

			public ParseInfo<ParameterExpression> Parameter;
			public ParseInfo<MemberExpression>    Body;
			//public List<MemberInfo>               Members;
		}

		protected QuerySource(SqlBuilder sqlBilder, QuerySource parentQuery, ParseInfo expr)
		{
			SqlBuilder  = sqlBilder;
			ParentQuery = parentQuery;
			Expression  = expr;
		}

		public SqlBuilder   SqlBuilder;
		public QuerySource  ParentQuery;
		public ParseInfo    Expression;

		public readonly Dictionary<MemberInfo,QueryField> Fields = new Dictionary<MemberInfo, QueryField>();

		public QueryField GetField(Expression expr)
		{
			switch (expr.NodeType)
			{
				case ExpressionType.Parameter:
					return this;

				case ExpressionType.MemberAccess:
					{
						var ma = (MemberExpression)expr;

						if (ma.Expression.NodeType == ExpressionType.Parameter)
						{
							QueryField fld;
							Fields.TryGetValue(ma.Member, out fld);
							return fld;
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

						var        source = this;
						QueryField field  = null;

						for (var i = 0; i < list.Count; i++)
						{
							var mi = list[i];

							source.Fields.TryGetValue(mi, out field);

							if (field == null || i + 1 == list.Count)
								 break;

							if (!(field is QuerySource))
								return null;

							source = (QuerySource)field;
						}

						return field;
					}

				default:
					return null;
			}
		}

		int[] _indexes;

		public override int[] Select<T>(ExpressionParser<T> parser)
		{
			if (_indexes == null)
			{
				_indexes = new int[Fields.Count];

				var i = 0;

				foreach (var field in Fields.Values)
					_indexes[i++] = field.Select(parser)[0];
			}

			return _indexes;
		}

		public override ISqlExpression GetExpression<T>(ExpressionParser<T> parser)
		{
			throw new InvalidOperationException();
			//throw new LinqException("Cannot convert '{0}' to SQL.", Field.GetExpression(parser));
		}

		public void Match(
			Action<Table>        tableAction,
			Action<Expr>         exprAction,
			Action<SubQuery>     subQueryAction,
			Action<MemberAccess> memberAccessAction)
		{
			if      (this is Table)        tableAction       (this as Table);
			else if (this is Expr)         exprAction        (this as Expr);
			else if (this is SubQuery)     subQueryAction    (this as SubQuery);
			else if (this is MemberAccess) memberAccessAction(this as MemberAccess);
		}
	}
}
