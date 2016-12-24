using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RREngine.Engine;

namespace RRTestApp
{
    class Program
    {
        static void Main(string[] args)
        {
            App app = new App(800, 600);
            app.Run();
        }
    }
}
