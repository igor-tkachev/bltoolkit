using System;
using System.Linq.Expressions;

namespace BLToolkit.Data.Linq.Parser
{
	using Data.Sql;

	public class ParseInfo
	{
		public ParseInfo(IParseContext parent, Expression expression, SqlQuery sqlQuery)
		{
			Parent     = parent;
			Expression = expression;
			SqlQuery   = sqlQuery;
		}

		public ParseInfo(ParseInfo parseInfo, Expression expression)
			: this(parseInfo.Parent, expression, parseInfo.SqlQuery)
		{
			SequenceInfo = parseInfo;
		}

		public ParseInfo(ParseInfo parseInfo, Expression expression, SqlQuery sqlQuery)
			: this(parseInfo.Parent, expression, sqlQuery)
		{
			SequenceInfo = parseInfo;
		}

		public ParseInfo     SequenceInfo        { get; set; }
		public IParseContext Parent              { get; set; }
		public Expression    Expression          { get; set; }
		public SqlQuery      SqlQuery            { get; set; }

		public bool          IsSubQuery          { get { return Parent != null; } }

		private bool _isAssociationParsed;
		public  bool  IsAssociationParsed
		{
			get { return _isAssociationParsed; }
			set
			{
				_isAssociationParsed = value;

				if (SequenceInfo != null)
					SequenceInfo.IsAssociationParsed = value;
			}
		}
	}
}
