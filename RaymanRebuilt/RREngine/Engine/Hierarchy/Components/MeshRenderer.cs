using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL4;
using RREngine.Engine.Graphics;
using OpenTK;
using RREngine.Engine.Math;

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

        public override void OnUpdate()
        {
            if (RenderableMesh != null && Material != null)
            {
                Owner.UsesTransparency = (Material.DiffuseTexture.HasTransparentPixels);

                if (Material.DiffuseTexture.HasTransparentPixels)
                {
                    Vector3 myPos = Owner.GetComponent<Transform>().Position + RenderableMesh.AverageVertex;
                    Vector3 cameraPos = Owner.Scene.SceneRenderer.CurrentCamera.Position;
                    Vector3 delta = Vector3.Subtract(cameraPos, myPos);
                    
                    Owner.RenderOrder = Int32.MaxValue - ((int)Mathf.Round(delta.LengthSquared));
                }
            }
        }

        public override void OnRender()
        {
            if (RenderableMesh != null)
            {
                if (Material != null)
                {
                    Owner.SceneRenderer.FirstPassShader.UseMaterial(Material);
                }
                LoadMatrix();

                GL.Enable(EnableCap.DepthTest);
                GL.DepthFunc(DepthFunction.Less);

                RenderableMesh.Draw();

                GL.Disable(EnableCap.TextureCubeMap);
            }
        }
    }
}
