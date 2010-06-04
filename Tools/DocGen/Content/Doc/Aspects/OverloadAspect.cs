[BLToolkitGenerated]
public sealed class OverloadTestObject : OverloadTestObject
{
    [BLToolkitGenerated]
    public override int Test(int intVal)
    {
        return this.Test(intVal, string.Empty);
    }

    [BLToolkitGenerated]
    public override int Test(string strVal)
    {
        return this.Test(0x0, strVal);
    }

    [BLToolkitGenerated]
    public override int Test(int intVal, string strVal, bool boolVal)
    {
        return this.Test(intVal, strVal);
    }
}