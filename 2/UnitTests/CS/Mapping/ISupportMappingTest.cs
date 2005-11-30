using System;

using NUnit.Framework;

using BLToolkit.Mapping;
using BLToolkit.Reflection;
using BLToolkit.TypeBuilder;

namespace Mapping
{
	[TestFixture]
	public class ISupportMappingTest
	{
		public class SourceObject
		{
			public int Int1 = 10;
			public int Int2 = 20;
			public int Int3 = 30;
		}

		public class Object1 : ISupportMapping
		{
			public Object1(InitContext initContext)
			{
				if (initContext != null)
					Int11 = (int)initContext.Parameters[0];

				if (Int11 == 77)
					initContext.StopMapping = true;
			}

			public int Int11;
			public int Int22;
			public int Int3;
			public int Int44;

			public void BeginMapping(InitContext initContext)
			{
				Int22 = (int)initContext.Parameters[1];

				if (Int22 == 66)
					initContext.StopMapping = true;
			}

			public void EndMapping(InitContext initContext)
			{
				Int44 = (int)initContext.Parameters[2];
			}
		}

		[Test]
		public void Test1()
		{
			Object1 o = (Object1)Map.ToObject(new SourceObject(), typeof(Object1), 11, 22, 44);

			Assert.AreEqual(11, o.Int11);
			Assert.AreEqual(22, o.Int22);
			Assert.AreEqual(30, o.Int3);
			Assert.AreEqual(44, o.Int44);
		}

		[Test]
		public void Test2()
		{
			Object1 o = new Object1(null);

			Map.ToObject(new SourceObject(), o, 11, 22, 44);

			Assert.AreEqual(0,  o.Int11);
			Assert.AreEqual(22, o.Int22);
			Assert.AreEqual(30, o.Int3);
			Assert.AreEqual(44, o.Int44);
		}

		[Test]
		public void Test3()
		{
			Object1 o = (Object1)Map.ToObject(new SourceObject(), typeof(Object1), 77, 66, 44);

			Assert.AreEqual(77, o.Int11);
			Assert.AreEqual(0,  o.Int22);
			Assert.AreEqual(0,  o.Int3);
			Assert.AreEqual(0,  o.Int44);
		}

		[Test]
		public void Test4()
		{
			Object1 o = (Object1)Map.ToObject(new SourceObject(), typeof(Object1), 11, 66, 44);

			Assert.AreEqual(11, o.Int11);
			Assert.AreEqual(66, o.Int22);
			Assert.AreEqual(0,  o.Int3);
			Assert.AreEqual(0,  o.Int44);
		}

		public abstract class Object5 : ISupportMapping
		{
			public Object5(InitContext initContext)
			{
				Int11 = (int)initContext.Parameters[0];
			}

			[Parameter(77)]
			public abstract int   Int00 { get; set; }
			public abstract int   Int11 { get; set; }
			public abstract int   Int22 { get; set; }
			public abstract short Int3  { get; set; }
			public abstract int   Int44 { get; set; }

			public void BeginMapping(InitContext initContext)
			{
				Int22 = (int)initContext.Parameters[1];
			}

			public void EndMapping(InitContext initContext)
			{
				Int44 = (int)initContext.Parameters[2];
			}
		}

		[Test]
		public void Test5()
		{
			Object5 o = (Object5)Map.ToObject(new SourceObject(), typeof(Object5), 11, 22, 44);

			Assert.AreEqual(77, o.Int00);
			Assert.AreEqual(11, o.Int11);
			Assert.AreEqual(22, o.Int22);
			Assert.AreEqual(30, o.Int3);
			Assert.AreEqual(44, o.Int44);
		}
	}
}
