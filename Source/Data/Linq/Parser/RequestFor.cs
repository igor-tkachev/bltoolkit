using System;

namespace BLToolkit.Data.Linq.Parser
{
	public enum RequestFor
	{
		Association,

		/// <summary>
		/// Checks the sequence if the expression is a field.
		/// </summary>
		Field,

		/// <summary>
		/// Checks the sequence if the expression is a table, association, new {}, or new MyClass {}.
		/// </summary>
		Object,

		/// <summary>
		/// Checks the sequence if the expression is a not a field, but scalar value.
		/// </summary>
		Expression,

		/// <summary>
		/// Checks the context if it's a subquery.
		/// </summary>
		SubQuery,

		/// <summary>
		/// Checks the sequence if the expression can be parsed.
		/// </summary>
		//CanBeParsed,

		/// <summary>
		/// Checks the context if it's a root of the expression.
		/// </summary>
		Root,
	}
}
