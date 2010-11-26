using System.Collections.Generic;
namespace Limaki.Drawing {
    public interface IStyleSheet:IStyle {
        IStyle this [ string styleName ] { get; set; }
        ICollection<IStyle> Styles { get;}
        IStyle DefaultStyle { get; set; }
        IStyle SelectedStyle { get; set; }
        IStyle HoveredStyle { get; set; }
        IStyle EdgeStyle { get; set; }
        IStyle EdgeSelectedStyle { get; set; }
        IStyle EdgeHoveredStyle { get; set; }
        Color BackColor { get; set; }
    }
}