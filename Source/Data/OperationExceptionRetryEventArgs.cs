namespace BLToolkit.Data
{
	public delegate void OperationExceptionRetryEventHandler(object sender, OperationExceptionRetryEventArgs ea);

	public class OperationExceptionRetryEventArgs : OperationTypeEventArgs
	{
		private readonly DataException _exception;
		public           DataException  Exception
		{
			get { return _exception; }
		}

        public bool RetryOperation { get; set; }

		public OperationExceptionRetryEventArgs(OperationType operation, DataException exception)
			: base (operation)
		{
			_exception = exception;
            RetryOperation = false;
		}
	}
}
