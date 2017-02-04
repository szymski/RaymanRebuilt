using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RREngine.Engine.Assets
{
    public class AssetManager
    {
        private Dictionary<string, Type> _registeredTypes = new Dictionary<string, Type>();

        private List<Asset> _loadedAssets = new List<Asset>();
        public IEnumerable<Asset> LoadedAssets => _loadedAssets.AsEnumerable();

        public AssetManager()
        {
            RegisterType(".obj", typeof(ModelAsset));
            RegisterType(".dae", typeof(ModelAsset));
            RegisterType(".3ds", typeof(ModelAsset));
            RegisterType(".fbx", typeof(ModelAsset));

            RegisterType(".png", typeof(TextureAsset));
            RegisterType(".jpg", typeof(TextureAsset));
            RegisterType(".bmp", typeof(TextureAsset));
            RegisterType(".tga", typeof(TextureAsset));
        }

        public void RegisterType(string fileExtension, Type type)
        {
            if (!type.IsSubclassOf(typeof(Asset)))
                throw new Exception("The registered type must inherit from Asset.");

            _registeredTypes.Add(fileExtension, type);
        }

        public Asset LoadAsset(string filename)
        {
            Type type = GetTypeFromFileName(filename);

            var stream = File.OpenRead(filename);

            Viewport.Current.Logger.Log(new[] { "asset" }, $"Loading asset type {type} from file {filename}");

            return (Asset)Activator.CreateInstance(type, stream);
        }

        public T LoadAsset<T>(string filename) where T : Asset
        {
            Type type = GetTypeFromFileName(filename);

            if (type != typeof(T))
                throw new Exception($"This asset is of type {type} and cannot be loaded as {typeof(T)}.");

            return (T)LoadAsset(filename);
        }

        public Type GetTypeFromFileName(string filename)
        {
            return _registeredTypes.Select(p => new KeyValuePair<string, Type>?(p))
                .FirstOrDefault(p => filename.EndsWith(p.Value.Key))?.Value ?? typeof(RawAsset);
        }

        public void UnloadAsset(Asset asset)
        {
            _loadedAssets.Remove(asset);
        }

        public static AssetManager Instance = new AssetManager(); // TODO: Is this a good idea?
    }
}
