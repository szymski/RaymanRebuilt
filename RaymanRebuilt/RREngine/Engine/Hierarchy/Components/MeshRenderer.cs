using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL4;
using RREngine.Engine.Graphics;

namespace RREngine.Engine.Hierarchy.Components
{
    public class MeshRenderer : Component
    {
        private Transform _transform;
        public Mesh Mesh { get; set; }

        public MeshRenderer(GameObject owner) : base(owner)
        {
        }

        public override void OnInit()
        {
            _transform = Owner.GetComponent<Transform>();
        }

        protected void LoadMatrix()
        {
            var model = _transform.ModelMatrix;
            var view = Owner.Scene.CurrentCamera.ViewMatrix;
            var projection = Owner.Scene.CurrentCamera.ProjectionMatrix;

            Viewport.Current.ShaderManager.CurrentShader.SetUniform("modelMatrix", model);
            Viewport.Current.ShaderManager.CurrentShader.SetUniform("viewMatrix", view);
            Viewport.Current.ShaderManager.CurrentShader.SetUniform("projectionMatrix", projection);
        }

        public override void OnRender()
        {
            if (Mesh != null)
            {
                LoadMatrix();
                GL.Enable(EnableCap.DepthTest);
                GL.DepthFunc(DepthFunction.Less);
                Mesh.Draw();
            }
        }
    }
}
