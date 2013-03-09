namespace BLToolkit.Fluent.Test.MockDataBase
{
	public static class AssertMockDataBaseEx
	{
		 public static AssertDb Assert(this MockDb db)
		 {
		 	return new AssertDb(db);
		 }

		 public static AssertCommandData Assert(this MockCommandData data)
		 {
		 	return new AssertCommandData(data);
		 }
	}
}