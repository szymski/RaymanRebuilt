using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RREngine.Engine.Hierarchy;
using RREngine.Engine.Hierarchy.Components;

namespace RREngineTests.Hierarchy
{
    [TestClass]
    public class GameObjects
    {
        interface ITestInterface
        {
            
        }

        class TestComponent1 : Component, ITestInterface
        {
            public bool removed = false;

            public TestComponent1(GameObject owner) : base(owner)
            {
            }

            public override void OnRemove()
            {
                removed = true;
            }
        }

        [TestMethod]
        public void TestComponents()
        {
            GameObject obj = new GameObject(new Scene());

            // With one component added
            var component = obj.AddComponent<TestComponent1>();
            Assert.IsNotNull(component);
            Assert.AreEqual(obj, component.Owner);
            Assert.AreEqual(component, obj.GetComponent<TestComponent1>());
            Assert.AreEqual(component, obj.GetComponent<ITestInterface>());
            Assert.AreEqual(component, obj.GetComponents<ITestInterface>().First());

            // Removed component
            obj.RemoveComponent(component);
            Assert.IsTrue(component.removed);
            Assert.IsNull(obj.GetComponent<TestComponent1>());
            Assert.AreEqual(0, obj.Components.Count());
        }
    }
}
