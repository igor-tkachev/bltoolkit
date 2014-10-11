﻿using System;

using BLToolkit.Mapping;

namespace Data.Linq.Model
{
	public enum TypeValue
	{
		[NullValue]
		Value0 = 0,
		[MapValue(1)] Value1 = 10,
		Value2 = 2,
		Value3 = 3,
		Value4 = 4,
		Value5 = 5,
		Value6 = 6
	}
}
