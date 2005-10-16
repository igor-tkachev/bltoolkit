using System;

namespace UnitTests.All
{
	public class Class1
	{
		public struct MyStruct
		{
			public int     i;
			public Guid    g;
			public decimal d;
		}

		public void InitOut(
			bool      pbool,
			byte      pbyte,
			sbyte     psbyte, 
			char      pchar,
			decimal   pdecimal,
			double    pdouble,
			float     pfloat,
			int       pint,
			uint      puint,
			long      plong,
			ulong     pulong,
			object    pobject,
			short     pshort,
			ushort    pushort,
			string    pstring,
			DateTime  pDateTime,
			Guid      pGuid,
			Type      pType,
			MyStruct  pMyStruct,
			DayOfWeek pDayOfWeek)
		{
			pbool      = false;
			pbyte      = 0;
			psbyte     = 0;
			pchar      = '\0';
			pdecimal   = 0m;
			pdouble    = 0;
			pfloat     = 0;
			pint       = 0;
			puint      = 0;
			plong      = 0;
			pulong     = 0;
			pobject    = 0;
			pshort     = 0;
			pushort    = 0;
			pstring    = string.Empty;
			pDateTime  = new DateTime();
			pGuid      = Guid.Empty;
			pType      = null;
			pMyStruct  = new MyStruct();
			pDayOfWeek = new DayOfWeek();
		}
	}
}
