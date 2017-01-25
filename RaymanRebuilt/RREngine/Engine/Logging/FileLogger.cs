using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace RREngine.Engine.Logging
{
    public class FileLogger : SimplifiedLogger, IDisposable
    {
        private TextWriter _writer;

        public FileLogger(string filename)
        {
            _writer = File.CreateText(filename);
        }

        protected override void SimplifiedLog(LogType type, string[] tags, string message)
        {
            string tagsStr = tags.Any() ? tags.Select(t => $"({t})").Aggregate((a, b) => a + b) : "";
            _writer.WriteLine($"[{DateTime.Now.ToShortTimeString()}] {_logTypeStringLookup[type]} {tagsStr} : {message}");
            _writer.Flush();
        }

        private Dictionary<LogType, string> _logTypeStringLookup = new Dictionary<LogType, string>()
        {
            { LogType.Regular, "[REGULAR]" },
            { LogType.Warning, "[WARNING]" },
            { LogType.Error, "[ERROR]" },
            { LogType.Exception, "[EXCEPTION]" },
        };

        private bool _disposed = false;

        public void Dispose()
        {
            if (!_disposed)
            {
                _writer.Close();
                _writer.Dispose();
                _disposed = true;
            }
        }
    }
}