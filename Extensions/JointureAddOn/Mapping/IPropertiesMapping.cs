using System.Collections.Generic;

namespace BLToolkit.Mapping
{
    public interface IPropertiesMapping
    {
        List<IMapper> PropertiesMapping { get; }
        IPropertiesMapping ParentMapping { get; set; }
    }
}