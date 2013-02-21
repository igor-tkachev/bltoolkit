using BLToolkit.Data;
using NUnit.Framework;

namespace UnitTests.CS.JointureTests
{
    [TestFixture]
    public class MediaDiscTest : AssociationTests
    {
        #region Overrides of AssociationTests

        public override DbConnectionFactory CreateFactory()
        {
            return new MediaDiscFactory();
        }

        #endregion
    }
}