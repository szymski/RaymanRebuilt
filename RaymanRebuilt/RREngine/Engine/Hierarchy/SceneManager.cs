using Google.Protobuf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RREngine.Engine.Hierarchy
{
    public class SceneManager
    {
        public void SaveSceneToFile(Scene scene, string filename)
        {
            Engine.Logger.Log("Writing scene to "+filename);
            long timeStart = System.DateTime.Now.Millisecond;
            try
            {
                using (var outputFile = File.Create(filename)) {
                    scene.Serialize().WriteTo(outputFile);
                }
                float timeDelta = (System.DateTime.Now.Millisecond - timeStart) / 1000.0f;
                Engine.Logger.Log("Finished writing scene in " + timeDelta + " seconds");
            } catch (IOException exception)
            {
                Engine.Logger.LogError("Failed writing to file " + filename + ": " + exception);
            }
        }
    }
}
