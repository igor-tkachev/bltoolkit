using System;
using System.Collections;

namespace BLToolkit.ComponentModel
{
	public interface ISortable
	{
		void Sort(int index, int count, IComparer comparer);
	}
}
