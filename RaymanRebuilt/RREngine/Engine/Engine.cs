using RREngine.Engine.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using RREngine.Engine.Assets;
using RREngine.Engine.Resources;

namespace RREngine.Engine
{
    /// <summary>
    /// Here goes structures common for each viewport.
    /// </summary>
    public static class Engine
    {
        public static bool Initialized { get; private set; }

        public static MultiLogger Logger { get; private set; }
        public static AssetManager AssetManager { get; private set; }
        public static ResourceManager ResourceManager { get; private set; }

        public static void Initialize()
        {
            if(Initialized)
                throw new Exception("Already initialized.");

            Logger = new MultiLogger();
            Logger.Loggers.Add(new ConsoleLogger());
            Logger.Loggers.Add(new FileLogger("log.txt"));

            Logger.Log(new[] { "init" }, $"Initializing RaymanRebuilt");

            AssetManager = new AssetManager();
            ResourceManager = new ResourceManager();

            Initialized = true;
        }
    }
}
