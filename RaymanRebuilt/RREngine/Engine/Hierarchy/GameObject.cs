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

        private List<Component> _components = new List<Component>();
        public IEnumerable<Component> Components => _components.AsEnumerable();

        public DateTime TimeCreated { get; } = DateTime.Now;

        public GameObject(Scene scene)
        {
            Scene = scene;
        }

        public void Update()
        {
            foreach (var component in _components)
                if(component.Enabled)
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
        public T AddComponent<T>() where T : Component
        {
            T instance = (T)Activator.CreateInstance(typeof(T), this);
            _components.Add(instance);

            return instance;
        }

        public void RemoveComponent(Component component)
        {
            component.OnRemove();

            if(!_components.Remove(component))
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
                component.OnInit();
        }

        public void OnDestroy()
        {
            foreach (Component component in _components)
                component.OnRemove();
        }

        #endregion
    }
}
