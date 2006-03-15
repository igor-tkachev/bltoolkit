using System;
using System.Data;
using System.Data.OleDb;

namespace BLToolkit.Data.DataProvider
{
	public class AccessDataProvider : OleDbDataProvider
	{
		public override bool DeriveParameters(IDbCommand command)
		{
			return false;
		}

		public override void SetParameterType(IDbDataParameter parameter, object value)
		{
			if (value is DateTime)
				((OleDbParameter)parameter).OleDbType = OleDbType.Date;
		}

		public override string Name
		{
			get { return "Access"; }
		}
	}
}
