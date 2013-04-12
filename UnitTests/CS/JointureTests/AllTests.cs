using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using BLToolkit.Data;
using NUnit.Framework;

namespace UnitTests.CS.JointureTests
{
    public abstract partial class AllTests
    {
        private DbConnectionFactory ConnectionFactory;

        protected abstract DbConnectionFactory CreateFactory();

        [TestFixtureSetUp]
        public void Setup()
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