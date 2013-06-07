/*
 * Limada 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2011-2013 Lytico
 *
 * http://www.limada.org
 */


using System.IO;
using Limada.View;
using Limaki.Common;
using Limaki.Model.Content;
using Limaki.Viewers;
using System;
using Limaki.Viewers.StreamViewers;

namespace Limaki.View.Swf.Backends {

    public class SwfDocumentSchemaViewer : DocumentSchemaViewer {

        //TODO: replace with factory methods or BackendHandler (look in xwt)
        DocumentSchemaBackend _backend = null;
        public override IVidgetBackend Backend {
            get {
                if (_backend == null) {
                    _backend = new DocumentSchemaBackend(this);
                    OnAttachBackend(_backend);
                }
                return _backend;
            }
        }

      
    }
}