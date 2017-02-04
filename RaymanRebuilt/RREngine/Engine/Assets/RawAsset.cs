using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RREngine.Engine.Assets
{
    public class RawAsset : Asset
    {
        public Stream Stream { get; set; }

        public RawAsset(Stream stream) : base(stream)
        {
            Stream = stream;
        }
    }
}
