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


using System;
using Limada.Data;
using Limada.Model;
using Limaki.Data;
using System.Collections.Generic;
using Limaki.Drawing;
using Limaki.Visuals;
using Limaki.Model.Content.IO;
using System.IO;

namespace Limada.VisualThings {
    /// <summary>
    /// Attachs a Model from a DataSource
    /// to a Scene
    /// </summary>
    public interface ISceneProvider {
        IThingGraphProvider Provider { get; set; }
        IGraphScene<IVisual, IVisualEdge> Scene { get; set; }
        bool Open(IoInfo FileName);
        bool Open ( Action openProvider );
        bool Open();
        void SaveCurrent();
        void Save();
        void ExportAsThingGraph(IGraphScene<IVisual, IVisualEdge> scene, IoInfo FileName);
        
        //bool SaveAs(DataBaseInfo FileName);
        void Close();

        Action<IGraphScene<IVisual, IVisualEdge>> BeforeOpen { get; set; }
        Action<IGraphScene<IVisual, IVisualEdge>> DataBound { get; set; }
        Action<IGraphScene<IVisual, IVisualEdge>> BeforeClose { get; set; }
        Action<IGraphScene<IVisual, IVisualEdge>> AfterClose { get; set; }

        Action<string,int,int> Progress { get; set; }
    }
}