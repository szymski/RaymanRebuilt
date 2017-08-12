using System;
using RREngine.Engine.Protobuf.Scene.SceneSerialization;

namespace RREngine.Engine.Hierarchy.Components
{
    public abstract class Component
    {
        public GameObject Owner { get; private set; }

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

        public Component(GameObject owner)
        {
            Owner = owner;
        }

        #region Events

        /// <summary>
        /// Called when the scene is Scene.Initialize() is called.
        /// </summary>
        public virtual void OnInit() { }

        /// <summary>
        /// Called just before the component is removed from the GameObject. Called along with OnDestroy.
        /// </summary>
        public virtual void OnRemove(bool destroyingGameObject) { }

        /// <summary>
        /// Called just before the GameObject is removed from Scene.
        /// </summary>
        public virtual void OnDestroy() { }

        /// <summary>
        /// Called when the component or GameObject is disabled.
        /// </summary>
        public virtual void OnDisable()
        {

        }

        /// <summary>
        /// Called when the component or GameObject is enabled, along with OnInit.
        /// </summary>
        public virtual void OnEnable()
        {

        }

        /// <summary>
        /// Called every frame.
        /// </summary>
        public virtual void OnUpdate() { }

        /// <summary>
        /// Called every frame. All drawing operations should be done here.MO
        /// </summary>
        public virtual void OnRender() { }

        #endregion

        #region Serialization

        internal ComponentProto Serialize()
        {
            var componentProto = new ComponentProto();
            componentProto.ComponentName = "<UnknownComponent>";
            return componentProto;
        }

        #endregion
    }
}
