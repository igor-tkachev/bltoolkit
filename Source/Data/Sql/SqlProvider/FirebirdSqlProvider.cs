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

	public class FirebirdSqlProvider : BasicSqlProvider
	{
		public FirebirdSqlProvider(DataProviderBase dataProvider) : base(dataProvider)
		{
		}

		protected override void BuildSelectClause(StringBuilder sb)
		{
			if (SqlBuilder.From.Tables.Count == 0)
			{
				AppendIndent(sb);
				sb.Append("SELECT").AppendLine();
				BuildColumns(sb);
				AppendIndent(sb);
				sb.Append("FROM rdb$database").AppendLine();
			}
			else
				base.BuildSelectClause(sb);
		}

		protected override bool   SkipFirst   { get { return false;       } }
		protected override string SkipFormat  { get { return "SKIP {0}";  } }
		protected override string FirstFormat { get { return "FIRST {0}"; } }

		public override ISqlExpression ConvertExpression(ISqlExpression expr)
		{
			expr = base.ConvertExpression(expr);

			if (expr is SqlBinaryExpression)
			{
				SqlBinaryExpression be = (SqlBinaryExpression)expr;

				switch (be.Operation)
				{
					case "%": return new SqlFunction("Mod",     be.Expr1, be.Expr2);
					case "&": return new SqlFunction("Bin_And", be.Expr1, be.Expr2);
					case "|": return new SqlFunction("Bin_Or",  be.Expr1, be.Expr2);
					case "^": return new SqlFunction("Bin_Xor", be.Expr1, be.Expr2);
					case "+": return be.Type == typeof(string)? new SqlBinaryExpression(be.Expr1, "||", be.Expr2, be.Type, be.Precedence): expr;
				}
			}
			else if (expr is SqlFunction)
			{
				SqlFunction func = (SqlFunction)expr;

				switch (func.Name)
				{
					case "Substring" : return new SqlExpression("Substring({0} from {1} for {2})", Precedence.Primary, func.Parameters);
					case "Convert"   : return new SqlExpression("Cast({0} as {1})", Precedence.Primary, func.Parameters[1], func.Parameters[0]);
				}
			}

			return expr;
		}

#if FW3
		protected override Dictionary<MemberInfo,BaseExpressor> GetExpressors() { return _members; }
		static    readonly Dictionary<MemberInfo,BaseExpressor> _members = new Dictionary<MemberInfo,BaseExpressor>
		{
			{ MI<S>(_ => Sql.Space(0         )), new F<I,S>      ( p0           => Sql.PadRight(" ", p0, ' ')) },
			{ MI<S>(s => Sql.Stuff(s, 0, 0, s)), new F<S,I,I,S,S>((p0,p1,p2,p3) => AltStuff(p0, p1, p2, p3)) },
		};
#endif
	}
}
