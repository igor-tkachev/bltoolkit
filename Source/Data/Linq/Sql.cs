using System;
using System.Data.Linq.SqlClient;
using System.Globalization;
using System.Reflection;

namespace BLToolkit.Data.Linq
{
	using Data.Sql;

	public static class Sql
	{
		#region Common Functions

		[CLSCompliant(false)]
		[SqlExpression("{0}", 0, ServerSideOnly = true)]
		public static T OnServer<T>(T obj)
		{
			return obj;
		}

		#endregion

		#region Convert Functions

		[CLSCompliant(false)]
		[SqlFunction("Convert", 0, 1, ServerSideOnly = true)]
		public static TTo Convert<TTo,TFrom>(TTo to, TFrom from)
		{
			return Common.ConvertTo<TTo>.From(from);
		}

		[CLSCompliant(false)]
		[SqlFunction("Convert", 1, 0)]
		public static TTo Convert<TTo,TFrom>(TFrom obj)
		{
			return Common.ConvertTo<TTo>.From(obj);
		}

		public static class ConvertTo<TTo>
		{
			[CLSCompliant(false)]
			[SqlFunction("Convert", 1, 0)]
			public static TTo From<TFrom>(TFrom obj)
			{
				return Common.ConvertTo<TTo>.From(obj);
			}
		}

		[SqlProperty("Oracle",     "Number(19)",     ServerSideOnly=true)]
		[SqlProperty(              "BigInt",         ServerSideOnly=true)] public static Int64          BigInt                            { get { return 0; } }

		[SqlProperty("MySql",      "Signed",         ServerSideOnly=true)]
		[SqlProperty(              "Int",            ServerSideOnly=true)] public static Int32          Int                               { get { return 0; } }

		[SqlProperty("MySql",      "Signed",         ServerSideOnly=true)]
		[SqlProperty(              "SmallInt",       ServerSideOnly=true)] public static Int16          SmallInt                          { get { return 0; } }

		[SqlProperty("DB2",        "SmallInt",       ServerSideOnly=true)]
		[SqlProperty("Informix",   "SmallInt",       ServerSideOnly=true)]
		[SqlProperty("Oracle",     "Number(3)",      ServerSideOnly=true)]
		[SqlProperty("DB2",        "SmallInt",       ServerSideOnly=true)]
		[SqlProperty("Firebird",   "SmallInt",       ServerSideOnly=true)]
		[SqlProperty("PostgreSQL", "SmallInt",       ServerSideOnly=true)]
		[SqlProperty("MySql",      "Unsigned",       ServerSideOnly=true)]
		[SqlProperty(              "TinyInt",        ServerSideOnly=true)] public static Byte           TinyInt                           { get { return 0; } }

		[SqlProperty(              "Decimal",        ServerSideOnly=true)] public static Decimal DefaultDecimal                           { get { return 0; } }
		[SqlFunction(                                ServerSideOnly=true)] public static Decimal        Decimal(int precision)            {       return 0;   }
		[SqlFunction(                                ServerSideOnly=true)] public static Decimal        Decimal(int precision, int scale) {       return 0;   }

		[SqlProperty("Oracle",     "Number(19,4)",   ServerSideOnly=true)]
		[SqlProperty("Firebird",   "Decimal(18,4)",  ServerSideOnly=true)]
		[SqlProperty("PostgreSQL", "Decimal(19,4)",  ServerSideOnly=true)]
		[SqlProperty("MySql",      "Decimal(19,4)",  ServerSideOnly=true)]
		[SqlProperty(              "Money",          ServerSideOnly=true)] public static Decimal        Money                             { get { return 0; } }

		[SqlProperty("Informix",   "Decimal(10,4)",  ServerSideOnly=true)]
		[SqlProperty("Oracle",     "Number(10,4)",   ServerSideOnly=true)]
		[SqlProperty("Firebird",   "Decimal(10,4)",  ServerSideOnly=true)]
		[SqlProperty("PostgreSQL", "Decimal(10,4)",  ServerSideOnly=true)]
		[SqlProperty("MySql",      "Decimal(10,4)",  ServerSideOnly=true)]
		[SqlProperty("SqlCe",      "Decimal(10,4)",  ServerSideOnly=true)]
		[SqlProperty(              "SmallMoney",     ServerSideOnly=true)] public static Decimal        SmallMoney                        { get { return 0; } }

		[SqlProperty("MySql",      "Decimal(29,10)", ServerSideOnly=true)]
		[SqlProperty(              "Float",          ServerSideOnly=true)] public static Double         Float                             { get { return 0; } }

		#endregion

		#region String Functions

		[SqlFunction]
		[SqlFunction("Access",   "Len")]
		[SqlFunction("Firebird", "Char_Length")]
		[SqlFunction("MSSql",    "Len")]
		[SqlFunction("SqlCe",    "Len")]
		[SqlFunction("Sybase",   "Len")]
		public static int Length(string str)
		{
			return (str ?? "").Length;
		}

		[SqlFunction]
		[SqlFunction  ("Access",   "Mid")]
		[SqlFunction  ("DB2",      "Substr")]
		[SqlFunction  ("Informix", "Substr")]
		[SqlFunction  ("Oracle",   "Substr")]
		[SqlFunction  ("SQLite",   "Substr")]
		[SqlExpression("Firebird", "Substring({0} from {1} for {2})")]
		public static string Substring(string str, int startIndex, int length)
		{
			return str.Substring(startIndex, length);
		}

		[SqlFunction]
		public static bool Like(string matchExpression, string pattern)
		{
			return SqlMethods.Like(matchExpression, pattern);
		}

		[SqlFunction]
		public static bool Like(string matchExpression, string pattern, char escapeCharacter)
		{
			return SqlMethods.Like(matchExpression, pattern, escapeCharacter);
		}

		[SqlFunction]
		[SqlFunction("DB2",   "Locate")]
		[SqlFunction("MySql", "Locate")]
		public static int? CharIndex(string value, string str)
		{
			if (str == null || value == null)
				return null;

			return str.IndexOf(value) + 1;
		}

		[SqlFunction]
		[SqlFunction("DB2",   "Locate")]
		[SqlFunction("MySql", "Locate")]
		public static int? CharIndex(string value, string str, int startLocation)
		{
			if (str == null || value == null)
				return null;

			return str.IndexOf(value, startLocation - 1) + 1;
		}

		[SqlFunction]
		[SqlFunction("DB2",   "Locate")]
		[SqlFunction("MySql", "Locate")]
		public static int? CharIndex(char value, string str)
		{
			if (str == null)
				return null;

			return str.IndexOf(value) + 1;
		}

		[SqlFunction]
		[SqlFunction("DB2",   "Locate")]
		[SqlFunction("MySql", "Locate")]
		public static int? CharIndex(char value, string str, int startLocation)
		{
			if (str == null)
				return null;

			return str.IndexOf(value, startLocation - 1) + 1;
		}

		[SqlFunction]
		public static string Reverse(string str)
		{
			if (string.IsNullOrEmpty(str))
				return str;

			var chars = str.ToCharArray();
			Array.Reverse(chars);
			return new string(chars);
		}

		[SqlFunction]
		[SqlFunction("SQLite", "LeftStr")]
		public static string Left(string str, int length)
		{
			return str == null || str.Length < length? null: str.Substring(1, length);
		}

		[SqlFunction]
		[SqlFunction("SQLite", "RightStr")]
		public static string Right(string str, int length)
		{
			return str == null || str.Length < length? null: str.Substring(str.Length - length);
		}

		[SqlFunction]
		public static string Stuff(string str, int startLocation, int length, string value)
		{
			return str.Remove(startLocation - 1, length).Insert(startLocation - 1, value);
		}

		[SqlFunction]
		public static string Space(int length)
		{
			return "".PadRight(length);
		}

		[SqlFunction(Name = "LPad")]
		public static string PadLeft(string str, int totalWidth, char paddingChar)
		{
			return str.PadLeft(totalWidth, paddingChar);
		}

		[SqlFunction(Name = "RPad")]
		public static string PadRight(string str, int totalWidth, char paddingChar)
		{
			return str.PadRight(totalWidth, paddingChar);
		}

		[SqlFunction]
		[SqlFunction("Sybase", "Str_Replace")]
		public static string Replace(string str, string oldValue, string newValue)
		{
			return str.Replace(oldValue, newValue);
		}

		[SqlFunction]
		[SqlFunction("Sybase", "Str_Replace")]
		public static string Replace(string str, char oldValue, char newValue)
		{
			return str.Replace(oldValue, newValue);
		}

		[SqlFunction]
		public static string Trim(string str)
		{
			return str.Trim();
		}

		[SqlFunction(Name = "LTrim")]
		public static string TrimLeft(string str)
		{
			return str.TrimStart();
		}

		[SqlFunction(Name = "RTrim")]
		public static string TrimRight(string str)
		{
			return str.TrimEnd();
		}

		[SqlFunction]
		[SqlFunction("Access",   "LCase")]
		public static string Lower(string str)
		{
			return str.ToLower();
		}

		[SqlFunction]
		[SqlFunction("Access",   "UCase")]
		public static string Upper(string str)
		{
			return str.ToUpper();
		}

		#endregion

		#region DateTime Functions

		[SqlProperty("CURRENT_TIMESTAMP")]
		[SqlProperty("Informix", "CURRENT")]
		[SqlProperty("Access",   "Now")]
		public static DateTime GetDate()
		{
			return DateTime.Now;
		}

		[SqlProperty("CURRENT_TIMESTAMP",   ServerSideOnly = true)]
		[SqlProperty("Informix", "CURRENT", ServerSideOnly = true)]
		[SqlProperty("Access",   "Now",     ServerSideOnly = true)]
		[SqlFunction("SqlCe",    "GetDate", ServerSideOnly = true)]
		[SqlFunction("Sybase",   "GetDate", ServerSideOnly = true)]
		public static DateTime CurrentTimestamp
		{
			get { throw new LinqException("The 'CurrentTimestamp' is server side only property."); }
		}

		[SqlProperty("CURRENT_TIMESTAMP")]
		[SqlProperty("Informix", "CURRENT")]
		[SqlProperty("Access",   "Now")]
		[SqlFunction("SqlCe",    "GetDate")]
		[SqlFunction("Sybase",   "GetDate")]
		public static DateTime CurrentTimestamp2
		{
			get { return DateTime.Now; }
		}

		[SqlFunction]
		public static DateTime ToDate(int year, int month, int day, int hour, int minute, int second, int millisecond)
		{
			return new DateTime(year, month, day, hour, minute, second, millisecond);
		}

		[SqlFunction]
		public static DateTime ToDate(int year, int month, int day, int hour, int minute, int second)
		{
			return new DateTime(year, month, day, hour, minute, second);
		}

		[SqlFunction]
		public static DateTime ToDate(int year, int month, int day)
		{
			return new DateTime(year, month, day);
		}

		public enum DateParts
		{
			Year        =  0,
			Quarter     =  1,
			Month       =  2,
			DayOfYear   =  3,
			Day         =  4,
			Week        =  5,
			WeekDay     =  6,
			Hour        =  7,
			Minute      =  8,
			Second      =  9,
			Millisecond = 10,
		}

		class DatePartAttribute : SqlExpressionAttribute
		{
			public DatePartAttribute(string sqlProvider, string expression, int datePartIndex, params int[] argIndices)
				: this(sqlProvider, expression, Data.Sql.Precedence.Primary, false, null, datePartIndex, argIndices)
			{
			}

			public DatePartAttribute(string sqlProvider, string expression, bool isExpression, int datePartIndex, params int[] argIndices)
				: this(sqlProvider, expression, Data.Sql.Precedence.Primary, isExpression, null, datePartIndex, argIndices)
			{
			}

			public DatePartAttribute(string sqlProvider, string expression, bool isExpression, string[] partMapping, int datePartIndex, params int[] argIndices)
				: this(sqlProvider, expression, Data.Sql.Precedence.Primary, isExpression, partMapping, datePartIndex, argIndices)
			{
			}

			public DatePartAttribute(string sqlProvider, string expression, int precedence, bool isExpression, string[] partMapping, int datePartIndex, params int[] argIndices)
				: base(sqlProvider, expression, argIndices)
			{
				_isExpression  = isExpression;
				_partMapping   = partMapping;
				_datePartIndex = datePartIndex;
				Precedence     = precedence;
			}

			readonly bool     _isExpression;
			readonly string[] _partMapping;
			readonly int      _datePartIndex;

			public override ISqlExpression GetExpression(MemberInfo member, params ISqlExpression[] args)
			{
				var part = (DateParts)((SqlValue)args[_datePartIndex]).Value;
				var pstr = _partMapping != null ? _partMapping[(int)part] : part.ToString();
				var str  = string.Format(Expression, pstr ?? part.ToString());

				return _isExpression ?
					new SqlExpression(str, Precedence, ConvertArgs(member, args)) :
					(ISqlExpression)new SqlFunction  (str, ConvertArgs(member, args));
			}
		}

		[CLSCompliant(false)]
		[SqlFunction]
		[DatePart("Oracle",     "Add{0}",                                                   false, 0, 2, 1)]
		[DatePart("DB2",        "{{1}} + {0}",                         Precedence.Additive, true,  new[] { "{0} Year",          "({0} * 3) Month",         "{0} Month",           "{0} Day",         "{0} Day",         "({0} * 7) Day",       "{0} Day",         "{0} Hour",          "{0} Minute",            "{0} Second",            "({0} * 1000) Microsecond" }, 0, 1, 2)]
		[DatePart("Informix",   "{{1}} + Interval({0}",                Precedence.Additive, true,  new[] { "{0}) Year to Year", "{0}) Month to Month * 3", "{0}) Month to Month", "{0}) Day to Day", "{0}) Day to Day", "{0}) Day to Day * 7", "{0}) Day to Day", "{0}) Hour to Hour", "{0}) Minute to Minute", "{0}) Second to Second", null                       }, 0, 1, 2)]
		[DatePart("PostgreSQL", "{{1}} + Interval '{{0}} {0}",         Precedence.Additive, true,  new[] { "Year'",             "Month' * 3",              "Month'",              "Day'",            "Day'",            "Day' * 7",            "Day'",            "Hour'",             "Minute'",               "Second'",               "Millisecond'"             }, 0, 1, 2)]
		[DatePart("MySql",      "Date_Add({{1}}, Interval {{0}} {0})",                      true,  new[] { null,                null,                      null,                  "Day",             null,              null,                  "Day",             null,                null,                    null,                    null                       }, 0, 1, 2)]
		[DatePart("SQLite",     "DateTime({{1}}, '{{0}} {0}')",                             true,  new[] { null,                null,                      null,                  "Day",             null,              null,                  "Day",             null,                null,                    null,                    null                       }, 0, 1, 2)]
		[DatePart("Access",     "DateAdd({0}, {{0}}, {{1}})",                               true,  new[] { "'yyyy'",            "'q'",                     "'m'",                 "'y'",             "'d'",             "'ww'",                "'w'",             "'h'",               "'n'",                   "'s'",                   null                       }, 0, 1, 2)]
		public static DateTime DateAdd(DateParts part, double number, DateTime date)
		{
			switch (part)
			{
				case DateParts.Year        : return date.AddYears       ((int)number);
				case DateParts.Quarter     : return date.AddMonths      ((int)number * 3);
				case DateParts.Month       : return date.AddMonths      ((int)number);
				case DateParts.DayOfYear   : return date.AddDays        (number);
				case DateParts.Day         : return date.AddDays        (number);
				case DateParts.Week        : return date.AddDays        (number * 7);
				case DateParts.WeekDay     : return date.AddDays        (number);
				case DateParts.Hour        : return date.AddHours       (number);
				case DateParts.Minute      : return date.AddMinutes     (number);
				case DateParts.Second      : return date.AddSeconds     (number);
				case DateParts.Millisecond : return date.AddMilliseconds(number);
			}

			throw new InvalidOperationException();
		}

		[CLSCompliant(false)]
		[SqlFunction]
		[DatePart("DB2",        "{0}",                               false, new[] { null,     null,  null,   null,      null,   null,   "DayOfWeek", null,   null,   null,   null   }, 0, 1)]
		[DatePart("Informix",   "{0}",                                      0, 1)]
		[DatePart("MySql",      "Extract({0} from {{0}})",           true,  0, 1)]
		[DatePart("PostgreSQL", "Extract({0} from {{0}})",           true,  new[] { null,     null,  null,   "DOY",     null,   null,   "DOW",       null,   null,   null,   null   }, 0, 1)]
		[DatePart("Firebird",   "Extract({0} from {{0}})",           true,  new[] { null,     null,  null,   "YearDay", null,   null,   null,        null,   null,   null,   null   }, 0, 1)]
		[DatePart("Oracle",     "To_Number(To_Char({{0}}, {0}))",    true,  new[] { "'YYYY'", "'Q'", "'MM'", "'DDD'",   "'DD'", "'WW'", "'D'",       "'HH'", "'MI'", "'SS'", "'FF'" }, 0, 1)]
		[DatePart("SQLite",     "Cast(StrFTime({0}, {{0}}) as int)", true,  new[] { "'%Y'",   null,  "'%m'", "'%j'",    "'%d'", "'%W'", "'%w'",      "'%H'", "'%M'", "'%S'", "'%f'" }, 0, 1)]
		[DatePart("Access",     "DatePart({0}, {{0}})",              true,  new[] { "'yyyy'", "'q'", "'m'",  "'y'",     "'d'",  "'ww'", "'w'",       "'h'",  "'n'",  "'s'",  null   }, 0, 1)]
		public static int DatePart(DateParts part, DateTime date)
		{
			switch (part)
			{
				case DateParts.Year        : return date.Year;
				case DateParts.Quarter     : return (date.Month - 1) / 3 + 1;
				case DateParts.Month       : return date.Month;
				case DateParts.DayOfYear   : return date.DayOfYear;
				case DateParts.Day         : return date.Day;
				case DateParts.Week        : return CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(date, CalendarWeekRule.FirstDay, DayOfWeek.Sunday);
				case DateParts.WeekDay     : return ((int)date.DayOfWeek + 1 + DateFirst + 6) % 7 + 1;
				case DateParts.Hour        : return date.Hour;
				case DateParts.Minute      : return date.Minute;
				case DateParts.Second      : return date.Second;
				case DateParts.Millisecond : return date.Millisecond;
			}

			throw new InvalidOperationException();
		}

		[SqlProperty("@@DATEFIRST")]
		public static int DateFirst
		{
			get { return 7; }
		}

		#endregion
	}
}
