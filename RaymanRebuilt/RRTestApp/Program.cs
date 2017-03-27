using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL4;
using QuickFont;
using RREngine.Engine;
using RREngine.Engine.Assets;
using RREngine.Engine.Graphics;
using RREngine.Engine.Graphics.Shaders;
using RREngine.Engine.Hierarchy;
using RREngine.Engine.Hierarchy.Components;
using RREngine.Engine.Input;
using RREngine.Engine.Math;
using PixelFormat = OpenTK.Graphics.OpenGL4.PixelFormat;
using ShaderType = RREngine.Engine.Graphics.ShaderType;
using Vertex = RREngine.Engine.Graphics.Vertex;
using Viewport = RREngine.Engine.Viewport;

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
            Engine.Initialize();

            Window window = new Window(1280, 720);
            window.GameWindow.Title = "RaymanRebuilt WIP";

            var viewport = window.Viewport;

            RenderableMesh dragonRenderableMesh = null;

            Scene scene = new Scene();
            SceneRenderer sceneRenderer = new SceneRenderer(scene);
            GameObject camera, dragon, plane, teapot;

            QFont qfont = null;
            QFontDrawing drawing = null;

            RenderableMesh unitMesh = null;

            Texture2D texture = null;

            window.Load += (sender, eventArgs) =>
            {
                scene.SceneRenderer = sceneRenderer;

                dragonRenderableMesh = Engine.AssetManager.LoadAsset<ModelAsset>("dragon.obj").GenerateRenderableMesh();
                var teapotMesh = Engine.AssetManager.LoadAsset<ModelAsset>("teapot.obj").GenerateRenderableMesh();
                var sphereMesh = Engine.AssetManager.LoadAsset<ModelAsset>("sphere.obj").GenerateRenderableMesh();
                texture = Engine.AssetManager.LoadAsset<TextureAsset>("debug.png").GenerateTexture();
                var texture2 = Engine.AssetManager.LoadAsset<TextureAsset>("textures/rocks.jpg").GenerateTexture();
                var texture2_normal = Engine.AssetManager.LoadAsset<TextureAsset>("textures/rocks_normal.jpg").GenerateTexture();

                var fontAsset = Engine.AssetManager.LoadAsset<FontAsset>("comic.ttf");
                qfont = fontAsset.GetFont(20f);
                drawing = new QFontDrawing();

                var planeData = Plane.GenerateXY(Vector2.One, Vector2.One, Vector2.One, 0f);
                unitMesh = RenderableMesh.CreateManaged(new Mesh(planeData.Item1, planeData.Item2));

                #region Skybox

                TextureAsset[] skyboxTextures =
                {
                    Engine.AssetManager.LoadAsset<TextureAsset>("skybox/xpos.png"),
                    Engine.AssetManager.LoadAsset<TextureAsset>("skybox/xneg.png"),
                    Engine.AssetManager.LoadAsset<TextureAsset>("skybox/ypos.png"),
                    Engine.AssetManager.LoadAsset<TextureAsset>("skybox/yneg.png"),
                    Engine.AssetManager.LoadAsset<TextureAsset>("skybox/zpos.png"),
                    Engine.AssetManager.LoadAsset<TextureAsset>("skybox/zneg.png"),
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
                    DiffuseTexture = texture,
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
                        SpecularPower = 10f,
                    };

                    dragon = scene.CreateGameObject();
                    var transform = dragon.AddComponent<Transform>();
                    transform.Position = Vector3Directions.Forward * 10f + Vector3Directions.Left * 4f * i;
                    //transform.Scale *= 0.3f;
                    var renderer = dragon.AddComponent<MeshRenderer>();
                    renderer.RenderableMesh = dragonRenderableMesh;
                    renderer.Material = mat;
                    dragon.AddComponent<RotatingComponent>();
                }

                Material mat2 = new Material()
                {
                    BaseColor = new Vector4((float)rand.NextDouble(), (float)rand.NextDouble(), (float)rand.NextDouble(), 1),
                    //BaseColor = new Vector4(0f, 0f, 0f, 1f),
                    DiffuseTexture = texture,
                    SpecularIntensity = 1f,
                };

                {
                    teapot = scene.CreateGameObject();
                    var transform2 = teapot.AddComponent<Transform>();
                    transform2.Position = Vector3Directions.Up * 2f;
                    var renderer2 = teapot.AddComponent<MeshRenderer>();
                    renderer2.RenderableMesh = teapotMesh;
                    renderer2.Material = mat2;
                }

                {
                    Material mat3 = new Material()
                    {
                        BaseColor = new Vector4(1f, 1f, 1f, 1f),
                        DiffuseTexture = texture2,
                        NormalTexture = texture2_normal,
                        SpecularIntensity = 1f,
                    };

                    GameObject sphere = scene.CreateGameObject();
                    var transform2 = sphere.AddComponent<Transform>();
                    transform2.Position = Vector3Directions.Up * 2f + Vector3Directions.Left * 8f;
                    transform2.Scale = Vector3.One * 2f;
                    var renderer2 = sphere.AddComponent<MeshRenderer>();
                    renderer2.RenderableMesh = sphereMesh;
                    renderer2.Material = mat3;
                    sphere.AddComponent<RotatingComponent>();
                }

                {
                    for (int i = -3; i < 3; i++)
                    {
                        Material mat3 = new Material()
                        {
                            BaseColor = new Vector4(0.8f, 0.8f, 0.8f, 1f),
                            SpecularIntensity = Rng.Instance.GetFloat(0f, 1.2f),
                            SpecularPower = Rng.Instance.GetFloat(1f, 40f),
                        };

                        GameObject sphere = scene.CreateGameObject();
                        var transform2 = sphere.AddComponent<Transform>();
                        transform2.Position = Vector3Directions.Up * 2f + Vector3Directions.Right * 18f +
                                              Vector3Directions.Backward * (i * 5f);
                        transform2.Scale = Vector3.One * 2f;
                        var renderer2 = sphere.AddComponent<MeshRenderer>();
                        renderer2.RenderableMesh = sphereMesh;
                        renderer2.Material = mat3;
                        sphere.AddComponent<RotatingComponent>();
                    }
                }

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

            Vector2 resolutionBeforeChange = Vector2.Zero;

            Dictionary<KeyboardKey, int> keyToNumber = new Dictionary<KeyboardKey, int>()
            {
                { KeyboardKey.Number1, 1 },
                { KeyboardKey.Number2, 2 },
                { KeyboardKey.Number3, 3 },
                { KeyboardKey.Number4, 4 },
                { KeyboardKey.Number5, 5 },
                { KeyboardKey.Number6, 6 },
                { KeyboardKey.Number7, 7 },
                { KeyboardKey.Number8, 8 },
                { KeyboardKey.Number9, 9 },
                { KeyboardKey.Number0, 0 },
            };

            viewport.UpdateFrame += (sender, eventArgs) =>
            {
                if (viewport.Keyboard.GetKeyDown(KeyboardKey.Escape))
                    window.GameWindow.Close();

                if (viewport.Keyboard.GetKeyUp(KeyboardKey.F))
                {
                    if (!Viewport.Current.Screen.IsFullscreen)
                    {
                        resolutionBeforeChange = new Vector2(Viewport.Current.Screen.Width, Viewport.Current.Screen.Height);

                        var device = DisplayDevice.GetDisplay(DisplayIndex.Default);
                        Viewport.Current.Screen.SetResolution(device.Width, device.Height);
                    }
                    else
                        Viewport.Current.Screen.SetResolution((int)resolutionBeforeChange.X, (int)resolutionBeforeChange.Y);

                    Viewport.Current.Screen.IsFullscreen = !Viewport.Current.Screen.IsFullscreen;
                }

                if (viewport.Keyboard.GetKeyUp(KeyboardKey.L))
                {
                    Viewport.Current.Mouse.Locked = !Viewport.Current.Mouse.Locked;
                    //Viewport.Current.Mouse.CursorVisible = !Viewport.Current.Mouse.Locked;
                }

                if (viewport.Keyboard.GetKeyUp(KeyboardKey.P))
                {
                    Engine.Logger.LogWarning("Doing something bad");
                    Engine.ResourceManager.FreeAllResources();
                }

                foreach (var pair in keyToNumber)
                {
                    if (viewport.Keyboard.GetKeyDown(pair.Key))
                        sceneRenderer.Mode = (SceneRenderer.RenderingMode)(pair.Value - 1);
                }

                scene.Update();
            };

            viewport.RenderFrame += (sender, eventArgs) =>
            {
                GL.ClearColor(0.05f, 0.05f, 0.1f, 1f);
                GL.Clear(ClearBufferMask.DepthBufferBit | ClearBufferMask.ColorBufferBit);

                sceneRenderer.Render();



                var projMatrix = Matrix4.CreateOrthographic(viewport.Screen.Width, viewport.Screen.Height, -1f, 1f);
                sceneRenderer.OrthoShader.ProjectionMatrix = projMatrix;
                sceneRenderer.OrthoShader.ViewMatrix = Matrix4.CreateTranslation(new Vector3(viewport.Screen.Size / 2f));
                sceneRenderer.OrthoShader.ModelMatrix = Matrix4.CreateScale(50f);
                sceneRenderer.OrthoShader.Texture = 0;

                viewport.ShaderManager.BindShader(sceneRenderer.OrthoShader);

                GL.Enable(EnableCap.Texture2D);
                texture.Bind(0);

                //unitMesh.Draw();

                drawing.ProjectionMatrix = projMatrix;

                drawing.DrawingPrimitives.Clear();
                drawing.Print(qfont, $"FPS: {viewport.Time.AverageFPS}", new Vector3(-viewport.Screen.Width / 2f + 5, viewport.Screen.Height / 2f - 5, 0f), QFontAlignment.Left);

                drawing.RefreshBuffers();
                drawing.Draw();
            };

            window.Unload += (sender, eventArgs) =>
            {
                Engine.ResourceManager.FreeAllResources();
            };

            window.Run();
        }
    }
}
