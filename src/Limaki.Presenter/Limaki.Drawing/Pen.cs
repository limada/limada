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
using Xwt.Drawing;

namespace Limaki.Drawing {
    public class Pen:ICloneable, IDisposable {
        public Pen() {
            this.Thickness = 1;
        }

        public Pen(Color color):this() {
            this.Color = color;
        }

        public virtual Color Color { get; set; }
        public virtual double Thickness { get; set; }
        public virtual PenLineCap StartCap { get; set; }
        public virtual PenLineCap EndCap { get; set; }
        public virtual PenLineJoin LineJoin { get; set; }

        public virtual object CustomEndCap { get; set; }
        public virtual object CustomStartCap { get; set; }

        protected Pen (Pen pen) {
            this.Color = pen.Color;
            this.Thickness = pen.Thickness;
            this.StartCap = pen.StartCap;
            this.EndCap = pen.EndCap;
            this.LineJoin = pen.LineJoin;
            this.CustomEndCap = pen.CustomEndCap;
            this.CustomStartCap = pen.CustomStartCap;
        }

        public virtual object Clone() {
            return new Pen(this);
        }


        public virtual void Dispose(bool disposing) {

        }

        ~Pen() {
            Dispose(false);
        }

        public virtual void Dispose() {
            Dispose (true);
            System.GC.SuppressFinalize(this);
        }

        public override bool Equals(object obj) {
            if (!(obj is Pen))
                return false;
            var other = (Pen)obj;
            return
                   this.Thickness == other.Thickness &&
                   this.Color.Equals(other.Color) &&
                   this.StartCap == other.StartCap &&
                   this.EndCap == other.EndCap &&
                   this.LineJoin == other.LineJoin
                   &&
                   this.CustomEndCap == other.CustomEndCap &&
                   this.CustomStartCap == other.CustomStartCap
                   ;
        }

        public override int GetHashCode() {
            int result =
                this.Color.GetHashCode () ^
                this.Thickness.GetHashCode () ^
                this.StartCap.GetHashCode () ^
                this.EndCap.GetHashCode ()^
                this.LineJoin.GetHashCode();
            if (this.CustomStartCap!=null) {
                result ^= this.CustomStartCap.GetHashCode ();
            }
            if (this.CustomEndCap != null) {
                result ^= this.CustomEndCap.GetHashCode();
            }
            return result;
        }
    }

    public enum PenLineCap {
        Flat,
        Square,
        Round,
        Triangle
    }

    public enum PenLineJoin {
        Miter,
        Bevel,
        Round
    }
}