//
// NullableTypes.Tests.Locale
// 
// Authors: Luca Minudel (lukadotnet@users.sourceforge.net)
//
// Date         Author  Changes    Reasons
// 27-Feb-2003  Luca    Create
// 12-May-2003  Luca    Upgrade    Added test for missing string
// 06-Oct-2003  Luca    Upgrade    Code upgrade: Replaced tabs with spaces
//

namespace NullableTypes.Tests
{
    using nu = NUnit.Framework;
    using nua = NUnit.Framework.Assertion;
    
#if DEBUG
    [nu.TestFixture]
    public class Locale
    {
        [nu.Test]
        public void GetText() {
            nua.AssertEquals("Read Locale String", "Test Locale", NullableTypes.Locale.GetText("Test Locale"));
        }

        [nu.Test, nu.ExpectedException(typeof(System.IndexOutOfRangeException))]
        public void GetTextIndexOutOfRangeException() {
            NullableTypes.Locale.GetText("_*a-string-that-do-not-exists*_");
        }

    }
#endif
}
