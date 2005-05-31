//
// NullableTypes.INullable
// 
// Authors: Luca Minudel (lukadotnet@users.sourceforge.net)
//
// Date         Author  Changes    Reasons
// 17-Feb-2003  Luca    Create
// 06-Oct-2003  Luca    Upgrade      Code upgrade: Replaced tabs with spaces
//

namespace NullableTypes
{
    /// <summary>
    /// All of the NullableTypes objects and structures implement the INullable 
    /// interface, reflecting the fact that, unlike the corresponding system types, 
    /// NullableTypes can legally contain the value Null.
    /// </summary>
    /// <remarks>
    /// A Null is the absence of a value because of a missing, unknown, or inapplicable 
    /// value. A Null should not be used to imply any other value (such as zero).
    /// Also any value (such as zero) should not be used to imply the absence of a 
    /// value, that's why Null exists.
    /// </remarks>
    public interface INullable {
        /// <summary>
        /// Indicates whether a NullableTypes structure is Null.
        /// </summary>
        /// <value>
        /// true if the value of this structure is Null, 
        /// otherwise false.
        /// </value>
        bool IsNull {get;}
    }
}
