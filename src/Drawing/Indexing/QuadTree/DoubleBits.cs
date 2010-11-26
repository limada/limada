/*
 * Limaki 
 * Version 0.064
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the license below.
 *
 * Changes:
 * Adopted to work with RectangleF and PointF
 * Generic Items introduced
 * 
 * Author of changes: Lytico
 *
 * http://limada.sourceforge.net
 * 
 */

/* NetTopologySuite is a collection of .NET classes written in C# that
implement the fundamental operations required to validate a given
geo-spatial data set to a known topological specification.

This collection of classes is a porting (with some additions and modifications) of 
JTS Topology Suite (see next license for more informations).

Copyright (C) 2005 Diego Guidi

This library is free software; you can redistribute it and/or
modify it under the terms of the GNU Lesser General Public
License as published by the Free Software Foundation; either
version 2.1 of the License, or (at your option) any later version.

This library is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
Lesser General Public License for more details.

You should have received a copy of the GNU Lesser General Public
License along with this library; if not, write to the Free Software
Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA

For more information, contact:

    Diego Guidi
    via Po 15
	61031 Cuccurano di Fano (PU)
    diegoguidi@libero.it
    http://blogs.ugidotnet.org/gissharpblog

*/



using System;
using System.Collections;
using System.Text;


namespace Limaki.Drawing.Indexing.QuadTrees {
    /// <summary>
    /// DoubleBits manipulates Double numbers
    /// by using bit manipulation and bit-field extraction.
    /// For some operations (such as determining the exponent)
    /// this is more accurate than using mathematical operations
    /// (which suffer from round-off error).
    /// The algorithms and constants in this class
    /// apply only to IEEE-754 double-precision floating point format.
    /// </summary>
    public class DoubleBits {
        /// <summary>
        /// 
        /// </summary>
        public const int ExponentBias = 1023;

        /// <summary> 
        /// This value is chosen to be a few powers of 2 less than the
        /// number of bits available in the double representation (i.e. 53).
        /// This should allow enough extra precision for simple computations to be correct,
        /// at least for comparison purposes.
        /// </summary>
        public const int MinBinaryExponent = -50;

        private const double minValue = double.Epsilon * 4;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        public static double PowerOf2 ( int exp ) {
            if ( exp > 1023 || exp < -1022 )
                throw new ArgumentException("Exponent out of bounds");
            long expBias = exp + ExponentBias;
            long bits = (long) expBias << 52;
            return System.BitConverter.Int64BitsToDouble(bits);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public static int GetExponent ( double d ) {
            DoubleBits db = new DoubleBits(d);
            return db.Exponent;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public static double TruncateToPowerOfTwo ( double d ) {
            DoubleBits db = new DoubleBits(d);
            db.ZeroLowerBits(52);
            return db.Double;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public static string ToBinaryString ( double d ) {
            DoubleBits db = new DoubleBits(d);
            return db.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="d1"></param>
        /// <param name="d2"></param>
        /// <returns></returns>
        public static double MaximumCommonMantissa ( double d1, double d2 ) {
            if ( d1 == 0.0 || d2 == 0.0 )
                return 0.0;

            DoubleBits db1 = new DoubleBits(d1);
            DoubleBits db2 = new DoubleBits(d2);

            if ( db1.Exponent != db2.Exponent )
                return 0.0;

            int maxCommon = db1.NumCommonMantissaBits(db2);
            db1.ZeroLowerBits(64 - ( 12 + maxCommon ));
            return db1.Double;
        }

        private double x;
        private long xBits;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        public DoubleBits ( double x ) {
            this.x = x;
            xBits = System.BitConverter.DoubleToInt64Bits(x);
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual double Double {
            get {
                return System.BitConverter.Int64BitsToDouble(xBits);
            }
        }

        /// <summary>
        /// Determines the exponent for the number.
        /// </summary>
        public virtual int BiasedExponent {
            get {
                int signExp = (int) ( xBits >> 52 );
                int exp = signExp & 0x07ff;
                return exp;
            }
        }

        /// <summary>
        /// Determines the exponent for the number.
        /// </summary>
        public virtual int Exponent {
            get { return BiasedExponent - ExponentBias; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="nBits"></param>
        public virtual void ZeroLowerBits ( int nBits ) {
            long invMask = ( 1L << nBits ) - 1L;
            long mask = ~invMask;
            xBits &= mask;
        }


        public virtual int GetBit ( int i ) {
            long mask = ( 1L << i );
            return ( xBits & mask ) != 0 ? 1 : 0;
        }

        /// <summary> 
        /// This computes the number of common most-significant bits in the mantissa.
        /// It does not count the hidden bit, which is always 1.
        /// It does not determine whether the numbers have the same exponent - if they do
        /// not, the value computed by this function is meaningless.
        /// </summary>
        /// <param name="db"></param>
        /// <returns> The number of common most-significant mantissa bits.</returns>
        public virtual int NumCommonMantissaBits ( DoubleBits db ) {
            for ( int i = 0; i < 52; i++ ) {
                if ( GetBit(i) != db.GetBit(i) )
                    return i;
            }
            return 52;
        }

        /// <summary>
        /// A representation of the Double bits formatted for easy readability.
        /// </summary>
        public override string ToString () {
            string numStr = HexConverter.ConvertAny2Any(xBits.ToString(), 10, 2);

            // 64 zeroes!
            string zero64 = "0000000000000000000000000000000000000000000000000000000000000000";
            string padStr = zero64 + numStr;
            string bitStr = padStr.Substring(padStr.Length - 64);
            string str = bitStr.Substring(0, 1) + "  "
                + bitStr.Substring(1, 12) + "(" + Exponent + ") "
                + bitStr.Substring(12)
                + " [ " + x + " ]";
            return str;
        }

        /// <summary> 
        /// Computes whether the interval [min, max] is effectively zero width.
        /// I.e. the width of the interval is so much less than the
        /// location of the interval that the midpoint of the interval cannot be
        /// represented precisely.
        /// </summary>
        public static bool IsZeroWidth ( double min, double max ) {
            if ( max > min )
                return ( max - min ) < minValue;
            else
                return ( min - max ) < minValue;

            //double width = max - min;
            //double maxAbs = Math.Max(Math.Abs(min), Math.Abs(max));
            //double scaledInterval = width / maxAbs;
            //int level = DoubleBits.GetExponent(scaledInterval);
            //return level <= MinBinaryExponent;
        }
    }
}
