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

        public override void CopyTo(IStyle target) {
            base.CopyTo(target);
        }

        public virtual void CopyTo(IStyleGroup result) {
            base.CopyTo(result);
            if (DefaultStyle != null) {
                result.DefaultStyle = this.DefaultStyle.Clone() as IStyle;
                result.DefaultStyle.ParentStyle = result;
            }
            if (SelectedStyle != null) {
                result.SelectedStyle = this.SelectedStyle.Clone() as IStyle;
                result.SelectedStyle.ParentStyle = result;

            }
            if (HoveredStyle != null) {
                result.HoveredStyle = this.HoveredStyle.Clone() as IStyle;
                result.HoveredStyle.ParentStyle = result;
            }
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