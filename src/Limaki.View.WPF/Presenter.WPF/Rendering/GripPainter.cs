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

using System.Collections.Generic;
using Limaki.Drawing;
using Limaki.Drawing.Painters;
using Limaki.Drawing.Shapes;
using Limaki.Common;
using Limaki.Drawing.WPF;
using System.Windows.Shapes;
using System.Windows.Media;
using Limaki.Drawing.WPF.Shapes;
using System.Windows.Controls;
using Limaki.View.Rendering;
using Limaki.View.UI;
using Xwt;

namespace Limaki.View.WPF {
    /// <summary>
    /// Paints the grips of a rectangle
    /// </summary>
    public class GripPainter : GripPainterBase {

        protected Path _path = null ;
        RectangleGeometry[] _geoms = new RectangleGeometry[(int)Anchor.None];
        protected Path Path {
            get {
                return (Path)((IWPFShape)Shape).Shape;
            }
        }

        public override void Render(ISurface surface) {
            var size = new Size(GripSize, GripSize);

            InnerPainter.Style = this.Style;
            int halfWidth = GripSize / 2;
            int halfHeight = GripSize / 2;

            var path = Path;
            path.Data = null;

            InnerPainter.Render(surface);

            var _group = new GeometryGroup();
            var camera = new Camera(this.Camera.Matrix);

            foreach (Anchor anchor in Grips) {
                Point anchorPoint = TargetShape[anchor];
                // no translation in wpf, as the whole surface is translated:
                //anchorPoint = camera.FromSource (anchorPoint);
                var location = new Point (anchorPoint.X - halfWidth, anchorPoint.Y - halfHeight);
                var _geom = new RectangleGeometry ();
                _geom.Rect = new System.Windows.Rect(location.X,location.Y,GripSize, GripSize);
                _group.Children.Add (_geom);
            }

            path.Data = _group;
            var g = ((WPFSurface)surface).Graphics;
            if (!g.Children.Contains(path)) {
                path.IsHitTestVisible = false;
                g.Children.Add(path);
            }
        }

        public void RemoveGrips(ISurface surface) {
            var g = ((WPFSurface)surface).Graphics;
            g.Children.Remove (( (IWPFShape) Shape ).Shape);
        }

       

        
    }
}