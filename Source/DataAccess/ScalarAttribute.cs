using System;

namespace BLToolkit.DataAccess
{
	/// <summary>
	/// This attribute is used to represent that a type is a UDT type linked to a UDT Type in the DB (Used most commanly in Oracle)
	/// 
	/// Without this attribute if you try to use the code generation method to call a Stored Procedure in the DB and try to pass the UDT as a parm 
	/// BLToolkit will flaten the object and cause error's
	/// 
	/// Another solution is to convert your UDT to a Struct which also causes BLToolkits 'IsScaler()' Method to return true.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
	public class ScalarAttribute : Attribute
	{
	}
}
