using System;

namespace BLToolkit.Data.Sql
{
	public interface IChild<T>
	{
		string Name   { get; }
		T      Parent { get; set; }
	}
}
