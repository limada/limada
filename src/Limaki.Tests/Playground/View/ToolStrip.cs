using System.Collections.Generic;

namespace Limaki.Prototype.Viewers.Toolbars {
    /// <summary>
    ///  this is a prototype; not used so far
    /// </summary>
    public class ToolStrip {
        public ICollection<ToolStripItem> Items { get; set; }
    }

    public class MenuStrip : ToolStrip {

    }

    public class StatusStrip : ToolStrip {

    }

    public class ContextMenuStrip : ToolStrip {

    }

    public class ToolStripItem {

    }

    public class ToolStripButton : ToolStripItem {

    }
    public class ToolStripControlHost : ToolStripItem {

    }

    public class ToolStripLabel : ToolStripItem {

    }
    public class ToolStripSeparator : ToolStripItem {

    }


    public class ToolStripDropDownItem : ToolStripItem {

    }

    public class ToolStripDropDownButton : ToolStripDropDownItem {

    }
    public class ToolStripMenuItem : ToolStripDropDownItem {

    }
    public class ToolStripSplitButton : ToolStripDropDownItem {

    }
}