#region

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using BLToolkit.Data;
using NUnit.Framework;

#endregion

namespace UnitTests.CS.JointureTests.Tools
{
    public abstract class TestsBaseClass
    {
        protected DbConnectionFactory ConnectionFactory;

        protected abstract DbConnectionFactory CreateFactory();

        [TestFixtureSetUp]
        public virtual void Setup()
        {
            ConnectionFactory = CreateFactory();

            DbManager.AddDataProvider(ConnectionFactory.Provider);
            DbManager.AddConnectionString(ConnectionFactory.Provider.Name, ConnectionFactory.ConnectionString);

            DbManager.TurnTraceSwitchOn();
        }

        protected void SimulateWork(Action<DbManager> action, IDbConnectionFactory connectionFactory, int maxUsers = 5, int execCount = 3)
        {
            int count = 0;
            while (count <= execCount)
            {
                var tasks = new List<Task>();
                for (int i = 0; i < maxUsers; i++)
                {
                    var task = Task.Factory.StartNew(o =>
                        {
                            using (var dbManager = connectionFactory.CreateDbManager())
                            {
                                action(dbManager);
                            }
                        }, i, TaskCreationOptions.LongRunning);

                    tasks.Add(task);
                }
                Task.WaitAll(tasks.ToArray());
                Thread.Sleep(1000);

                count++;
            }
        }
    }
}