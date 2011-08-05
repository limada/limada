
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Limaki.UnitTest;
using NUnit.Framework;
using Limaki.Common;
using Limaki.Model.Streams;
using System.Linq;

namespace Limaki.Tests.Data.Streams {
    [TestFixture]
    public class ContentProviderTest : DomainTest {
        string GetTestDir() {
            return IOUtils.FindSubDirInRootPath("TestData") + Path.DirectorySeparatorChar;
        }

        IContentProvider FindProvider(long streamType) {
            var providers = Registry.Pool.TryGetCreate<ContentProviders>();
            return providers.Find(streamType);
        }

        [Test]
        public void ContentProvidersTest() {
            var providers = Registry.Pool.TryGetCreate<ContentProviders>();
            IContentProvider provider = null;

            Assert.IsNotNull(providers.Find(StreamTypes.RTF), "rtf");
            Assert.IsNotNull(providers.Find("rtf"), "rtf");

            Assert.IsNotNull(providers.Find(StreamTypes.HTML), "html");
            Assert.IsNotNull(providers.Find("html"), "html");

            Assert.IsNotNull(providers.Find(StreamTypes.JPG), "jpg");
            Assert.IsNotNull(providers.Find("jpg"), "jpg");
        }


        public void ContentProviderFileTest(string fileName, long streamtype) {
            ReportDetail(fileName);
            var provider = FindProvider(streamtype);
            Assert.IsNotNull(provider, "No Provider found");

            Assert.IsTrue(File.Exists(fileName));
            var uri = IOUtils.UriFromFileName(fileName);
            var content = default(Content<Stream>);
            try {
                content = provider.Open(uri);
                Assert.IsNotNull(content);
                Assert.AreNotEqual(content.Data.Length, 0);
            } finally {
                if (content != null && content.Data != null) {
                    content.Data.Close();
                }
            }
        }

        [Test]
        public void ContentProviderFileTests() {
            ContentProviderFileTest(GetTestDir() + "sampleDoc.rtf", StreamTypes.RTF);
            ContentProviderFileTest(GetTestDir() + "sample.html", StreamTypes.HTML);

            ContentProviderFileTest(GetTestDir() + "sample.jpg", StreamTypes.JPG);
        }

        [Test]
        public void JPGTest() {
            var filename = GetTestDir() + "sample.jpg";
            var stream = new FileStream(filename, FileMode.Open);
            try {
                var buffer = new byte[64];
                stream.Read(buffer, 0, buffer.Length);
                for (int i = 0; i < buffer.Length; i++) {
                    if (i % 8 == 0) {
                        System.Console.WriteLine();
                    }
                    System.Console.Write(buffer[i].ToString("X") + "\t");
                }

            } finally {
                stream.Close();
            }

        }
    }
}
