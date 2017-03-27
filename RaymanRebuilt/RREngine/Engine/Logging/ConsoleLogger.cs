using System;
using System.Collections.Generic;
using System.Linq;

namespace RREngine.Engine.Logging
{
    public class ConsoleLogger : SimplifiedLogger
    {
        protected override void SimplifiedLog(LogType type, string[] tags, string message)
        {
            string tagsStr = tags.Any() ? tags.Select(t => $"({t})").Aggregate((a, b) => a + b) : "";

            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write($"[{DateTime.Now.ToLongTimeString()}]");
            Console.ForegroundColor = _logTypeColorLookup[type];
            Console.Write($" {_logTypeStringLookup[type]}");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($" {tagsStr} : {message}");
        }

        private Dictionary<LogType, string> _logTypeStringLookup = new Dictionary<LogType, string>()
        {
            { LogType.Regular, "[REGULAR]" },
            { LogType.Warning, "[WARNING]" },
            { LogType.Error, "[ERROR]" },
            { LogType.Exception, "[EXCEPTION]" },
        };

        private Dictionary<LogType, ConsoleColor> _logTypeColorLookup = new Dictionary<LogType, ConsoleColor>()
        {
            { LogType.Regular, ConsoleColor.Green },
            { LogType.Warning, ConsoleColor.Yellow },
            { LogType.Error, ConsoleColor.Red },
            { LogType.Exception, ConsoleColor.Red },
        };
    }
}