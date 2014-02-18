/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2006-2011 Lytico
 *
 * http://www.limada.org
 * 
 */

using Limaki.Drawing;
using Limaki.Tests.View.Display;
using Limaki.View.Visualizers;
using Limaki.View.Swf.Visualizers;
using Limaki.Visuals;
using NUnit.Framework;
using System.Windows.Forms;

namespace Limaki.Tests.View.Winform {
    public class WinformVisualsDisplayTest<T>:DomainTest
    where T : DisplayTest008<IGraphScene<IVisual, IVisualEdge>>, new() {
        public T Test { get; set; }
        public override void Setup() {
            base.Setup();
            var test = new T();
            var testinst = new WinformDisplayTestComposer<IGraphScene<IVisual, IVisualEdge>>();

            //testinst.Factory = () => new WinformVisualsDisplay().Display;
            testinst.Factor(test);
            testinst.Compose(test);


            test.Setup();
            (test.TestForm as Form).Show();
            this.Test = test;
        }
        public override void TearDown() {
            Test.TearDown ();
            base.TearDown();
        }
    }
}