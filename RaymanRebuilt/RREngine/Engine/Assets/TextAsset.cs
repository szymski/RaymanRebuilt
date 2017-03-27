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

        public TextAsset(Stream stream) : base(stream)
        {
            StreamReader = new StreamReader(stream);
        }

        public string Text
        {
            get
            {
                StreamReader.BaseStream.Seek(0, SeekOrigin.Begin);
                return StreamReader.ReadToEnd();
            }   
        }
    }
}
