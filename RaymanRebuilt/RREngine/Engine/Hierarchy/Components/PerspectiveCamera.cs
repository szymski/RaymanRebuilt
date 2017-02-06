using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL4;
using RREngine.Engine.Math;

namespace RREngine.Engine.Hierarchy.Components
{
    public class PerspectiveCamera : Camera
    {
        private Transform _transform;

        public float FOV { get; set; } = 90f * Mathf.DegToRad;
        public float ZNear { get; set; } = 0.1f;
        public float ZFar { get; set; } = 1000f;

        public PerspectiveCamera(GameObject owner) : base(owner)
        {
        }

        public override void OnInit()
        {
            _transform = Owner.GetComponent<Transform>();
        }

        public override Matrix4 ProjectionMatrix
            => Matrix4.CreatePerspectiveFieldOfView(FOV,
                Viewport.Current.Screen.Width / (float)Viewport.Current.Screen.Height,
                ZNear,
                ZFar);

        public override Matrix4 ViewMatrix
            => _transform.ModelMatrix.Inverted();

        public override void Use()
        {
            GL.Viewport(0, 0, Viewport.Current.Screen.Width, Viewport.Current.Screen.Height);
            Owner.SceneRenderer.StandardShader.ProjectionMatrix = ProjectionMatrix;
            Owner.SceneRenderer.StandardShader.ViewMatrix = ViewMatrix;
            Owner.SceneRenderer.StandardShader.CameraPosition = _transform.Position;
        }
    }
}
