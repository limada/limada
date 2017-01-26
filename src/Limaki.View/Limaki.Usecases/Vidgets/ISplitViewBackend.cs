/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2013 Lytico
 *
 * http://www.limada.org
 * 
 */

using System;
using Limaki.View;

namespace Limaki.Usecases.Vidgets {

    public interface ISplitViewBackend:IVidgetBackend {

        void GraphGraphView ();
        void GraphContentView ();
        void ToggleView ();

        void AttachViewer (IVidget vidget, Action onShowAction);

        void ShowTextDialog (string title, string text, Action<string> onOk);

        void ViewInWindow (IVidget vidget, Action onClose);
    }
}