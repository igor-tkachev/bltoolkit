using System;

using BLToolkit.Mapping;
using BLToolkit.Reflection.Extension;

namespace BLToolkit.Data.SqlBuilder
{
	public class Field : IChild<ITableSource>, ISqlExpression
	{
		public Field()
		{
		}

		public Field(string name, string physicalName)
		{
			_name         = name;
			_physicalName = physicalName;
		}

		private string _name;
		public  string  Name { get { return _name; } set { _name = value; } }

		private string _physicalName;
		public  string  PhysicalName { get { return _physicalName ?? _name; } set { _physicalName = value; } }

		private      ITableSource        _parent;
		ITableSource IChild<ITableSource>.Parent { get { return _parent; } set { _parent = value; } }
		public       ITableSource         Table  { get { return _parent; } }

		#region IExpressionScannable Members

		void IExpressionScannable.ForEach(Action<ISqlExpression> action)
		{
			action(this);
		}

		#endregion

		#region IEquatable<ISqlExpression> Members

		bool IEquatable<ISqlExpression>.Equals(ISqlExpression other)
		{
			return (object)this == other;
		}

		#endregion
	}
}
