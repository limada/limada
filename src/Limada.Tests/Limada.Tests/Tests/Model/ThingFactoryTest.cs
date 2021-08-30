#if !__ANDROID__
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
            Assert.IsInstanceOf (typeof(IThing), thing);

            thing = factory.CreateItem<object>(null);
            Assert.AreEqual(typeof(Thing), thing.GetType());
            Assert.IsInstanceOf (typeof(IThing), thing);

            thing = factory.CreateItem<Empty>(new Empty());
            Assert.AreEqual(typeof(Thing), thing.GetType());
            Assert.IsInstanceOf (typeof(IThing), thing);

            thing = factory.CreateItem(new Empty());
            Assert.AreEqual(typeof(Thing), thing.GetType());
            Assert.IsInstanceOf (typeof(IThing), thing);

            thing = factory.CreateItem("");
            Assert.IsInstanceOf (typeof(IThing<string>), thing);
            thing = factory.CreateItem<object>("");
            Assert.IsInstanceOf (typeof(IThing<string>), thing);

            thing = factory.CreateItem(0);
            Assert.IsInstanceOf (typeof(INumberThing), thing);
            thing = factory.CreateItem<object>(0);
            Assert.IsInstanceOf (typeof(INumberThing), thing);

            thing = factory.CreateItem(0f);
            Assert.IsInstanceOf (typeof(INumberThing), thing);
            thing = factory.CreateItem<object>(0f);
            Assert.IsInstanceOf (typeof(INumberThing), thing);

            thing = factory.CreateItem(0d);
            Assert.IsInstanceOf (typeof(INumberThing), thing);
            thing = factory.CreateItem<object>(0d);
            Assert.IsInstanceOf (typeof(INumberThing), thing);
            
            thing = factory.CreateItem(DateTime.Now);
            Assert.IsInstanceOf (typeof(INumberThing), thing);
            thing = factory.CreateItem<object>(DateTime.Now);
            Assert.IsInstanceOf (typeof(INumberThing), thing);

            thing = factory.CreateItem(new Quad16(0,0,0,0));
            Assert.IsInstanceOf (typeof(INumberThing), thing);
            thing = factory.CreateItem<object>(new Quad16(0, 0, 0, 0));
            Assert.IsInstanceOf (typeof(INumberThing), thing);

            using (var stream = new MemoryStream()) {
                thing = factory.CreateItem (stream);
                Assert.IsInstanceOf (typeof (IStreamThing), thing);
                thing = factory.CreateItem<object> (stream);
                Assert.IsInstanceOf (typeof (IStreamThing), thing);
            }

            thing = factory.CreateItem<Stream>(null);
            Assert.IsInstanceOf (typeof(IStreamThing), thing);
        }
    }
}
#endif