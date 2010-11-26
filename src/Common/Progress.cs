/*
 * Limaki 
 * Version 0.063
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

namespace Limaki.Common {
    public struct Progress {
        public int Current;
        public int Maximum;
        public Progress(int maximum) {
            this.Maximum = maximum;
            this.Current = 0;
        }

    }

    public delegate void ProgressChangedEventHandler (Object sender,ProgressChangedEventArgs e);

    public class ProgressChangedEventArgs : EventArgs {
        public Progress Progress;
        public ProgressChangedEventArgs(Progress progress) {
            this.Progress = progress;
        }

    }

}
