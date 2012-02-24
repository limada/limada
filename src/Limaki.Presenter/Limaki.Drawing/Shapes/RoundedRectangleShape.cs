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
using Xwt;

namespace Limaki.Drawing.Shapes {
    
#if !SILVERLIGHT    
    [Serializable]
#endif
    public class RoundedRectangleShape : RectangleShapeBase, IRoundedRectangleShape {
        public RoundedRectangleShape():base() {}
        public RoundedRectangleShape(RectangleD data): base(data) {}
         
        public override Point this[Anchor i] {
            get {
                switch (i) {
                    case Anchor.LeftTop:
                    case Anchor.MostLeft:
                    case Anchor.MostTop:
                        return Data.Location;
                        
                    case Anchor.LeftBottom:
                        return new Point(
                            Data.Left,
                            Data.Bottom);
                        
                    case Anchor.RightTop:
                    case Anchor.MostRight:
                        return new Point(
                            Data.Right,
                            Data.Top);
								
                    case Anchor.RightBottom:
                    case Anchor.MostBottom:
                        return new Point(
                            Data.Right,		
                            Data.Bottom
                            );

                    case Anchor.MiddleTop:
                        return new Point(
                            Data.Left+Data.Width/2,
                            Data.Top);

                    case Anchor.LeftMiddle:
                        return new Point(
                            Data.Left,
                            Data.Top+Data.Height/2);

                    case Anchor.RightMiddle:
                        return new Point(
                            Data.Right,
                            Data.Top+Data.Height/2);

                    case Anchor.MiddleBottom:
                        return new Point(
                            Data.Left+Data.Width/2,
                            Data.Bottom);

                    case Anchor.Center:
                        return new Point(
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