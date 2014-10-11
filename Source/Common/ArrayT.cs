using System;

using JetBrains.Annotations;

namespace BLToolkit.Common
{
	public static class Array<T>
	{
		[NotNull]
		public static readonly T[] Empty = new T[0];
	}
}
