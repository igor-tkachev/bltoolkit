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

	using C = Char;
	using S = String;
	using I = Int32;
#endif

	public class AccessSqlProvider : BasicSqlProvider
	{
		public AccessSqlProvider(DataProviderBase dataProvider) : base(dataProvider)
		{
		}

		public override bool IsSkipSupported          { get { return SqlQuery.Select.TakeValue != null; } }
		public override bool TakeAcceptsParameter     { get { return false; } }
		public override bool IsCountSubQuerySupported { get { return false; } }
		public override bool IsNestedJoinSupported    { get { return false; } }

		#region Skip / Take Support

		protected override string FirstFormat { get { return "TOP {0}"; } }

		protected override void BuildSql(StringBuilder sb)
		{
			if (NeedSkip)
				AlternativeBuildSql2(sb, base.BuildSql);
			else
				base.BuildSql(sb);
		}

		protected override IEnumerable<SqlQuery.Column> GetSelectedColumns()
		{
			if (NeedSkip && !SqlQuery.OrderBy.IsEmpty)
				return AlternativeGetSelectedColumns(base.GetSelectedColumns);
			return base.GetSelectedColumns();
		}

		protected override void BuildSkipFirst(StringBuilder sb)
		{
			if (NeedSkip)
			{
				if (!NeedTake)
				{
					sb.AppendFormat(" TOP {0}", int.MaxValue);
				}
				else if (!SqlQuery.OrderBy.IsEmpty)
				{
					sb.Append(" TOP ");
					BuildExpression(sb, Add<int>(SqlQuery.Select.SkipValue, SqlQuery.Select.TakeValue));
				}
			}
			else
				base.BuildSkipFirst(sb);
		}

		#endregion

		protected override bool ParenthesizeJoin()
		{
			return true;
		}

		public override ISqlPredicate ConvertPredicate(ISqlPredicate predicate)
		{
			if (predicate is SqlQuery.Predicate.Like)
			{
				SqlQuery.Predicate.Like l = (SqlQuery.Predicate.Like)predicate;

				if (l.Escape != null)
				{
					if (l.Expr2 is SqlValue && l.Escape is SqlValue)
					{
						string   text = ((SqlValue) l.Expr2).Value.ToString();
						SqlValue val  = new SqlValue(ReescapeLikeText(text, (char)((SqlValue)l.Escape).Value));

						return new SqlQuery.Predicate.Like(l.Expr1, l.IsNot, val, null);
					}

					if (l.Expr2 is SqlParameter)
					{
						SqlParameter p = (SqlParameter)l.Expr2;
						string       v = "";
						
						if (p.ValueConverter != null)
							v = p.ValueConverter(" ") as string;

						p.ValueConverter = GetLikeEscaper(v.StartsWith("%") ? "%" : "", v.EndsWith("%") ? "%" : "");

						return new SqlQuery.Predicate.Like(l.Expr1, l.IsNot, p, null);
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
					case '%': return new SqlBinaryExpression(be.SystemType, be.Expr1, "MOD", be.Expr2, Precedence.Additive - 1);
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
							return new SqlFunction(func.SystemType, func.Name, func.Parameters[0], new SqlFunction(func.SystemType, func.Name, parms));
						}

						SqlQuery.SearchCondition sc = new SqlQuery.SearchCondition();

						sc.Conditions.Add(new SqlQuery.Condition(false, new SqlQuery.Predicate.IsNull(func.Parameters[0], false)));

						return new SqlFunction(func.SystemType, "Iif", sc, func.Parameters[1], func.Parameters[0]);

					case "CASE"      : return ConvertCase(func.SystemType, func.Parameters, 0);
					case "CharIndex" :
						return func.Parameters.Length == 2?
							new SqlFunction(func.SystemType, "InStr", new SqlValue(1),    func.Parameters[1], func.Parameters[0], new SqlValue(1)):
							new SqlFunction(func.SystemType, "InStr", func.Parameters[2], func.Parameters[1], func.Parameters[0], new SqlValue(1));

					case "Convert"   : return func.Parameters[1];

						/*
					case "Convert"   :
						{
							string name = null;

							switch (((SqlDataType)func.Parameters[0]).DbType)
							{
								case SqlDbType.BigInt           : name = "CLng"; break;
								case SqlDbType.TinyInt          : name = "CByte"; break;
								case SqlDbType.Int              :
								case SqlDbType.SmallInt         : name = "CInt"; break;
								case SqlDbType.Bit              : name = "CBool"; break;
								case SqlDbType.Char             :
								case SqlDbType.Text             :
								case SqlDbType.VarChar          :
								case SqlDbType.NChar            :
								case SqlDbType.NText            :
								case SqlDbType.NVarChar         : name = "CStr"; break;
								case SqlDbType.DateTime         :
								case SqlDbType.Date             :
								case SqlDbType.Time             :
								case SqlDbType.DateTime2        :
								case SqlDbType.SmallDateTime    :
								case SqlDbType.DateTimeOffset   : name = "CDate"; break;
								case SqlDbType.Decimal          : name = "CDec"; break;
								case SqlDbType.Float            : name = "CDbl"; break;
								case SqlDbType.Money            :
								case SqlDbType.SmallMoney       : name = "CCur"; break;
								case SqlDbType.Real             : name = "CSng"; break;
								case SqlDbType.Image            :
								case SqlDbType.Binary           :
								case SqlDbType.UniqueIdentifier :
								case SqlDbType.Timestamp        :
								case SqlDbType.VarBinary        :
								case SqlDbType.Variant          :
								case SqlDbType.Xml              :
								case SqlDbType.Udt              :
								case SqlDbType.Structured       : name = "CVar"; break;
							}

							return new SqlFunction(name, func.Parameters[1]);
						}
						*/
				}
			}

			return expr;
		}

		SqlFunction ConvertCase(Type systemType, ISqlExpression[] parameters, int start)
		{
			int len = parameters.Length - start;

			if (len < 3)
				throw new SqlException("CASE statement is not supported by the {0}.", GetType().Name);

			if (len == 3)
				return new SqlFunction(systemType, "Iif", parameters[start], parameters[start + 1], parameters[start + 2]);

			return new SqlFunction(systemType, "Iif", parameters[start], parameters[start + 1], ConvertCase(systemType, parameters, start + 2));
		}

		protected override void BuildValue(StringBuilder sb, object value)
		{
			if (value is bool)
				sb.Append(value);
			else
				base.BuildValue(sb, value);
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
