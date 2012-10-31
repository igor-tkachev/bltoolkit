using System;

using BLToolkit.Data;

namespace BLToolkit.DataAccess
{
	[AttributeUsage(AttributeTargets.Method)]
	public class ScalarSourceAttribute : ScalarFieldNameAttribute
	{
		public ScalarSourceAttribute(ScalarSourceType scalarType)
			: base(0)
		{
			_scalarType = scalarType;
		}

		public ScalarSourceAttribute(ScalarSourceType scalarType, string name)
			: base(name)
		{
			_scalarType = scalarType;
		}

		public ScalarSourceAttribute(ScalarSourceType scalarType, int index)
			: base(index)
		{
			_scalarType = scalarType;
		}
		
		private ScalarSourceType _scalarType;
		public  ScalarSourceType  ScalarType
		{
			get { return _scalarType; }
			set { _scalarType = value; }
		}
	}
}
