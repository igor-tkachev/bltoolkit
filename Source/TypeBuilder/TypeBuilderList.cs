using System;

#if FW2
using System.Collections.Generic;
#else
using System.Collections;
#endif

namespace BLToolkit.TypeBuilder
{
#if FW2
	public class TypeBuilderList : List<ITypeBuilder>
	{
		public TypeBuilderList()
			: base()
		{
		}

		public TypeBuilderList(int capacity)
			: base(capacity)
		{
		}
	}
#else
	public class TypeBuilderList : ArrayList
	{
		public TypeBuilderList() 
			: base()
		{
		}

		public TypeBuilderList(int capacity) 
			: base(capacity)
		{
		}

		public new ITypeBuilder this[int i]
		{
			get { return (ITypeBuilder)base[i]; }
			set { base[i] = value;              }
		}
	}
#endif
}
