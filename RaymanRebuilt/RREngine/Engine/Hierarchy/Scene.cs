using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assimp;
using RREngine.Engine.Graphics;
using RREngine.Engine.Hierarchy.Components;
using Camera = RREngine.Engine.Hierarchy.Components.Camera;

namespace RREngine.Engine.Hierarchy
{
    /// <summary>
    /// A container and a handler for GameObjects.
    /// </summary>
    public class Scene
    {
        public bool Initialized { get; set; }

        private uint _lastGameObjectId = 0;
        private List<GameObject> _gameObjects = new List<GameObject>();
        public IEnumerable<GameObject> GameObjects => _gameObjects.AsEnumerable();

        public StandardShader StandardShader { get; private set; } // TODO

        public Camera CurrentCamera { get; set; }

        public void Init()
        {
            if (Initialized)
                throw new Exception("Already initialized.");

            foreach (var gameObject in _gameObjects)
                foreach (var component in gameObject.Components)
                    if (component.Enabled)
                        component.OnInit();

            Initialized = true;
        }

        public void Update()
        {
            if (!Initialized)
                throw new Exception("Scene have to be initialized first.");

            foreach (var gameObject in _gameObjects)
                if (gameObject.Enabled)
                    gameObject.Update();
        }

        public void Render()
        {
            if (!Initialized)
                throw new Exception("Scene have to be initialized first.");

            PrepareCamera();

            foreach (var gameObject in _gameObjects)
                if (gameObject.Enabled)
                    gameObject.Render();
        }

        private void PrepareCamera()
        {
            if (CurrentCamera == null)
                return;

            CurrentCamera.LoadMatrix();
        }

        #region Game objects

        public GameObject CreateGameObject()
        {
            var gameObject = new GameObject(this, _lastGameObjectId++); // TODO: Weird things can happen, if we create more than 2^32 - 1 game objects.

            Viewport.Current.Logger.Log(new[] { "scene" }, $"Created a new GameObject id {gameObject.Id}");

            _gameObjects.Add(gameObject);

            return gameObject;
        }

        public void RemoveGameObject(GameObject gameObject)
        {
            Viewport.Current.Logger.Log(new[] { "scene" }, $"Destroying a GameObject id {gameObject.Id}");

            gameObject.OnDestroy();
            _gameObjects.Remove(gameObject);
        }

        #endregion
    }
}
