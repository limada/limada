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


using System.IO;
using Limada.Model;
using Limada.Test;
using Limaki.Data;
using Limaki.Common;
using Limaki.Contents;
using NUnit.Framework;
using Id = System.Int64;

namespace Limada.Tests.ThingGraphs {

    public class StreamThingTest : ThingGraphTestBase {

        protected Stream _stream = null;
        public virtual Stream Stream {
            get {
                if (_stream == null) {
                    _stream = new StreamSources().Pdf().AsUnicodeStream();
                }
                return _stream;
            }
            set { _stream = value; }
        }
        
        public Id CreateAndAddStreamThing() {
            long len = Stream.Length;

            var factory = new ThingFactory();
            
            var thing = factory.CreateItem(Graph, Stream) as IStreamThing;
            Graph.Add(thing);

            thing.Flush();
            thing.ClearRealSubject();

            this.Close();

            
            return thing.Id;


        }

        public void ProveStream(Id id) {
            IStreamThing thing = Graph.GetById(id) as IStreamThing;
            thing.DeCompress ();
            Assert.AreEqual(thing.Data.Length, Stream.Length);

            thing.ClearRealSubject();

            this.Close();

        }

        public void ChangeStream(Id id) {
            Stream = new StreamSources ().RandomHTML;

            var thing = Graph.GetById(id) as IStreamThing;
            thing.Data = Stream;

            Assert.AreEqual(thing.Data.Length, Stream.Length);

            
            thing.Flush ();
            thing.ClearRealSubject();

            this.Close();
        }

        public void DeleteStream(Id id) {

            var thing = Graph.GetById (id);
            Graph.Remove (thing);

            var dataContainer = Graph.ContentContainer;
            Assert.IsFalse (dataContainer.Contains (id));

            var data = dataContainer.GetById (id);
            Assert.IsNull (data);

        }

        [Test]
        public virtual void CreateStreamTest() {
            ReportDetail("CreateStreamTest");
            var id = CreateAndAddStreamThing();
            ProveStream(id);
        }

        [Test]
        public virtual void CrUD() {
            ReportDetail("CrUD");
            var id  = CreateAndAddStreamThing ();
            ProveStream (id);
            ChangeStream (id);
            ProveStream (id);
            DeleteStream(id);

        }

        [Test]
        public void CrU() {
            ReportDetail("CrU");
            var id = CreateAndAddStreamThing();
            ProveStream(id);
            ChangeStream(id);
            ProveStream(id);
        }
    }
}
