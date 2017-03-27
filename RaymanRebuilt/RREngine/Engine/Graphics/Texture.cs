using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL4;
using RREngine.Engine.Resources;

namespace RREngine.Engine.Graphics
{
    public abstract class Texture : Resource
    {
        public int Id { get; protected set; }
        public int Width { get; protected set; } = 0;
        public int Height { get; protected set; } = 0;

        public abstract TextureTarget Target { get; }
        public PixelInternalFormat PixelInternalFormat { get; protected set; } = PixelInternalFormat.Rgba;
        public PixelType PixelType { get; protected set; } = PixelType.UnsignedByte;
        public PixelFormat PixelFormat { get; protected set; } = PixelFormat.Rgba;

        /// <summary>
        /// Generates OpenGL texture ands sets filter and wrap properties.
        /// </summary>
        protected virtual void GenerateTexture()
        {
            Id = GL.GenTexture();

            GL.BindTexture(Target, Id);

            SetupParameters();
        }

        protected virtual void SetupParameters()
        {
            MinFilter = TextureMinFilter.Linear;
            MagFilter = TextureMagFilter.Linear;

            WrapS = TextureWrapMode.Repeat;
            WrapT = TextureWrapMode.Repeat;
            WrapR = TextureWrapMode.Repeat;
        }

        public override void Destroy()
        {
            GL.DeleteTexture(Id);
        }

        public virtual void Resize(int width, int height)
        {
            if (Width == width && Height == height)
                return;

            Width = width;
            Height = height;

            Bind();
            GL.TexImage2D(Target, 0, PixelInternalFormat, width, height, 0, PixelFormat, PixelType, IntPtr.Zero);
        }

        #region Texture parameters

        /// <summary>
        /// Note: Texture has to be binded.
        /// </summary>
        public TextureMinFilter MinFilter
        {
            set
            {
                GL.TexParameter(Target, TextureParameterName.TextureMinFilter, (int)value);
            }
        }

        /// <summary>
        /// Note: Texture has to be binded.
        /// </summary>
        public TextureMagFilter MagFilter
        {
            set
            {
                GL.TexParameter(Target, TextureParameterName.TextureMagFilter, (int)value);
            }
        }

        /// <summary>
        /// Note: Texture has to be binded.
        /// </summary>
        public TextureWrapMode WrapS
        {
            set
            {
                GL.TexParameter(Target, TextureParameterName.TextureWrapS, (int)value);
            }
        }

        /// <summary>
        /// Note: Texture has to be binded.
        /// </summary>
        public TextureWrapMode WrapT
        {
            set
            {
                GL.TexParameter(Target, TextureParameterName.TextureWrapT, (int)value);
            }
        }

        /// <summary>
        /// Note: Texture has to be binded.
        /// </summary>
        public TextureWrapMode WrapR
        {
            set
            {
                GL.TexParameter(Target, TextureParameterName.TextureWrapR, (int)value);
            }
        }

        #endregion

        #region Binding

        public void Bind()
        {
            GL.BindTexture(Target, Id);
        }

        public void Bind(int unit)
        {
            GL.ActiveTexture(TextureUnit.Texture0 + unit);
            GL.BindTexture(Target, Id);
        }

        public void Unbind()
        {
            GL.BindTexture(Target, 0);
        }

        #endregion
    }
}
