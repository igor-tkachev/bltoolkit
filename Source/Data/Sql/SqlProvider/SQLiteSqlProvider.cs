using System;
using System.Collections.Generic;
using System.Reflection;

namespace BLToolkit.Data.Sql.SqlProvider
{
	using DataProvider;

#if FW3
	using Linq;

	using C = Char;
	using S = String;
	using I = Int32;
#endif

	public class SQLiteSqlProvider : BasicSqlProvider
	{
		public SQLiteSqlProvider(DataProviderBase dataProvider) : base(dataProvider)
		{
		}

		protected override string LimitFormat  { get { return "LIMIT {0}";  } }
		protected override string OffsetFormat { get { return "OFFSET {0}"; } }

		public override bool IsSkipSupported       { get { return SqlQuery.Select.TakeValue != null; } }
		public override bool IsNestedJoinSupported { get { return false; } }

		public override ISqlExpression ConvertExpression(ISqlExpression expr)
		{
			expr = base.ConvertExpression(expr);

			if (expr is SqlBinaryExpression)
			{
				SqlBinaryExpression be = (SqlBinaryExpression)expr;

				switch (be.Operation)
				{
					case "+": return be.SystemType == typeof(string)? new SqlBinaryExpression(be.SystemType, be.Expr1, "||", be.Expr2, be.Precedence): expr;
					case "^": // (a + b) - (a & b) * 2
						return Sub(
							Add(be.Expr1, be.Expr2, be.SystemType),
							Mul(new SqlBinaryExpression(be.SystemType, be.Expr1, "&", be.Expr2), 2), be.SystemType);
				}
			}
			else if (expr is SqlFunction)
			{
				SqlFunction func = (SqlFunction) expr;

				switch (func.Name)
				{
					case "Space"   : return new SqlFunction  (func.SystemType, "PadR", new SqlValue(" "), func.Parameters[0]);
					case "Convert" :
						{
							switch (Type.GetTypeCode(func.SystemType))
							{
								//case TypeCode.String   : return new SqlFunction(func.SystemType, "To_Char", func.Parameters[1]);
								case TypeCode.DateTime : return new SqlFunction(func.SystemType, "DateTime", func.Parameters[1]);
							}

							return new SqlExpression(func.SystemType, "Cast({0} as {1})", Precedence.Primary, func.Parameters[1], func.Parameters[0]);
						}
				}
			}
			else if (expr is SqlExpression)
			{
				SqlExpression e = (SqlExpression)expr;

				if (e.Expr.StartsWith("Cast(StrFTime(Quarter"))
					return Inc(Div(Dec(new SqlExpression(e.SystemType, e.Expr.Replace("Cast(StrFTime(Quarter", "Cast(StrFTime('%m'"), e.Values)), 3));

				if (e.Expr.StartsWith("Cast(StrFTime('%w'"))
					return Inc(new SqlExpression(e.SystemType, e.Expr.Replace("Cast(StrFTime('%w'", "Cast(strFTime('%w'"), e.Values));

				if (e.Expr.StartsWith("Cast(StrFTime('%f'"))
					return new SqlExpression(e.SystemType, "Cast(strFTime('%f', {0}) * 1000 as int) % 1000", Precedence.Multiplicative, e.Values);

				if (e.Expr.StartsWith("DateTime"))
				{
					if (e.Expr.EndsWith("Quarter')"))
						return new SqlExpression(e.SystemType, "DateTime({1}, '{0} Month')", Precedence.Primary, Mul(e.Values[0], 3), e.Values[1]);

					if (e.Expr.EndsWith("Week')"))
						return new SqlExpression(e.SystemType, "DateTime({1}, '{0} Day')",   Precedence.Primary, Mul(e.Values[0], 7), e.Values[1]);
				}
			}

			return expr;
		}

#if FW3
		protected override Dictionary<MemberInfo,BaseExpressor> GetExpressors() { return _members; }
		static    readonly Dictionary<MemberInfo,BaseExpressor> _members = new Dictionary<MemberInfo,BaseExpressor>
		{
			{ MI(() => Sql.Stuff   ("",0,0,"")), new F<S,I,I,S,S>((p0,p1,p2,p3) => AltStuff(p0, p1, p2, p3)) },
			{ MI(() => Sql.PadRight("",0,' ') ), new F<S,I,C,S>  ((p0,p1,p2)    => p0.Length > p1 ? p0 : p0 + Replicate(p2, p1 - p0.Length)) },
			{ MI(() => Sql.PadLeft ("",0,' ') ), new F<S,I,C,S>  ((p0,p1,p2)    => p0.Length > p1 ? p0 : Replicate(p2, p1 - p0.Length) + p0) },
		};
#endif
	}
}
