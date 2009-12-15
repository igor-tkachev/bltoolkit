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

	public class DB2SqlProvider : BasicSqlProvider
	{
		public DB2SqlProvider(DataProviderBase dataProvider) : base(dataProvider)
		{
		}

		public override bool TakeAcceptsParameter { get { return SqlQuery.Select.SkipValue != null; } }

		protected override void BuildSql(StringBuilder sb)
		{
			AlternativeBuildSql(sb, false, base.BuildSql);
		}

		protected override void BuildSelectClause(StringBuilder sb)
		{
			if (SqlQuery.From.Tables.Count == 0)
			{
				AppendIndent(sb).Append("SELECT").AppendLine();
				BuildColumns(sb);
				AppendIndent(sb).Append("FROM SYSIBM.SYSDUMMY1 FETCH FIRST 1 ROW ONLY").AppendLine();
			}
			else
				base.BuildSelectClause(sb);
		}

		protected override string LimitFormat
		{
			get { return SqlQuery.Select.SkipValue == null ? "FETCH FIRST {0} ROWS ONLY" : null; }
		}

		public override ISqlExpression ConvertExpression(ISqlExpression expr)
		{
			expr = base.ConvertExpression(expr);

			if (expr is SqlBinaryExpression)
			{
				SqlBinaryExpression be = (SqlBinaryExpression)expr;

				switch (be.Operation)
				{
					case "%": return new SqlFunction(be.SystemType, "Mod",    be.Expr1, be.Expr2);
					case "&": return new SqlFunction(be.SystemType, "BitAnd", be.Expr1, be.Expr2);
					case "|": return new SqlFunction(be.SystemType, "BitOr",  be.Expr1, be.Expr2);
					case "^": return new SqlFunction(be.SystemType, "BitXor", be.Expr1, be.Expr2);
					case "+": return be.SystemType == typeof(string)? new SqlBinaryExpression(be.SystemType, be.Expr1, "||", be.Expr2, be.Precedence): expr;
				}
			}
			else if (expr is SqlFunction)
			{
				SqlFunction func = (SqlFunction) expr;

				switch (func.Name)
				{
					case "Convert"    :
						if (func.Parameters[0] is SqlDataType)
						{
							SqlDataType type = (SqlDataType)func.Parameters[0];

							if (type.Length > 0)
								return new SqlFunction(func.SystemType, type.DbType.ToString(), func.Parameters[1], new SqlValue(type.Length));

							if (type.Precision > 0)
								return new SqlFunction(func.SystemType, type.DbType.ToString(), func.Parameters[1], new SqlValue(type.Precision), new SqlValue(type.Scale));

							return new SqlFunction(func.SystemType, type.DbType.ToString(), func.Parameters[1]);
						}

						if (func.Parameters[0] is SqlFunction)
						{
							SqlFunction f = (SqlFunction)func.Parameters[0];

							return f.Parameters.Length == 1 ?
								new SqlFunction(func.SystemType, f.Name, func.Parameters[1], f.Parameters[0]):
								new SqlFunction(func.SystemType, f.Name, func.Parameters[1], f.Parameters[0], f.Parameters[1]);
						}

						{
							SqlExpression e = (SqlExpression)func.Parameters[0];
							return new SqlFunction(func.SystemType, e.Expr, func.Parameters[1]);
						}

					case "TinyInt"    : return new SqlFunction(func.SystemType, "SmallInt", func.Parameters);
					case "Money"      : return new SqlFunction(func.SystemType, "Decimal",  func.Parameters[0], new SqlValue(19), new SqlValue(4));
					case "SmallMoney" : return new SqlFunction(func.SystemType, "Decimal",  func.Parameters[0], new SqlValue(10), new SqlValue(4));
					case "Millisecond": return Div(new SqlFunction(func.SystemType, "Microsecond", func.Parameters), 1000);
					case "DateTime"   : return new SqlFunction(func.SystemType, "TimeStamp", func.Parameters);
					case "VarChar"    : return new SqlFunction(typeof(string), "RTrim", new SqlFunction(typeof(string), "Char", func.Parameters[0]));
				}
			}

			return expr;
		}

#if FW3
		// DB2
		//
		[SqlFunction]
		static string Varchar(object obj, int size)
		{
			return obj.ToString();
		}

		protected override Dictionary<MemberInfo,BaseExpressor> GetExpressors() { return _members; }
		static    readonly Dictionary<MemberInfo,BaseExpressor> _members = new Dictionary<MemberInfo,BaseExpressor>
		{
			{ MI(() => Sql.Space   (0)        ), new F<I,S>      ( p0           => Varchar(Replicate(" ", p0), 1000)) },
			{ MI(() => Sql.Stuff   ("",0,0,"")), new F<S,I,I,S,S>((p0,p1,p2,p3) => AltStuff(p0, p1, p2, p3)) },
			{ MI(() => Sql.PadRight("",0,' ') ), new F<S,I,C,S>  ((p0,p1,p2)    => p0.Length > p1 ? p0 : p0 + VarChar(Replicate(p2, p1 - p0.Length), 1000)) },
			{ MI(() => Sql.PadLeft ("",0,' ') ), new F<S,I,C,S>  ((p0,p1,p2)    => p0.Length > p1 ? p0 : VarChar(Replicate(p2, p1 - p0.Length), 1000) + p0) },
		};
#endif
	}
}
