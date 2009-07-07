using System;
using System.Text;
using System.Collections.Generic;

namespace BLToolkit.Data.Sql
{
	public class SqlFunction : ISqlExpression, ISqlTableSource
	{
		public SqlFunction(string name, params ISqlExpression[] parameters)
			: this(name, Sql.Precedence.Primary, parameters)
		{
		}

		public SqlFunction(string name, int precedence, params ISqlExpression[] parameters)
		{
			_sourceID = ++SqlBuilder.SourceIDCounter;

			if (parameters == null) throw new ArgumentNullException("parameters");

			foreach (ISqlExpression p in parameters)
				if (p == null) throw new ArgumentNullException("parameters");

			_name       = name;
			_precedence = precedence;
			_parameters = parameters;
		}

		readonly string           _name;       public string           Name       { get { return _name;       } }
		readonly int              _precedence; public int              Precedence { get { return _precedence; } }
		readonly ISqlExpression[] _parameters; public ISqlExpression[] Parameters { get { return _parameters; } }

		#region Overrides

		public override string ToString()
		{
			StringBuilder sb = new StringBuilder(Name);

			sb.Append("(");

			foreach (ISqlExpression p in Parameters)
				sb.Append(p.ToString());

			sb.Append("(");

			return sb.ToString();
		}

		#endregion

		#region ISqlExpressionWalkable Members

		ISqlExpression ISqlExpressionWalkable.Walk(bool skipColumns, WalkingFunc action)
		{
			for (int i = 0; i < _parameters.Length; i++)
				_parameters[i] = _parameters[i].Walk(skipColumns, action);

			return action(this);
		}

		#endregion

		#region IEquatable<ISqlExpression> Members

		bool IEquatable<ISqlExpression>.Equals(ISqlExpression other)
		{
			if (this == other)
				return true;

			SqlFunction func = other as SqlFunction;

			if (func == null || _name != func._name || _parameters.Length != func._parameters.Length)
				return false;

			for (int i = 0; i < _parameters.Length; i++)
				if (!_parameters[i].Equals(func._parameters[i]))
					return false;

			return true;
		}

		#endregion

		#region ISqlTableSource Members

		private int _sourceID;
		public  int  SourceID { get { return _sourceID; } }

		#endregion

		#region ISqlExpression Members

		public object Clone(Dictionary<object,object> objectTree)
		{
			object clone;

			if (!objectTree.TryGetValue(this, out clone))
				objectTree.Add(this, clone = new SqlFunction(
					_name,
					_precedence,
					Array.ConvertAll<ISqlExpression,ISqlExpression>(_parameters, delegate(ISqlExpression e) { return (ISqlExpression)e.Clone(objectTree); })));

			return clone;
		}

		#endregion

		public class All    : SqlFunction { public All   (SqlBuilder subQuery) : base("ALL",    Sql.Precedence.Comparison, subQuery) {} }
		public class Some   : SqlFunction { public Some  (SqlBuilder subQuery) : base("SOME",   Sql.Precedence.Comparison, subQuery) {} }
		public class Any    : SqlFunction { public Any   (SqlBuilder subQuery) : base("ANY",    Sql.Precedence.Comparison, subQuery) {} }
		public class Exists : SqlFunction { public Exists(SqlBuilder subQuery) : base("EXISTS", Sql.Precedence.Comparison, subQuery) {} }
	}
}
