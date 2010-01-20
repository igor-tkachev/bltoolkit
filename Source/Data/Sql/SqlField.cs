using System;
using System.Collections.Generic;
using System.Text;

namespace BLToolkit.Data.Sql
{
	public class SqlField : IChild<ISqlTableSource>, ISqlExpression
	{
		public SqlField()
		{
		}

		public SqlField(SqlField field)
			: this(field.SystemType, field.Name, field.PhysicalName, field.Nullable, field.PrimaryKeyOrder)
		{
		}

		public SqlField(Type systemType, string name, string physicalName, bool nullable, int pkOrder)
		{
			_systemType   = systemType;
			_alias        = name.Replace('.', '_');
			_name         = name;
			_physicalName = physicalName;
			_nullable     = nullable;
			_pkOrder      = pkOrder;
		}

		private Type   _systemType;   public Type   SystemType      { get { return _systemType;            } set { _systemType   = value; } }
		private string _alias;        public string Alias           { get { return _alias;                 } set { _alias        = value; } }
		private string _name;         public string Name            { get { return _name;                  } set { _name         = value; } }
		private string _physicalName; public string PhysicalName    { get { return _physicalName ?? _name; } set { _physicalName = value; } }
		private bool   _nullable;     public bool   Nullable        { get { return _nullable;              } set { _nullable     = value; } }
		private int    _pkOrder;      public int    PrimaryKeyOrder { get { return _pkOrder;               } set { _pkOrder      = value; } }

		public bool IsPrimaryKey { get { return _pkOrder != int.MinValue; } }

		private         ISqlTableSource        _parent;
		ISqlTableSource IChild<ISqlTableSource>.Parent { get { return _parent; } set { _parent = value; } }
		public          ISqlTableSource         Table  { get { return _parent; } }

		#region Overrides

#if OVERRIDETOSTRING

		public override string ToString()
		{
			return ((IQueryElement)this).ToString(new StringBuilder(), new Dictionary<IQueryElement,IQueryElement>()).ToString();
		}

#endif

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

		public bool CanBeNull()
		{
			return Nullable;
		}

		#endregion

		#region ICloneableElement Members

		public ICloneableElement Clone(Dictionary<ICloneableElement, ICloneableElement> objectTree, Predicate<ICloneableElement> doClone)
		{
			if (!doClone(this))
				return this;

			_parent.Clone(objectTree, doClone);
			return objectTree[this];
		}

		#endregion

		#region IQueryElement Members

		public QueryElementType ElementType { get { return QueryElementType.SqlField; } }

		StringBuilder IQueryElement.ToString(StringBuilder sb, Dictionary<IQueryElement,IQueryElement> dic)
		{
			return sb
				.Append('t')
				.Append(Table.SourceID)
				.Append('.')
				.Append(Name);
		}

		#endregion
	}
}
