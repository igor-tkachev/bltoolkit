[BLToolkitGenerated]
public sealed class TestClass : ClearCacheAspect.TestClass
{
	[BLToolkitGenerated] private static MethodInfo _methodInfo1;
	[BLToolkitGenerated] private static MethodInfo _methodInfo2;
	[BLToolkitGenerated] private static Type       _type3;
	[BLToolkitGenerated] private static Type       _type4;

	[BLToolkitGenerated]
	public override int CachedMethod(int p1, int p2)
	{
		// Method implementation.
	}

	[BLToolkitGenerated]
	public override void ClearCache()
	{
		try
		{
			// Here should be main method implementation.
			// It is empty as this method does nothing.
		}
		finally
		{
			if (_methodInfo1 == null)
			{
				_methodInfo1 = 
					ClearCacheAspect.GetMethodInfo(this, null, "CachedMethod", null);
			}

			CacheAspect.ClearCache(_methodInfo1);
		}
	}

	[BLToolkitGenerated]
	public override void ClearCache2()
	{
		try
		{
		}
		finally
		{
			if (_methodInfo2 == null)
			{
				_methodInfo2 = ClearCacheAspect.GetMethodInfo(
					this,
					null,
					"CachedMethod",
					new Type[] { typeof(int), typeof(int) });
			}

			CacheAspect.ClearCache(_methodInfo2);
		}
	}

	[BLToolkitGenerated]
	public override void ClearAll()
	{
		try
		{
		}
		finally
		{
			if (_type3 == null)
			{
				_type3 = ClearCacheAspect.GetType(this, typeof(TestClass));
			}

			CacheAspect.ClearCache(_type3);
		}
	}

	[BLToolkitGenerated]
	public override void ClearAll2()
	{
		try
		{
		}
		finally
		{
			if (_type4 == null)
			{
				_type4 = ClearCacheAspect.GetType(this, null);
			}

			CacheAspect.ClearCache(_type4);
		}
	}
}
