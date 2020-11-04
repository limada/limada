using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Limaki.Common;
using Limaki.Common.IOC;
using Limaki.Contents;
using Limaki.Tests;

namespace Limaki.ImageLibs.Test {
    [TestFixture]
    public class ConverterTest {
        [SetUp]
        public void Register() {
            if (Registry.ConcreteContext == null)
                Registry.ConcreteContext = new ApplicationContext();
            new ImageLibResourceLoader().ApplyResources (Registry.ConcreteContext);
        }

        [Test]
        public void FindConverter () {
            var converterPool = Registry.Pooled<ConverterPool<Stream>>();
            var conv = converterPool.Find(ContentTypes.TIF,ContentTypes.PNG);
            Assert.IsTrue (conv is ImageConverter);
        }

        [Test]
        public void TestTif2Png() {
            var conv = new ImageConverter();
            var tifFile = File.OpenRead(TestLocations.TestDataDir + "sample.tif");
            var pngFile = File.Create("bw1.png");
            var result = conv.Tif2Png (tifFile, pngFile);
            Console.WriteLine($"{result}");
            Assert.IsNull (result);                              
            pngFile.Close();
        }
    }
}
