using System;
using System.Collections;

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

		public class TestImp1
		{
			int       _Int;
			public int       Int       { get { return _Int; } set { _Int = value; } }

			double    _Double;
			public double    Double    { get { return _Double; } set { _Double = value; } }

			DateTime  _DateTime;
			public DateTime  DateTime  { get { return _DateTime; } set { _DateTime = value; } }

			ArrayList _ArrayList;
			public ArrayList ArrayList 
			{
				get 
				{
					ArrayList al = null; 
					al = _ArrayList; 
					return al;
				}

				set { _ArrayList = value; }
			}

			Hashtable _this = new Hashtable();
			public string this[int i]
			{
				get { return (string)_this[i];  }
				set { _this[i] = value; }
			}

			public int this[string i]
			{
				get { return (int)_this[i]; }
				set { _this[i] = value; }
			}

			public DateTime this[DayOfWeek i]
			{
				get { return (DateTime)_this[i]; }
				set { _this[i] = value; }
			}

		}
	}
}
