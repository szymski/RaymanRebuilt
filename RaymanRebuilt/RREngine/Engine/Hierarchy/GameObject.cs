using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using RREngine.Engine.Hierarchy.Components;

namespace RREngine.Engine.Hierarchy
{
    public sealed class GameObject
    {
        public Scene Scene { get; }
        public SceneRenderer SceneRenderer { get; set; }
        public uint Id { get; }
        public string Name { get; set; }

        private bool _enabled = true;
        public bool Enabled
        {
            get { return _enabled; }
            set
            {
                if (value != _enabled)
                    if (value)
                        OnEnable();
                    else
                        OnDisable();

                _enabled = value;
            }
        }

        private List<Component> _components = new List<Component>();
        public IEnumerable<Component> Components => _components.AsEnumerable();

        public DateTime TimeCreated { get; } = DateTime.Now;

        public GameObject(Scene scene, uint id)
        {
            Scene = scene;
            Id = id;
        }

        public void Update()
        {
            foreach (var component in _components)
                if (component.Enabled)
                    component.OnUpdate();
        }

        public void Render()
        {
            foreach (var component in _components)
                if (component.Enabled)
                    component.OnRender();
        }

        #region Components

        /// <summary>
        /// Creates a component of specific type and adds it to the object.
        /// </summary>
        public T AddComponent<T>(bool enabled = true) where T : Component
        {
            T instance = (T)Activator.CreateInstance(typeof(T), this);
            _components.Add(instance);

            instance.Enabled = enabled;

            if (Scene.Initialized)
            {
                instance.OnInit();

                if (enabled)
                    instance.OnEnable();
            }

            return instance;
        }

        /// <summary>
        /// Removes specified component from from the object.
        /// Throws an exception, if no such component was attached.
        /// </summary>
        public void RemoveComponent(Component component)
        {
            component.OnRemove(false);

            if (!_components.Remove(component))
                throw new Exception("No such component attached to this object.");
        }

        /// <summary>
        /// Gets an array of attached components of specified type.
        /// </summary>
        public T[] GetComponents<T>() where T : class
        {
            return _components.Where(c => c is T).Cast<T>().ToArray();
        }

        /// <summary>
        /// Gets first component of specified type.
        /// </summary>
        public T GetComponent<T>() where T : class
        {
            return _components.FirstOrDefault(c => c is T) as T;
        }

        #endregion

        #region Events

        public void OnInit()
        {
            foreach (Component component in _components)
            {
                component.OnInit();

                if (component.Enabled)
                    component.OnEnable();
            }
        }

        public void OnDestroy()
        {
            foreach (Component component in _components)
            {
                component.OnRemove(true);
                component.OnDestroy();
            }
        }

        public void OnEnable()
        {
            foreach (Component component in _components)
                component.OnEnable();
        }

        public void OnDisable()
        {
            foreach (Component component in _components)
                component.OnDisable();
        }

        #endregion
    }
}
