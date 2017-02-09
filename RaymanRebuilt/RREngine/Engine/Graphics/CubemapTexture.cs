using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;
using PixelFormat = OpenTK.Graphics.OpenGL.PixelFormat;

namespace RREngine.Engine.Graphics
{
    public class CubemapTexture
    {
        public int Id { get; private set; }
        public int Width { get; private set; } = 0;
        public int Height { get; private set; } = 0;

        private int _previousTextureId = 0; // TODO: Is this necessary?

        /// <summary>
        /// Creates an uninitialized texture.
        /// Use LoadImage or Resize to initialize.
        /// </summary>
        public CubemapTexture()
        {
            GenerateTexture();
        }

        /// <summary>
        /// Creates a new texture with specified size.
        /// </summary>
        public CubemapTexture(int width, int height)
        {
            GenerateTexture();
            Resize(width, height);
        }

        public void Destroy()
        {
            GL.DeleteTexture(Id);
        }

        private void GenerateTexture()
        {
            Id = GL.GenTexture();

            GL.BindTexture(TextureTarget.TextureCubeMap, Id);

            MinFilter = TextureMinFilter.Linear;
            MagFilter = TextureMagFilter.Linear;

            WrapS = TextureWrapMode.ClampToEdge;
            WrapT = TextureWrapMode.ClampToEdge;
            WrapR = TextureWrapMode.ClampToEdge;
        }


        public void LoadImages(Bitmap[] bitmaps)
        {
            if (bitmaps.Length != 6)
                throw new Exception($"Invalid number of cubemap textures provided. Expected 6, got {bitmaps.Length}.");

            BitmapData[] bitmapDatas = new BitmapData[6];

            for (var i = 0; i < 6; i++)
                bitmapDatas[i] = bitmaps[i].LockBits(new Rectangle(0, 0, bitmaps[i].Width, bitmaps[i].Height),
                    ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            var width = bitmaps[0].Width;
            var height = bitmaps[0].Height;

            LoadImages(width, height, bitmapDatas.Select(d => d.Scan0).ToArray(), PixelFormat.Bgra);

            for (var i = 0; i < 6; i++)
                bitmaps[i].UnlockBits(bitmapDatas[i]);
        }

        private static readonly TextureTarget[] CUBEMAP_POSITION =
        {
            TextureTarget.TextureCubeMapPositiveX,
            TextureTarget.TextureCubeMapNegativeX,
            TextureTarget.TextureCubeMapPositiveY,
            TextureTarget.TextureCubeMapNegativeY,
            TextureTarget.TextureCubeMapPositiveZ,
            TextureTarget.TextureCubeMapNegativeZ,
        };

        public void LoadImages(int width, int height, IntPtr[] dataPtrs, PixelFormat format)
        {
            if (dataPtrs.Length != 6)
                throw new Exception($"Invalid number of cubemap textures provided. Expected 6, got {dataPtrs.Length}.");

            Width = width;
            Height = height;

            Bind();

            for (int i = 0; i < 6; i++)
                GL.TexImage2D(CUBEMAP_POSITION[i], 0, PixelInternalFormat.Rgba, width, height, 0, format,
                    PixelType.UnsignedByte, dataPtrs[i]);
        }

        public void Resize(int width, int height)
        {
            if (Width == width && Height == height)
                return;

            Width = width;
            Height = height;

            Bind();
            GL.TexImage2D(TextureTarget.TextureCubeMap, 0, PixelInternalFormat.Rgba, width, height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, (byte[])null);
        }

        #region Texture parameters

        /// <summary>
        /// Note: Texture has to be binded.
        /// </summary>
        public TextureMinFilter MinFilter
        {
            set
            {
                GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureMinFilter, (int)value);
            }
        }

        /// <summary>
        /// Note: Texture has to be binded.
        /// </summary>
        public TextureMagFilter MagFilter
        {
            set
            {
                GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureMagFilter, (int)value);
            }
        }

        /// <summary>
        /// Note: Texture has to be binded.
        /// </summary>
        public TextureWrapMode WrapS
        {
            set
            {
                GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapS, (int)value);
            }
        }

        /// <summary>
        /// Note: Texture has to be binded.
        /// </summary>
        public TextureWrapMode WrapT
        {
            set
            {
                GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapT, (int)value);
            }
        }

        /// <summary>
        /// Note: Texture has to be binded.
        /// </summary>
        public TextureWrapMode WrapR
        {
            set
            {
                GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapR, (int)value);
            }
        }

        #endregion

        public void Bind()
        {
            _previousTextureId = GL.GetInteger(GetPName.Texture2D);
            GL.BindTexture(TextureTarget.TextureCubeMap, Id);
        }

        public void Bind(int unit)
        {
            GL.ActiveTexture(TextureUnit.Texture0 + unit);
            _previousTextureId = GL.GetInteger(GetPName.TextureCubeMap);
            GL.BindTexture(TextureTarget.TextureCubeMap, Id);
        }

        public void Unbind()
        {
            GL.BindTexture(TextureTarget.Texture2D, _previousTextureId);
        }
    }
}
