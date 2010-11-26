/*
 * Limaki 
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

using System;
using System.IO;
using Limada.Model;
using Limaki.Drawing;
using Limaki.Widgets;
using Id = System.Int64;

namespace Limaki.Widgets{
    public interface ISheetManager {
        void Clear();
        SheetInfo GetSheetInfo ( Int64 id );
        SheetInfo SaveToThing ( Scene scene, ILayout<Scene, IWidget> layout, SheetInfo info );
        SheetInfo SaveToThing ( Scene scene, ILayout<Scene, IWidget> layout, IThing thing, string name );
        bool IsSaveable ( Scene scene );
        void LoadSheet ( Scene scene, ILayout<Scene, IWidget> layout, Stream stream );
        SheetInfo RegisterSheet ( Id id, string name );
    }

    public struct SheetInfo {
        public Id Id;
        public string Name;
        public bool Persistent;
    }
}