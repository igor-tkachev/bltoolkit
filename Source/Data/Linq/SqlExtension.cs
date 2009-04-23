using System;
using System.Data.Linq.SqlClient;

namespace BLToolkit.Data.Linq
{
	public static class SqlExtension
	{
		public static bool Like(this string matchExpression, string pattern)
		{
			return SqlMethods.Like(matchExpression, pattern);
		}

		public static bool Like(this string matchExpression, string pattern, char escapeCharacter)
		{
			return SqlMethods.Like(matchExpression, pattern, escapeCharacter);
		}
	}
}
