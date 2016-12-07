using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace BLToolkit.Data.DataProvider
{
	using Sql.SqlProvider;

	public sealed class Sql2012DataProvider : SqlDataProviderBase
	{
		static readonly List<Func<Type,string>> _udtTypeNameResolvers = new List<Func<Type,string>>();

		static Sql2012DataProvider()
		{
			AddUdtTypeNameResolver(ResolveStandartUdt);
		}

		public static void AddUdtTypeNameResolver(Func<Type, string> resolver)
		{
			_udtTypeNameResolvers.Add(resolver);
		}

		static string ResolveStandartUdt(Type type)
		{
			return type.Namespace == "Microsoft.SqlServer.Types" ? type.Name.Replace("Sql", "") : null;
		}

		public override string Name
		{
			get { return DataProvider.ProviderName.MsSql2012; }
		}

		public override ISqlProvider CreateSqlProvider()
		{
			return new MsSql2012SqlProvider();
		}

		public override void SetParameterValue(IDbDataParameter parameter, object value)
		{
			base.SetParameterValue(parameter, value);
			SetUdtTypeName(parameter, value);
		}

		static void SetUdtTypeName(IDbDataParameter parameter, object value)
		{
			var sqlParameter = parameter as System.Data.SqlClient.SqlParameter;
			var valueType    = value.GetType();

			if (sqlParameter != null)
				sqlParameter.UdtTypeName = _udtTypeNameResolvers.Select(_=>_(valueType)).FirstOrDefault(_=>!string.IsNullOrEmpty(_));
		}
	}
}
