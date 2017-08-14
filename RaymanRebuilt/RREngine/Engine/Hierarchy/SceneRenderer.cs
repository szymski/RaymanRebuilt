using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.ES10;
using OpenTK.Graphics.OpenGL4;
using RREngine.Engine.Assets;
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
        public bool Initialized { get; set; }
        public Scene Scene { get; set; }

        #region Shaders

        public SkyboxShader SkyboxShader { get; private set; }
        public FirstPassShader FirstPassShader { get; private set; }
        public OrthoShader OrthoShader { get; private set; }
        public AmbientLightShader AmbientLightShader { get; private set; }
        public DirectionalLightShader DirectionalLightShader { get; private set; }
        public PointLightShader PointLightShader { get; private set; }
        public CubemapReflectionShader CubemapReflectionShader { get; private set; }

        #endregion

        #region Rendering properties

        public Camera CurrentCamera { get; set; }

        public GBuffer GBuffer { get; private set; }

        public DirectionalLight DirectionalLight { get; } = new DirectionalLight();
        public Vector3 AmbientLightColor { get; set; } = new Vector3(0.2f, 0.2f, 0.2f);
        public List<PointLight> PointLights { get; } = new List<PointLight>();

        public CubemapTexture CubemapTexture { get; set; }

        public enum RenderingMode
        {
            Final,
            Diffuse,
            Position,
            Normal,
            TexCoord,
            Specular,
            Depth,
        }

        public RenderingMode Mode { get; set; } = RenderingMode.Final;

        #endregion

        private RenderableMesh _skyboxMesh;
        private RenderableMesh _unitMesh;

        public RenderTarget AfterLightingScene { get; set; }

        public SceneRenderer(Scene scene)
        {
            Scene = scene;
        }

        public void Init()
        {
            if (Initialized)
                throw new Exception("Already initialized.");

            LoadShaders();

            AfterLightingScene = RenderTarget.CreateManaged(Viewport.Current.Screen.Width, Viewport.Current.Screen.Height);

            GenerateSkyboxMesh();
            GenerateUnitMesh();

            GBuffer = new GBuffer(Viewport.Current.Screen.Width, Viewport.Current.Screen.Height);

            Viewport.Current.Screen.WindowModeChanged += (sender, args) =>
            {
                GBuffer.Resize(args.Width, args.Height);
                AfterLightingScene.Resize(args.Width, args.Height);
            };

            Initialized = true;
        }

        private void LoadShaders()
        {
            FirstPassShader = new FirstPassShader(Engine.AssetManager.LoadAsset<TextAsset>("shaders/deferred/pass1.vs"), Engine.AssetManager.LoadAsset<TextAsset>("shaders/deferred/pass1.fs"));
            Viewport.Current.ShaderManager.AddShader(FirstPassShader);

            SkyboxShader = new SkyboxShader(Engine.AssetManager.LoadAsset<TextAsset>("shaders/skybox.vs"), Engine.AssetManager.LoadAsset<TextAsset>("shaders/skybox.fs"));
            Viewport.Current.ShaderManager.AddShader(SkyboxShader);

            OrthoShader = new OrthoShader(Engine.AssetManager.LoadAsset<TextAsset>("shaders/ortho.vs"), Engine.AssetManager.LoadAsset<TextAsset>("shaders/ortho.fs"));
            Viewport.Current.ShaderManager.AddShader(OrthoShader);

            AmbientLightShader = new AmbientLightShader(Engine.AssetManager.LoadAsset<TextAsset>("shaders/deferred/ambient.vs"), Engine.AssetManager.LoadAsset<TextAsset>("shaders/deferred/ambient.fs"));
            Viewport.Current.ShaderManager.AddShader(AmbientLightShader);

            DirectionalLightShader = new DirectionalLightShader(Engine.AssetManager.LoadAsset<TextAsset>("shaders/deferred/directionallight.vs"), Engine.AssetManager.LoadAsset<TextAsset>("shaders/deferred/directionallight.fs"));
            Viewport.Current.ShaderManager.AddShader(DirectionalLightShader);

            PointLightShader = new PointLightShader(Engine.AssetManager.LoadAsset<TextAsset>("shaders/deferred/pointlight.vs"), Engine.AssetManager.LoadAsset<TextAsset>("shaders/deferred/pointlight.fs"));
            Viewport.Current.ShaderManager.AddShader(PointLightShader);

            CubemapReflectionShader = new CubemapReflectionShader(Engine.AssetManager.LoadAsset<TextAsset>("shaders/deferred/cubemapreflection.vs"), Engine.AssetManager.LoadAsset<TextAsset>("shaders/deferred/cubemapreflection.fs"));
            Viewport.Current.ShaderManager.AddShader(CubemapReflectionShader);
        }

        private void GenerateSkyboxMesh()
        {
            var plane = Plane.GenerateXY(Vector2.One * 10f, Vector2.One * 10f, Vector2.One, -1);
            var mesh = RenderableMesh.CreateManaged(new Mesh(plane.Item1, plane.Item2));

            _skyboxMesh = mesh;
        }

        private void GenerateUnitMesh()
        {
            var plane = Plane.GenerateXY(Vector2.One, Vector2.One, Vector2.One);
            var mesh = RenderableMesh.CreateManaged(new Mesh(plane.Item1, plane.Item2));

            _unitMesh = mesh;
        }

        public void Render()
        {
            if (!Initialized)
                throw new Exception("Scene has to be initialized first.");

            // Rendering the scene into GBuffer

            GBuffer.Bind();

            GL.ClearColor(0f, 0f, 0f, 0f);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            RenderSceneObjects();

            GBuffer.Unbind();

            RenderLighting();

            // Displaying the GBuffer with lights

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

            switch (Mode)
            {
                case RenderingMode.Final:
                    AfterLightingScene.Texture.Bind(0);
                    break;

                case RenderingMode.Diffuse:
                    GBuffer.TextureDiffuse.Bind(0);
                    break;

                case RenderingMode.Position:
                    GBuffer.TexturePosition.Bind(0);
                    break;

                case RenderingMode.Normal:
                    GBuffer.TextureNormal.Bind(0);
                    break;

                case RenderingMode.TexCoord:
                    GBuffer.TextureTexCoord.Bind(0);
                    break;

                case RenderingMode.Specular:
                    GBuffer.TextureSpecular.Bind(0);
                    break;

                case RenderingMode.Depth:
                    GBuffer.TextureDepth.Bind(0);
                    break;
            }

            OrthoShader.Texture = 0;
            _unitMesh.Draw();
        }

        private void RenderSceneObjects()
        {
            Viewport.Current.ShaderManager.BindShader(FirstPassShader);

            GL.Enable(EnableCap.DepthTest);
            GL.DepthFunc(DepthFunction.Less);

            //GL.Enable(EnableCap.CullFace);
            //GL.CullFace(CullFaceMode.Back);

            PrepareCamera();

            //GL.Enable(EnableCap.TextureCubeMap);
            //StandardShader.CubemapTexture = CubemapTexture;

            var sortedList = Scene.GameObjects.OrderBy(o => {
                return o.RenderOrder;
            }).ToList();

            foreach (var gameObject in sortedList)
            {
                if (gameObject.Enabled)
                    gameObject.Render();
            }

        }

        private void PrepareCamera()
        {
            CurrentCamera?.Use();
        }

        private void RenderSkybox()
        {
            if (CubemapTexture == null)
                return;

            GL.DepthFunc(DepthFunction.Lequal);

            GL.Viewport(0, 0, Viewport.Current.Screen.Width, Viewport.Current.Screen.Height);

            Viewport.Current.ShaderManager.BindShader(SkyboxShader);

            SkyboxShader.ProjectionMatrix = CurrentCamera.ProjectionMatrix;
            SkyboxShader.ViewMatrix = CurrentCamera.ViewMatrix;

            GL.Enable(EnableCap.TextureCubeMap);
            SkyboxShader.CubemapTexture = CubemapTexture;
            _skyboxMesh.Draw();
            GL.Disable(EnableCap.TextureCubeMap);
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
            //RenderCubemapReflections(); TODO: Re-enable this?

            Viewport.Current.ShaderManager.BindShader(OrthoShader);

            GL.Disable(EnableCap.Blend);

            AfterLightingScene.Unbind();
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
