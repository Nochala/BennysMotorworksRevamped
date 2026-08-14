using System;
using System.IO;

namespace BennysMotorworksRevamped
{
    public static class Logger
    {
        private static readonly object SyncRoot = new object();
        private static bool _sessionPrepared;

        public static bool Enabled => Helper.optLogging;

        public static void Initialize()
        {
            try
            {
                lock (SyncRoot)
                {
                    PrepareSessionFiles();
                }
            }
            catch
            {
            }
        }

        private static string LogDirectory => Path.Combine(AppDomain.CurrentDomain.BaseDirectory);
        private static string LogFilePath => Path.Combine(LogDirectory, "BennysMotorworksRevamped.log");

        public static void Log(object message)
        {
            if (!Enabled)
            {
                return;
            }

            AppendLine(LogFilePath, message);
        }

        private static void AppendLine(string filePath, object message)
        {
            try
            {
                lock (SyncRoot)
                {
                    PrepareSessionFiles();
                    File.AppendAllText(filePath, $"{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}: {message}{Environment.NewLine}");
                }
            }
            catch
            {
            }
        }

        private static void PrepareSessionFiles()
        {
            if (_sessionPrepared)
            {
                return;
            }

            Directory.CreateDirectory(LogDirectory);

            try { File.WriteAllText(LogFilePath, string.Empty); } catch { }

            _sessionPrepared = true;
        }
    }

    public static class logger
    {
        public static void Log(object message) => Logger.Log(message);
    }
}