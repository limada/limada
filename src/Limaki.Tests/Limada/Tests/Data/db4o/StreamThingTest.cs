/*
 * Limada 
 * Version 0.08
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


using System.IO;
using Limada.Model;
using Limada.Test;
using Limada.View;
using Limaki.Data;
using Limaki.Model.Streams;
using NUnit.Framework;
using Id = System.Int64;

namespace Limada.Tests.Data.db4o {
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
        
        [Test]
        public void CreateStreamTest() {
            Id id = CreateStream ();
            ProveStream (id);
        }



        public Id CreateStream() {
            long len = stream.Length;

            ThingFactory factory = new ThingFactory();
            IStreamThing thing = factory.CreateThing(Graph, stream) as IStreamThing;
            Graph.Add(thing);

            thing.Flush();
            thing.ClearRealSubject();

            Gateway.Close();

            
            return thing.Id;


        }

        public void ProveStream(Id id) {
            IStreamThing thing = Graph.GetById(id) as IStreamThing;

            Assert.AreEqual(thing.Data.Length, stream.Length);

            
            thing.ClearRealSubject();

            Gateway.Close();

        }

        public void ChangeStream(Id id) {
            stream = new StreamSources ().RandomHTML;

            IStreamThing thing = Graph.GetById(id) as IStreamThing;
            thing.Data = stream;

            Assert.AreEqual(thing.Data.Length, stream.Length);

            
            thing.Flush ();
            thing.ClearRealSubject();

            Gateway.Close();
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
        public void CrUD() {
            Id id  = CreateStream ();
            ProveStream (id);
            ChangeStream (id);
            ProveStream (id);
            DeleteStream(id);

        }

        [Test]
        public void CrU() {
            Id id = CreateStream();
            ProveStream(id);
            ChangeStream(id);
            ProveStream(id);
        }
    }
}
