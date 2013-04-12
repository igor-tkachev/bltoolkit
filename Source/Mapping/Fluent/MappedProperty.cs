using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLToolkit.Mapping.Fluent
{
    public class MappedProperty
    {
        public string Name { get; internal set; }
        public Type Type { get; internal set; }
        public Type ParentType { get; internal set; }        
    }
}
