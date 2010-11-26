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


using Limaki.Drawing.UI;
using Limaki.Widgets;

namespace Limaki.Widgets {
    public interface IDataBaseHandler {
        IDataDisplay<Scene> Display { get; set; }
        void Open ( string FileName );
        void SaveCurrent();
        void ShowRoots(IDataDisplay<Scene> display);
        void Save();
        void Close();
    }
}