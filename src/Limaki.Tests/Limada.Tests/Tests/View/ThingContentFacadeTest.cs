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


using Limada.Model;
using Limada.Test;
using Limaki.Contents;
using NUnit.Framework;
using System.IO;
using Limaki.Common;

namespace Limada.Tests.ThingGraphs {

    public class ThingContentFacadeTest : ThingGraphTestBase {

        protected Stream _stream = null;
        public virtual Stream stream {
            get {
                if (_stream == null) {
                    _stream = new StreamSources().Pdf().AsUnicodeStream();
                }
                return _stream;
            }
            set { _stream = value; }
        }

        [Test]
        public virtual void Test() {
            ReportDetail("ThingContentFacade");
            var graph = this.Graph;
            var thingContentFacade = new ThingContentFacade ();
           
            var thing = thingContentFacade.CreateAndAdd (
                graph, new Content<Stream>(stream,CompressionType.None));
			#if !__ANDROID__
            Assert.IsInstanceOfType (typeof (Thing), thing);
			#else
			#endif
			Assert.AreEqual(((IThing<Stream>)thing).Data.Length, stream.Length);

            
        }
    }
}
