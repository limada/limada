
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Limaki.UnitTest;
using NUnit.Framework;
using Limaki.Common;
using Limaki.Model.Content;
using System.Linq;
using System.Diagnostics;
using Limaki.Model.Content.IO;

namespace Limaki.Tests.Data.Streams {
    [TestFixture]
    public class ContentProviderTest : DomainTest {
        string GetTestDir() {
            return IoUtils.FindSubDirInRootPath("TestData") + Path.DirectorySeparatorChar;
        }

        ISinkIo<Stream> FindProvider (long streamType) {
            var provider = Registry.Pool.TryGetCreate<IoProvider<Stream,Content<Stream>>>();
            return provider.Find(streamType);
        }

        [Test]
        public void ContentProvidersTest() {
            var provider = Registry.Pool.TryGetCreate<IoProvider<Stream, Content<Stream>>>();

            Assert.IsNotNull(provider.Find(ContentTypes.RTF), "rtf");
            Assert.IsNotNull(provider.Find("rtf",IoMode.ReadWrite), "rtf");

            Assert.IsNotNull(provider.Find(ContentTypes.HTML), "html");
            Assert.IsNotNull(provider.Find("html", IoMode.ReadWrite), "html");

            Assert.IsNotNull(provider.Find(ContentTypes.JPG), "jpg");
            Assert.IsNotNull(provider.Find("jpg", IoMode.ReadWrite), "jpg");
        }


        public void ContentProviderFileTest(string fileName, long streamtype) {
            ReportDetail(fileName);
            var provider = FindProvider(streamtype);
            Assert.IsNotNull(provider, "No Provider found");

            Assert.IsTrue(File.Exists(fileName));
            var uri = IoUtils.UriFromFileName(fileName);
            var content = default(Content<Stream>);
            try {
                var sink = provider as ISink<Uri, Content<Stream>>;
                content = sink.Use(uri);
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
            ContentProviderFileTest(GetTestDir() + "sampleDoc.rtf", ContentTypes.RTF);
            ContentProviderFileTest(GetTestDir() + "sample.html", ContentTypes.HTML);

            ContentProviderFileTest(GetTestDir() + "sample.jpg", ContentTypes.JPG);
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
                        Trace.WriteLine("");
                    }
                    Trace.Write(buffer[i].ToString("X") + "\t");
                }

            } finally {
                stream.Close();
            }

        }
    }
}
