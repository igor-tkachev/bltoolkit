using System;
using System.Data.Linq.SqlClient;

namespace BLToolkit.Data.Linq
{
	public static class Sql
	{
		[SqlFunction]
		public static int Length(string str)
		{
			return str.Length;
		}

		[SqlFunction]
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
		public static int? CharIndex(string value, string str)
		{
			if (str == null || value == null)
				return null;

			return str.IndexOf(value) + 1;
		}

		[SqlFunction]
		public static int? CharIndex(string value, string str, int startLocation)
		{
			if (str == null || value == null)
				return null;

			return str.IndexOf(value, startLocation - 1) + 1;
		}

		[SqlFunction]
		public static int? CharIndex(char value, string str)
		{
			if (str == null)
				return null;

			return str.IndexOf(value) + 1;
		}

		[SqlFunction]
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
		public static string Left(string str, int length)
		{
			return str == null || str.Length < length? null: str.Substring(1, length);
		}

		[SqlFunction]
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
		public static string Replace(string str, string oldValue, string newValue)
		{
			return str.Replace(oldValue, newValue);
		}

		[SqlFunction]
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
		public static string Lower(string str)
		{
			return str.ToLower();
		}

		[SqlFunction]
		public static string Upper(string str)
		{
			return str.ToUpper();
		}
	}
}
