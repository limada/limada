using System;
using System.Collections.Generic;
using Xwt.Drawing;

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
        Int64 Id { get; set; }
        IStyle DefaultStyle { get; set; }
        IStyle SelectedStyle { get; set; }
        IStyle HoveredStyle { get; set; }
    }

    public interface IStyleSheet : IStyle, IEnumerable<IStyle> {
        Int64 Id { get; set; }
        IStyle this[string styleName] { get; set; }
        ICollection<IStyle> Styles { get; }
        ICollection<IStyleGroup> StyleGroups { get; }
        IStyle BaseStyle { get; set; }
        IStyleGroup ItemStyle { get; set; }
        IStyleGroup EdgeStyle { get; set; }
        Color BackColor { get; set; }
    }
}