using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL4;
using RREngine.Engine;
using RREngine.Engine.Assets;
using RREngine.Engine.Graphics;
using RREngine.Engine.Hierarchy;
using RREngine.Engine.Hierarchy.Components;
using RREngine.Engine.Input;
using RREngine.Engine.Math;
using RREngine.Engine.Objects;
using Mesh = RREngine.Engine.Graphics.Mesh;
using PixelFormat = OpenTK.Graphics.OpenGL4.PixelFormat;
using ShaderType = RREngine.Engine.Graphics.ShaderType;
using Vertex = RREngine.Engine.Graphics.Vertex;

namespace RRTestApp
{
    class Program
    {
        class RandomLight : Component
        {
            private Transform _transform;
            private PointLightComponent _pointLight;

            private Vector3 _startPosition;
            private float _distance = 5f;
            private float _timeFactor = Rng.Instance.GetFloat(0.5f, 1.5f);

            public RandomLight(GameObject owner) : base(owner)
            {
            }

            public override void OnInit()
            {
                _transform = Owner.GetComponent<Transform>();
                _pointLight = Owner.GetComponent<PointLightComponent>();

                _startPosition = _transform.Position;
            }

            public override void OnUpdate()
            {
                var elapsed = Viewport.Current.Time.Elapsed * _timeFactor;

                _transform.Position = _startPosition +
                                      Vector3Directions.Right * Mathf.Sin(elapsed) * _distance +
                                      Vector3Directions.Forward * Mathf.Cos(elapsed) * _distance;

                _pointLight.Color = new Vector3(0.5f + Mathf.Sin(elapsed) * 0.5f, 0.5f + Mathf.Sin(elapsed * 0.33f) * 0.5f, 0.5f + Mathf.Sin(elapsed * 0.77f) * 0.5f);
            }
        }

        static void Main(string[] args)
        {
            Window window = new Window(1280, 720);

            var viewport = window.Viewport;

            Mesh dragonMesh = null;

            Scene scene = new Scene();
            SceneRenderer sceneRenderer = new SceneRenderer(scene);
            GameObject camera, dragon, plane, teapot;

            window.Load += (sender, eventArgs) =>
            {
                scene.SceneRenderer = sceneRenderer;

                dragonMesh = AssetManager.Instance.LoadAsset<ModelAsset>("dragon.obj").GenerateMesh();
                var teapotMesh = AssetManager.Instance.LoadAsset<ModelAsset>("teapot.obj").GenerateMesh();
                var texture = AssetManager.Instance.LoadAsset<TextureAsset>("debug.png").GenerateTexture();

                #region Skybox

                TextureAsset[] skyboxTextures =
                {
                    AssetManager.Instance.LoadAsset<TextureAsset>("skybox/xpos.png"),
                    AssetManager.Instance.LoadAsset<TextureAsset>("skybox/xneg.png"),
                    AssetManager.Instance.LoadAsset<TextureAsset>("skybox/ypos.png"),
                    AssetManager.Instance.LoadAsset<TextureAsset>("skybox/yneg.png"),
                    AssetManager.Instance.LoadAsset<TextureAsset>("skybox/zpos.png"),
                    AssetManager.Instance.LoadAsset<TextureAsset>("skybox/zneg.png"),
                };

                CubemapTexture cubemapTexture = new CubemapTexture();
                cubemapTexture.LoadImages(skyboxTextures.Select(a => a.Bitmap).ToArray());

                #endregion

                #region Scene populating

                camera = scene.CreateGameObject();
                camera.AddComponent<Transform>().Position = Vector3Directions.Backward * 20f;
                var camComponent = camera.AddComponent<PerspectiveCamera>();
                sceneRenderer.CurrentCamera = camComponent;
                camera.AddComponent<FlyingCamera>();

                plane = scene.CreateGameObject();
                plane.AddComponent<Transform>();
                plane.AddComponent<MeshRenderer>().Material = new Material()
                {
                    BaseColor = new Vector4(1f, 1f, 1f, 1f),
                    Texture = texture,
                };
                var planeGen = plane.AddComponent<PlaneGenerator>();
                planeGen.TexCoordScaling = Vector2.One * 10f;
                planeGen.MinBounds = Vector2.One * 10;
                planeGen.MaxBounds = Vector2.One * 10;

                Random rand = new Random();

                for (int i = -2; i < -1; i++)
                {
                    Material mat = new Material()
                    {
                        BaseColor = new Vector4((float)rand.NextDouble(), (float)rand.NextDouble(), (float)rand.NextDouble(), 1),
                        // Texture = texture,
                    };

                    dragon = scene.CreateGameObject();
                    var transform = dragon.AddComponent<Transform>();
                    transform.Position = Vector3Directions.Forward * 10f + Vector3Directions.Left * 4f * i;
                    //transform.Scale *= 0.3f;
                    var renderer = dragon.AddComponent<MeshRenderer>();
                    renderer.Mesh = dragonMesh;
                    renderer.Material = mat;
                    dragon.AddComponent<RotatingComponent>();
                }

                Material mat2 = new Material()
                {
                    BaseColor = new Vector4((float)rand.NextDouble(), (float)rand.NextDouble(), (float)rand.NextDouble(), 1),
                    //BaseColor = new Vector4(0f, 0f, 0f, 1f),
                    Texture = texture,
                    SpecularIntensity = 1f,
                };

                teapot = scene.CreateGameObject();
                var transform2 = teapot.AddComponent<Transform>();
                transform2.Position = Vector3Directions.Up * 2f;
                var renderer2 = teapot.AddComponent<MeshRenderer>();
                renderer2.Mesh = teapotMesh;
                renderer2.Material = mat2;

                var light = scene.CreateGameObject();
                light.AddComponent<Transform>().Position = Vector3Directions.Up * 2f;
                var pointLightComponent = light.AddComponent<PointLightComponent>();
                pointLightComponent.Intensity = 50f;
                light.AddComponent<RandomLight>();

                var light2 = scene.CreateGameObject();
                light2.AddComponent<Transform>().Position = Vector3Directions.Up * 2f;
                var pointLightComponent2 = light2.AddComponent<PointLightComponent>();
                pointLightComponent2.Intensity = 50f;
                light2.AddComponent<RandomLight>();

                var dirLight = scene.CreateGameObject();
                dirLight.AddComponent<Transform>().Rotation = Quaternion.FromEulerAngles(0f, Mathf.PI / 2f, -Mathf.PI / 2f);
                var dirLightComponent = dirLight.AddComponent<DirectionalLightComponent>();
                dirLightComponent.Intensity = 1f;
                dirLightComponent.Color = new Vector3(1f, 0.95f, 0.9f);

                #endregion

                sceneRenderer.Init();
                scene.Init();

                sceneRenderer.CubemapTexture = cubemapTexture;
                //sceneRenderer.StandardShader.AmbientLight = Vector3.One * 0.3f; 
            };

            viewport.Keyboard.KeyDown += (sender, eventArgs) => Console.WriteLine(eventArgs.Key);

            viewport.UpdateFrame += (sender, eventArgs) =>
            {
                if (viewport.Keyboard.GetKeyDown(KeyboardKey.Escape))
                    window.GameWindow.Close();

                if (viewport.Keyboard.GetKeyUp(KeyboardKey.F))
                    Viewport.Current.Screen.IsFullscreen = !Viewport.Current.Screen.IsFullscreen;

                if (viewport.Keyboard.GetKeyUp(KeyboardKey.L))
                {
                    Viewport.Current.Mouse.Locked = !Viewport.Current.Mouse.Locked;
                    Viewport.Current.Mouse.CursorVisible = !Viewport.Current.Mouse.Locked;
                }

                scene.Update();
            };

            viewport.RenderFrame += (sender, eventArgs) =>
            {
                GL.ClearColor(0.05f, 0.05f, 0.1f, 1f);
                GL.Clear(ClearBufferMask.DepthBufferBit | ClearBufferMask.ColorBufferBit);

                sceneRenderer.Render();
            };

            window.Run();
        }
    }
}
