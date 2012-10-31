using System;
using BLToolkit.Mapping;

namespace NorthwindDataService
{
	partial class DataContext
	{
	}

	[InheritanceMapping(Code=true,  Type=typeof(DiscontinuedProduct))]
	[InheritanceMapping(Code=false, Type=typeof(ActiveProduct))]
	partial class Product
	{
	}

	public class ActiveProduct       : Product {}
	public class DiscontinuedProduct : Product {}
}
