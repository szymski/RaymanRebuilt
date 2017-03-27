using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuickFont;
using QuickFont.Configuration;

namespace RREngine.Engine.Assets
{
    public class FontAsset : Asset
    {
        private Stream _stream;
        private Dictionary<float, QFont> _fonts = new Dictionary<float, QFont>();

        public FontAsset(Stream stream) : base(stream)
        {
            _stream = stream;
        }

        public QFont GetFont(float size)
        {
            byte[] data = new byte[_stream.Length];

            _stream.Seek(0, SeekOrigin.Begin);
            _stream.Read(data, 0, (int)_stream.Length);

            var font = new QFont(data, size, new QFontBuilderConfiguration());

            return font;
        }
    }
}
