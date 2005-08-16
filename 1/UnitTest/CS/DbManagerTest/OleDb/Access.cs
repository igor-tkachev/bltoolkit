using System;
using System.Data;

using NUnit.Framework;

using Rsdn.Framework.Data;

namespace CS.DbManagerTest.OleDb
{
	[TestFixture]
	public class Access : CS.DbManagerTest.Test
	{
		public override string ConfigurationString 
		{
			get 
			{
				return "Access.oledb";
			}
		}

		public override void ExecuteDictionary() {}
		public override void ExecuteDictionary_CommandType_Text() {}
		public override void ExecuteDictionary_Parameters() {}
		public override void ExecuteDictionary_CommandType_Text_Parameters() {}
		public override void ExecutePreparedDictionary() {}
		public override void ExecuteDictionary_Dictionary() {}
		public override void ExecuteDictionary_Dictionary_CommandType_Text() {}
		public override void ExecuteDictionary_Dictionary_Parameters() {}
		public override void ExecuteDictionary_Dictionary_CommandType_Text_Parameters() {}
	}
}
