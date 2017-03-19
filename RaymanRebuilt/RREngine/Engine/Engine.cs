using RREngine.Engine.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RREngine.Engine
{
    /// <summary>
    /// Here goes structures common for each viewport.
    /// </summary>
    public class Engine
    {
        public MultiLogger Logger { get; }

        private Engine()
        {
            Logger = new MultiLogger();
            Logger.Loggers.Add(new ConsoleLogger());
            Logger.Loggers.Add(new FileLogger("log.txt"));

            Logger.Log(new[] { "init" }, $"Initializing RaymanRebuilt");
        }

        public static void Initialize()
        {
            Instance = new Engine();
        }

        public static Engine Instance { get; private set; }
    }
}
