/*
 * Limada
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
using Id = System.Int64;
using System.Runtime.Serialization;


namespace Limada.Model {

    public interface INumberThing : IThing<long> {
        NumberType NumberType { get; set; }
        object Number { get; set; }
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
    public class NumberThing : Thing, INumberThing, IThing<int>, IThing<double>, IThing<DateTime>, IThing<Quad16> {

        internal NumberThing() : base (){}
        public NumberThing(long data) : base() {
            this.Data = data;
        }

        public NumberThing(Id id, long data): base(id) {
            this._data = data;
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
                    long data = 0;
                    var numberType = NumberType.Long;
                    var type = value.GetType();
                    if (type == typeof(long)) {
                        data = (long)value;
                        numberType = NumberType.Long;
                    } else if (type == typeof(DateTime)) {
                        data = LongConverters.DateTimeToLong((DateTime)value);
                        numberType = NumberType.DateTime;
                    } else if (type == typeof(int)) {
                        data = LongConverters.IntToLong((int)value);
                        numberType = NumberType.Integer;
                    } else if (type == typeof(double) ) {
                        data = LongConverters.DoubleToLong((double)value);
                        numberType = NumberType.Double;
                    } else if (type == typeof(float)) {
                        double d = (float)value;
                        data = LongConverters.DoubleToLong(d);
                        numberType = NumberType.Double;
                    } else if (type == typeof(Quad16)) {
                        data = LongConverters.Quad16ToLong((Quad16)value);
                        numberType = NumberType.Quad16;
                    } else {
                        numberType = NumberType.Special;
                    }
                    this.Data = data;
                    this.NumberType = numberType;


                } catch (Exception e) {
                    throw new NotSupportedException(value.GetType() + " is not supported by NumberNode", e);

                }
            }

        }

        object IThing.Data {
            get { return this.Data; }
            set {
                if (value is long)
                    Data = (long) value;
                else  if (value is int)
                    Data = LongConverters.IntToLong ((int) value);
                else if (value is string) {
                    FromString ((string) value);
                }
            }
        }

        public virtual void FromString (string s) {
            if (this.NumberType.HasFlag(NumberType.Long) || this.NumberType.HasFlag(NumberType.Integer)) {
                long number = 0;
                if (long.TryParse(s, out number)) {
                    Data = number;
                }
            }
        }

        public override string ToString() {
            return Number.ToString ();
        }

       
        int IThing<int>.Data {
            get { return LongConverters.LongToInt(this.Data); }
            set { this.Data = LongConverters.IntToLong(value); }
        }

        double IThing<double>.Data {
            get { return LongConverters.LongToDouble(this.Data); }
            set { this.Data = LongConverters.DoubleToLong(value); }
        }

        DateTime IThing<DateTime>.Data {
            get { return LongConverters.LongToDateTime(this.Data); }
            set { this.Data = LongConverters.DateTimeToLong(value); }
        }

        Quad16 IThing<Quad16>.Data {
            get { return LongConverters.LongToQuad16(this.Data); }
            set { this.Data = LongConverters.Quad16ToLong(value); }
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
            return value;
            //				byte[] bytes = BitConverter.GetBytes(value);
            //				result = BitConverter.ToInt64(bytes,0);
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
        public override string ToString () {
            return string.Format("{{X={0} Y={1} W={2} H={3}}}",X,Y,W,H);
        }
    }
}