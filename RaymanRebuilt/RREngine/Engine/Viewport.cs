using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Input;
using RREngine.Engine.Graphics;
using RREngine.Engine.Input;
using RREngine.Engine.Logging;

namespace RREngine.Engine
{
    /// <summary>
    /// This class covers everything related to a single viewport.
    /// A viewport has its own instances for time, input, screen classes, etc.
    /// A viewport is self-sufficient and doesn't need other ones to work.
    /// Why separate viewports? Let's suppose we have a level editor and we 
    /// want to preview a model or a material. The best way to do this is by
    /// creating a new viewport and rendering the model there. This new viewport
    /// will have its own input system, its own framerate and stuff, so we can 
    /// focus on actual functionality of specific viewport.
    /// </summary>
    public class Viewport
    {
        private Stopwatch _stopwatch = new Stopwatch();
        private Stopwatch _fpsStopwatch = new Stopwatch();

        public MultiLogger Logger { get; } // TODO: Should each viewport have its own logger?
        public Time Time { get; }
        public Screen Screen { get; }
        public Input.Keyboard Keyboard { get; }
        public Input.Mouse Mouse { get; }
        public ShaderManager ShaderManager { get; set; }

        public Viewport()
        {
            SetAsCurrent();

            Logger = new MultiLogger();
            Logger.Loggers.Add(new ConsoleLogger());
            Logger.Loggers.Add(new FileLogger("log.txt"));

            Logger.Log(new[] { "init" }, "Initializing the viewport");

            Logger.Log(new[] { "init" }, "Initializing Time sub-system");
            Time = new Time();
            Logger.Log(new[] { "init" }, "Initializing Screen sub-system");
            Screen = new Screen();
            Logger.Log(new[] { "init" }, "Initializing Keyboard sub-system");
            Keyboard = new Input.Keyboard();
            Logger.Log(new[] { "init" }, "Initializing Mouse sub-system");
            Mouse = new Input.Mouse();
            ShaderManager = new ShaderManager();

            _stopwatch.Start();
            _fpsStopwatch.Start();
        }

        /// <summary>
        /// Updates Viewport.Current, so that other modules work in proper context.
        /// This function should be called always when drawing or updating the viewport.
        /// </summary>
        public void SetAsCurrent()
        {
            _currentViewport = this;
        }

        #region Events

        public event EventHandler<EventArgs> UpdateFrame;

        public void OnUpdateFrame()
        {
            try
            {
                SetAsCurrent();

                PreUpdate?.Invoke(this, EventArgs.Empty);

                UpdateFrame?.Invoke(this, EventArgs.Empty);
            }
            catch (Exception e)
            {
                Logger.LogException(e);
                throw;
            }

            Time.RealDeltaTime = ((float)_stopwatch.Elapsed.TotalSeconds - Time.Elapsed);
            Time.DeltaTime = Time.RealDeltaTime * Time.TimeScale;
            Time.Elapsed = (float)_stopwatch.Elapsed.TotalSeconds;
        }

        public event EventHandler<EventArgs> RenderFrame;

        private int _frames = 0;
        private float _secondsRest = 0f;

        public void OnRenderFrame()
        {
            SetAsCurrent();

            //try
            //{
                RenderFrame?.Invoke(this, EventArgs.Empty);
                PostUpdate?.Invoke(this, EventArgs.Empty);
            //}
            //catch (Exception e)
            //{
            //    Logger.LogException(e);
            //    throw;
            //}

            // FPS counting
            if (_fpsStopwatch.Elapsed.TotalSeconds >= 1 - _secondsRest)
            {
                _secondsRest = (float)_fpsStopwatch.Elapsed.TotalSeconds - 1f;
                Time.FPS = _frames;
                _fpsStopwatch.Restart();
                _frames = 0;

                Console.WriteLine(Time.FPS);
            }
            _frames++;
        }

        /// <summary>
        /// Called before UpdateFrame. Used by input controllers.
        /// </summary>
        public event EventHandler PreUpdate;

        /// <summary>
        /// Called after RenderFrame. Used by input controllers.
        /// </summary>
        public event EventHandler PostUpdate;

        #endregion

        [ThreadStatic]
        private static Viewport _currentViewport;
        public static Viewport Current => _currentViewport;
    }
}
