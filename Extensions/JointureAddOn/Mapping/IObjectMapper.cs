
﻿using System.Collections.Generic;
using BLToolkit.Emit;

namespace BLToolkit.Mapping
{
    public interface IObjectMapper : IPropertiesMapping, IMapper, ILazyMapper
    {
        bool IsLazy { get; set; }

        bool ContainsLazyChild { get; set; }

        List<string> PrimaryKeyNames { get; set; }

        /// <summary>
        ///     Is set only for CollectionFullObjectMapper. TODO : Should refactor this?
        /// </summary>
        GetHandler Getter { get; set; }

        List<GetHandler> PrimaryKeyValueGetters { get; set; }
        Association Association { get; set; }
    }
}