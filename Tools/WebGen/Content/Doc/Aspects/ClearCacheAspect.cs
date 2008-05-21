[BLToolkitGenerated]
public sealed class TestClass : ClearCacheAspect.TestClass
{
	[BLToolkitGenerated]
	private static MethodInfo _methodInfo$ClearCacheAspect$1;
	[BLToolkitGenerated]
	private static MethodInfo _methodInfo$ClearCacheAspect$2;

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
			if (_methodInfo$ClearCacheAspect$1 == null)
			{
				_methodInfo$ClearCacheAspect$1 = 
					ClearCacheAspect.GetMethodInfo(this, null, "CachedMethod", null);
			}

			CacheAspect.ClearCache(_methodInfo$ClearCacheAspect$1);
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
			if (_methodInfo$ClearCacheAspect$2 == null)
			{
				_methodInfo$ClearCacheAspect$2 =
					ClearCacheAspect.GetMethodInfo(
						this,
						null,
						"CachedMethod",
						new Type[] { typeof(int), typeof(int) });
			}

			CacheAspect.ClearCache(_methodInfo$ClearCacheAspect$2);
		}
	}
}
