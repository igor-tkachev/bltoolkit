using System;
using System.Data;
using System.Data.OleDb;
using System.Text.RegularExpressions;

namespace BLToolkit.Data.DataProvider
{
	public sealed class AccessDataProvider : OleDbDataProvider
	{
		private static Regex _paramsExp;

		// Based on idea from http://qapi.blogspot.com/2006/12/deriveparameters-oledbprovider-ii.html
		//
		public override bool DeriveParameters(IDbCommand command)
		{
			if (command == null)
				throw new ArgumentNullException("command");

			if (command.CommandType != CommandType.StoredProcedure)
				throw new InvalidOperationException("command.CommandType must be CommandType.StoredProcedure");

			OleDbConnection conn = command.Connection as OleDbConnection;

			if (conn == null || conn.State != ConnectionState.Open)
				throw new InvalidOperationException("Invalid connection state.");

			command.Parameters.Clear();

			DataTable dt = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Procedures, new object[]{null, null, command.CommandText});

			if (dt.Rows.Count == 0)
			{
				// Jet does convert parameretless procedures to views.
				//
				dt = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Views, new object[]{null, null, command.CommandText});

				if (dt.Rows.Count == 0)
					throw new DataException(string.Format("Stored procedure '{0}' not found", command.CommandText));

				// Do nothing. There is no parameters.
				//
			}
			else
			{
				DataColumn col = dt.Columns["PROCEDURE_DEFINITION"];
				if (col == null)
				{
					// Not really possible
					//
					return false;
				}

				if (_paramsExp == null)
					_paramsExp = new Regex(@"PARAMETERS ((\[(?<name>[^\]]+)\]|(?<name>[^\s]+))\s(?<type>[^,;\s]+(\s\([^\)]+\))?)[,;]\s)*", RegexOptions.Compiled | RegexOptions.ExplicitCapture);

				Match             match = _paramsExp.Match((string)dt.Rows[0][col.Ordinal]);
				CaptureCollection names = match.Groups["name"].Captures;
				CaptureCollection types = match.Groups["type"].Captures;

				if (names.Count != types.Count)
				{
					// Not really possible
					//
					return false;
				}

				char[] separators = new char[]{' ', '(', ',', ')'};

				for (int i = 0; i < names.Count; ++i)
				{
					string paramName = names[i].Value;
					string[] rawType = types[i].Value.Split(separators, StringSplitOptions.RemoveEmptyEntries);
					OleDbParameter p = new OleDbParameter(paramName, GetOleDbType(rawType[0]));

					if (rawType.Length > 2)
					{
						p.Precision = BLToolkit.Common.Convert.ToByte(rawType[1]);
						p.Scale     = BLToolkit.Common.Convert.ToByte(rawType[2]);
					}
					else if (rawType.Length > 1)
					{
						p.Size      = BLToolkit.Common.Convert.ToInt32(rawType[1]);
					}

					command.Parameters.Add(p);
				}
			}

			return true;
		}

		private static OleDbType GetOleDbType(string jetType)
		{
			switch (jetType.ToLower())
			{
				case "byte":
				case "tinyint":
				case "integer1":
					return OleDbType.TinyInt;

				case "short":
				case "smallint":
				case "integer2":
					return OleDbType.SmallInt;

				case "int":
				case "integer":
				case "long":
				case "integer4":
				case "counter":
				case "identity":
				case "autoincrement":
					return OleDbType.Integer;

				case "single":
				case "real":
				case "float4":
				case "ieeesingle":
					return OleDbType.Single;


				case "double":
				case "number":
				case "double precision":
				case "float":
				case "float8":
				case "ieeedouble":
					return OleDbType.Double;

				case "currency":
				case "money":
					return OleDbType.Currency;

				case "dec":
				case "decimal":
				case "numeric":
					return OleDbType.Decimal;

				case "bit":
				case "yesno":
				case "logical":
				case "logical1":
					return OleDbType.Boolean;

				case "datetime":
				case "date":
				case "time":
					return OleDbType.Date;

				case "alphanumeric":
				case "char":
				case "character":
				case "character varying":
				case "national char":
				case "national char varying":
				case "national character":
				case "national character varying":
				case "nchar":
				case "string":
				case "text":
				case "varchar":
					return OleDbType.VarWChar;

				case "longchar":
				case "longtext":
				case "memo":
				case "note":
				case "ntext":
					return OleDbType.LongVarWChar;

				case "binary":
				case "varbinary":
				case "binary varying":
				case "bit varying":
					return OleDbType.VarBinary;

				case "longbinary":
				case "image":
				case "general":
				case "oleobject":
					return OleDbType.LongVarBinary;

				case "guid":
				case "uniqueidentifier":
					return OleDbType.Guid;

				default:
					// Each release of Jet brings many new aliases to existing types.
					// This list may be outdated, please send a report to us.
					//
					throw new NotSupportedException("Unknown DB type '" + jetType + "'");
			}
		}

		public override void AttachParameter(IDbCommand command, IDbDataParameter parameter)
		{
			if (parameter.Value is DateTime)
				((OleDbParameter)parameter).OleDbType = OleDbType.Date;

			base.AttachParameter(command, parameter);
		}

		public new const string NameString = "Access";

		public override string Name
		{
			get { return NameString; }
		}
	}
}
