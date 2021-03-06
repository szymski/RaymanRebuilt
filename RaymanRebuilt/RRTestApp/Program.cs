﻿using System;
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
using Jitter.Collision.Shapes;
using RREngine.Engine.Physics;

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

            RenderableMesh unitMesh = null;

            Texture2D texture = null;

            window.Load += (sender, eventArgs) =>
            {
                scene.SceneRenderer = sceneRenderer;

                #region Asset Loading/Generation

                dragonRenderableMesh = Engine.AssetManager.LoadAsset<ModelAsset>("dragon.obj").GenerateFirstRenderableMesh();

                var learn30fullmesh = Engine.AssetManager.LoadAsset<ModelAsset>("testmap/testmap.dae").GenerateAllRenderableMeshesAndMaterials(true);

                var teapotMesh = Engine.AssetManager.LoadAsset<ModelAsset>("teapot.obj").GenerateFirstRenderableMesh();
                var sphereMesh = Engine.AssetManager.LoadAsset<ModelAsset>("sphere.obj").GenerateFirstRenderableMesh();
                texture = Engine.AssetManager.LoadAsset<TextureAsset>("debug.png").GenerateTexture();
                var texture2 = Engine.AssetManager.LoadAsset<TextureAsset>("textures/rocks.jpg").GenerateTexture();
                var texture2_normal = Engine.AssetManager.LoadAsset<TextureAsset>("textures/rocks_normal.jpg").GenerateTexture();


                var fontAsset = Engine.AssetManager.LoadAsset<FontAsset>("comic.ttf");

                var planeData = Plane.GenerateXY(Vector2.One, Vector2.One, Vector2.One, 0f);
                unitMesh = RenderableMesh.CreateManaged(new Mesh(planeData.Item1, planeData.Item2));
                #endregion

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

                Random rand = new Random();

                #region Camera
                camera = scene.CreateGameObject();
                camera.AddComponent<Transform>().Position = Vector3Directions.Backward * 20f;
                var camComponent = camera.AddComponent<PerspectiveCamera>();
                sceneRenderer.CurrentCamera = camComponent;
                camera.AddComponent<FlyingCamera>();
                #endregion

                if (false)
                {
                    #region Ground plane
                    plane = scene.CreateGameObject();
                    plane.AddComponent<Transform>();
                    RigidBodyComponent planeBody = plane.AddComponent<RigidBodyComponent>();
                    planeBody.Shape = new BoxShape(80, 0, 80);
                    planeBody.Material = new Jitter.Dynamics.Material()
                    {
                        Restitution = 0.1f,
                        StaticFriction = 0.2f,
                        KineticFriction = 0.2f
                    };
                    planeBody.Static = true;

                    plane.AddComponent<MeshRenderer>().Material = new Material()
                    {
                        BaseColor = new Vector4(1f, 1f, 1f, 1f),
                        DiffuseTexture = texture,
                    };
                    var planeGen = plane.AddComponent<PlaneGenerator>();
                    planeGen.TexCoordScaling = Vector2.One * 10f;
                    planeGen.MinBounds = Vector2.One * 40;
                    planeGen.MaxBounds = Vector2.One * 40;

                    #endregion
                }

                #region Falling Spheres

                for (int i = -3; i <= 3; i++)
                {
                    for (int j = -3; j <= 3; j++)
                    {

                        Material boxMat = new Material()
                        {
                            BaseColor = new Vector4((float)rand.NextDouble(), (float)rand.NextDouble(), (float)rand.NextDouble(), 1),
                            // Texture = texture,
                            SpecularPower = 10f,
                        };

                        var box = scene.CreateGameObject();
                        var boxTransform = box.AddComponent<Transform>();
                        boxTransform.Position = new Vector3(i * 0.3f, 15 + (float)rand.NextDouble() * 30, j * 0.4f);
                        boxTransform.Rotation = new Quaternion((float)rand.NextDouble() * Mathf.PI, (float)rand.NextDouble() * Mathf.PI, (float)rand.NextDouble() * Mathf.PI);

                        //transform.Scale *= 0.3f;
                        var boxRenderer = box.AddComponent<MeshRenderer>();
                        boxRenderer.RenderableMesh = sphereMesh;

                        float boxSize = 1f;
                        
                        boxRenderer.Material = boxMat;

                        RigidBodyComponent rigidBody = box.AddComponent<RigidBodyComponent>();
                        rigidBody.Shape = new SphereShape(boxSize);
                        rigidBody.Material = new Jitter.Dynamics.Material()
                        {
                            Restitution = 0.25f,
                            KineticFriction = 0.1f,
                            StaticFriction = 0.1f
                        };
                    }
                }

                #endregion

                if (false)
                {
                    #region Dragons

                    Material mat = new Material()
                    {
                        BaseColor = new Vector4((float)rand.NextDouble(), (float)rand.NextDouble(), (float)rand.NextDouble(), 1),
                        // Texture = texture,
                        SpecularPower = 10f,
                    };

                    dragon = scene.CreateGameObject();
                    var transform = dragon.AddComponent<Transform>();
                    transform.Position = new Vector3(0, 0, 0);

                    //transform.Scale *= 0.3f;
                    var renderer = dragon.AddComponent<MeshRenderer>();
                    renderer.RenderableMesh = dragonRenderableMesh;
                    renderer.Material = mat;


                    #endregion

                    #region Teapots and balls

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
                    #endregion

                }

                // Generate GameObjects for level mesh
                foreach (var learn30mesh in learn30fullmesh)
                    {

                        var learn30gameObject = scene.CreateGameObject();
                        learn30gameObject.AddComponent<Transform>();
                        var learn30renderer = learn30gameObject.AddComponent<MeshRenderer>();
                        learn30renderer.RenderableMesh = learn30mesh.Item1;
                        learn30renderer.Material = learn30mesh.Item2;
                        var collision = learn30gameObject.AddComponent<RigidBodyComponent>();
                        collision.Material = new Jitter.Dynamics.Material();
                        collision.Static = true;
                        collision.Shape = PhysicsUtil.CreateTriangleMeshShapeFromRenderableMesh(learn30mesh.Item1);
                    }
                /*foreach (var mesh in learn_30_meshes) {
                    var learn30gameObject = scene.CreateGameObject();
                    learn30gameObject.AddComponent<Transform>();
                    var learn30renderer = learn30gameObject.AddComponent<MeshRenderer>();
                    learn30renderer.RenderableMesh = mesh;
                    learn30renderer.Material = new Material()
                    {
                        BaseColor = new Vector4(1f, 1f, 1f, 1f),
                        DiffuseTexture = texture,
                    };
                }*/

                /*var light = scene.CreateGameObject();
                light.AddComponent<Transform>().Position = Vector3Directions.Up * 2f;
                var pointLightComponent = light.AddComponent<PointLightComponent>();
                pointLightComponent.Intensity = 50f;
                light.AddComponent<RandomLight>();

                var light2 = scene.CreateGameObject();
                light2.AddComponent<Transform>().Position = Vector3Directions.Up * 2f;
                var pointLightComponent2 = light2.AddComponent<PointLightComponent>();
                pointLightComponent2.Intensity = 50f;
                light2.AddComponent<RandomLight>();
                */
                var dirLight = scene.CreateGameObject();
                dirLight.AddComponent<Transform>().Rotation = Quaternion.FromEulerAngles(0f, Mathf.PI / 2f, -Mathf.PI / 2f);
                var dirLightComponent = dirLight.AddComponent<DirectionalLightComponent>();
                dirLightComponent.Intensity = 1f;
                dirLightComponent.Color = new Vector3(1f, 0.95f, 0.9f);

                #endregion

                sceneRenderer.Init();
                scene.Init();

                sceneRenderer.CubemapTexture = cubemapTexture;
                sceneRenderer.AmbientLightColor = new Vector3(0.8f, 0.8f, 0.8f);
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

                    if (viewport.Keyboard.GetKeyUp(KeyboardKey.K))
                    {
                        Engine.SceneManager.SaveSceneToFile(scene, "test.scene");
                    }

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


            };

            window.Unload += (sender, eventArgs) =>
            {
                Engine.ResourceManager.FreeAllResources();
            };

            window.Run();
        }
    }
}
