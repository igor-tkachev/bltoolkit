using System;
using System.Data;
using System.Data.OleDb;

namespace BLToolkit.Data.DataProvider
{
	public sealed class AccessDataProvider : OleDbDataProvider
	{
		public override bool DeriveParameters(IDbCommand command)
		{
			return false;
		}

		public override void AttachParameter(IDbCommand command, IDbDataParameter parameter)
		{
			if (parameter.Value is DateTime)
				((OleDbParameter)parameter).OleDbType = OleDbType.Date;

			base.AttachParameter(command, parameter);
		}

		public override string Name
		{
			get { return "Access"; }
		}
	}
}
