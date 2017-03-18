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
using RREngine.Engine.Graphics.Shaders;
using RREngine.Engine.Graphics.Shaders.Deferred;
using RREngine.Engine.Hierarchy.Components;
using RREngine.Engine.Math;
using BlendingFactorDest = OpenTK.Graphics.OpenGL4.BlendingFactorDest;
using BlendingFactorSrc = OpenTK.Graphics.OpenGL4.BlendingFactorSrc;
using ClearBufferMask = OpenTK.Graphics.OpenGL4.ClearBufferMask;
using CullFaceMode = OpenTK.Graphics.OpenGL4.CullFaceMode;
using EnableCap = OpenTK.Graphics.OpenGL4.EnableCap;
using GL = OpenTK.Graphics.OpenGL4.GL;

namespace RREngine.Engine.Hierarchy
{
    public class SceneRenderer
    {
        public Scene Scene { get; set; }
        public bool Initialized { get; set; }

        public FirstPassShader FirstPassShader { get; private set; }
        public SkyboxShader SkyboxShader { get; private set; }

        public Camera CurrentCamera { get; set; }

        public CubemapTexture CubemapTexture { get; set; }
        private Mesh _skyboxMesh;

        public OrthoShader OrthoShader { get; private set; }
        public GBuffer GBuffer { get; private set; }
        private Mesh _unitMesh;

        public Vector3 AmbientLightColor { get; set; } = new Vector3(0.2f, 0.2f, 0.2f);
        public AmbientLightShader AmbientLightShader { get; private set; }

        public DirectionalLight DirectionalLight { get; } = new DirectionalLight();
        public DirectionalLightShader DirectionalLightShader { get; private set; }

        public RenderTarget LightRenderTarget;

        public RenderTarget AfterLightingScene;

        public SceneRenderer(Scene scene)
        {
            Scene = scene;
        }

        public void Init()
        {
            if (Initialized)
                throw new Exception("Already initialized.");

            FirstPassShader = new FirstPassShader(File.ReadAllText("shaders/deferred/pass1.vs"), File.ReadAllText("shaders/deferred/pass1.fs"));
            Viewport.Current.ShaderManager.AddShader(FirstPassShader);

            SkyboxShader = new SkyboxShader(File.ReadAllText("shaders/skybox.vs"), File.ReadAllText("shaders/skybox.fs"));
            Viewport.Current.ShaderManager.AddShader(SkyboxShader);

            OrthoShader = new OrthoShader(File.ReadAllText("shaders/ortho.vs"), File.ReadAllText("shaders/ortho.fs"));
            Viewport.Current.ShaderManager.AddShader(OrthoShader);

            AmbientLightShader = new AmbientLightShader(File.ReadAllText("shaders/deferred/ambient.vs"), File.ReadAllText("shaders/deferred/ambient.fs"));
            Viewport.Current.ShaderManager.AddShader(AmbientLightShader);

            DirectionalLightShader = new DirectionalLightShader(File.ReadAllText("shaders/deferred/directionallight.vs"), File.ReadAllText("shaders/deferred/directionallight.fs"));
            Viewport.Current.ShaderManager.AddShader(DirectionalLightShader);

            LightRenderTarget = new RenderTarget(Viewport.Current.Screen.Width, Viewport.Current.Screen.Height);
            AfterLightingScene = new RenderTarget(Viewport.Current.Screen.Width, Viewport.Current.Screen.Height);

            Initialized = true;

            GenerateSkyboxMesh();
            GenerateUnitMesh();

            GBuffer = new GBuffer(Viewport.Current.Screen.Width, Viewport.Current.Screen.Height);
        }

        private void GenerateSkyboxMesh()
        {
            var plane = Plane.GenerateXY(Vector2.One * 10f, Vector2.One * 10f, Vector2.One, -1);
            var mesh = new Mesh(plane.Item1, plane.Item2);

            _skyboxMesh = mesh;
        }

        private void GenerateUnitMesh()
        {
            var plane = Plane.GenerateXY(Vector2.One, Vector2.One, Vector2.One);
            var mesh = new Mesh(plane.Item1, plane.Item2);

            _unitMesh = mesh;
        }

        public void Render()
        {
            if (!Initialized)
                throw new Exception("Scene has to be initialized first.");

            GBuffer.Bind();

            GL.ClearColor(0f, 0f, 0f, 0f);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            //GL.Enable(EnableCap.CullFace);
            //GL.CullFace(CullFaceMode.Back);

            RenderSceneObjects();

            GBuffer.Unbind();

            RenderLighting();

            // Ortho

            GL.Viewport(0, 0, Viewport.Current.Screen.Width, Viewport.Current.Screen.Height);

            GL.ClearColor(0f, 0f, 0f, 0f);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            RenderSkybox();

            GL.DepthFunc(DepthFunction.Lequal);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactorSrc.One, BlendingFactorDest.OneMinusSrcAlpha);

            Viewport.Current.ShaderManager.BindShader(OrthoShader);

            OrthoShader.ProjectionMatrix = Matrix4.CreateOrthographic(Viewport.Current.Screen.Width,
                Viewport.Current.Screen.Height, -10f, 10f);
            OrthoShader.ViewMatrix = Matrix4.Identity;
            OrthoShader.ModelMatrix = Matrix4.CreateScale(Viewport.Current.Screen.Width / 2f, -Viewport.Current.Screen.Height / 2f, 1f);

           // GBuffer.TextureNormal.Bind(0); // TODO: Normals and positions don't respect zbuffer
            AfterLightingScene.Texture.Bind(0);
            OrthoShader.Texture = 0;
            _unitMesh.Draw();
        }

        private void RenderSceneObjects()
        {
            Viewport.Current.ShaderManager.BindShader(FirstPassShader);

            GL.Enable(EnableCap.DepthTest);
            GL.DepthFunc(DepthFunction.Less);

            PrepareCamera();

            //GL.Enable(EnableCap.TextureCubeMap);
            //StandardShader.CubemapTexture = CubemapTexture;

            foreach (var gameObject in Scene.GameObjects)
                if (gameObject.Enabled)
                    gameObject.Render();

            Viewport.Current.ShaderManager.UnbindShader();
        }

        private void PrepareCamera()
        {
            CurrentCamera?.Use();
        }

        private void RenderSkybox()
        {
            GL.DepthFunc(DepthFunction.Lequal);

            GL.Viewport(0, 0, Viewport.Current.Screen.Width, Viewport.Current.Screen.Height);
            Viewport.Current.ShaderManager.BindShader(SkyboxShader);

            if (CubemapTexture != null)
            {
                SkyboxShader.ProjectionMatrix = CurrentCamera.ProjectionMatrix;
                SkyboxShader.ViewMatrix = Matrix4.CreateFromQuaternion(CurrentCamera.ViewMatrix.ExtractRotation()) * Matrix4.CreateTranslation(0, 0, 0);

                GL.Enable(EnableCap.TextureCubeMap);
                SkyboxShader.CubemapTexture = CubemapTexture;
                _skyboxMesh.Draw();
                GL.Disable(EnableCap.TextureCubeMap);
            }
        }

        private void RenderLighting()
        {
            // Rendering scene after lighting

            Viewport.Current.ShaderManager.BindShader(OrthoShader);
            AfterLightingScene.Bind();

            GL.ClearColor(0f, 0f, 0f, 0f);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            GL.DepthFunc(DepthFunction.Always);
            GL.Enable(EnableCap.Texture2D);

            GL.Enable(EnableCap.Blend);

            GL.BlendFunc(BlendingFactorSrc.One, BlendingFactorDest.One);
            RenderAmbientLight();
            RenderDirectionalLight();

            Viewport.Current.ShaderManager.BindShader(OrthoShader);

            GL.Disable(EnableCap.Blend);

            AfterLightingScene.Unbind();

            Viewport.Current.ShaderManager.UnbindShader();
        }

        private void RenderAmbientLight()
        {
            Viewport.Current.ShaderManager.BindShader(AmbientLightShader);

            AmbientLightShader.ProjectionMatrix = Matrix4.CreateOrthographic(Viewport.Current.Screen.Width,
                Viewport.Current.Screen.Height, -10f, 10f);
            AmbientLightShader.ViewMatrix = Matrix4.Identity;
            AmbientLightShader.ModelMatrix = Matrix4.CreateScale(Viewport.Current.Screen.Width / 2f, -Viewport.Current.Screen.Height / 2f, 1f);

            AmbientLightShader.GBuffer = GBuffer;
            AmbientLightShader.Color = AmbientLightColor;

            _unitMesh.Draw();
        }

        private void RenderDirectionalLight()
        {
            Viewport.Current.ShaderManager.BindShader(DirectionalLightShader);

            DirectionalLightShader.ProjectionMatrix = Matrix4.CreateOrthographic(Viewport.Current.Screen.Width,
                Viewport.Current.Screen.Height, -10f, 10f);
            DirectionalLightShader.ViewMatrix = Matrix4.Identity;
            DirectionalLightShader.ModelMatrix = Matrix4.CreateScale(Viewport.Current.Screen.Width / 2f, -Viewport.Current.Screen.Height / 2f, 1f);

            DirectionalLightShader.GBuffer = GBuffer;
            DirectionalLightShader.Color = DirectionalLight.Color;
            DirectionalLightShader.Intensity = DirectionalLight.Intensity;
            DirectionalLightShader.Direction = DirectionalLight.Direction;
            DirectionalLightShader.CameraPosition = CurrentCamera.Position;

            _unitMesh.Draw();
        }
    }
}
