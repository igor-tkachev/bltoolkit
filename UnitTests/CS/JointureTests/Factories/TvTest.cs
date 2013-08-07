using BLToolkit.Data;
using NUnit.Framework;

namespace UnitTests.CS.JointureTests.Factories
{
    [TestFixture]
    public class TvTest : AllTests
    {
        #region Overrides of AssociationTests

        protected override DbConnectionFactory CreateFactory()
        {
            return new TvPigeFactory();
        }

        #endregion
    }
}