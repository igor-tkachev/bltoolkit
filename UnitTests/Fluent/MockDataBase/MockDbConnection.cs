using System.Data;

namespace BLToolkit.Fluent.Test.MockDataBase
{
	/// <summary>
	/// IDbConnection
	/// </summary>
	public partial class MockDb : IDbConnection
	{
		private ConnectionState _state;

		IDbTransaction IDbConnection.BeginTransaction(IsolationLevel il)
		{
			throw new System.NotImplementedException();
		}

		IDbTransaction IDbConnection.BeginTransaction()
		{
			throw new System.NotImplementedException();
		}

		void IDbConnection.ChangeDatabase(string databaseName)
		{
			throw new System.NotImplementedException();
		}

		void IDbConnection.Close()
		{
			_state = ConnectionState.Closed;
		}

		string IDbConnection.ConnectionString { get; set; }

		int IDbConnection.ConnectionTimeout
		{
			get { throw new System.NotImplementedException(); }
		}

		IDbCommand IDbConnection.CreateCommand()
		{
			return new MockCommand(this);
		}

		string IDbConnection.Database
		{
			get { throw new System.NotImplementedException(); }
		}

		void IDbConnection.Open()
		{
			_state = ConnectionState.Open;
		}

		ConnectionState IDbConnection.State
		{
			get { return _state; }
		}

		void System.IDisposable.Dispose()
		{
		}
	}
}