[BLToolkitGenerated]
public sealed class TestObject : NotNullTest.TestObject
{
	public override void Foo1(string str1, string str2, string str3)
	{
		if (str2 == null) throw new ArgumentNullException("str2");

		base.Foo1(str1, str2, str3);
	}

	public override void Foo2(string str1, string str2, string str3)
	{
		if (str2 == null) throw new ArgumentNullException("str2", "Null");

		base.Foo2(str1, str2, str3);
	}

	public override void Foo3(string str1, string str2, string str3)
	{
		if (str2 == null) throw new ArgumentNullException("str2", "Null: str2");

		base.Foo3(str1, str2, str3);
	}
}
