/*
 * Limada
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2006 - 2021 Lytico
 *
 * http://www.limada.org
 * 
 */

using System;
using System.Runtime.Serialization;

namespace Limada.Things.Model.Dto {

    [DataContract]
    public class NumberThing : Thing<long>, INumberThing, IThing<int>, IThing<double>, IThing<DateTime>,
        IThing<Quad16> {

        public NumberThing () : base () { }

        public NumberThing (long data) : base () { Data = data; }

        public NumberThing (Guid id, long data) : base (id, data) { }

        [DataMember]
        public virtual NumberType NumberType { get; set; }

        public virtual object Number {
            get {
                object result = Data;

                switch (NumberType) {
                    case NumberType.DateTime:
                        return LongConverters.LongToDateTime (Data);
                    case NumberType.Integer:
                        return LongConverters.LongToInt (Data);
                    case NumberType.Double:
                        return LongConverters.LongToDouble (Data);
                    case NumberType.Quad16:
                        return LongConverters.LongToQuad16 (Data);
                    case NumberType.Special:
                        break;
                }

                return result;
            }
            set {
                try {
                    long data = 0;
                    var numberType = NumberType.Long;
                    var type = value.GetType ();

                    if (type == typeof(long)) {
                        data = (long)value;
                        numberType = NumberType.Long;
                    } else if (type == typeof(DateTime)) {
                        data = LongConverters.DateTimeToLong ((DateTime)value);
                        numberType = NumberType.DateTime;
                    } else if (type == typeof(int)) {
                        data = LongConverters.IntToLong ((int)value);
                        numberType = NumberType.Integer;
                    } else if (type == typeof(double)) {
                        data = LongConverters.DoubleToLong ((double)value);
                        numberType = NumberType.Double;
                    } else if (type == typeof(float)) {
                        double d = (float)value;
                        data = LongConverters.DoubleToLong (d);
                        numberType = NumberType.Double;
                    } else if (type == typeof(Quad16)) {
                        data = LongConverters.Quad16ToLong ((Quad16)value);
                        numberType = NumberType.Quad16;
                    } else {
                        numberType = NumberType.Special;
                    }

                    Data = data;
                    NumberType = numberType;

                } catch (Exception e) {
                    throw new NotSupportedException (value.GetType () + " is not supported by NumberNode", e);

                }
            }
        }

        object IThing.Data {
            get => Data;
            set {
                switch (value) {
                    case long l:
                        Data = l;

                        break;
                    case int i:
                        Data = LongConverters.IntToLong (i);

                        break;
                    case string s:
                        FromString (s);

                        break;
                }
            }
        }

        public virtual void FromString (string s) {
            if (NumberType.HasFlag (NumberType.Long) || NumberType.HasFlag (NumberType.Integer)) {
                long number = 0;

                if (long.TryParse (s, out number)) {
                    Data = number;
                }
            }
        }

        public override string ToString () { return Number.ToString (); }

        int IThing<int>.Data { get => LongConverters.LongToInt (Data); set => Data = LongConverters.IntToLong (value); }

        double IThing<double>.Data {
            get => LongConverters.LongToDouble (Data);
            set => Data = LongConverters.DoubleToLong (value);
        }

        DateTime IThing<DateTime>.Data {
            get => LongConverters.LongToDateTime (Data);
            set => Data = LongConverters.DateTimeToLong (value);
        }

        Quad16 IThing<Quad16>.Data {
            get => LongConverters.LongToQuad16 (Data);
            set => Data = LongConverters.Quad16ToLong (value);
        }

    }

}