using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using RREngine.Engine.Graphics;
using RREngine.Engine.Math;

namespace RREngine.Engine.Hierarchy.Components
{
    public class DirectionalLightComponent : Component
    {
        private Transform _transform;

        public Vector3 Color { get; set; } = new Vector3(1f, 1f, 1f);
        public float Intensity { get; set; } = 1f;

        public DirectionalLightComponent(GameObject owner) : base(owner)
        {
        }

        public override void OnInit()
        {
            _transform = Owner.GetComponent<Transform>();
        }

        public override void OnDisable()
        {
            Owner.Scene.SceneRenderer.StandardShader.DirectionalLight.Intensity = 0f;
        }

        public override void OnRemove(bool destroyingGameObject)
        {
            Owner.Scene.SceneRenderer.StandardShader.DirectionalLight.Intensity = 0f;
        }

        public override void OnUpdate()
        {
            Owner.Scene.SceneRenderer.StandardShader.DirectionalLight.Direction = _transform.Rotation * Vector3Directions.Forward;
            Owner.Scene.SceneRenderer.StandardShader.DirectionalLight.Color = Color;
            Owner.Scene.SceneRenderer.StandardShader.DirectionalLight.Intensity = Intensity;
        }
    }
}
