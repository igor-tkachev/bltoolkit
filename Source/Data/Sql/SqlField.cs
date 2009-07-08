using System;
using System.Collections.Generic;

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

		private string _name;         public string Name         { get { return _name;                  } set { _name         = value; } }
		private string _physicalName; public string PhysicalName { get { return _physicalName ?? _name; } set { _physicalName = value; } }

		private         ISqlTableSource        _parent;
		ISqlTableSource IChild<ISqlTableSource>.Parent { get { return _parent; } set { _parent = value; } }
		public          ISqlTableSource         Table  { get { return _parent; } }

		#region Overrides

		public override string ToString()
		{
			return "t" + Table.SourceID + "." + Name;
		}

		#endregion

		#region ISqlExpression Members

		public int Precedence
		{
			get { return Sql.Precedence.Primary; }
		}

		#endregion

		#region ISqlExpressionWalkable Members

		ISqlExpression ISqlExpressionWalkable.Walk(bool skipColumns, WalkingFunc func)
		{
			return func(this);
		}

		#endregion

		#region IEquatable<ISqlExpression> Members

		bool IEquatable<ISqlExpression>.Equals(ISqlExpression other)
		{
			return this == other;
		}

		#endregion

		#region ISqlExpression Members

		public object Clone(Dictionary<object,object> objectTree)
		{
			_parent.Clone(objectTree);
			return objectTree[this];
		}

		#endregion
	}
}
