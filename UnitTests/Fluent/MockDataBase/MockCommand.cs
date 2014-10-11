using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;

namespace BLToolkit.Fluent.Test.MockDataBase
{
	public partial class MockDb
	{
		/// <summary>
		/// IDbCommand
		/// </summary>
		private partial class MockCommand : IDbCommand
		{
			private readonly MockDb _db;
			private readonly Regex _findTableRx = new Regex(MockSqlProvider.TableMarker + @"(\w+)");
			private readonly Regex _findFieldRx = new Regex(MockSqlProvider.FieldMarker + @"(\w+)");

			public MockCommand(MockDb db)
			{
				_db = db;
				Parameters = new DataParameterCollection();
			}

			public void Dispose()
			{
			}

			public void Prepare()
			{
			}

			public void Cancel()
			{
			}

			public IDbDataParameter CreateParameter()
			{
				return new MockDbDataParameter();
			}

			public int ExecuteNonQuery()
			{
				return MockCommandData().NonQueryResult;
			}

			public IDataReader ExecuteReader()
			{
				return new MockReader(MockCommandData(false));
			}

			public IDataReader ExecuteReader(CommandBehavior behavior)
			{
				return ExecuteReader();
			}

			public object ExecuteScalar()
			{
				return MockCommandData().ScalarResult;
			}

			public IDbConnection Connection { get; set; }

			public IDbTransaction Transaction { get; set; }

			public string CommandText { get; set; }

			public int CommandTimeout { get; set; }

			public CommandType CommandType { get; set; }

			public IDataParameterCollection Parameters { get; private set; }

			public UpdateRowSource UpdatedRowSource { get; set; }

			private MockCommandData MockCommandData(bool isUsing = true)
			{
				Dictionary<string, int> fields;
				List<string> tables;
				FindFields(CommandText, out fields);
				FindTables(CommandText, out tables);
				CommandText = ClearMockMarkers(CommandText);

				var cmd = _db.NextCommand();
				cmd.CommandText = CommandText;
				cmd.Parameters = Parameters.Cast<MockDbDataParameter>().ToList();
				cmd.Fields = fields;
				cmd.Tables = tables;
				cmd.IsUsing = isUsing;
				return cmd;
			}

			private string ClearMockMarkers(string commandText)
			{
				return commandText
					.Replace(MockSqlProvider.FieldMarker, "")
					.Replace(MockSqlProvider.TableMarker, "");
			}

			private void FindTables(string commandText, out List<string> tables)
			{
				tables = (from Match match in _findTableRx.Matches(commandText) select match.Groups[1].Value)
					.Distinct().ToList();
			}

			private void FindFields(string commandText, out Dictionary<string, int> fields)
			{
				fields = (from Match match in _findFieldRx.Matches(commandText) select match.Groups[1].Value)
					.GroupBy(s => s, (s, ss) => new { Key = s, Value = ss.Count() })
					.ToDictionary(kv => kv.Key, kv => kv.Value);
			}

			private class DataParameterCollection : List<MockDbDataParameter>, IDataParameterCollection
			{
				public bool Contains(string parameterName)
				{
					return this.Any(p => p.ParameterName == parameterName);
				}

				public int IndexOf(string parameterName)
				{
					for (int i = 0; i < Count; i++)
					{
						if (this[i].ParameterName == parameterName)
						{
							return i;
						}
					}
					return -1;
				}

				public void RemoveAt(string parameterName)
				{
					var data = this.FirstOrDefault(p => p.ParameterName == parameterName);
					if (null != data)
					{
						Remove(data);
					}
				}

				public object this[string parameterName]
				{
					get
					{
						var data = this.FirstOrDefault(p => p.ParameterName == parameterName);
						return null == data ? null : data.Value;
					}
					set
					{
						var data = this.FirstOrDefault(p => p.ParameterName == parameterName);
						if (null == data)
						{
							Add(new MockDbDataParameter { ParameterName = parameterName, Value = value });
						}
						else
						{
							data.Value = value;
						}
					}
				}
			}
		}
	}
}