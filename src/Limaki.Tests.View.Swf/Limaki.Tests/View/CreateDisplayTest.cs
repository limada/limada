using System;
using Limaki.Drawing;
using Limaki.View.UI;
using Limaki.Tests.Graph.Model;
using Limaki.Visuals;
using NUnit.Framework;
using Limaki.View.Visuals.Visualizers;

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
