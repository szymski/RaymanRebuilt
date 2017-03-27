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
        public RenderableMesh RenderableMesh { get; set; }
        public Material Material { get; set; }

        public MeshRenderer(GameObject owner) : base(owner)
        {
        }

        public override void OnInit()
        {
            _transform = Owner.GetComponent<Transform>();
        }

        protected void LoadMatrix()
        {
            Owner.SceneRenderer.FirstPassShader.ModelMatrix = _transform.ModelMatrix;
        }

        public override void OnRender()
        {
            if (RenderableMesh != null)
            {
                if(Material != null) 
                    Owner.SceneRenderer.FirstPassShader.UseMaterial(Material);

                LoadMatrix();

                GL.Enable(EnableCap.DepthTest);
                GL.DepthFunc(DepthFunction.Less);

                RenderableMesh.Draw();

                GL.Disable(EnableCap.TextureCubeMap);
            }
        }
    }
}
