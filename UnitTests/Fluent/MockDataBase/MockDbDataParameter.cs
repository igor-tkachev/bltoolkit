using System.Data;

namespace BLToolkit.Fluent.Test.MockDataBase
{
	/// <summary>
	/// IDbDataParameter
	/// </summary>
	public class MockDbDataParameter : IDbDataParameter
	{
		public DbType DbType { get; set; }

		public ParameterDirection Direction { get; set; }

		public bool IsNullable
		{
			get { throw new System.NotImplementedException(); }
		}

		public string ParameterName { get; set; }

		public string SourceColumn { get; set; }

		public DataRowVersion SourceVersion { get; set; }

		public object Value { get; set; }

		public byte Precision { get; set; }

		public byte Scale { get; set; }

		public int Size { get; set; }
	}
}