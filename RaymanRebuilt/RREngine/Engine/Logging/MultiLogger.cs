using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RREngine.Engine.Logging
{
    public class MultiLogger : ILogger
    {
        public List<ILogger> Loggers { get; } = new List<ILogger>();

        public void Log(LogType type, string message)
        {
            foreach (var logger in Loggers)
                logger.Log(type, message);
        }

        public void LogFormatted(LogType type, string message, params string[] args)
        {
            foreach (var logger in Loggers)
                logger.LogFormatted(type, message, args);
        }

        public void Log(string message)
        {
            foreach (var logger in Loggers)
                logger.Log(message);
        }

        public void LogWarning(string message)
        {
            foreach (var logger in Loggers)
                logger.LogWarning(message);
        }

        public void LogError(string message)
        {
            foreach (var logger in Loggers)
                logger.LogError(message);
        }

        public void LogException(Exception exception)
        {
            foreach (var logger in Loggers)
                logger.LogException(exception);
        }

        public void Log(LogType type, string[] tags, string message)
        {
            foreach (var logger in Loggers)
                logger.Log(type, tags, message);
        }

        public void LogFormatted(LogType type, string[] tags, string message, params string[] args)
        {
            foreach (var logger in Loggers)
                logger.LogFormatted(type, tags, message, args);
        }

        public void Log(string[] tags, string message)
        {
            foreach (var logger in Loggers)
                logger.Log(tags, message);
        }

        public void LogWarning(string[] tags, string message)
        {
            foreach (var logger in Loggers)
                logger.LogWarning(tags, message);
        }

        public void LogError(string[] tags, string message)
        {
            foreach (var logger in Loggers)
                logger.LogError(tags, message);
        }

        public void LogException(string[] tags, Exception exception)
        {
            foreach (var logger in Loggers)
                logger.LogException(tags, exception);
        }
    }
}
