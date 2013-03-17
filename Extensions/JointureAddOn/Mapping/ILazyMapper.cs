using BLToolkit.Emit;

namespace BLToolkit.Mapping
{
    public interface ILazyMapper
    {
        GetHandler ParentKeyGetter { get; set; }
    }
}