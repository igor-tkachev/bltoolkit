using System;
using BLToolkit.Data.Linq;
using BLToolkit.Mapping;

namespace Linq.Demo.DataModel
{
	partial class NorthwindDB
	{
		public Table<DiscontinuedProduct> DiscontinuedProduct { get { return GetTable<DiscontinuedProduct>(); } }
		public Table<ActiveProduct>       ActiveProduct       { get { return GetTable<ActiveProduct>();       } }
	}

	[InheritanceMapping(Code="True",  Type=typeof(DiscontinuedProduct))]
	[InheritanceMapping(Code="False", Type=typeof(ActiveProduct))]
	abstract partial class Product
	{
	}

	public class ActiveProduct       : Product {}
	public class DiscontinuedProduct : Product {}
}
