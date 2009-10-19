using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace BLToolkit.Data.Sql.SqlProvider
{
	using DataProvider;

#if FW3
	using Linq;

	using C = Char;
	using S = String;
	using I = Int32;
#endif

	public class OracleSqlProvider : BasicSqlProvider
	{
		public OracleSqlProvider(DataProviderBase dataProvider) : base(dataProvider)
		{
		}

		public override bool IsCountSubQuerySupported { get { return false; } }

		protected override void BuildSelectClause(StringBuilder sb)
		{
			if (SqlQuery.From.Tables.Count == 0)
			{
				AppendIndent(sb).Append("SELECT").AppendLine();
				BuildColumns(sb);
				AppendIndent(sb).Append("FROM SYS.DUAL").AppendLine();
			}
			else
				base.BuildSelectClause(sb);
		}

		protected override bool BuildWhere()
		{
			return base.BuildWhere() || !NeedSkip && NeedTake && SqlQuery.OrderBy.IsEmpty && SqlQuery.Having. IsEmpty;
		}

		string _rowNumberAlias;

		protected override void BuildSql(StringBuilder sb)
		{
			bool buildRowNum = NeedSkip || NeedTake && (!SqlQuery.OrderBy.IsEmpty || !SqlQuery.Having.IsEmpty);

			string[] aliases = null;

			if (buildRowNum)
			{
				aliases = GetTempAliases(2, "t");

				if (_rowNumberAlias == null)
					_rowNumberAlias = GetTempAliases(1, "rn")[0];

				AppendIndent(sb).AppendFormat("SELECT {0}.*", aliases[1]).AppendLine();
				AppendIndent(sb).Append("FROM").    AppendLine();
				AppendIndent(sb).Append("(").       AppendLine();
				Indent++;

				AppendIndent(sb).AppendFormat("SELECT {0}.*, ROWNUM as {1}", aliases[0], _rowNumberAlias).AppendLine();
				AppendIndent(sb).Append("FROM").    AppendLine();
				AppendIndent(sb).Append("(").       AppendLine();
				Indent++;
			}

			base.BuildSql(sb);

			if (buildRowNum)
			{
				Indent--;
				AppendIndent(sb).Append(") ").Append(aliases[0]).AppendLine();
				Indent--;
				AppendIndent(sb).Append(") ").Append(aliases[1]).AppendLine();
				AppendIndent(sb).Append("WHERE").AppendLine();

				Indent++;

				if (NeedTake && NeedSkip)
				{
					AppendIndent(sb).AppendFormat("{0}.{1} BETWEEN ", aliases[1], _rowNumberAlias);
					BuildExpression(sb, Add(SqlQuery.Select.SkipValue, 1));
					sb.Append(" AND ");
					BuildExpression(sb, Add<int>(SqlQuery.Select.SkipValue, SqlQuery.Select.TakeValue));
				}
				else if (NeedTake)
				{
					AppendIndent(sb).AppendFormat("{0}.{1} <= ", aliases[1], _rowNumberAlias);
					BuildExpression(sb, Precedence.Comparison, SqlQuery.Select.TakeValue);
				}
				else
				{
					AppendIndent(sb).AppendFormat("{0}.{1} > ", aliases[1], _rowNumberAlias);
					BuildExpression(sb, Precedence.Comparison, SqlQuery.Select.SkipValue);
				}

				sb.AppendLine();
				Indent--;
			}
		}

		protected override void BuildWhereSearchCondition(StringBuilder sb, SqlQuery.SearchCondition condition)
		{
			if (NeedTake && !NeedSkip && SqlQuery.OrderBy.IsEmpty && SqlQuery.Having.IsEmpty)
			{
				BuildPredicate(
					sb,
					Precedence.LogicalConjunction,
					new SqlQuery.Predicate.ExprExpr(
						new SqlExpression("ROWNUM", Precedence.Primary),
						SqlQuery.Predicate.Operator.LessOrEqual,
						SqlQuery.Select.TakeValue));

				if (base.BuildWhere())
				{
					sb.Append(" AND ");
					BuildSearchCondition(sb, Precedence.LogicalConjunction, condition);
				}
			}
			else
				BuildSearchCondition(sb, Precedence.Unknown, condition);
		}

		public override ISqlExpression ConvertExpression(ISqlExpression expr)
		{
			expr = base.ConvertExpression(expr);

			if (expr is SqlBinaryExpression)
			{
				SqlBinaryExpression be = (SqlBinaryExpression)expr;

				switch (be.Operation)
				{
					case "%": return new SqlFunction("MOD",    be.Expr1, be.Expr2);
					case "&": return new SqlFunction("BITAND", be.Expr1, be.Expr2);
					case "|": // (a + b) - BITAND(a, b)
						return Sub(
							Add(be.Expr1, be.Expr2, be.Type),
							new SqlFunction("BITAND", be.Expr1, be.Expr2),
							be.Type);

					case "^": // (a + b) - BITAND(a, b) * 2
						return Sub(
							Add(be.Expr1, be.Expr2, be.Type),
							Mul(new SqlFunction("BITAND", be.Expr1, be.Expr2), 2),
							be.Type);
					case "+": return be.Type == typeof(string)? new SqlBinaryExpression(be.Expr1, "||", be.Expr2, be.Type, be.Precedence): expr;
				}
			}
			else if (expr is SqlFunction)
			{
				SqlFunction func = (SqlFunction) expr;

				switch (func.Name)
				{
					case "Coalesce"  : return new SqlFunction("Nvl",    func.Parameters);
					case "Convert"   : return new SqlExpression("Cast({0} as {1})", Precedence.Primary, func.Parameters[1], func.Parameters[0]);
					case "CharIndex" :
						return func.Parameters.Length == 2?
							new SqlFunction("InStr", func.Parameters[1], func.Parameters[0]):
							new SqlFunction("InStr", func.Parameters[1], func.Parameters[0], func.Parameters[2]);
				}
			}

			return expr;
		}

#if FW3
		protected override Dictionary<MemberInfo,BaseExpressor> GetExpressors() { return _members; }
		static    readonly Dictionary<MemberInfo,BaseExpressor> _members = new Dictionary<MemberInfo,BaseExpressor>
		{
			{ MI(() => Sql.Left ("",0)     ), new F<S,I,S>    ((p0,p1)       => Sql.Substring(p0, 1, p1)) },
			{ MI(() => Sql.Right("",0)     ), new F<S,I,S>    ((p0,p1)       => Sql.Substring(p0, p0.Length - p1 + 1, p1)) },
			{ MI(() => Sql.Stuff("",0,0,"")), new F<S,I,I,S,S>((p0,p1,p2,p3) => AltStuff(p0, p1, p2, p3)) },
			{ MI(() => Sql.Space(0)        ), new F<I,S>      ( p0           => Sql.PadRight(" ", p0, ' ')) },
		};
#endif
	}
}
