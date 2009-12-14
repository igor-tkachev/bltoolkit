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

	public class SybaseSqlProvider : BasicSqlProvider
	{
		public SybaseSqlProvider(DataProviderBase dataProvider) : base(dataProvider)
		{
		}

		protected override string FirstFormat              { get { return "TOP {0}"; } }

		public    override bool   IsSkipSupported          { get { return false;     } }
		public    override bool   TakeAcceptsParameter     { get { return false;     } }
		public    override bool   IsSubQueryTakeSupported  { get { return false;     } }
		public    override bool   IsCountSubQuerySupported { get { return false;     } }

		public override ISqlExpression ConvertExpression(ISqlExpression expr)
		{
			expr = base.ConvertExpression(expr);

			if (expr is SqlFunction)
			{
				SqlFunction func = (SqlFunction) expr;

				switch (func.Name)
				{
					case "CharIndex" :
						if (func.Parameters.Length == 3)
							return Add<int>(
								ConvertExpression(new SqlFunction(func.SystemType, "CharIndex",
									func.Parameters[0],
									ConvertExpression(new SqlFunction(typeof(string), "Substring",
										func.Parameters[1],
										func.Parameters[2], new SqlFunction(typeof(int), "Len", func.Parameters[1]))))),
								Sub(func.Parameters[2], 1));
						break;

					case "Stuff"     :
						if (func.Parameters[3] is SqlValue)
						{
							SqlValue value = (SqlValue)func.Parameters[3];

							if (value.Value is string && string.IsNullOrEmpty((string)value.Value))
								return new SqlFunction(
									func.SystemType,
									func.Name,
									func.Precedence,
									func.Parameters[0],
									func.Parameters[1],
									func.Parameters[1],
									new SqlValue(null));
						}

						break;
				}
			}

			return expr;
		}

		bool _isSelect;
		bool _skipAliases;

		SybaseSqlProvider(DataProviderBase dataProvider, bool skipAliases) : base(dataProvider)
		{
			_skipAliases = skipAliases;
		}

		protected override void BuildSelectClause(System.Text.StringBuilder sb)
		{
			_isSelect = true;
			base.BuildSelectClause(sb);
			_isSelect = false;
		}

		protected override void BuildColumn(System.Text.StringBuilder sb, SqlQuery.Column col, ref bool addAlias)
		{
			base.BuildColumn(sb, col, ref addAlias);
			if (_skipAliases) addAlias = false;
		}

		protected override ISqlProvider CreateSqlProvider()
		{
			return new SybaseSqlProvider(DataProvider, _isSelect);
		}

#if FW3
		protected override Dictionary<MemberInfo,BaseExpressor> GetExpressors() { return _members; }
		static    readonly Dictionary<MemberInfo,BaseExpressor> _members = new Dictionary<MemberInfo,BaseExpressor>
		{
			{ MI(() => Sql.PadRight("",0,' ')), new F<S,I,C,S>((p0,p1,p2) => p0.Length > p1 ? p0 : p0 + Replicate(p2, p1 - p0.Length)) },
			{ MI(() => Sql.PadLeft ("",0,' ')), new F<S,I,C,S>((p0,p1,p2) => p0.Length > p1 ? p0 : Replicate(p2, p1 - p0.Length) + p0) },
			{ MI(() => Sql.Trim    ("")      ), new F<S,S>    ( p0        => Sql.TrimLeft(Sql.TrimRight(p0))) },
		};
#endif
	}
}
