using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RREngine.Engine.Assets;

namespace RREngineTests
{
    [TestClass]
    public class Assets
    {
        class TestAsset1 : Asset
        {
            public TestAsset1(Stream stream) : base(stream)
            {
            }
        }

        class TestAsset2 : Asset
        {
            public TestAsset2(Stream stream) : base(stream)
            {
            }
        }

        [TestMethod]
        public void test_extension_recognition()
        {
            AssetManager manager = new AssetManager();
            manager.RegisterType(".test", typeof(TestAsset1));
            manager.RegisterType(".another", typeof(TestAsset2));

            Assert.AreEqual(typeof(TestAsset1), manager.GetTypeFromFileName("super.test"));
            Assert.AreEqual(typeof(TestAsset1), manager.GetTypeFromFileName(".test"));

            Assert.AreEqual(typeof(TestAsset2), manager.GetTypeFromFileName(".another"));
            Assert.AreEqual(typeof(TestAsset2), manager.GetTypeFromFileName("derp.another"));

            Assert.AreEqual(typeof(RawAsset), manager.GetTypeFromFileName(".unknown"));
            Assert.AreEqual(typeof(RawAsset), manager.GetTypeFromFileName("unknown2"));

        }
    }
}
