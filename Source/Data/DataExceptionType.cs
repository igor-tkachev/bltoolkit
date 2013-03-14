using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLToolkit.Data
{
    public enum DataExceptionType
    {
        undefined,
        Deadlock,
        Timeout,
        ForeignKeyViolation,
        UniqueIndexViolation,
        ConstraintViolation,
    }
}
