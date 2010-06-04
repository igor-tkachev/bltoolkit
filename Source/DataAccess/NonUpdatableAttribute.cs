using System;

namespace BLToolkit.DataAccess
{
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
	public class NonUpdatableAttribute : Attribute
	{
		public NonUpdatableAttribute()
			: this(true, true, false)
		{
		}

		public NonUpdatableAttribute(bool onInsert, bool onUpdate, bool isIdentity)
		{
			_onInsert   = onInsert;
			_onUpdate   = onUpdate;
			_isIdentity = isIdentity;
		}

		private bool _onInsert;   public  bool  OnInsert   { get { return _onInsert;   } set { _onInsert   = value; } }
		private bool _onUpdate;   public  bool  OnUpdate   { get { return _onUpdate;   } set { _onUpdate   = value; } }
		private bool _isIdentity; public  bool  IsIdentity { get { return _isIdentity; } set { _isIdentity = value; } }
	}
}
