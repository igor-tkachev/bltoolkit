using System;

using NUnit.Framework;

using Rsdn.Framework.EditableObject;
using Rsdn.Framework.Data.Mapping;

namespace Toys.Test
{
	[TestFixture]
	public class EditableObject
	{
		public class Source
		{
			public int    ID   = 10;
			public string Name = "20";
		}

		public abstract class Dest: EditableObjectBase
		{
			public string ChangedPropertyName;

			public abstract int    ID   { get; set; }
			public abstract string Name { get; set; }

			protected override void OnPropertyChanged(MapPropertyInfo pi)
			{
				ChangedPropertyName = pi.PropertyName;
			}
		}

		[Test]
		public void Notification()
		{
			Dest o = (Dest)Map.ToObject(new Source(), typeof(Dest));

			Assert.IsNull(o.ChangedPropertyName);

			o.ID = 1;

			Assert.AreEqual("ID", o.ChangedPropertyName);
		}

		public abstract class Object1 : EditableObjectBase
		{
			public Object1()
			{
			}

			[MapField("ObjectId")]
			public abstract int      ID      { get;  }
			public abstract short    Field1  { get; set; }

			[MapValue(true,  "Y")]
			[MapValue(false, "N")]
			public abstract bool     Field2  { get; set; }

			public abstract DateTime Field3  { get; set; }
			public abstract long     Field4  { get; set; }
			public abstract byte     Field5  { get; set; }
			public abstract char     Field6  { get; set; }
			public abstract ushort   Field7  { get; set; }
			public abstract uint     Field8  { get; set; }
			public abstract ulong    Field9  { get; set; }
			public abstract sbyte    Field10 { get; set; }
			public abstract float    Field11 { get; set; }
			public abstract double   Field12 { get; set; }
			public abstract decimal  Field13 { get; set; }
			public abstract string   Field14 { get; set; }
			public abstract Guid     Field15 { get; set; }

			public static Object1 CreateInstance()
			{
				return (Object1)Map.CreateInstance(typeof(Object1));
			}
		}

		[Test]
		public void TestCreate()
		{
			Object1.CreateInstance();
		}

		public static bool AAA(object o)
		{
			return true;
		}

		public static bool GenTest()
		{
			bool b = AAA((byte)0);
			if (b)
			{
				b = AAA((char)0);
				if (b)
				{
					b = AAA((ushort)0);
					if (b)
					{
						b = AAA((uint)0);
						if (b)
						{
							b = AAA((ulong)0);
							if (b)
							{
								b = AAA(true);
								if (b)
								{
									b = AAA((short)0);
									if (b)
									{
										b = AAA((int)0);
										if (b)
										{
											b = AAA((long)0);
											if (b)
											{
												b = AAA((float)0);
												if (b)
												{
													b = AAA((double)0);
													if (b)
													{
														b = AAA((double)0);
														if (b)
														{
															b = AAA((double)0);
															if (b)
															{
																b = AAA((double)0);
																if (b)
																{
																	b = AAA((double)0);
																	if (b)
																	{
																		b = AAA((double)0);
																		if (b)
																		{
																			b = AAA((double)0);
																			if (b)
																			{
																				b = AAA((double)0);
																				if (b)
																				{
																					b = AAA((double)0);
																					if (b)
																					{
																						b = AAA((double)0);
																					}
																				}
																			}
																		}
																	}
																}
															}
														}
													}
												}
											}
										}
									}
								}
							}
						}
					}
				}
			}
			return b;
		}

		public static void GenTestMin()
		{
			AAA(byte.MinValue);
			AAA(char.MinValue);
			AAA(ushort.MinValue);
			AAA(uint.MinValue);
			AAA(ulong.MinValue);
			AAA(true);
			AAA(short.MinValue);
			AAA(int.MinValue);
			AAA(long.MinValue);
			AAA(float.MinValue);
			AAA(double.MinValue);
		}

		public static void GenTestMax()
		{
			AAA(byte.MaxValue);
			AAA(char.MaxValue);
			AAA((ushort)(ushort.MaxValue - 1000));
			AAA(uint.MaxValue - 1000);
			AAA((ulong)((ulong)uint.MaxValue + 2));
			AAA(true);
			AAA(short.MaxValue);
			AAA(int.MaxValue);
			AAA(long.MaxValue - 1000);
			AAA(float.MaxValue);
			AAA(double.MaxValue);
		}

		public static void GenTest(object o)
		{
			AAA((byte)o);
			AAA((char)o);
			AAA((ushort)o);
			AAA((uint)o);
			AAA((ulong)o);
			AAA((bool)o);
			AAA((short)o);
			AAA((int)o);
			AAA((long)o);
			AAA((float)o);
			AAA((double)o);
		}

	}
}
