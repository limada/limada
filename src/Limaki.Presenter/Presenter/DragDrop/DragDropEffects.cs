using System;

namespace Limaki.Presenter.UI.DragDrop {
    /// <summary>
    /// taken from: Mono\olive\class\PresentationCore\System.Windows
    /// Copyright (c) 2007 Novell, Inc. (http://www.novell.com)
    /// Authors:
    ///	Chris Toshok (toshok@novell.com)
    /// </summary>
    [Flags]
    public enum DragDropEffects {
        None = 0,
        Copy = 1 << 0,
        Move = 1 << 1,
        Link = 1 << 2,
        Scroll = unchecked((int)0x80000000),

        /* LAMESPEC don't ask me why, but this doesn't include Link */
        All = Copy | Move | Scroll
    }
}