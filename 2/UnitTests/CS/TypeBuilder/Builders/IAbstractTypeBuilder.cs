using System;
using System.Collections;
using System.Reflection;

using NUnit.Framework;

using BLToolkit.Reflection;
using BLToolkit.Reflection.Emit;
using BLToolkit.TypeBuilder;
using BLToolkit.TypeBuilder.Builders;

namespace TypeBuilder.Builders
{
	[TestFixture]
	public class IAbstractTypeBuilder
	{
		public interface ITest
		{
			void      Method();
			Type      PropertyType      { get; }
			DateTime  PropertyDateTime  { get; }
			DayOfWeek PropertyDayOfWeek { get; }
			int       PropertyInt       { get; set; }
			void      InitOut(
				out bool      pbool,
				out byte      pbyte,
				out sbyte     psbyte, 
				out char      pchar,
				out decimal   pdecimal,
				out double    pdouble,
				out float     pfloat,
				out int       pint,
				ref uint      puint,
				out long      plong,
				out ulong     pulong,
				out object    pobject,
				out short     pshort,
				out ushort    pushort,
				out string    pstring,
				out DateTime  pDateTime,
				out Guid      pGuid,
				out Type      pType,
				out DayOfWeek pDayOfWeek);
		}

		public class AbstractTypeBuilder : AbstractTypeBuilderBase
		{
			public override Type[] GetInterfaces() 
			{
				return new Type[] { typeof(ITest) };
			}

			protected override void BuildType()
			{
				ConstructorBuilderHelper cb = Context.TypeBuilder.DefaultConstructor;

				cb = Context.TypeBuilder.TypeInitializer;
				cb = Context.TypeBuilder.InitConstructor;
			}
		}

		public class TestTypeBuilderAttribute : AbstractTypeBuilderAttribute
		{
			public override BLToolkit.TypeBuilder.Builders.IAbstractTypeBuilder TypeBuilder
			{
				get { return new AbstractTypeBuilder(); }
			}
		}

		[TestTypeBuilder]
		public abstract class Object
		{
			protected abstract string this[int i]    { get; set; }
			protected abstract int    this[string i] { get; set; }
		}

		[Test]
		public void Test()
		{
			TypeFactory.SaveTypes = true;

			ITest test = (ITest)TypeAccessor.GetAccessor(typeof(Object)).CreateInstance();

			bool      pbool;
			byte      pbyte;
			sbyte     psbyte;
			char      pchar;
			decimal   pdecimal;
			double    pdouble;
			float     pfloat;
			int       pint;
			uint      puint = 15;
			long      plong;
			ulong     pulong = 20;
			object    pobject;
			short     pshort;
			ushort    pushort;
			string    pstring;
			DateTime  pDateTime;
			Guid      pGuid;
			Type      pType;
			DayOfWeek pDayOfWeek;

			test.InitOut(out pbool, out pbyte, out psbyte, out pchar, out pdecimal, out pdouble,
				out pfloat, out pint, ref puint, out plong, out pulong, out pobject, out pshort,
				out pushort, out pstring, out pDateTime, out pGuid, out pType, out pDayOfWeek);

			Console.WriteLine("bool     : {0}", pbool);
			Console.WriteLine("byte     : {0}", pbyte);
			Console.WriteLine("sbyte    : {0}", psbyte);
			//Console.WriteLine("char     : {0}", pchar);
			Console.WriteLine("decimal  : {0}", pdecimal);
			Console.WriteLine("double   : {0}", pdouble);
			Console.WriteLine("float    : {0}", pfloat);
			Console.WriteLine("int      : {0}", pint);
			Console.WriteLine("uint     : {0}", puint);
			Console.WriteLine("long     : {0}", plong);
			Console.WriteLine("ulong    : {0}", pulong);
			Console.WriteLine("object   : {0}", pobject);
			Console.WriteLine("short    : {0}", pshort);
			Console.WriteLine("ushort   : {0}", pushort);
			Console.WriteLine("string   : {0}", pstring);
			Console.WriteLine("DateTime : {0}", pDateTime);
			Console.WriteLine("Guid     : {0}", pGuid);
			Console.WriteLine("Type     : {0}", pType);
			Console.WriteLine("DayOfWeek: {0}", pDayOfWeek);

			Assert.AreEqual(15, puint);
			Assert.AreEqual(0,  pulong);
			Assert.AreEqual(DayOfWeek.Sunday, test.PropertyDayOfWeek);
		}
	}
}
