using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;
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
            GL.MatrixMode(MatrixMode.Modelview);
            var matrix = _transform.ModelMatrix * Owner.Scene.CurrentCamera.ViewMatrix;
            GL.LoadMatrix(ref matrix);
        }

        public override void OnRender()
        {
            LoadMatrix();

            if (Mesh != null)
            {
                GL.Enable(EnableCap.Texture2D);
                GL.Color3(1f, 1f, 1f);
                Mesh.Draw();
            }
        }
    }
}
