using System;
using System.ServiceModel;

using BLToolkit.Data.Linq;

namespace BLToolkit.ServiceModel
{
	[ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple)]
	public class LinqService : ILinqService
	{
		public virtual IDataContext CreateDataContext()
		{
			return Settings.CreateDefaultDataContext();
		}

		#region ILinqService Members

		Type _sqlProviderType;

		public Type GetSqlProviderType()
		{
			if (_sqlProviderType == null)
			{
				var ctx = CreateDataContext();

				try
				{
					_sqlProviderType = ctx.CreateSqlProvider().GetType();
				}
				finally
				{
					if (ctx is IDisposable)
						((IDisposable)ctx).Dispose();
				}
			}

			return _sqlProviderType;
		}

		#endregion
	}
}
