using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RREngine.Engine.Assets
{
    public class TextAsset : Asset
    {
        public StreamReader StreamReader { get; }
        private string _text = "";

        public TextAsset(Stream stream) : base(stream)
        {
            StreamReader = new StreamReader(stream);

            StreamReader.BaseStream.Seek(0, SeekOrigin.Begin);
            _text = StreamReader.ReadToEnd();
        }

        private TextAsset(string text) : base(null)
        {
            _text = text;
        }

        public string Text => _text;

        public static TextAsset FromText(string text)
        {
            return FromText(text, "Runtime");
        }

        public static TextAsset FromText(string text, string location)
        {
            return new TextAsset(text)
            {
                Location = location
            };
        }
    }
}
