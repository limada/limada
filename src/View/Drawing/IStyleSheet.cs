namespace Limaki.Drawing {
    public interface IStyleSheet:IStyle {
        IStyle this [ string styleName ] { get; set; }
        IStyle DefaultStyle { get; set; }
        IStyle SelectedStyle { get; set; }
        IStyle HoveredStyle { get; set; }
        IStyle EdgeStyle { get; set; }
        IStyle EdgeSelectedStyle { get; set; }
        IStyle EdgeHoveredStyle { get; set; }
    }
}