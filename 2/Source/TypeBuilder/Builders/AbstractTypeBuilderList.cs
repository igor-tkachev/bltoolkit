using System;
using System.Diagnostics.CodeAnalysis;

#if FW2
using System.Collections.Generic;
#else
using System.Collections;
#endif

namespace BLToolkit.TypeBuilder.Builders
{
#if FW2
	public class AbstractTypeBuilderList : List<IAbstractTypeBuilder>
	{
		public AbstractTypeBuilderList()
			: base()
		{
		}

        public AbstractTypeBuilderList(int capacity)
			: base(capacity)
		{
		}
	}
#else
	public class AbstractTypeBuilderList : ArrayList
	{
		public AbstractTypeBuilderList() 
			: base()
		{
		}

		public AbstractTypeBuilderList(int capacity) 
			: base(capacity)
		{
		}

		public new IAbstractTypeBuilder this[int i]
		{
			get { return (IAbstractTypeBuilder)base[i]; }
			set { base[i] = value;              }
		}
	}
#endif
}
