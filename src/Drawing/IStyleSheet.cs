namespace Limaki.Drawing {
    public interface IStyleSheet:IStyle {
        IStyle this [ string styleName ] { get; set; }
        IStyle DefaultStyle { get; set; }
        IStyle SelectedStyle { get; set; }
        IStyle HoveredStyle { get; set; }
        IStyle LinkStyle { get; set; }
        IStyle LinkSelectedStyle { get; set; }
        IStyle LinkHoveredStyle { get; set; }
    }
}