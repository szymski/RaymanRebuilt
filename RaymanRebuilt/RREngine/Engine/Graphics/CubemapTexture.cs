using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL4;
using PixelFormat = OpenTK.Graphics.OpenGL4.PixelFormat;

namespace RREngine.Engine.Graphics
{
    public class CubemapTexture : Texture
    {
        public override TextureTarget Target => TextureTarget.TextureCubeMap;

        /// <summary>
        /// Creates an uninitialized texture.
        /// Use LoadImage or Resize to initialize.
        /// </summary>
        public CubemapTexture()
        {
            GenerateTexture();
        }

        protected override void SetupParameters()
        {
            MinFilter = TextureMinFilter.Linear;
            MagFilter = TextureMagFilter.Linear;

            WrapR = TextureWrapMode.MirroredRepeat;
            WrapS = TextureWrapMode.MirroredRepeat;
            WrapT = TextureWrapMode.MirroredRepeat;
        }

        public void Destroy()
        {
            GL.DeleteTexture(Id);
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
    }
}
