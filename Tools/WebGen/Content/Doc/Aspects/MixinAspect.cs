[BLToolkitGenerated]
public sealed class TestClass : MixinAspectTest.TestClass, MixinAspectTest.ITestInterface1, MixinAspectTest.ITestInterface2
{
	[BLToolkitGenerated]
	int MixinAspectTest.ITestInterface1.TestMethod(int value)
	{
		if (base._testInterface1 == null)
			throw new InvalidOperationException("'ITestInterface1._testInterface1' is not initialized.");

		return base._testInterface1.TestMethod(value);
	}

	[BLToolkitGenerated]
	int MixinAspectTest.ITestInterface2.TestMethod1(int value)
	{
		// The [link][file]Aspects/MixinOverrideAttribute.cs[/file]MixinOverride[/link] attribute enforces direct method call.
		//
		return base.TestMethod1(value);
	}

	[BLToolkitGenerated]
	int MixinAspectTest.ITestInterface2.TestMethod2(int value)
	{
		if (base.TestInterface2 == null)
			throw new InvalidOperationException("'ITestInterface2.TestInterface2' is null.");

		return base.TestInterface2.TestMethod2(value);
	}
}
