﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace RREngine.Engine
{
    public class App : GameWindow
    {
        private Stopwatch _stopwatch = new Stopwatch();

        public App(int width, int heigth) : base(width, heigth, GraphicsMode.Default)
        {
            
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            
            _stopwatch.Start();
        }


        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);

            Time.DeltaTime = (float) _stopwatch.Elapsed.TotalSeconds - Time.Elapsed;
            Time.Elapsed = (float)_stopwatch.Elapsed.TotalSeconds;
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            GL.ClearColor(0.1f, 0.1f, 0.1f, 1f);
            GL.Clear(ClearBufferMask.DepthBufferBit | ClearBufferMask.ColorBufferBit);

            SwapBuffers();
        }
    }
}
