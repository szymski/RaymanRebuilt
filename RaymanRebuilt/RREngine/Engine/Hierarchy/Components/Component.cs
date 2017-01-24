using System;

namespace RREngine.Engine.Hierarchy.Components
{
    public abstract class Component
    {
        public GameObject Owner { get; private set; }
        public bool Enabled { get; set; } = true;

        public Component(GameObject owner)
        {
            Owner = owner;
        }

        #region Events

        /// <summary>
        /// Called when the scene is Scene.Initialize() is called. TODO call on GameObject.AddComponent
        /// </summary>
        public virtual void OnInit() { }
        
        /// <summary>
        /// Called just before the component is removed from the GameObject. TODO: Should this be called along with OnDestroy()?
        /// </summary>
        public virtual void OnRemove() { }

        /// <summary>
        /// Called just before the GameObject is removed from Scene.
        /// </summary>
        public virtual void OnDestroy() { }

        /// <summary>
        /// Called every frame.
        /// </summary>
        public virtual void OnUpdate() { }

        /// <summary>
        /// Called every frame.
        /// </summary>
        public virtual void OnRender() { }

        #endregion
    }
}
