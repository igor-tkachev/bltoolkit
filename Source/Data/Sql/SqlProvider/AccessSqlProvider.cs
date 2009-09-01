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

	public class AccessSqlProvider : BasicSqlProvider
	{
		public AccessSqlProvider(DataProviderBase dataProvider) : base(dataProvider)
		{
		}

		protected override void BuildTop(StringBuilder sb)
		{
			sb.Append(" TOP ");
			BuildExpression(sb, SqlBuilder.Select.TakeValue);
		}

		protected override bool ParenthesizeJoin()
		{
			return true;
		}

		public override ISqlPredicate ConvertPredicate(ISqlPredicate predicate)
		{
			if (predicate is SqlBuilder.Predicate.Like)
			{
				SqlBuilder.Predicate.Like l = (SqlBuilder.Predicate.Like)predicate;

				if (l.Escape != null)
				{
					if (l.Expr2 is SqlValue && l.Escape is SqlValue)
					{
						string   text = ((SqlValue) l.Expr2).Value.ToString();
						SqlValue val  = new SqlValue(ReescapeLikeText(text, (char)((SqlValue)l.Escape).Value));

						return new SqlBuilder.Predicate.Like(l.Expr1, l.IsNot, val, null);
					}

					if (l.Expr2 is SqlParameter)
					{
						SqlParameter p = (SqlParameter)l.Expr2;
						string       v = "";
						
						if (p.ValueConverter != null)
							v = p.ValueConverter(" ") as string;

						p.ValueConverter = GetLikeEscaper(v.StartsWith("%") ? "%" : "", v.EndsWith("%") ? "%" : "");

						return new SqlBuilder.Predicate.Like(l.Expr1, l.IsNot, p, null);
					}
				}
			}

			return base.ConvertPredicate(predicate);
		}

		static string ReescapeLikeText(string text, char esc)
		{
			StringBuilder sb = new StringBuilder(text.Length);

			for (int i = 0; i < text.Length; i++)
			{
				char c = text[i];

				if (c == esc)
				{
					sb.Append('[');
					sb.Append(text[++i]);
					sb.Append(']');
				}
				else if (c == '[')
					sb.Append("[[]");
				else
					sb.Append(c);
			}

			return sb.ToString();
		}

		static Converter<object,object> GetLikeEscaper(string start, string end)
		{
			return delegate(object value)
			{
				if (value == null)
					throw new SqlException("NULL cannot be used as a LIKE predicate parameter.");

				string text = value.ToString();

				if (text.IndexOfAny(new char[] { '%', '_', '[' }) < 0)
					return start + text + end;

				StringBuilder sb = new StringBuilder(start, text.Length + start.Length + end.Length);

				for (int i = 0; i < text.Length; i++)
				{
					char c = text[i];

					if (c == '%' || c == '_' || c == '[')
					{
						sb.Append('[');
						sb.Append(c);
						sb.Append(']');
					}
					else
						sb.Append(c);
				}

				return sb.ToString();
			};
		}

		public override ISqlExpression ConvertExpression(ISqlExpression expr)
		{
			expr = base.ConvertExpression(expr);

			if (expr is SqlBinaryExpression)
			{
				SqlBinaryExpression be = (SqlBinaryExpression)expr;

				switch (be.Operation[0])
				{
					case '%': return new SqlBinaryExpression(be.Expr1, "MOD", be.Expr2, be.Type, Precedence.Additive - 1);
					case '&':
					case '|':
					case '^': throw new SqlException("Operator '{0}' is not supported by the {1}.", be.Operation, GetType().Name);
				}
			}
			else if (expr is SqlFunction)
			{
				SqlFunction func = (SqlFunction) expr;

				switch (func.Name)
				{
					case "Coalesce":

						if (func.Parameters.Length > 2)
						{
							ISqlExpression[] parms = new ISqlExpression[func.Parameters.Length - 1];

							Array.Copy(func.Parameters, 1, parms, 0, parms.Length);
							return new SqlFunction(func.Name, func.Parameters[0], new SqlFunction(func.Name, parms));
						}

						SqlBuilder.SearchCondition sc = new SqlBuilder.SearchCondition();

						sc.Conditions.Add(new SqlBuilder.Condition(false, new SqlBuilder.Predicate.IsNull(func.Parameters[0], false)));

						return new SqlFunction("Iif", sc, func.Parameters[1], func.Parameters[0]);

					case "CASE"      : return ConvertCase(func.Parameters, 0);
					case "Length"    : return new SqlFunction("Len",    func.Parameters);
					case "Substring" : return new SqlFunction("Mid",    func.Parameters);
					case "Lower"     : return new SqlFunction("LCase",  func.Parameters);
					case "Upper"     : return new SqlFunction("UCase",  func.Parameters);
					case "Replicate" : return new SqlFunction("String", func.Parameters[1], func.Parameters[0]);
					case "CharIndex" :
						return func.Parameters.Length == 2?
							new SqlFunction("InStr", new SqlValue(1),    func.Parameters[1], func.Parameters[0], new SqlValue(1)):
							new SqlFunction("InStr", func.Parameters[2], func.Parameters[1], func.Parameters[0], new SqlValue(1));
				}
			}
			else if (expr is SqlExpression)
			{
				SqlExpression ex = (SqlExpression)expr;

				switch (ex.Expr)
				{
					case "CURRENT_TIMESTAMP" : return new SqlExpression("Now");
				}
			}

			return expr;
		}

		SqlFunction ConvertCase(ISqlExpression[] parameters, int start)
		{
			int len = parameters.Length - start;

			if (len < 3)
				throw new SqlException("CASE statement is not supported by the {0}.", GetType().Name);

			if (len == 3)
				return new SqlFunction("Iif", parameters[start], parameters[start + 1], parameters[start + 2]);

			return new SqlFunction("Iif", parameters[start], parameters[start + 1], ConvertCase(parameters, start + 2));
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
