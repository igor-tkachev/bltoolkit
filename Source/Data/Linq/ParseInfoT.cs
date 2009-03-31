using System;
using System.Linq.Expressions;

namespace BLToolkit.Data.Linq
{
	class ParseInfo<T> : ParseInfo
		where T : Expression
	{
		public new T Expr { get { return (T)base.Expr; } set { base.Expr = value; } }

		public bool Match(params Func<ParseInfo<T>,bool>[] matches)
		{
			foreach (var match in matches)
				if (match(this))
					return true;
			return false;
		}
	}
}
