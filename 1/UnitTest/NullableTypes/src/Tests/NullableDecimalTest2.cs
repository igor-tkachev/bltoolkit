//
// NullableTypes.NullableTypes.Tests.NullableDecimalTest2
// 
// Authors: Luca Minudel (lukadotnet@users.sourceforge.net)
//
// Date         Author  Changes    Reasons
// 27-Jun-2003  Luca    Created
//

namespace NullableTypes.Tests {

    using nu = NUnit.Framework;
    using nua = NUnit.Framework.Assertion;
    using sys = System;
    
    [nu.TestFixture]
    public class NullableDecimalTest2 {

        [nu.Test]
        public void Ceiling() {
            NullableDecimal oneEps = new NullableDecimal(1.000000000000001M);
            nua.AssertEquals ("#A01", 2M, NullableDecimal.Ceiling(oneEps).Value);

            NullableDecimal minusOneEps = new NullableDecimal(-1.000000000000001M);
            nua.AssertEquals ("#A02", -1M, NullableDecimal.Ceiling(minusOneEps).Value);
        }


    }
}
