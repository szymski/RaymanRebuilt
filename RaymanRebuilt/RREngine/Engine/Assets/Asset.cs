using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RREngine.Engine.Assets
{
    /// <summary>
    /// An asset is a raw resource has to loaded into memory before it can be used.
    /// </summary>
    public abstract class Asset
    {
        public Asset(Stream stream)
        {
            
        }
    }
}
