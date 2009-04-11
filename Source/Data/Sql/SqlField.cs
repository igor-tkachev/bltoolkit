using System;

namespace BLToolkit.Data.Sql
{
	public class SqlField : IChild<ISqlTableSource>, ISqlExpression
	{
		public SqlField()
		{
		}

		public SqlField(string name, string physicalName)
		{
			_name         = name;
			_physicalName = physicalName;
		}

		private string _name;
		public  string  Name { get { return _name; } set { _name = value; } }

		private string _physicalName;
		public  string  PhysicalName { get { return _physicalName ?? _name; } set { _physicalName = value; } }

		private         ISqlTableSource        _parent;
		ISqlTableSource IChild<ISqlTableSource>.Parent { get { return _parent; } set { _parent = value; } }
		public          ISqlTableSource         Table  { get { return _parent; } }

		public override string ToString()
		{
			return SqlBuilder.LeaveAlias(Table) + "." + Name;
		}

		#region ISqlExpressionScannable Members

		void ISqlExpressionScannable.ForEach(bool skipColumns, Action<ISqlExpression> action)
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
