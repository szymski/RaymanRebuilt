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
using RREngine.Engine.Graphics.Lights;

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

        public PointLightShader PointLightShader { get; private set; }
        public List<PointLight> PointLights { get; } = new List<PointLight>();

        public CubemapReflectionShader CubemapReflectionShader { get; private set; }

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

            PointLightShader = new PointLightShader(File.ReadAllText("shaders/deferred/pointlight.vs"), File.ReadAllText("shaders/deferred/pointlight.fs"));
            Viewport.Current.ShaderManager.AddShader(PointLightShader);

            CubemapReflectionShader = new CubemapReflectionShader(File.ReadAllText("shaders/deferred/cubemapreflection.vs"), File.ReadAllText("shaders/deferred/cubemapreflection.fs"));
            Viewport.Current.ShaderManager.AddShader(CubemapReflectionShader);

            AfterLightingScene = new RenderTarget(Viewport.Current.Screen.Width, Viewport.Current.Screen.Height);

            Initialized = true;

            GenerateSkyboxMesh();
            GenerateUnitMesh();

            GBuffer = new GBuffer(Viewport.Current.Screen.Width, Viewport.Current.Screen.Height);

            Viewport.Current.Screen.WindowModeChanged += (sender, args) =>
            {
                GBuffer.Resize(args.Width, args.Height);
                AfterLightingScene.Resize(args.Width, args.Height);
            };
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

            //GBuffer.TexturePosition.Bind(0);
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
            RenderPointLights();
            RenderCubemapReflections();

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

        private void RenderPointLights()
        {
            Viewport.Current.ShaderManager.BindShader(PointLightShader);

            PointLightShader.GBuffer = GBuffer;
            PointLightShader.CameraPosition = CurrentCamera.Position;

            PointLightShader.ProjectionMatrix = Matrix4.CreateOrthographic(Viewport.Current.Screen.Width,
                Viewport.Current.Screen.Height, -10f, 10f);
            PointLightShader.ViewMatrix = Matrix4.Identity;
            PointLightShader.ModelMatrix = Matrix4.CreateScale(Viewport.Current.Screen.Width / 2f, -Viewport.Current.Screen.Height / 2f, 1f);

            foreach (var light in PointLights)
            {
                PointLightShader.Color = light.Color;
                PointLightShader.Intensity = light.Intensity;
                PointLightShader.Position = light.Position;
                PointLightShader.Attenuation = light.Attenuation;

                _unitMesh.Draw();
            }
        }

        private void RenderCubemapReflections()
        {
            Viewport.Current.ShaderManager.BindShader(CubemapReflectionShader);

            CubemapReflectionShader.GBuffer = GBuffer;
            CubemapReflectionShader.CameraPosition = CurrentCamera.Position;

            CubemapReflectionShader.ProjectionMatrix = Matrix4.CreateOrthographic(Viewport.Current.Screen.Width,
                Viewport.Current.Screen.Height, -10f, 10f);
            CubemapReflectionShader.ViewMatrix = Matrix4.Identity;
            CubemapReflectionShader.ModelMatrix = Matrix4.CreateScale(Viewport.Current.Screen.Width / 2f, -Viewport.Current.Screen.Height / 2f, 1f);

            CubemapReflectionShader.Texture = CubemapTexture;

            _unitMesh.Draw();
        }
    }
}
