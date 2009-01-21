using System;

namespace BLToolkit.Data.Sql
{
	public class Parameter : ISqlExpression
	{
		public Parameter(string name)
		{
			if (name == null) throw new ArgumentNullException("name");

			_name = name;
		}

		private string _name;
		public  string  Name { get { return _name; } set { _name = value; } }

		bool IEquatable<ISqlExpression>.Equals(ISqlExpression other)
		{
			Parameter p = other as Parameter;
			return _name == p._name;
		}
	}
}
