﻿/*
 * Limaki 
 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2008-2013 Lytico
 *
 * http://www.limada.org
 * 
 */

using System;
using System.IO;
using Limaki.Model.Content;
using Limaki.Net.WebProxyServer;

namespace Limaki.Viewers.StreamViewers {
    public interface IWebResponse {
        bool IsStreamOwner { get; set; }
        bool Done { get; set; }
        Func<string, WebContent> Getter (Content<Stream> content);
        string AbsoluteUri { get; }
    }
}