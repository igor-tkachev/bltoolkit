using System;
using System.IO;
using BLToolkit.Data;

using NUnit.Framework;

namespace UnitTests.CS
{
	[TestFixture, Explicit, Category("DB setup")]
	public class CreateDatabase
	{
		[Test]
		public void Test()
		{
			const string path = @"..\..\..\..\Data\Create Scripts\SqlCe.sql";

			using (DbManager db = new DbManager())
			{
				string cmd  = string.Empty;
				string term = "GO";

				foreach (string s in File.ReadAllLines(path))
				{
					string line = s.TrimEnd();

					if (!line.EndsWith(term))
					{
						cmd += line + Environment.NewLine;
						continue;
					}

					line = line.Substring(0, line.Length - term.Length).Trim();

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
