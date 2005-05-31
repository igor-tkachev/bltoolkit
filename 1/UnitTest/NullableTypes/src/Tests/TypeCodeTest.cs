//
// NullableTypes.Tests.TypeCodeTest
// 
// Authors: Luca Minudel (lukadotnet@users.sourceforge.net)
//
// Date         Author  Changes    Reasons
// 30-Jun-2003  Luca    Create
// 06-Oct-2003  Luca    Upgrade    Code upgrade: Replaced tabs with spaces and removed commented out code
//

namespace NullableTypes.Tests
{
    using nu = NUnit.Framework;
    using nua = NUnit.Framework.Assertion;

    [nu.TestFixture]
    public class TypeCodeTest {

        [nu.Test]
        public void GetTypeCode() {
            nua.AssertEquals("Test#A01", TypeCode.NullableBoolean, 
                NullableBoolean.One.GetTypeCode());

            nua.AssertEquals("Test#A02", TypeCode.NullableByte, 
                NullableByte.Zero.GetTypeCode());

            nua.AssertEquals("Test#A03", TypeCode.NullableDateTime, 
                NullableDateTime.Null.GetTypeCode());

            nua.AssertEquals("Test#A04", TypeCode.NullableDecimal, 
                NullableDecimal.One.GetTypeCode());

            nua.AssertEquals("Test#A05", TypeCode.NullableDouble, 
                NullableDouble.Zero.GetTypeCode());

            nua.AssertEquals("Test#A06", TypeCode.NullableInt16, 
                NullableInt16.Zero.GetTypeCode());

            nua.AssertEquals("Test#A07", TypeCode.NullableInt32, 
                NullableInt32.Zero.GetTypeCode());

            nua.AssertEquals("Test#A08", TypeCode.NullableInt64, 
                NullableInt64.Zero.GetTypeCode());

            nua.AssertEquals("Test#A09", TypeCode.NullableSingle, 
                NullableSingle.Zero.GetTypeCode());

            nua.AssertEquals("Test#A10", TypeCode.NullableString, 
                NullableString.Empty.GetTypeCode());

        }
        
    }
}
