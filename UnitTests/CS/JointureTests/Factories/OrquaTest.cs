using BLToolkit.Data;
using NUnit.Framework;

namespace UnitTests.CS.JointureTests
{
    [TestFixture]
    public class OrquaTest : AssociationTests
    {
        #region Overrides of AssociationTests

        public override DbConnectionFactory CreateFactory()
        {
            return new OrquaFactory();
        }

        #endregion
    }
}