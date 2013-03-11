using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;

using BLToolkit.Data.Sql.SqlProvider;
using BLToolkit.Mapping;

using Devart.Data.Oracle;

namespace BLToolkit.Data.DataProvider
{
	public class DevartOracleDataProvider : DataProviderBase
	{
		/// <summary>
		/// Data provider name string.
		/// </summary>
		public const string NameString = "DevartOracle";

		public override Type ConnectionType
		{
			get { return typeof(OracleConnection); }
		}

		public override string Name
		{
			get { return NameString; }
		}

		/// <summary>
		/// Gets or sets the database activity monitor.
		/// </summary>
		private static Devart.Common.DbMonitor DbMonitor { get; set; }

		/// <summary>
		/// Gets or sets value indicating whether the database activity monitor is enabled.
		/// </summary>
		/// <remarks>
		/// This feature requires Standard or Pro edition of Devart dotConnect for Oracle provider.
		/// </remarks>
		public static bool DbMonitorActive
		{
			get { return DbMonitor == null ? false : DbMonitor.IsActive; }
			set
			{
				// setting this property has no effect in Express edition
#if DEVART_PRO
				if (DbMonitorActive != value)
				{
					DbMonitor = DbMonitor ?? new OracleMonitor();
					DbMonitor.IsActive = value;
				}
#endif
			}
		}

		public override IDbConnection CreateConnectionObject()
		{
			return new OracleConnection();
		}

		public override DbDataAdapter CreateDataAdapterObject()
		{
			return new OracleDataAdapter();
		}

		public override bool DeriveParameters(IDbCommand command)
		{
			var oraCommand = command as OracleCommand;

			if (null != oraCommand)
			{
			try
			{
				OracleCommandBuilder.DeriveParameters(oraCommand);
			}
			catch (Exception ex)
			{
				// Make Oracle less laconic.
				//
				throw new DataException(string.Format("{0}\nCommandText: {1}", ex.Message, oraCommand.CommandText), ex);
			}

			return true;
			}

			return false;
		}

		public override void AttachParameter(IDbCommand command, IDbDataParameter parameter)
		{
			var value = parameter.Value;

			if (value is DateTime)
			{
				parameter.Value = new OracleTimeStamp((DateTime)value);
			}
			else if (value is Guid)
			{
				parameter.Value = ((Guid)value).ToByteArray();
				parameter.DbType = DbType.Binary;
				((OracleParameter)parameter).OracleDbType = OracleDbType.Raw;
			}
//			else if (value is string)
//			{
//				((OracleParameter)parameter).OracleDbType = OracleDbType.NVarChar;
//			}

			base.AttachParameter(command, parameter);
		}

		public override int ExecuteArray(IDbCommand command, int iterations)
		{
			var cmd = (OracleCommand)command;
			return cmd.ExecuteArray(iterations);
		}

		public override ISqlProvider CreateSqlProvider()
		{
			return new OracleSqlProvider();
		}


		#region InsertBatch

		public override int InsertBatch<T>(
			DbManager      db,
			string         insertText,
			IEnumerable<T> collection,
			MemberMapper[] members,
			int            maxBatchSize,
			DbManager.ParameterProvider<T> getParameters)
		{
			var sb  = new StringBuilder();
			var sp  = new OracleSqlProvider();
			var n   = 0;
			var cnt = 0;
			var str = "\t" + insertText
				.Substring(0, insertText.IndexOf(") VALUES ("))
				.Substring(7)
				.Replace("\r", "")
				.Replace("\n", "")
				.Replace("\t", " ")
				.Replace("( ", "(")
				//.Replace("  ", " ")
				+ ") VALUES (";

			foreach (var item in collection)
			{
				if (sb.Length == 0)
					sb.AppendLine("INSERT ALL");

				sb.Append(str);

				foreach (var member in members)
				{
					var value = member.GetValue(item);

					if (value is Nullable<DateTime>)
						value = ((DateTime?)value).Value;

					if (value is DateTime)
					{
						var dt = (DateTime)value;
						sb.Append(string.Format("to_timestamp('{0:dd.MM.yyyy HH:mm:ss.ffffff}', 'DD.MM.YYYY HH24:MI:SS.FF6')", dt));
					}
					else
						sp.BuildValue(sb, value);

					sb.Append(", ");
				}

				sb.Length -= 2;
				sb.AppendLine(")");

				n++;

				if (n >= maxBatchSize)
				{
					sb.AppendLine("SELECT * FROM dual");

					var sql = sb.ToString();

					if (DbManager.TraceSwitch.TraceInfo)
						DbManager.WriteTraceLine("\n" + sql.Replace("\r", ""), DbManager.TraceSwitch.DisplayName);

					cnt += db.SetCommand(sql).ExecuteNonQuery();

					n = 0;
					sb.Length = 0;
				}
			}

			if (n > 0)
			{
				sb.AppendLine("SELECT * FROM dual");

				var sql = sb.ToString();

				if (DbManager.TraceSwitch.TraceInfo)
					DbManager.WriteTraceLine("\n" + sql.Replace("\r", ""), DbManager.TraceSwitch.DisplayName);

				cnt += db.SetCommand(sql).ExecuteNonQuery();
			}

			return cnt;
		}

		#endregion
	}
}
