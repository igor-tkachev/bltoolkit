using BLToolkit.Emit;

namespace BLToolkit.Mapping
{
    public interface IObjectMapper : IPropertiesMapping, IMapper, ILazyMapper
    {
        bool IsLazy { get; set; }
        bool ContainsLazyChild { get; set; }

        /// <summary>
        ///     Is set only for CollectionFullObjectMapper. TODO : Should refactor this?
        /// </summary>
        GetHandler Getter { get; set; }

        GetHandler PrimaryKeyValueGetter { get; set; }
        AssociationAttribute Association { get; set; }
    }
}