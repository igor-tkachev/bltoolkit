using System;

namespace BLToolkit.DataAccess
{
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
	public class NonUpdatableAttribute : Attribute
	{
		public NonUpdatableAttribute()
			: this(true, true)
		{
		}

		public NonUpdatableAttribute(bool onInsert, bool onUpdate)
		{
			_onInsert = onInsert;
			_onUpdate = onUpdate;
		}

		private bool _onInsert;
		public  bool  OnInsert
		{
			get { return _onInsert;  }
			set { _onInsert = value; }
		}

		private bool _onUpdate;
		public  bool  OnUpdate
		{
			get { return _onUpdate;  }
			set { _onUpdate = value; }
		}
	}
}
