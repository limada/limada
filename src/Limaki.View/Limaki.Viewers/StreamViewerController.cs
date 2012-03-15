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
 * http://limada.sourceforge.net
 * 
 */

using System;
using System.IO;
using Limaki.Model.Streams;

namespace Limaki.Viewers {

    public abstract class StreamViewerController : ViewerController {
        public virtual bool IsStreamOwner { get; set; }
        public abstract bool Supports ( Int64 streamType );
        public abstract bool CanSave();
        
        public abstract void SetContent ( Content<Stream> content );
        public abstract void Save( Content<Stream> content);

       
    }
}