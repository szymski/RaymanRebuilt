using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RREngine.Engine.Assets
{
    /// <summary>
    /// An asset is an external resource located outside of the engine.
    /// </summary>
    public abstract class Asset
    {
        public string Location { get; set; }

        public Asset(Stream stream)
        {
            
        }
    }
}
