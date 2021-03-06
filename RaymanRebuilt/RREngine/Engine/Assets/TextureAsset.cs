﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RREngine.Engine.Graphics;

namespace RREngine.Engine.Assets
{
    public class TextureAsset : Asset
    {
        private Stream _stream;
        public bool isPartlyTransparent { get; set; } = false;

        public TextureAsset(Stream stream) : base(stream)
        {
            _stream = stream;  
        }

        public Bitmap Bitmap => new Bitmap(new Bitmap(_stream));

        public Texture2D GenerateTexture()
        {
            var bitmap = new Bitmap(_stream);
            var locked = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height),
                ImageLockMode.ReadOnly, PixelFormat.Format32bppPArgb);

            Texture2D texture = Texture2D.CreateManaged();
            texture.LoadImage(bitmap.Width, bitmap.Height, locked.Scan0, OpenTK.Graphics.OpenGL4.PixelFormat.Bgra);


            bitmap.UnlockBits(locked);

            return texture;
        }
    }
}
