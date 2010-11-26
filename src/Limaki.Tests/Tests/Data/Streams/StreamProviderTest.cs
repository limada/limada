
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Limaki.UnitTest;
using NUnit.Framework;
using Limaki.Common;
using Limaki.Model.Streams;
using System.Linq;

namespace Limaki.Tests.Data.Streams
{
	[TestFixture]
	public class StreamProviderTest:DomainTest
	{
		string GetTestDir(){
			return IOUtils.FindSubDirInRootPath("TestData")+Path.DirectorySeparatorChar;
		}
		
		IStreamProvider FindProvider(long streamType){
			var providers = Registry.Pool.TryGetCreate<StreamProviders>();
			return providers.Find(streamType);
		}
		
		[Test]
		public void StreamProvidersTest(){
			var providers = Registry.Pool.TryGetCreate<StreamProviders>();
			IStreamProvider provider = null;
			
			Assert.IsNotNull(providers.Find(StreamTypes.RTF),"rtf");
			Assert.IsNotNull(providers.Find("rtf"),"rtf");
			
			Assert.IsNotNull(providers.Find(StreamTypes.HTML),"hmtl");
			Assert.IsNotNull(providers.Find("hmtl"),"hmtl");
			
			Assert.IsNotNull(providers.Find(StreamTypes.JPG),"jpg");
			Assert.IsNotNull(providers.Find("jpg"),"jpg");
		}
		
		
		public void StreamProviderFileTest(string fileName, long streamtype){
			ReportDetail(fileName);
			var provider = FindProvider(streamtype);
			Assert.IsNotNull(provider,"No Provider found");

			Assert.IsTrue(File.Exists(fileName));
			var uri = IOUtils.UriFromFileName(fileName);
			var streamInfo = default(StreamInfo<Stream>);
			try {
				streamInfo = provider.Open(uri);
				Assert.IsNotNull(streamInfo);
				Assert.AreNotEqual(streamInfo.Data.Length,0);
			} finally {
				if(streamInfo != null && streamInfo.Data !=null){
					streamInfo.Data.Close();
				}
			}
		}
		
		[Test]
		public void StreamProviderFileTests(){
			StreamProviderFileTest( GetTestDir()+"sampleDoc.rtf",StreamTypes.RTF);
			StreamProviderFileTest( GetTestDir()+"sample.html",StreamTypes.HTML);
			
			StreamProviderFileTest( GetTestDir()+"sample.jpg",StreamTypes.JPG);
		}
		
		[Test]
		public void JPGTest(){
			var filename = GetTestDir()+"sample.jpg";
			var stream = new FileStream(filename,FileMode.Open);
			try {
				var buffer = new byte[64];
				stream.Read(buffer,0,buffer.Length);
				for(int i = 0; i < buffer.Length;i++){
					if (i%8==0){
						System.Console.WriteLine();
					}
					System.Console.Write(buffer[i].ToString("X")+"\t");
				}
				
			}
			finally {
				stream.Close();
			}
			
		}
	}
}
