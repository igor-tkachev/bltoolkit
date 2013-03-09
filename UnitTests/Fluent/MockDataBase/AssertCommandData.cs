using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BLToolkit.Fluent.Test.MockDataBase
{
	public class AssertCommandData
	{
		private readonly MockCommandData _data;

		public AssertCommandData(MockCommandData data)
		{
			_data = data;
		}

		public void AreField(string fieldName, int? count = null, string message = null)
		{
			int fCount = _data.Fields.TryGetValue(fieldName, out fCount) ? fCount : 0;
			if ((null == count) && (0 < fCount))
			{
				return;
			}
			if ((null != count) && (count.Value == fCount))
			{
				return;
			}
			Assert.Fail(message ?? string.Format("Fail field '{0}'", fieldName));
		}

		public void AreField(string fieldName, string message = null)
		{
			AreField(fieldName, null, message);
		}

		public void AreNotField(string fieldName, string message = null)
		{
			AreField(fieldName, 0, message);
		}

		public void AreTable(string tableName, string message = null)
		{
			if (!_data.Tables.Contains(tableName))
			{
				Assert.Fail(message ?? string.Format("Fail table '{0}'", tableName));
			}
		}
	}
}