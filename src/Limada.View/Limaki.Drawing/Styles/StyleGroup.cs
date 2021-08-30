using System;

namespace Limaki.Drawing.Styles {

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
                    if (DefaultStyle != null && DefaultStyle!=this)
                        DefaultStyle.Name = value + ".Default";
                    if (SelectedStyle != null)
                        SelectedStyle.Name = value + ".Selected";
                    if (HoveredStyle != null)
                        HoveredStyle.Name = value + ".Hovered";
                }
                base.Name = value;
            }
        }

        public override void CopyTo (IStyle other) {
            if (other is IStyleGroup && this != other) {
                CopyTo ((IStyleGroup) other);
                return;
            }
            base.CopyTo (other);
        }


        public virtual void CopyTo (IStyleGroup other) {
            base.CopyTo (other);

            other.DefaultStyle = MakeCopy (this.DefaultStyle, other);
            other.SelectedStyle = MakeCopy (this.SelectedStyle, other);
            other.HoveredStyle = MakeCopy (this.HoveredStyle, other);
        }

        public override bool Equals (object obj) {
            var other = obj as IStyleGroup;
            if (other != null) {
                return base.Equals (other) &&
                       this.DefaultStyle.Equals (other.DefaultStyle) &&
                       this.SelectedStyle.Equals (other.DefaultStyle) &&
                       this.HoveredStyle.Equals (other.DefaultStyle);
            }
            return false;
        }

        public override object Clone() {
            var result = base.Clone() as IStyleGroup;
            CopyTo(result);
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