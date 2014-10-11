using System;

namespace BLToolkit.Common
{
	[Obsolete("Use System.Tuple")]
	public struct Tuple<T1,T2>
	{
		public Tuple(T1 field1, T2 field2)
		{
			Field1 = field1;
			Field2 = field2;
		}

		public readonly T1 Field1;
		public readonly T2 Field2;

		public override int GetHashCode()
		{
			int hash1 = Field1 == null ? 0 : Field1.GetHashCode();
			int hash2 = Field2 == null ? 0 : Field2.GetHashCode();

			return ((hash1 << 5) + hash1) ^ hash2;
		}

		public override bool Equals(object obj)
		{
			if (obj is Tuple<T1,T2>)
			{
				Tuple<T1,T2> t = (Tuple<T1,T2>)obj;

				return
					(Field1 == null ? t.Field1 == null : Field1.Equals(t.Field1)) &&
					(Field2 == null ? t.Field2 == null : Field2.Equals(t.Field2));
			}

			return false;
		}
	}

	[Obsolete("Use System.Tuple")]
	public struct Tuple<T1,T2,T3>
	{
		public Tuple(T1 field1, T2 field2, T3 field3)
		{
			Field1 = field1;
			Field2 = field2;
			Field3 = field3;
		}

		public readonly T1 Field1;
		public readonly T2 Field2;
		public readonly T3 Field3;

		public override int GetHashCode()
		{
			int hash1 = Field1 == null ? 0 : Field1.GetHashCode();
			int hash2 = Field2 == null ? 0 : Field2.GetHashCode();

			hash1 = ((hash1 << 5) + hash1) ^ hash2;
			hash2 = Field3 == null ? 0 : Field3.GetHashCode();

			return ((hash1 << 5) + hash1) ^ hash2;
		}

		public override bool Equals(object obj)
		{
			if (obj is Tuple<T1,T2,T3>)
			{
				Tuple<T1,T2,T3> t = (Tuple<T1,T2,T3>)obj;

				return
					(Field1 == null ? t.Field1 == null : Field1.Equals(t.Field1)) &&
					(Field2 == null ? t.Field2 == null : Field2.Equals(t.Field2)) &&
					(Field3 == null ? t.Field3 == null : Field3.Equals(t.Field3));
			}

			return false;
		}
	}

	[Obsolete("Use System.Tuple")]
	public struct Tuple<T1,T2,T3,T4>
	{
		public Tuple(T1 field1, T2 field2, T3 field3, T4 field4)
		{
			Field1 = field1;
			Field2 = field2;
			Field3 = field3;
			Field4 = field4;
		}

		public readonly T1 Field1;
		public readonly T2 Field2;
		public readonly T3 Field3;
		public readonly T4 Field4;

		public override int GetHashCode()
		{
			int hash1 = Field1 == null ? 0 : Field1.GetHashCode();
			int hash2 = Field2 == null ? 0 : Field2.GetHashCode();

			hash1 = ((hash1 << 5) + hash1) ^ hash2;
			hash2 = Field3 == null ? 0 : Field3.GetHashCode();

			hash1 = ((hash1 << 5) + hash1) ^ hash2;
			hash2 = Field4 == null ? 0 : Field4.GetHashCode();

			return ((hash1 << 5) + hash1) ^ hash2;
		}

		public override bool Equals(object obj)
		{
			if (obj is Tuple<T1,T2,T3,T4>)
			{
				Tuple<T1,T2,T3,T4> t = (Tuple<T1,T2,T3,T4>)obj;

				return
					(Field1 == null ? t.Field1 == null : Field1.Equals(t.Field1)) &&
					(Field2 == null ? t.Field2 == null : Field2.Equals(t.Field2)) &&
					(Field3 == null ? t.Field3 == null : Field3.Equals(t.Field3)) &&
					(Field4 == null ? t.Field4 == null : Field4.Equals(t.Field4));
			}

			return false;
		}
	}
}
