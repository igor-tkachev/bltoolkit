using System;
using System.Data;
using System.Data.Common;

namespace BLToolkit.Data.DataProvider
{
	[Obsolete]
	public class ObsoleteDataProvider : DataProviderBase
	{
		public ObsoleteDataProvider(IDataProvider dataProvider)
		{
			_dataProvider = dataProvider;
		}

		private IDataProvider _dataProvider;

		public override IDbConnection CreateConnectionObject()
		{
			return _dataProvider.CreateConnectionObject();
		}

		public override DbDataAdapter CreateDataAdapterObject()
		{
			return _dataProvider.CreateDataAdapterObject();
		}

		public override bool DeriveParameters(IDbCommand command)
		{
			return _dataProvider.DeriveParameters(command);

		}

		public override object Convert(object value, ConvertType convertType)
		{
			return _dataProvider.Convert(value, convertType);
		}

		public override void SetParameterType(IDbDataParameter parameter, object value)
		{
			_dataProvider.SetParameterType(parameter, value);
		}
		public override Type ConnectionType
		{
			get { return _dataProvider.ConnectionType; }
		}

		public override string Name
		{
			get { return _dataProvider.Name; }
		}
	}
}
