/*
 * Limada 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2010-2013 Lytico
 *
 * http://www.limada.org
 */

using System;
using Limaki.View;
using Limaki.View.Vidgets;
using Limaki.View.Visuals;
using Limaki.Data;

namespace Limaki.Usecases {

    public interface IGraphSceneUiManager {

        Action<IGraphScene<IVisual, IVisualEdge>> DataBound { get; set; }
        Action<string> DataPostProcess { get; set; }

        FileDialogMemento OpenFileDialog { get; set; }
        FileDialogMemento SaveFileDialog { get; set; }
        Func<FileDialogMemento, bool, DialogResult> FileDialogShow { get; set; }
        Func<string, string, MessageBoxButtons, DialogResult> MessageBoxShow { get; set; }

        Action<string, int, int> Progress { get; set; }

        Action ApplicationQuit { get; set; }

        void Open ();
		bool Open (Iori iori);

        bool ProcessCommandLine ();
        bool OpenCommandLine ();

        void ShowEmptyScene ();

        void ExportSceneView (IGraphScene<IVisual, IVisualEdge> graphScene);
        void ImportRawSource ();

        void Save ();
        void SaveAs ();

        void Close ();
        
    }
}