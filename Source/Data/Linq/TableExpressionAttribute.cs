using System;

namespace BLToolkit.Data.Linq
{
	[SerializableAttribute]
	[AttributeUsageAttribute(AttributeTargets.Property | AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
	public class TableExpressionAttribute : TableFunctionAttribute
	{
		public TableExpressionAttribute(string expression)
			: base(expression)
		{
		}

		public TableExpressionAttribute(string sqlProvider, string expression)
			: base(sqlProvider, expression)
		{
		}

		protected new string Name
		{
			get { return base.Name; }
		}

		public string Expression
		{
			get { return base.Name;  }
			set { base.Name = value; }
		}
	}
}
