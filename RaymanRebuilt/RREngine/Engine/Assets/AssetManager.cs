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

        private Dictionary<Asset, int> _loadedAssets = new Dictionary<Asset, int>();
        public IEnumerable<Asset> LoadedAssets => _loadedAssets.Select(p => p.Key);

        public AssetManager()
        {
            RegisterType(".txt", typeof(TextAsset));
            RegisterType(".cfg", typeof(TextAsset));
            RegisterType(".vs", typeof(TextAsset));
            RegisterType(".fs", typeof(TextAsset));

            RegisterType(".obj", typeof(ModelAsset));
            RegisterType(".dae", typeof(ModelAsset));
            RegisterType(".3ds", typeof(ModelAsset));
            RegisterType(".fbx", typeof(ModelAsset));

            RegisterType(".png", typeof(TextureAsset));
            RegisterType(".jpg", typeof(TextureAsset));
            RegisterType(".bmp", typeof(TextureAsset));
            RegisterType(".tga", typeof(TextureAsset));

            RegisterType(".ttf", typeof(FontAsset));
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

            var asset = (Asset) Activator.CreateInstance(type, stream);

            asset.Location = filename;

            return asset;
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
    }
}
