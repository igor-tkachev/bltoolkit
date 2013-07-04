using BLToolkit.Data;
using NUnit.Framework;
using UnitTests.CS.JointureTests.Factories;

namespace UnitTests.CS.JointureTests
{
    [TestFixture]
    public class MusicProd : AllTests
    {
        #region Overrides of AssociationTests

        protected override DbConnectionFactory CreateFactory()
        {
            return new MusicFactory(true);
        }

        #endregion
    }
}