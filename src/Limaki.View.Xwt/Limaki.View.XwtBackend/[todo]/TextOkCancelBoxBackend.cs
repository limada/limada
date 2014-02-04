/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2014 Lytico
 *
 * http://www.limada.org
 * 
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Limaki.View.XwtBackend {
    public class TextOkCancelBoxBackend:DummyBackend {
        public string Title { get; set; }
        public string Text { get; set; }
    }
}
