using System;

namespace BLToolkit.Data.Sql
{
	public class SqlFunction : ISqlExpression, ITableSource
	{
		public SqlFunction(string name, params ISqlExpression[] parameters)
		{
			if (parameters == null) throw new ArgumentNullException("values");

			foreach (ISqlExpression p in parameters)
				if (p == null) throw new ArgumentNullException("values");

			_name       = name;
			_parameters = parameters;
		}

		readonly string           _name;       public string           Name       { get { return _name;       } }
		readonly ISqlExpression[] _parameters; public ISqlExpression[] Parameters { get { return _parameters; } }

		bool IEquatable<ISqlExpression>.Equals(ISqlExpression other)
		{
			SqlFunction func = other as SqlFunction;

			if (func == null || _name != func._name || _parameters.Length != func._parameters.Length)
				return false;

			for (int i = 0; i < _parameters.Length; i++)
				if (!_parameters[i].Equals(func._parameters[i]))
					return false;

			return true;
		}

		public class All    : SqlFunction { public All   (SqlBuilder sql) : base("ALL",    sql) {} }
		public class Some   : SqlFunction { public Some  (SqlBuilder sql) : base("SOME",   sql) {} }
		public class Any    : SqlFunction { public Any   (SqlBuilder sql) : base("ANY",    sql) {} }
		public class Exists : SqlFunction { public Exists(SqlBuilder sql) : base("EXISTS", sql) {} }
	}
}
