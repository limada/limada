using System;
using Limaki.Drawing;
using Limaki.Tests.Graph.Model;
using Limaki.View.Viz.Visuals;
using NUnit.Framework;

namespace Limaki.Tests.View.Display {

	[TestFixture]
	public class CreateDisplayTest
	{

		[Test]
		public void Test ()
		{
			var display = new VisualsDisplay();
		    Assert.IsNotNull(display.Backend);

		}
	}
}
