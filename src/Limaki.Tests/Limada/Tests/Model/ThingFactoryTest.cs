using Limada.Model;
using Limaki.Tests;
using NUnit.Framework;
using System;
using System.IO;
using Limaki.Common;

namespace Limada.Tests.Model {
    public class ThingFactoryTest : DomainTest {

        [Test]
        public void Test() {
            IThingFactory factory = Registry.Factory.Create<IThingFactory>();
            IThing thing = factory.CreateItem();
            Assert.AreEqual(typeof (Thing), thing.GetType());
            Assert.IsInstanceOfType (typeof(IThing), thing);

            thing = factory.CreateItem<object>(null);
            Assert.AreEqual(typeof(Thing), thing.GetType());
            Assert.IsInstanceOfType(typeof(IThing), thing);

            thing = factory.CreateItem<Empty>(new Empty());
            Assert.AreEqual(typeof(Thing), thing.GetType());
            Assert.IsInstanceOfType(typeof(IThing), thing);

            thing = factory.CreateItem(new Empty());
            Assert.AreEqual(typeof(Thing), thing.GetType());
            Assert.IsInstanceOfType(typeof(IThing), thing);

            thing = factory.CreateItem("");
            Assert.IsInstanceOfType(typeof(IThing<string>), thing);
            thing = factory.CreateItem<object>("");
            Assert.IsInstanceOfType(typeof(IThing<string>), thing);

            thing = factory.CreateItem(0);
            Assert.IsInstanceOfType(typeof(INumberThing), thing);
            thing = factory.CreateItem<object>(0);
            Assert.IsInstanceOfType(typeof(INumberThing), thing);

            thing = factory.CreateItem(0f);
            Assert.IsInstanceOfType(typeof(INumberThing), thing);
            thing = factory.CreateItem<object>(0f);
            Assert.IsInstanceOfType(typeof(INumberThing), thing);

            thing = factory.CreateItem(0d);
            Assert.IsInstanceOfType(typeof(INumberThing), thing);
            thing = factory.CreateItem<object>(0d);
            Assert.IsInstanceOfType(typeof(INumberThing), thing);
            
            thing = factory.CreateItem(DateTime.Now);
            Assert.IsInstanceOfType(typeof(INumberThing), thing);
            thing = factory.CreateItem<object>(DateTime.Now);
            Assert.IsInstanceOfType(typeof(INumberThing), thing);

            thing = factory.CreateItem(new Quad16(0,0,0,0));
            Assert.IsInstanceOfType(typeof(INumberThing), thing);
            thing = factory.CreateItem<object>(new Quad16(0, 0, 0, 0));
            Assert.IsInstanceOfType(typeof(INumberThing), thing);

            using (var stream = new MemoryStream()) {
                thing = factory.CreateItem (stream);
                Assert.IsInstanceOfType (typeof (IStreamThing), thing);
                thing = factory.CreateItem<object> (stream);
                Assert.IsInstanceOfType (typeof (IStreamThing), thing);
            }

            thing = factory.CreateItem<Stream>(null);
            Assert.IsInstanceOfType(typeof(IStreamThing), thing);
        }
    }
}