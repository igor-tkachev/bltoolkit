using System;
using System.Diagnostics;

namespace UnitTests.CS.JointureTests.Tools
{
    public class ExecTimeInfo:IDisposable
    {
        private readonly Stopwatch _stopwatch;

        public ExecTimeInfo()
        {
            _stopwatch = new Stopwatch();
            _stopwatch.Start();
        }

        #region Implementation of IDisposable

        public void Dispose()
        {
            _stopwatch.Stop();
            Console.WriteLine(_stopwatch.Elapsed);
        }

        #endregion
    }
}