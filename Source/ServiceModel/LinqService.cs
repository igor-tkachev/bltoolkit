using System;
using System.ServiceModel;

using BLToolkit.Data.Linq;

namespace BLToolkit.ServiceModel
{
	[ServiceBehavior(
		InstanceContextMode = InstanceContextMode.Single,
		ConcurrencyMode     = ConcurrencyMode.Multiple)]
	public class LinqService : ILinqService
	{
		public virtual IDataContext CreateDataContext()
		{
			return Settings.CreateDefaultDataContext();
		}

		#region ILinqService Members

		public Type SqlProviderType { get; set; }

		public virtual Type GetSqlProviderType()
		{
			if (SqlProviderType == null)
			{
				var ctx = CreateDataContext();

				try
				{
					SqlProviderType = ctx.CreateSqlProvider().GetType();
				}
				finally
				{
					if (ctx is IDisposable)
						((IDisposable)ctx).Dispose();
				}
			}

			return SqlProviderType;
		}

		#endregion
	}
}
