using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.ES10;
using OpenTK.Graphics.OpenGL4;
using RREngine.Engine.Graphics;
using RREngine.Engine.Hierarchy.Components;
using RREngine.Engine.Math;
using CullFaceMode = OpenTK.Graphics.OpenGL4.CullFaceMode;
using EnableCap = OpenTK.Graphics.OpenGL4.EnableCap;
using GL = OpenTK.Graphics.OpenGL4.GL;

namespace RREngine.Engine.Hierarchy
{
    public class SceneRenderer
    {
        public Scene Scene { get; set; }
        public bool Initialized { get; set; }

        public StandardShader StandardShader { get; private set; }
        public SkyboxShader SkyboxShader { get; private set; }

        public Camera CurrentCamera { get; set; }

        public CubemapTexture CubemapTexture { get; set; }
        private Mesh _skyboxMesh;

        public SceneRenderer(Scene scene)
        {
            Scene = scene;
        }

        public void Init()
        {
            if (Initialized)
                throw new Exception("Already initialized.");

            StandardShader = new StandardShader(File.ReadAllText("shaders/standard.vs"), File.ReadAllText("shaders/standard.fs"));
            Viewport.Current.ShaderManager.AddShader(StandardShader);

            SkyboxShader = new SkyboxShader(File.ReadAllText("shaders/skybox.vs"), File.ReadAllText("shaders/skybox.fs"));
            Viewport.Current.ShaderManager.AddShader(SkyboxShader);

            Initialized = true;

            GenerateSkyboxMesh();
        }

        private void GenerateSkyboxMesh()
        {
            Vector2 MinBounds = new Vector2(10f, 10f);
            Vector2 MaxBounds = new Vector2(10f, 10f);

            Vertex[] vertices = new[]
            {
                new Vertex()
                {
                    Position = new Vector3(-MinBounds.X, -MinBounds.Y, -1),
                },
                new Vertex()
                {
                    Position = new Vector3(MaxBounds.X, -MinBounds.Y, -1),
                },
                new Vertex()
                {
                    Position = new Vector3(MaxBounds.X, MaxBounds.Y, -1),
                },
                new Vertex()
                {
                    Position = new Vector3(-MinBounds.X, MaxBounds.Y, -1),
                }
            };

            int[] faces = { 2, 1, 0, 3, 2, 0 };

            var mesh = new Mesh(vertices, faces);

            _skyboxMesh = mesh;
        }

        public void Render()
        {
            if (!Initialized)
                throw new Exception("Scene has to be initialized first.");

            //GL.Enable(EnableCap.CullFace);
            GL.CullFace(CullFaceMode.Back);

            RenderSkybox();
            RenderScene();
        }

        private void RenderSkybox()
        {
            GL.DepthFunc(DepthFunction.Lequal);

            GL.Viewport(0, 0, Viewport.Current.Screen.Width, Viewport.Current.Screen.Height);
            Viewport.Current.ShaderManager.BindShader(SkyboxShader);

            if (CubemapTexture != null)
            {
                SkyboxShader.ProjectionMatrix = CurrentCamera.ProjectionMatrix;
                SkyboxShader.ViewMatrix = Matrix4.CreateFromQuaternion(CurrentCamera.ViewMatrix.ExtractRotation());

                GL.Enable(EnableCap.TextureCubeMap);
                SkyboxShader.CubemapTexture = CubemapTexture;
                _skyboxMesh.Draw();
                GL.Disable(EnableCap.TextureCubeMap);
            }
        }

        private void RenderScene()
        {
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
