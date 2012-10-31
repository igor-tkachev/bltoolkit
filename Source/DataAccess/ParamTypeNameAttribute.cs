using System;

namespace BLToolkit.DataAccess
{
	[AttributeUsage(AttributeTargets.Parameter)]
	public class ParamTypeNameAttribute : Attribute
	{
		public ParamTypeNameAttribute(string typeName)
		{
			_typeName = typeName;
		}

		private readonly string _typeName;
		public           string  TypeName
		{
			get { return _typeName; }
		}
	}
}