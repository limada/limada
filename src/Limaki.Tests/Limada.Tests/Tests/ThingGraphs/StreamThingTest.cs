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
 * http://limada.sourceforge.net
 * 
 */


using System.IO;
using Limada.Model;
using Limada.Test;
using Limada.View;
using Limaki.Data;
using Limaki.Model.Streams;
using NUnit.Framework;
using Id = System.Int64;

namespace Limada.Tests.ThingGraphs {
    public class StreamThingTest : ThingGraphTestBase {
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
        
        



        public Id CreateStream() {
            long len = stream.Length;

            var factory = new ThingFactory();
            
            var thing = factory.CreateItem(Graph, stream) as IStreamThing;
            Graph.Add(thing);

            thing.Flush();
            thing.ClearRealSubject();

            this.Close();

            
            return thing.Id;


        }

        public void ProveStream(Id id) {
            IStreamThing thing = Graph.GetById(id) as IStreamThing;
            thing.DeCompress ();
            Assert.AreEqual(thing.Data.Length, stream.Length);

            thing.ClearRealSubject();

            this.Close();

        }

        public void ChangeStream(Id id) {
            stream = new StreamSources ().RandomHTML;

            IStreamThing thing = Graph.GetById(id) as IStreamThing;
            thing.Data = stream;

            Assert.AreEqual(thing.Data.Length, stream.Length);

            
            thing.Flush ();
            thing.ClearRealSubject();

            this.Close();
        }

        public void DeleteStream(Id id) {
            IThing thing = Graph.GetById (id);
            Graph.Remove (thing);

            IDataContainer<Id> dataContainer = Graph.DataContainer;
            Assert.IsFalse (dataContainer.Contains (id));

            IRealData<Id> data = dataContainer.GetById (id);
            Assert.IsNull (data);

        }

        [Test]
        public virtual void CreateStreamTest() {
            ReportDetail("CreateStreamTest");
            Id id = CreateStream();
            ProveStream(id);
        }

        [Test]
        public virtual void CrUD() {
            ReportDetail("CrUD");
            Id id  = CreateStream ();
            ProveStream (id);
            ChangeStream (id);
            ProveStream (id);
            DeleteStream(id);

        }

        [Test]
        public void CrU() {
            ReportDetail("CrU");
            Id id = CreateStream();
            ProveStream(id);
            ChangeStream(id);
            ProveStream(id);
        }
    }
}
