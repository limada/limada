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
using System.IO;
using Limaki.Common;
using Limaki.Model.Content;
using Id = System.Int64;
namespace Limada.Model {
    public class ByteStreamWrapper {
        private Id _id; 
        public ByteStreamWrapper(Id id) {
            this._id = id;
        }
        public ByteStreamWrapper(IRealData<Id,byte[]> innerData) {
            this.InnerData = innerData;
            this._id = innerData.Id;
        }


        public State State = new State();

        private IRealData<Id, byte[]> _innerData = null;
        public IRealData<Id, byte[]> InnerData {
            get {
                if(_innerData == null) {
                    _innerData = new RealData<byte[]> (_id);
                }
                return _innerData;
            }
            set { _innerData = value; }
        }

        
        Stream _stream = null;
        public virtual Stream Stream {
            get {
                if ((_stream == null) && (InnerData.Data != null)) {
                    _stream = new MemoryStream(InnerData.Data,0,InnerData.Data.Length,true,true);
                    _stream.Position = 0;
                }
                return _stream;
            }
            set {
                if (_stream != value) {
                    _stream = value;
                    if (_stream != null) {
                        Flush();
                    }
                }

            }
        }

        public virtual void Clear() {
            _stream = null;
        }

        /// <summary>
        /// writes stream to streamBuffer
        /// sets stream = null;
        /// </summary>
        public virtual void Flush() {

            if (Stream != null) {
                InnerData.Data = new Byte[Stream.Length];
                Stream.Position = 0;
                int i = System.Convert.ToInt32(Stream.Length);
                Stream.Read(InnerData.Data, 0, i);
                Stream.Position = 0;
            }
            _stream = null;

        }
        

        
    }
}