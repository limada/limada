/*
 * Limada
 * Version 0.081
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2006-2008 Lytico
 *
 * http://limada.sourceforge.net
 * 
 */

using System;
using Id = System.Int64;
using System.Runtime.Serialization;


namespace Limada.Model {
    public interface INumberThing : IThing<long> {
        NumberType NumberType { get; set; }
    }

    [Flags]
    public enum NumberType : int {
        Long = 0,
        Special = 1,
        Double = 2,
        DateTime = 4,
        /// <summary>
        /// lower 32-bit as integer
        /// </summary>
        Integer = 8,
        /// <summary>
        /// four 16bit-numbers; can be used to store small rectangles
        /// </summary>
        Quad16 = 16
    }

	[DataContract]
    public class NumberThing : Thing, INumberThing {
        public NumberThing() : base (){}
        public NumberThing(long data) : base() {
            this.Data = data;
        }

        public NumberThing(Id id, long data): base(id) {
            this.Data = data;
        }

        int _numberType = (int)NumberType.Long;
		
		[DataMember]
        public NumberType NumberType {
            get { return (NumberType)_numberType; }
            set { this.State.Setter(ref _numberType, (int)value); }
        }

        private long _data = 0;
		[DataMember]
        public virtual long Data {
            get { return _data; }
            set {
                this.State.Setter(ref _data, value);
                _numberType = (int)NumberType.Long;
            }
        }

        public override void MakeEqual(IThing thing) {
            base.MakeEqual(thing);
            if (thing is INumberThing) {
                this.NumberType = ( (INumberThing) thing ).NumberType;
            }
        }

        public virtual object Number {
            get {
                object result = Data;
                if (NumberType == NumberType.DateTime) {
                    result = LongConverters.LongToDateTime(Data);
                } else if (NumberType == NumberType.Integer) {
                    result = LongConverters.LongToInt(Data);
                } else if (NumberType == NumberType.Double) {
                    result = LongConverters.LongToDouble(Data);
                }
                if (NumberType == NumberType.Quad16) {
                    result = LongConverters.LongToQuad16(Data);
                } else if (NumberType == NumberType.Special) {
                    ;
                }
                return result;
            }
            set {
                try {
                    long Data = 0;
                    Type type = value.GetType();
                    if (type == typeof(long)) {
                        Data = (long)value;
                        NumberType = NumberType.Long;
                    } else if (type == typeof(DateTime)) {
                        Data = LongConverters.DateTimeToLong((DateTime)value);
                        NumberType = NumberType.DateTime;
                    } else if (type == typeof(int)) {
                        Data = LongConverters.IntToLong((int)value);
                        NumberType = NumberType.Integer;
                    } else if (type == typeof(double) ) {
                        Data = LongConverters.DoubleToLong((double)value);
                        NumberType = NumberType.Double;
                    } else if (type == typeof(float)) {
                        double d = (float)value;
                        Data = LongConverters.DoubleToLong(d);
                        NumberType = NumberType.Double;
                    } else if (type == typeof(Quad16)) {
                        Data = LongConverters.Quad16ToLong((Quad16)value);
                        NumberType = NumberType.Quad16;
                    } else {
                        NumberType = NumberType.Special;
                    }

                } catch (Exception e) {
                    throw new NotSupportedException(value.GetType() + " is not supported by NumberNode", e);

                }
            }

        }

        object IThing.Data {
            get { return this.Data; }
            set {
                if (value is long)
                    Data = (long)value;
            }
        }

        public override string ToString() {
            return Number.ToString ();
        }
    }

    public class LongConverters {
        // Remark: maybe everything better with: mcs\class\corlib\Mono\DataConverter


        /// <summary>
        /// 0x FFFF FFFF FFFF FFFF
        ///    hihi hilo lohi lolo
        /// </summary>
        protected enum Quarter : byte { hihi = 3, hilo = 2, lohi = 1, lolo = 0 };
        protected static long SetQuarter(Int16 value, Quarter quarter) {
            long result = value;
            long mask = 0x000000000000FFFFL;
            mask = mask << (16 * (byte)quarter);
            result = result << (16 * (byte)quarter);
            unchecked {
                result = result & mask;
            }

            return result;
        }

        protected static long GetQuarter(long value, Quarter quarter) {
            long result = value;
            long mask = 0x000000000000FFFFL;
            result = result >> (16 * (byte)quarter);
            // mask = mask << (16*(byte)quarter);
            unchecked {
                result = result & mask;
            }

            return result;
        }

        public static long Quad16ToLong(Quad16 rect) {
            long result = 0;

            result = result | SetQuarter(System.Convert.ToInt16(rect.X), Quarter.hihi);
            result = result | SetQuarter(System.Convert.ToInt16(rect.Y), Quarter.hilo);
            result = result | SetQuarter(System.Convert.ToInt16(rect.W), Quarter.lohi);
            result = result | SetQuarter(System.Convert.ToInt16(rect.H), Quarter.lolo);

            return result;
        }

        public static Quad16 LongToQuad16(long value) {
            long temp = GetQuarter(value, Quarter.hihi);

            Quad16 result = new Quad16(
                System.Convert.ToInt16(GetQuarter(value, Quarter.hihi)),
                System.Convert.ToInt16(GetQuarter(value, Quarter.hilo)),
                System.Convert.ToInt16(GetQuarter(value, Quarter.lohi)),
                System.Convert.ToInt16(GetQuarter(value, Quarter.lolo))
                );

            return result;

        }

        public static double LongToDouble(long value) {
#if !SILVERLIGHT
            double result = BitConverter.Int64BitsToDouble(value);
            return result;
#else
            throw new NotImplementedException ();
#endif

        }

        public static long DoubleToLong(double value) {
#if !SILVERLIGHT
            long result = BitConverter.DoubleToInt64Bits(value);
            return result;
#else
            throw new NotImplementedException();
#endif
        }

        public static DateTime LongToDateTime(long value) {
            DateTime result;
            // nice, but not compatible with version 1.0
            result = DateTime.FromOADate(DoubleToLong(value));
            return result;

        }

        public static long DateTimeToLong(DateTime value) {
            long result = 0;
            // nice, but not compatible with version 1.0
            result = DoubleToLong(value.ToOADate());
            return result;

        }

        /// <summary>
        /// lower 32-bit of long as integer
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int LongToInt(long value) {
            int result;
            byte[] bytes = BitConverter.GetBytes(value);
            result = BitConverter.ToInt32(bytes, 0);
            return result;
        }

        public static long IntToLong(int value) {
            long result = 0;
            //				byte[] bytes = BitConverter.GetBytes(value);
            //				result = BitConverter.ToInt64(bytes,0);
            result = value;
            return result;
        }
    }

    public struct Quad16 {
        public Int16 X;
        public Int16 Y;
        public Int16 W;
        public Int16 H;
        public Quad16(Int16 x, Int16 y, Int16 w, Int16 h) {
            this.X = x;
            this.Y = y;
            this.W = w;
            this.H = h;
        }
    }
}