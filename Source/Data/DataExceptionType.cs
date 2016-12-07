using System;

namespace BLToolkit.Data
{
	public enum DataExceptionType
	{
		Undefined,
		Deadlock,
		Timeout,
		ForeignKeyViolation,
		UniqueIndexViolation,
		ConstraintViolation,
	}
}
