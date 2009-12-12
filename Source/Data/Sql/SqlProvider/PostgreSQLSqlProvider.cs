using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;

namespace BLToolkit.Data.Sql.SqlProvider
{
	using DataProvider;

#if FW3
	using Linq;
	using S = String;
	using I = Int32;
#endif

	public class PostgreSQLSqlProvider : BasicSqlProvider
	{
		public PostgreSQLSqlProvider(DataProviderBase dataProvider) : base(dataProvider)
		{
		}

		protected override string LimitFormat  { get { return "LIMIT {0}"; } }
		protected override string OffsetFormat { get { return "OFFSET {0} "; } }

		public override ISqlExpression ConvertExpression(ISqlExpression expr)
		{
			expr = base.ConvertExpression(expr);

			if (expr is SqlBinaryExpression)
			{
				SqlBinaryExpression be = (SqlBinaryExpression)expr;

				switch (be.Operation)
				{
					case "^": return new SqlBinaryExpression(be.Expr1, "#", be.Expr2, be.Type);
					case "+": return be.Type == typeof(string)? new SqlBinaryExpression(be.Expr1, "||", be.Expr2, be.Type, be.Precedence): expr;
				}
			}
			else if (expr is SqlFunction)
			{
				SqlFunction func = (SqlFunction) expr;

				switch (func.Name)
				{
					case "Convert"   : return new SqlExpression("Cast({0} as {1})", Precedence.Primary, func.Parameters[1], func.Parameters[0]);
					case "CharIndex" :
						return func.Parameters.Length == 2?
							new SqlExpression("Position({0} in {1})", Precedence.Primary, func.Parameters[0], func.Parameters[1]):
							Add<int>(
								new SqlExpression("Position({0} in {1})", Precedence.Primary, func.Parameters[0],
									ConvertExpression(new SqlFunction("Substring",
										func.Parameters[1],
										func.Parameters[2],
										Sub<int>(ConvertExpression(new SqlFunction("Length", func.Parameters[1])), func.Parameters[2])))),
								Sub(func.Parameters[2], 1));
				}
			}
			else if (expr is SqlExpression)
			{
				SqlExpression e = (SqlExpression)expr;

				if (e.Expr.StartsWith("Extract(DOW"))
					return Inc(new SqlExpression(e.Expr.Replace("Extract(DOW", "Extract(Dow"), e.Values));

				if (e.Expr.StartsWith("Extract(Millisecond"))
					return new SqlExpression("Cast(To_Char(t.DateTimeValue, 'MS') as int)", e.Values);
			}

			return expr;
		}

		protected override void BuildValue(StringBuilder sb, object value)
		{
			if (value is bool)
				sb.Append(value);
			else
				base.BuildValue(sb, value);
		}

		protected override void BuildDataType(StringBuilder sb, SqlDataType type)
		{
			switch (type.DbType)
			{
				case SqlDbType.TinyInt    : sb.Append("SmallInt");        break;
				case SqlDbType.Money      : sb.Append("Decimal(19,4)");   break;
				case SqlDbType.SmallMoney : sb.Append("Decimal(10,4)");   break;
				default                   : base.BuildDataType(sb, type); break;
			}
		}

#if FW3
		protected override Dictionary<MemberInfo,BaseExpressor> GetExpressors() { return _members; }
		static    readonly Dictionary<MemberInfo,BaseExpressor> _members = new Dictionary<MemberInfo,BaseExpressor>
		{
			{ MI(() => Sql.Left ("",0)     ), new F<S,I,S>    ((p0,p1)       => Sql.Substring(p0, 1, p1)) },
			{ MI(() => Sql.Right("",0)     ), new F<S,I,S>    ((p0,p1)       => Sql.Substring(p0, p0.Length - p1 + 1, p1)) },
			{ MI(() => Sql.Stuff("",0,0,"")), new F<S,I,I,S,S>((p0,p1,p2,p3) => AltStuff(p0, p1, p2, p3)) },
			{ MI(() => Sql.Space(0)        ), new F<I,S>      ( p0           => Replicate(" ", p0)) },
		};
#endif
	}
}
