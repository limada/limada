namespace Limaki.Swf.Backends.Viewers.ToolStrips {
    public interface IToolStripCommandItem {
        ToolStripCommand Command { get; set; }
        IToolStripCommandItem ToggleOnClick { get; set; }
    }
}