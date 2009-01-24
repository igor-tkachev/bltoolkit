using System;

namespace BLToolkit.Data.SqlBuilder
{
	public interface IChild<T>
	{
		string Name   { get; }
		T      Parent { get; set; }
	}
}
