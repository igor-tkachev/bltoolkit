using System;
using System.Data;

using NUnit.Framework;

using Rsdn.Framework.Data;

namespace CS.DbManagerTest.OleDb
{
	[TestFixture]
	public class SqlServer : CS.DbManagerTest.Test
	{
		public override string ConfigurationString 
		{
			get 
			{
				return "SqlServer.OleDb";
			}
		}

		public override string ParamText(string param)
		{
			return "?";
		}
	}
}
