using System;

namespace BLToolkit.Data
{
	public delegate void OperationExceptionEventHandler(object sender, OperationExceptionEventArgs ea);

	public class OperationExceptionEventArgs : OperationTypeEventArgs
	{
		private readonly Exception _exception;
		public           Exception  Exception
		{
			get { return _exception; }
		}

		public OperationExceptionEventArgs(OperationType operation, Exception exception)
			: base (operation)
		{
			_exception = exception;
		}
	}
}
