using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assimp;
using RREngine.Engine.Hierarchy.Components;
using Camera = RREngine.Engine.Hierarchy.Components.Camera;

namespace RREngine.Engine.Hierarchy
{
    /// <summary>
    /// A container and a handler for GameObjects.
    /// </summary>
    public class Scene
    {
        private bool _initialized = false;

        private List<GameObject> _gameObjects = new List<GameObject>();
        public IEnumerable<GameObject> GameObjects => _gameObjects.AsEnumerable();

        public Camera CurrentCamera { get; set; }

        public void Init()
        {
            if (_initialized)
                throw new Exception("Already initialized.");

            foreach (var gameObject in _gameObjects)
                foreach (var component in gameObject.Components)
                    if (component.Enabled)
                        component.OnInit();

            _initialized = true;
        }

        public void Update()
        {
            if (!_initialized)
                throw new Exception("Please call Scene.Init() first.");

            foreach (var gameObject in _gameObjects)
                gameObject.Update();
        }

        public void Render()
        {
            if (!_initialized)
                throw new Exception("Please call Scene.Init() first.");

            PrepareCamera();

            foreach (var gameObject in _gameObjects)
                gameObject.Render();
        }

        private void PrepareCamera()
        {
            if(CurrentCamera == null)
                return;

            CurrentCamera.LoadMatrix();
        }

        #region Game objects

        public GameObject CreateGameObject()
        {
            var gameObject = new GameObject(this);

            // TODO: Logs

            _gameObjects.Add(gameObject);

            return gameObject;
        }

        public void RemoveGameObject(GameObject gameObject)
        {
            // TODO: Logs

            gameObject.OnDestroy();
            _gameObjects.Remove(gameObject);
        }

        #endregion
    }
}
