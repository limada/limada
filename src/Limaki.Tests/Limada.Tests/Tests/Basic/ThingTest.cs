/*
 * Limada 
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


using System;
using System.Collections.Generic;
using System.Text;
using Limada.Model;
using Limaki.Tests;
using Limaki.UnitTest;
using NUnit.Framework;

using Id = System.Int64;
using Limaki.Common;

namespace Limada.Tests.Basic {
    public class ThingTest:DomainTest {

        IThing aLeaf = null;
        IThing aRoot = null;
        IThing aMarker = null;
        ILink aLink = null;

        string leafText = "Leaf";
        string rootText = "Root";
        string markerText = "Marker";

        public override void Setup() {
            base.Setup();
            var factory = Registry.Factory.Create<IThingFactory>();
            aLeaf = factory.CreateItem(leafText);
            aRoot = factory.CreateItem(rootText);
            aMarker = factory.CreateItem(markerText);
        }

        [Test]
        public virtual void Prove() {
            Assert.AreEqual(leafText, aLeaf.Data);
            Assert.AreEqual(markerText, aMarker.Data);
            Assert.AreEqual(rootText, aRoot.Data);

            Assert.AreEqual(((IThing<Id>)aLeaf).Data, aLeaf.Id);
            Assert.AreEqual(((IThing<Id>)aRoot).Data, aRoot.Id);
            Assert.AreEqual(((IThing<Id>)aMarker).Data, aMarker.Id);

            aLink = new Link (aRoot, aLeaf, aMarker);
            Assert.AreEqual(aLink.Leaf, aLeaf);
            Assert.AreEqual(aLink.Marker, aMarker);
            Assert.AreEqual(aLink.Root, aRoot);

            Assert.AreEqual(((ILink<Id>)aLink).Leaf, aLeaf.Id);
            Assert.AreEqual(((ILink<Id>)aLink).Marker, aMarker.Id);
            Assert.AreEqual(((ILink<Id>)aLink).Root, aRoot.Id);

            Assert.AreEqual (( (IThing<IThing>) aLink ).Data, aLink.Marker);
        }

    }
}