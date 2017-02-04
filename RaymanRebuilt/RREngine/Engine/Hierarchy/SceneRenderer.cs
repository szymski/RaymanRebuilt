using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using RREngine.Engine.Graphics;
using RREngine.Engine.Hierarchy.Components;

namespace RREngine.Engine.Hierarchy
{
    public class SceneRenderer
    {
        public Scene Scene { get; set; }
        public bool Initialized { get; set; }

        public StandardShader StandardShader { get; private set; }
        public Camera CurrentCamera { get; set; }

        public SceneRenderer(Scene scene)
        {
            Scene = scene;
        }

        public void Init()
        {
            if (Initialized)
                throw new Exception("Already initialized.");

            StandardShader = new StandardShader(File.ReadAllText("shaders/test.vs"), File.ReadAllText("shaders/test.fs"));
            Viewport.Current.ShaderManager.AddShader(StandardShader);

            Initialized = true;
        }

        public void Render()
        {
            if (!Initialized)
                throw new Exception("Scene has to be initialized first.");

            Viewport.Current.ShaderManager.BindShader(StandardShader);

            StandardShader.AmbientLight = new Vector3(0.1f, 0.1f, 0.1f);

            PrepareCamera();

            foreach (var gameObject in Scene.GameObjects)
                if (gameObject.Enabled)
                    gameObject.Render();

            Viewport.Current.ShaderManager.UnbindShader();
        }

        private void PrepareCamera()
        {
            CurrentCamera?.Use();
        }
    }
}
