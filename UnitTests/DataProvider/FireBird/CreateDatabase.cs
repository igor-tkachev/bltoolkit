using System;
using System.Configuration;
using System.IO;
using BLToolkit.Data;
using FirebirdSql.Data.FirebirdClient;
using NUnit.Framework;

namespace UnitTests.CS
{
	[TestFixture(Description = "DB setup"), Explicit]
	public class CreateDatabase
	{
		[Test]
		public void Test()
		{
			FbConnection.CreateDatabase(ConfigurationManager.AppSettings.Get("ConnectionString.Fdp"), true);

			const string path = @"..\..\..\..\Data\Create Scripts\Firebird2.sql";

			using (DbManager db = new DbManager())
			{
				string cmd = string.Empty;
				string term = ";";

				foreach (string s in File.ReadAllLines(path))
				{
					string line = s.TrimEnd();
					if (!line.EndsWith(term))
					{
						cmd += line + Environment.NewLine;
						continue;
					}

					line = line.Substring(0, line.Length - term.Length).Trim();

					if (line.ToUpperInvariant().StartsWith("SET TERM "))
					{
						term = line.Substring("SET TERM ".Length).Trim();
						continue;
					}
					
					if (line.ToUpperInvariant().StartsWith("COMMIT"))
					{
						continue;
					}

					Console.WriteLine("Executing script:");
					Console.WriteLine(cmd + line);

					db
						.SetCommand(cmd + line)
						.ExecuteNonQuery()
						;

					Console.WriteLine("Succeeded.");

					cmd = string.Empty;
				}
	
			}
		}
	}
}
