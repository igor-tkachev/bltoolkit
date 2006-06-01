using System;
using BLToolkit.Common;
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

		public ScalarSourceAttribute(ScalarSourceType scalarType, string name)
		{
			_scalarType = scalarType;
			_nip = name;
		}

		public ScalarSourceAttribute(ScalarSourceType scalarType, int index)
		{
			_scalarType = scalarType;
			_nip = index;
		}
		
		private ScalarSourceType _scalarType;
		public  ScalarSourceType  ScalarType
		{
			get { return _scalarType; }
			set { _scalarType = value; }
		}

		private NameOrIndexParameter _nip;
		public  NameOrIndexParameter  NameOrIndex
		{
			get { return _nip; }
			set { _nip = value; }
		}
	}
}
