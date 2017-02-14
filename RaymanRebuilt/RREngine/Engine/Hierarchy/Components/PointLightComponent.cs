using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using RREngine.Engine.Graphics;
using RREngine.Engine.Graphics.Shaders;

namespace RREngine.Engine.Hierarchy.Components
{
    public class PointLightComponent : Component
    {
        private Transform _transform;

        private PointLight _pointLight = new PointLight();

        public Vector3 Color
        {
            get { return _pointLight.Color; }
            set { _pointLight.Color = value; }
        }

        public float Intensity
        {
            get { return _pointLight.Intensity; }
            set { _pointLight.Intensity = value; }
        }

        public Attenuation Attenuation
        {
            get { return _pointLight.Attenuation; }
            set { _pointLight.Attenuation = value; }
        }

        public PointLightComponent(GameObject owner) : base(owner)
        {
        }

        public override void OnInit()
        {
            _transform = Owner.GetComponent<Transform>();
        }

        public override void OnEnable()
        {
            //Owner.Scene.SceneRenderer.StandardShader.PointLights.Add(_pointLight);
        }

        public override void OnDisable()
        {
            //Owner.Scene.SceneRenderer.StandardShader.PointLights.Remove(_pointLight);
        }

        public override void OnRemove(bool destroyingGameObject)
        {
            //Owner.Scene.SceneRenderer.StandardShader.PointLights.Remove(_pointLight);
        }

        public override void OnUpdate()
        {
            _pointLight.Position = _transform.Position;
        }
    }
}
