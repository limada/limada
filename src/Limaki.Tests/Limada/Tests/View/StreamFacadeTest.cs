/*
 * Limada 
 * Version 0.081
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2006-2008 Lytico
 *
 * http://limada.sourceforge.net
 * 
 */


using System.Collections.Generic;
using Limada.Model;
using Limada.Tests.Model;
using Limaki.Data;
using Limaki.Data.db4o;
using Limaki.Graphs;
using Limaki.Graphs.Extensions;
using Limaki.Model;
using Limaki.Tests.Graph.Model;
using Limaki.UnitTest;
using NUnit.Framework;
using BasicThingGraphTest=Limada.Tests.Basic.BasicThingGraphTest;
using Id = System.Int64;
using ThingGraph=Limada.Data.db4o.ThingGraph;
using Limaki.Common.Collections;
using Limada.View;
using Limaki.Visuals;
using System.IO;
using Limada.Test;
using System;
using Limaki.Model.Streams;
using Limada.Tests.ThingGraphs;

namespace Limada.Tests.ThingGraphs {
    public class StreamFacadeTest : ThingGraphTestBase {
        protected Stream _stream = null;
        public virtual Stream stream {
            get {
                if (_stream == null) {
                    _stream = new StreamSources().Image;
                }
                return _stream;
            }
            set { _stream = value; }
        }

        [Test]
        public virtual void Test() {
            ReportDetail("ThingStreamFacade");
            IThingGraph graph = this.Graph;
            ThingStreamFacade thingStreamFacade = new ThingStreamFacade ();
           
            IThing thing = thingStreamFacade.CreateAndAdd (
                graph, new StreamInfo<Stream>(stream,CompressionType.None));

            Assert.IsInstanceOfType (typeof (Thing), thing);
            Assert.AreEqual(((IThing<Stream>)thing).Data.Length, stream.Length);

            
        }
    }
}
