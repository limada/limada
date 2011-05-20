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
 * http://limada.sourceforge.net
 * 
 */

using System;

namespace Limaki.Drawing.Shapes {
    
#if !SILVERLIGHT    
    [Serializable]
#endif
    public class RoundedRectangleShape : RectangleShapeBase, IRoundedRectangleShape {
        public RoundedRectangleShape():base() {}
        public RoundedRectangleShape(RectangleI data): base(data) {}
         
        public override PointI this[Anchor i] {
            get {
                switch (i) {
                    case Anchor.LeftTop:
                    case Anchor.MostLeft:
                    case Anchor.MostTop:
                        return Data.Location;
                        
                    case Anchor.LeftBottom:
                        return new PointI(
                            Data.Left,
                            Data.Bottom);
                        
                    case Anchor.RightTop:
                    case Anchor.MostRight:
                        return new PointI(
                            Data.Right,
                            Data.Top);
								
                    case Anchor.RightBottom:
                    case Anchor.MostBottom:
                        return new PointI(
                            Data.Right,		
                            Data.Bottom
                            );

                    case Anchor.MiddleTop:
                        return new PointI(
                            Data.Left+Data.Width/2,
                            Data.Top);

                    case Anchor.LeftMiddle:
                        return new PointI(
                            Data.Left,
                            Data.Top+Data.Height/2);

                    case Anchor.RightMiddle:
                        return new PointI(
                            Data.Right,
                            Data.Top+Data.Height/2);

                    case Anchor.MiddleBottom:
                        return new PointI(
                            Data.Left+Data.Width/2,
                            Data.Bottom);

                    case Anchor.Center:
                        return new PointI(
                            Data.Left+Data.Width/2,
                            Data.Top+Data.Height/2);

                    default:
                        return Data.Location;
                }

            }

            set {}
        }


        public override object Clone() {
            return new RoundedRectangleShape(_data);
        }
    }
}