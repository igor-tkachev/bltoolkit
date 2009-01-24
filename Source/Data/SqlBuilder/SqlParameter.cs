using System;

namespace BLToolkit.Data.SqlBuilder
{
	public class SqlParameter : ISqlExpression
	{
		public SqlParameter(string name, object value)
		{
			_name = name;
		}

		private string _name;
		public  string  Name  { get { return _name;  } set { _name = value;  } }

		private string _value;
		public  string  Value { get { return _value; } set { _value = value; } }

		#region IExpressionScannable Members

		void IExpressionScannable.ForEach(Action<ISqlExpression> action)
		{
			action(this);
		}

		#endregion

		#region IEquatable<ISqlExpression> Members

		bool IEquatable<ISqlExpression>.Equals(ISqlExpression other)
		{
			SqlParameter p = other as SqlParameter;
			return _name != null && p._name != null && _name == p._name;
		}

		#endregion
	}
}
