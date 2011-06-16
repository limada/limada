using System;

namespace Limaki.Drawing {
    public class StyleGroup : Style, IStyleGroup {
        public Int64 Id { get; set; }
        public StyleGroup(string name) : base(name) { }
        public StyleGroup(string name, IStyle parentStyle) : base(name, parentStyle) { }
        public IStyle DefaultStyle { get; set; }
        public IStyle SelectedStyle { get; set; }
        public IStyle HoveredStyle { get; set; }


        public override string Name {
            get { return base.Name; }
            set {
                if (base.Name != value) {
                    if (SelectedStyle != null)
                        SelectedStyle.Name = value + ".Selected";
                    if (HoveredStyle != null)
                        HoveredStyle.Name = value + ".Hovered";
                }
                base.Name = value;
            }
        }

        public object Clone() {
            var result = base.Clone() as IStyleGroup;
            if (DefaultStyle != null)
                result.DefaultStyle = this.DefaultStyle.Clone() as IStyle;
            if (SelectedStyle != null)
                result.SelectedStyle = this.SelectedStyle.Clone() as IStyle;
            if (HoveredStyle != null)
                result.HoveredStyle = this.HoveredStyle.Clone() as IStyle;
            return result;


        }
        public System.Collections.Generic.IEnumerator<IStyle> GetEnumerator() {
            yield return this;
            yield return DefaultStyle;
            yield return SelectedStyle;
            yield return HoveredStyle;
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
            return this.GetEnumerator();
        }

        public override void Dispose(bool disposing) {
            if (SelectedStyle != null)
                SelectedStyle.Dispose();
            if (HoveredStyle != null)
                HoveredStyle.Dispose();
            base.Dispose(disposing);
        }
    }
}