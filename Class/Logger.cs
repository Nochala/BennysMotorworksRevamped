using System;
using System.IO;

namespace BennysMotorworksRevamped
{
    public static class Logger
    {
        private static readonly object SyncRoot = new object();
        private static bool _sessionPrepared;

        public static bool Enabled => Helper.optLogging;
        public static bool DebugEnabled => Helper.optDebugLogging;

        private static string LogDirectory => AppDomain.CurrentDomain.BaseDirectory;
        private static string LogFilePath => Path.Combine(LogDirectory, "BennysMotorworksRevamped.log");

        public static void Initialize()
        {
            try
            {
                lock (SyncRoot)
                {
                    PrepareSessionFile();
                }
            }
            catch
            {
            }
        }

        public static void Log(object message)
        {
            if (!Enabled)
            {
                return;
            }

            AppendLine(message);
        }

        public static void Debug(object message)
        {
            if (!DebugEnabled)
            {
                return;
            }

            AppendLine("[DEBUG] " + message);
        }

        private static void AppendLine(object message)
        {
            try
            {
                lock (SyncRoot)
                {
                    PrepareSessionFile();
                    File.AppendAllText(
                        LogFilePath,
                        $"{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}: {message}{Environment.NewLine}");
                }
            }
            catch
            {
            }
        }

        private static void PrepareSessionFile()
        {
            if (_sessionPrepared || (!Enabled && !DebugEnabled))
            {
                return;
            }

            Directory.CreateDirectory(LogDirectory);

            File.WriteAllText(LogFilePath, string.Empty);
            _sessionPrepared = true;
        }
    }

    public static class logger
    {
        public static void Log(object message) => Logger.Log(message);
        public static void Debug(object message) => Logger.Debug(message);
    }
}
