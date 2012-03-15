namespace Limaki.SWF.Viewers.ToolStripViewers {
    public interface IToolStripCommandItem {
        ToolStripCommand Command { get; set; }
        IToolStripCommandItem ToggleOnClick { get; set; }
    }
}