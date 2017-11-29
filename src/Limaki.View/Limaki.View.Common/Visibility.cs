namespace Limaki.View.Vidgets {
    /// <summary>
    /// Specifies the display state of an element.
    /// </summary>
    public enum Visibility {

        /// <summary>
        /// Display the element.
        /// </summary>
        Visible = 0,

        /// <summary>
        /// Do not display the element, but reserve space for the element in layout.
        /// </summary>
        Hidden = 1,

        /// <summary>
        /// Do not display the element, and do not reserve space for it in layout.
        /// </summary>
        Collapsed = 2
    }
}