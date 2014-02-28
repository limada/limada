namespace Limaki.View.DragDrop {
    /// <summary>
    /// this class is used as an applicationwide dragdrop container
    /// </summary>
    public class DragDropContainer {
        public bool Dragging { get; set; }
        public bool Dropping { get; set; }
        public object Data { get; set; }

        /// <summary>
        /// disabled until Clipboard.Pasted-event is implemented
        /// </summary>
        public object ClipboardData { get { return null; } set { ; } }
    }
}