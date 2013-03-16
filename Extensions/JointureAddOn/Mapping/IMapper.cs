using System;
using BLToolkit.Emit;

namespace BLToolkit.Mapping
{
    public interface IMapper
    {
        int DataReaderIndex { get; set; }
        SetHandler Setter { get; set; }
        Type PropertyType { get; set; }
        string PropertyName { get; set; }
    }
}