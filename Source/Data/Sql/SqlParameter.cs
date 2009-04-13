using System;

namespace BLToolkit.Data.Sql
{
	public class SqlParameter : ISqlExpression
	{
		public SqlParameter(string name, object value)
		{
			_name  = name;
			_value = value;
		}

		private string _name;  public  string  Name  { get { return _name;  } set { _name = value;  } }
		private object _value; public  object  Value { get { return _value; } set { _value = value; } }

		#region Overrides

		public override string ToString()
		{
			return "@" + (Name ?? "parameter") + "[" + (Value ?? "NULL") + "]";
		}

		#endregion

		#region ISqlExpression Members

		public int Precedence
		{
			get { return Sql.Precedence.Primary; }
		}

		#endregion

		#region ISqlExpressionScannable Members

		void ISqlExpressionScannable.ForEach(bool skipColumns, Action<ISqlExpression> action)
		{
			action(this);
		}

		#endregion

		#region IEquatable<ISqlExpression> Members

		bool IEquatable<ISqlExpression>.Equals(ISqlExpression other)
		{
			if ((object)this == other)
				return true;

			SqlParameter p = other as SqlParameter;
			return _name != null && p._name != null && _name == p._name;
		}

		#endregion
	}
}
