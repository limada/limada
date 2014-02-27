namespace Limaki.View.DragDrop {
    /// <summary>
    /// this class is used as an applicationwide dragdrop container
    /// </summary>
    public class DragDropContainer {
        public bool Dragging { get; set; }
        public bool Dropping { get; set; }
        public object Data { get; set; }

        public object ClipboardData { get; set; }
    }
}