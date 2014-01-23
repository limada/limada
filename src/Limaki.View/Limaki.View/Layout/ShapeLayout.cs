/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2006-2011 Lytico
 *
 * http://www.limada.org
 * 
 */

using System;
using Limaki.Common;
using Limaki.Drawing;
using Xwt;
using Xwt.Drawing;

namespace Limaki.View.Layout {

    public class ShapeLayout : Layout<IShape> {

        public ShapeLayout(IStyleSheet stylesheet):base(stylesheet) { }

        public override void Reset() {
            throw new Exception("The method or operation is not implemented.");
        }

        public override IShape CreateShape(IShape item) {
            throw new Exception("The method or operation is not implemented.");
        }

        public override IShape GetShape(IShape item) {
            return item;
        }

        public override bool Perform(IShape item) {
            throw new Exception("The method or operation is not implemented.");
        }

        public override bool Perform(IShape item, IShape shape) {
            throw new Exception("The method or operation is not implemented.");
        }

        public override void Justify(IShape item) {
            throw new Exception("The method or operation is not implemented.");
        }

        public override void Justify(IShape item, IShape shape) {
            throw new Exception("The method or operation is not implemented.");
        }

        public override void Refresh(IShape item) {
            throw new Exception("The method or operation is not implemented.");
        }

        public override void BoundsChanged(IShape item) {
            throw new Exception("The method or operation is not implemented.");
        }

        public override IStyle GetStyle(IShape item) {
            return StyleSheet.BaseStyle;
        }

        public override IStyle GetStyle(IShape item, UiState uiState) {
            var style = StyleSheet.BaseStyle;
            if (uiState == UiState.Selected) {
                style = StyleSheet.ItemStyle.SelectedStyle;
            } else if (uiState == UiState.Hovered) {
                style = StyleSheet.ItemStyle.HoveredStyle;
            } 
            return style;
        }

        public override Point[] GetDataHull(IShape item, Matrix matrix, int delta, bool extend) {
            Registry.Pool.TryGetCreate<IExceptionHandler>()
                .Catch(new Exception("The method or operation is not implemented."), MessageType.OK);
            return new Point[0];

        }

        public override Point[] GetDataHull(IShape item, UiState uiState, Matrix matrix, int delta, bool extend) {
            Registry.Pool.TryGetCreate<IExceptionHandler>()
                .Catch(new Exception("The method or operation is not implemented."), MessageType.OK);
            return new Point[0];
        }

        public override Point[] GetDataHull(IShape item, int delta, bool extend) {
            Registry.Pool.TryGetCreate<IExceptionHandler>()
                .Catch(new Exception("The method or operation is not implemented."), MessageType.OK);
            return new Point[0];
        }

        public override Point[] GetDataHull(IShape item, UiState uiState, int delta, bool extend) {
            Registry.Pool.TryGetCreate<IExceptionHandler>()
                .Catch(new Exception("The method or operation is not implemented."), MessageType.OK);
            return new Point[0];
        }
    }
}