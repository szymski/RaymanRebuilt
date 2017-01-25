using System;
using System.Security.Cryptography.X509Certificates;

namespace RREngine.Engine.Logging
{
    public enum LogType
    {
        Regular,
        Warning,
        Error,
        Exception,
    }

    public interface ILogger
    {
        void Log(LogType type, string message);
        void LogFormatted(LogType type, string message, params string[] args);
        void Log(string message);
        void LogWarning(string message);
        void LogError(string message);
        void LogException(Exception exception);

        void Log(LogType type, string[] tags, string message);
        void LogFormatted(LogType type, string[] tags, string message, params string[] args);
        void Log(string[] tags, string message);
        void LogWarning(string[] tags, string message);
        void LogError(string[] tags, string message);
        void LogException(string[] tags, Exception exception);
    }
}