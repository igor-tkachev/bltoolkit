using System;
using System.Text;

namespace BLToolkit.Data.Sql.SqlProvider
{
	using DataProvider;

	public class AccessSqlProvider : BasicSqlProvider
	{
		public AccessSqlProvider(DataProviderBase dataProvider) : base(dataProvider)
		{
		}

		public override ISqlExpression ConvertExpression(ISqlExpression expr)
		{
			if (expr is SqlBinaryExpression)
			{
				SqlBinaryExpression be = (SqlBinaryExpression)expr;

				switch (be.Operation[0])
				{
					case '%': return new SqlBinaryExpression(be.Expr1, "MOD", be.Expr2, Precedence.Additive - 1);
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
					case "COALESCE":

						if (func.Parameters.Length > 2)
						{
							ISqlExpression[] parms = new ISqlExpression[func.Parameters.Length - 1];

							Array.Copy(func.Parameters, 1, parms, 0, parms.Length);
							return new SqlFunction(func.Name, func.Parameters[0], new SqlFunction(func.Name, parms));
						}

						SqlBuilder.SearchCondition sc = new SqlBuilder.SearchCondition();

						sc.Conditions.Add(new SqlBuilder.Condition(false, new SqlBuilder.Predicate.IsNull(func.Parameters[0], false)));

						return new SqlFunction("IIF", sc, func.Parameters[1], func.Parameters[0]);

					case "CASE":

						if (func.Parameters.Length == 3)
							return new SqlFunction("IIF", func.Parameters[0], func.Parameters[1], func.Parameters[2]);

						throw new SqlException("CASE statement is not supported by the {0}.", GetType().Name);

					case "CHARACTER_LENGTH": return new SqlFunction("LEN", func.Parameters);
					case "IndexOf":
						return new SqlBinaryExpression(
							func.Parameters.Length == 2?
								new SqlFunction("INSTR", new SqlValue(1),    func.Parameters[0], func.Parameters[1], new SqlValue(1)):
								new SqlFunction("INSTR",
									new SqlBinaryExpression(func.Parameters[2], "+", new SqlValue(1), Precedence.Additive),
									func.Parameters[0],
									func.Parameters[1],
									new SqlValue(1)),
							"-",
							new SqlValue(1),
							Precedence.Subtraction);
				}
			}

			return base.ConvertExpression(expr);
		}

		public override ISqlPredicate ConvertPredicate(ISqlPredicate predicate)
		{
			if (predicate is SqlBuilder.Predicate.Like)
			{
				var l = (SqlBuilder.Predicate.Like)predicate;

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
			var sb = new StringBuilder(text.Length);

			for (var i = 0; i < text.Length; i++)
			{
				var c = text[i];

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
			return value =>
			{
				if (value == null)
					throw new SqlException("NULL cannot be used as a LIKE predicate parameter.");

				string text = value.ToString();

				if (text.IndexOfAny(new[] { '%', '_', '[' }) < 0)
					return start + text + end;

				StringBuilder sb = new StringBuilder(start, text.Length + start.Length + end.Length);

				for (var i = 0; i < text.Length; i++)
				{
					var c = text[i];

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
	}
}
