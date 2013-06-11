/*
 * Limada 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2006-2012 Lytico
 *
 * http://www.limada.org
 * 
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Limaki.Playground.DOM {

    public class Document {
        public IEnumerable<Page> Pages { get { return null; } }
        public IEnumerable<Paragraph> Paragraphs { get { return null; } }
        
    }

    public class Page {
        public IEnumerable<Paragraph> Paragraphs { get { return null; } }
    }

    public class Paragraph {
        public IEnumerable<Section> Sections { get { return null; } }
    }

    /// <summary>
    /// is a chunk of text with the same attributs
    /// eg: /b/some text/b/ would be a section
    /// </summary>
    public class Section {
        public IEnumerable<Attribute> Attributes { get { return null; } }
    }

    public class Attribute {}

}