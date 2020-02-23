using System;

namespace AssemblyCSharp
{
/// <summary>
/// Log class for tracing and debugging purpose.
/// </summary>
    public static class Log
    {
        private const string DEFAULT_ASSERT_MESSAGE = "ASSERT FAIL!";

        public enum LogTypes
        {
            Trace,
            Warning,
            Error,
        }

        public delegate void LogHandler(LogTypes type, string message);
        public static event LogHandler OnLog;

        public static void Trace(string format, params object[] objs)
        {
            if (OnLog != null) OnLog(LogTypes.Trace, String.Format(format, objs));
        }

        public static void Error(string format, params object[] objs)
        {
            if (OnLog != null) OnLog(LogTypes.Error, String.Format(format, objs));
        }

        public static void Assert(bool condition)
        {
            Assert(condition, DEFAULT_ASSERT_MESSAGE);
        }

        public static void Assert(bool condition, string format, params object[] objs)
        {
            if (condition)
                return;

            if (OnLog != null) OnLog(LogTypes.Error, String.Format(format, objs));
        }

        public static void Warning(string format, params object[] objs)
        {
            if (OnLog != null) OnLog(LogTypes.Warning, String.Format(format, objs));
        }
    }
}
