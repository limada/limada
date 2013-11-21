using System;
using System.Runtime.InteropServices;
using Gecko;
using NUnit.Framework;

namespace GeckofxUnitTests
{
	/// <summary>
	/// Memory leaks(yes!!!) tests
	/// </summary>
	[TestFixture]
	public class MemoryLeakTests
	{
		private GeckoWebBrowser browser;

		[SetUp]
		public void BeforeEachTestSetup()
		{
			Xpcom.Initialize(XpComTests.XulRunnerLocation);
			browser = new GeckoWebBrowser();
			var unused = browser.Handle;
			Assert.IsNotNull(browser);
		}

		[TearDown]
		public void AfterEachTestTearDown()
		{
			browser.Dispose();
		}

		/// <summary>
		/// Test Freeing resources when DOM wrappers are freed by IDisposable::Dispose
		/// </summary>
		[Test]
		public void DomMemoryLeakTest_IDisposable()
		{
			browser.TestLoadHtml( "" );
			{
				GeckoHtmlElement element = browser.DomDocument.CreateHtmlElement( "div" );
				Assert.NotNull( element );
				Assert.AreEqual( "div", element.TagName.ToLowerInvariant() );
			}
			// wait for browser.DomDocument finalizer
			GC.Collect();
			GC.WaitForPendingFinalizers();

			for (int i = 0; i < 1000; i++)
			{
				GeckoDomDocument document = browser.Document;
				Func(document);
				document.Dispose();
			}
			var doc = browser.Window.Document;
			// it is VERY bad operation and it SHOULDN'T be used in real code
			var domDoc=Xpcom.QueryInterface<nsIDOMDocument>( doc.NativeDomDocument );
			int count = Marshal.ReleaseComObject( domDoc );
			//DomDocument can be 1 or 2
			Assert.Less(count, 3);
			Console.Error.WriteLine( "nsIDOMDocument count was {0}", count );
			// wait for GeckoWindow finalizers
			GC.Collect();
			GC.WaitForPendingFinalizers();
			var wnd = Xpcom.QueryInterface<nsIDOMWindow>(browser.Window.DomWindow);
			count = Marshal.ReleaseComObject(browser.Window.DomWindow);
			Console.Error.WriteLine("nsIDOMWindow count was {0}", count);
			// DomWindow must be only one
			Assert.AreEqual(count, 1);

			
		}

		/// <summary>
		/// Test Freeing resources when DOM wrappers are freed by Finalize (dtor)
		/// </summary>
		[Test]
		public void DomMemoryLeakTest_Finalize()
		{
			browser.TestLoadHtml("");
			{
				GeckoHtmlElement element = browser.DomDocument.CreateHtmlElement("div");
				Assert.NotNull(element);
				Assert.AreEqual("div", element.TagName.ToLowerInvariant());
			}
			// wait for browser.DomDocument finalizer
			GC.Collect();
			GC.WaitForPendingFinalizers();
			
			
			for (int i = 0; i < 1000; i++)
			{
				GeckoDomDocument document = browser.Document;
				Func(document);
			}

			GC.Collect();
			GC.WaitForPendingFinalizers();

			var doc = browser.Window.Document;
			var domDoc = Xpcom.QueryInterface<nsIDOMDocument>(doc.NativeDomDocument);
			// it is VERY bad operation and it SHOULDN'T be used in real code
			int count = Marshal.ReleaseComObject(domDoc);
			Console.Error.WriteLine("nsIDOMDocument count was {0}", count);
			//DomDocument can be 1 or 2
			Assert.Less( count, 3 );
			var wnd = Xpcom.QueryInterface<nsIDOMWindow>(browser.Window.DomWindow);
			count = Marshal.ReleaseComObject(browser.Window.DomWindow);
			Console.Error.WriteLine("nsIDOMWindow count was {0}", count);
			// DomWindow must be only one
			Assert.AreEqual(count, 1);
		}

		private void Func(GeckoDomDocument doc)
		{

		}
	}

}