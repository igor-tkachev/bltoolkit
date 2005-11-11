using System;

using NUnit.Framework;

using BLToolkit.TypeBuilder.Builders;
using BLToolkit.Reflection;

namespace UnitTests.All
{
	public class Accessor
	{
		public Accessor()
		{
			ObjectFactory = ((ObjectFactoryAttribute)AbstractTypeBuilderBase.
				GetFirstAttribute(typeof(Accessor), typeof(ObjectFactoryAttribute))).ObjectFactory;
		}

		public IObjectFactory ObjectFactory
		{
			get { return null; }
			set { }
		}
	}
}
