using System;
using System.Data.Linq.SqlClient;
using System.Reflection;
using BLToolkit.Data.Sql;

namespace BLToolkit.Data.Linq
{
	public static class Sql
	{
		#region String Finctions

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

		[SqlFunction]
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

			public DatePartAttribute(string sqlProvider, string expression, string[] partMapping, int datePartIndex, params int[] argIndices)
				: this(sqlProvider, expression, Data.Sql.Precedence.Primary, false, partMapping, datePartIndex, argIndices)
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

			public DatePartAttribute(string sqlProvider, string expression, int precedence, bool isExpression, int datePartIndex, params int[] argIndices)
				: this(sqlProvider, expression, precedence, isExpression, null, datePartIndex, argIndices)
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
				var str  = string.Format(Expression, _partMapping != null ? _partMapping[(int)part] : part.ToString());

				return _isExpression ?
					new SqlExpression(str, Precedence, ConvertArgs(args)) :
					(ISqlExpression)new SqlFunction  (str, ConvertArgs(args));
			}
		}

		[CLSCompliant(false)]
		[SqlFunction]
		[DatePart("DB2", "{{0}} + {{1}} {0}s", Precedence.Additive, true, 0, 2, 1)]
		public static DateTime DateAdd(DateParts part, int number, DateTime date)
		{
			switch (part)
			{
				case DateParts.Year        : return date.AddYears       (number);
				case DateParts.Quarter     : return date.AddMonths      (number * 3);
				case DateParts.Month       : return date.AddMonths      (number);
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
		[DatePart("DB2",        "{0}", 0, 1)]
		[DatePart("Informix",   "{0}", 0, 1)]
		[DatePart("Firebird",   "Extract({0} from {{0}})", true, 0, 1)]
		[DatePart("PostgreSQL", "Extract({0} from {{0}})", true, 0, 1)]
		[DatePart("MySql",      "Extract({0} from {{0}})", true, 0, 1)]
		[DatePart("Access",     "DatePart({0}, {{0}})",    true, new[] { "'yyyy'", "'q'", "'m'", "'y'", "'d'", "'ww'", "'w'", "'h'", "'n'", "'s'", null }, 0, 1)]
		public static int DatePart(DateParts part, DateTime date)
		{
			switch (part)
			{
				case DateParts.Year        : return date.Year;
				case DateParts.Quarter     : return (date.Month - 1) / 3 + 1;
				case DateParts.Month       : return date.Month;
				case DateParts.DayOfYear   : return date.DayOfYear;
				case DateParts.Day         : return date.Day;
				case DateParts.Week        : return date.DayOfYear / 7;
				case DateParts.WeekDay     : return date.DayOfYear % 7;
				case DateParts.Hour        : return date.Hour;
				case DateParts.Minute      : return date.Minute;
				case DateParts.Second      : return date.Second;
				case DateParts.Millisecond : return date.Millisecond;
			}

			throw new InvalidOperationException();
		}

		#endregion
	}
}
