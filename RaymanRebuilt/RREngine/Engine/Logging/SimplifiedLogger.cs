using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RREngine.Engine.Logging
{
    /// <summary>
    /// Simplifies ILogger interface, so child classes can be written quicker.
    /// </summary>
    public abstract class SimplifiedLogger : ILogger
    {
        public void Log(LogType type, string message)
        {
            SimplifiedLog(type, new string[0], message);
        }

        public void LogFormatted(LogType type, string message, params string[] args)
        {
            SimplifiedLog(type, new string[0], string.Format(message, args));
        }

        public void Log(string message)
        {
            SimplifiedLog(LogType.Regular, new string[0], message);
        }

        public void LogWarning(string message)
        {
            SimplifiedLog(LogType.Warning, new string[0], message);
        }

        public void LogError(string message)
        {
            SimplifiedLog(LogType.Error, new string[0], message);
        }

        public void LogException(Exception exception)
        {
            SimplifiedLog(LogType.Exception, new string[0], exception.ToString());
        }

        public void Log(LogType type, string[] tags, string message)
        {
            SimplifiedLog(LogType.Regular, tags, message);
        }

        public void LogFormatted(LogType type, string[] tags, string message, params string[] args)
        {
            SimplifiedLog(type, tags, message);
        }

        public void Log(string[] tags, string message)
        {
            SimplifiedLog(LogType.Regular, tags, message);
        }

        public void LogWarning(string[] tags, string message)
        {
            SimplifiedLog(LogType.Warning, tags, message);
        }

        public void LogError(string[] tags, string message)
        {
            SimplifiedLog(LogType.Error, tags, message);
        }

        public void LogException(string[] tags, Exception exception)
        {
            SimplifiedLog(LogType.Exception, tags, exception.ToString());
        }

        protected abstract void SimplifiedLog(LogType type, string[] tags, string message);
    }
}
