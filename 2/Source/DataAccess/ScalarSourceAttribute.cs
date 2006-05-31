using System;
using BLToolkit.Data;

namespace BLToolkit.DataAccess
{
	[AttributeUsage(AttributeTargets.Method)]
	public class ScalarSourceAttribute : Attribute
	{
		public ScalarSourceAttribute()
		{
		}

		public ScalarSourceAttribute(ScalarSourceType scalarType)
		{
			_scalarType = scalarType;
		}

		public ScalarSourceAttribute(ScalarSourceType scalarType, int index)
		{
			_scalarType = scalarType;
			_index = index;
		}
		
		private ScalarSourceType _scalarType;
		public  ScalarSourceType  ScalarType
		{
			get { return _scalarType; }
			set { _scalarType = value; }
		}

		private int _index;
		public  int  Index
		{
			get { return _index; }
			set { _index = value; }
		}
	}
}
