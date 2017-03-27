using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RREngine.Engine.Resources
{
    /// <summary>
    /// This class represents every unmanaged resource in the memory.
    /// Every allocated texture, mesh, sound, etc. should be an instance of this class.
    /// </summary>
    public abstract class Resource
    {
        public abstract void Destroy();
    }
}
