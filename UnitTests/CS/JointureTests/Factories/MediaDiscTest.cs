using BLToolkit.Data;
using NUnit.Framework;

namespace UnitTests.CS.JointureTests.Factories
{
    [TestFixture]
    public class MediaDiscTest : JointureTests
    {
        #region Overrides of AssociationTests

        protected override DbConnectionFactory CreateFactory()
        {
            return new MediaDiscFactory();
        }

        #endregion
    }
}