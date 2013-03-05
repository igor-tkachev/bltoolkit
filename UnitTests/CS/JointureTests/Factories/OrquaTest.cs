using BLToolkit.Data;
using NUnit.Framework;

namespace UnitTests.CS.JointureTests.Factories
{
    [TestFixture]
    public class OrquaTest : JointureTests
    {
        #region Overrides of AssociationTests

        protected override DbConnectionFactory CreateFactory()
        {
            return new OrquaFactory();
        }

        #endregion
    }
}