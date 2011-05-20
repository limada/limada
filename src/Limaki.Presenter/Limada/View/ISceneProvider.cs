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
 * http://limada.sourceforge.net
 * 
 */


using System;
using Limada.Data;
using Limaki.Data;
using Limaki.Visuals;

namespace Limada.View {
    /// <summary>
    /// Attachs a Model from a DataSource
    /// to a Scene
    /// </summary>
    public interface ISceneProvider {
        IThingGraphProvider Provider { get; set; }
        Scene Scene { get; set; }
        bool Open(DataBaseInfo FileName);
        bool Open ( Action openProvider );
        bool Open();
        void SaveCurrent();
        void Save();
        void ExportAs(Scene scene, DataBaseInfo FileName);
        //bool SaveAs(DataBaseInfo FileName);
        void Close();

        Action<Scene> BeforeOpen { get; set; }
        Action<Scene> DataBound { get; set; }
        Action<Scene> BeforeClose { get; set; }
        Action<Scene> AfterClose { get; set; }
    }
}