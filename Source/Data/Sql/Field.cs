using System;

using BLToolkit.Mapping;
using BLToolkit.Reflection.Extension;

namespace BLToolkit.Data.Sql
{
	public class Field : IChild<Table>, ISqlExpression
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

		private Table      _parent;
		Table IChild<Table>.Parent { get { return _parent; } set { _parent = value; } }
		public       Table  Table  { get { return _parent; } }

		bool IEquatable<ISqlExpression>.Equals(ISqlExpression other)
		{
			return (object)this == other;
		}
	}
}
