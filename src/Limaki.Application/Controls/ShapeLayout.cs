using System;
using System.Collections.Generic;
using Limaki.Drawing;
using Limaki.Drawing.UI;

namespace Limaki.Winform.Controls {
    public class ShapeLayout : Layout<ICollection<IShape>, IShape> {
        public ShapeLayout(Func<ICollection<IShape>> handler, IStyleSheet stylesheet)
            :
                base(handler, stylesheet) { }
        public override void Invoke() {
            throw new Exception("The method or operation is not implemented.");
        }

        public override IShape CreateShape(IShape item) {
            throw new Exception("The method or operation is not implemented.");
        }

        public override bool Invoke(IShape item) {
            throw new Exception("The method or operation is not implemented.");
        }

        public override bool Invoke(IShape item, IShape shape) {
            throw new Exception("The method or operation is not implemented.");
        }

        public override void Justify(IShape item) {
            throw new Exception("The method or operation is not implemented.");
        }

        public override void Justify(IShape item, IShape shape) {
            throw new Exception("The method or operation is not implemented.");
        }

        public override void Perform(IShape item) {
            throw new Exception("The method or operation is not implemented.");
        }

        public override void AddBounds(IShape item) {
            throw new Exception("The method or operation is not implemented.");
        }

        public override IStyle GetStyle(IShape item) {
            return StyleSheet.DefaultStyle;
        }

        public override IStyle GetStyle(IShape item, UiState uiState) {
            IStyle style = StyleSheet.DefaultStyle;
            if (uiState == UiState.Selected) {
                style = StyleSheet.SelectedStyle;
            } else if (uiState == UiState.Hovered) {
                style = StyleSheet.HoveredStyle;
            } else {
                style = StyleSheet.DefaultStyle;
            }

            return style;
        }

        public override PointI[] GetDataHull(IShape item, Matrice matrix, int delta, bool extend) {
            throw new Exception("The method or operation is not implemented.");
        }

        public override PointI[] GetDataHull(IShape item, UiState uiState, Matrice matrix, int delta, bool extend) {
            throw new Exception("The method or operation is not implemented.");
        }
    }
}