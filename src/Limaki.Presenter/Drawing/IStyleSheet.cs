using System.Collections.Generic;
using Id = System.Int64;

namespace Limaki.Drawing {
    public interface IStyleSheet0:IStyle {
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

    public interface IStyleGroup : IStyle, IEnumerable<IStyle> {
        Id Id { get; set; }
        IStyle DefaultStyle { get; set; }
        IStyle SelectedStyle { get; set; }
        IStyle HoveredStyle { get; set; }
    }

    public interface IStyleSheet : IStyle, IEnumerable<IStyle> {
        Id Id { get; set; }
        IStyle this[string styleName] { get; set; }
        ICollection<IStyle> Styles { get; }
        ICollection<IStyleGroup> StyleGroups { get; }
        IStyle BaseStyle { get; set; }
        IStyleGroup ItemStyle { get; set; }
        IStyleGroup EdgeStyle { get; set; }
        Color BackColor { get; set; }
    }
}