using System;

namespace Limaki.Presenter.UI.DragDrop {
    /// <summary>
    /// taken from: Mono\olive\class\PresentationCore\System.Windows
    /// Copyright (c) 2007 Novell, Inc. (http://www.novell.com)
    /// Authors:
    ///	Chris Toshok (toshok@novell.com)
    /// </summary>
    [Flags]
    public enum DragDropKeyStates {
        None = 0,
        LeftMouseButton = 1 << 0,
        RightMouseButton = 1 << 1,
        ShiftKey = 1 << 2,
        ControlKey = 1 << 3,
        MiddleMouseButton = 1 << 4,
        AltKey = 1 << 5,
    }
}