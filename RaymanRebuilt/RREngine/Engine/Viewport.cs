﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;

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

        public Time Time { get; }
        public Screen Screen { get; }

        public Viewport()
        {
            SetAsCurrent();

            Time = new Time();
            Screen = new Screen();

            _stopwatch.Start();
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

        public event EventHandler<ResolutionEventArgs> ChangeResolution;

        /// <summary>
        /// Requests resolution change.
        /// </summary>
        public void OnChangeResolution(ResolutionEventArgs e)
        {
            ChangeResolution?.Invoke(this, e);
        }

        public event EventHandler<ResolutionEventArgs> ResolutionChanged;

        /// <summary>
        /// Called by window when the resolution changes.
        /// </summary>
        public void OnResolutionChanged(ResolutionEventArgs e)
        {
            ResolutionChanged?.Invoke(this, e);
        }

        public event EventHandler<EventArgs> UpdateFrame;

        public void OnUpdateFrame()
        {
            SetAsCurrent();
            UpdateFrame?.Invoke(this, EventArgs.Empty);

            Time.DeltaTime = ((float)_stopwatch.Elapsed.TotalSeconds - Time.Elapsed) * Time.TimeScale;
            Time.Elapsed = (float)_stopwatch.Elapsed.TotalSeconds;
        }

        public event EventHandler<EventArgs> RenderFrame;


        public void OnRenderFrame()
        {
            SetAsCurrent();
            RenderFrame?.Invoke(this, EventArgs.Empty);
        }

        #endregion

        [ThreadStatic]
        private static Viewport _currentViewport;
        public static Viewport Current => _currentViewport;
    }
}
