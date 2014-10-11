using System;

using BLToolkit.Aspects;

using NUnit.Framework;

namespace UnitTests.All
{
	//[TestFixture]
	public class GetValueTest
	{
		[ExpectedException(typeof(ApplicationException))]
		public void Test()
		{
			try
			{
				var info = new InterceptCallInfo();

				info.ParameterValues = new object[2];
				info.ParameterValues[0] = "123";
				info.ParameterValues[1] = 123;

				throw new ApplicationException();
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
				Console.WriteLine(ex);
				throw;
			}
		}

		public void Test(out int i3, out decimal d4)
		{
			object o = (decimal)123;

			d4 = (decimal)o;

			o = (int)123;

			i3 = (int)o;
		}
	}
}
