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
using System.IO;
using Limaki.Common;
using Limaki.Model.Streams;
using Id = System.Int64;
using Limaki.Data;
using System.Runtime.Serialization;

namespace Limada.Model {

    [DataContract]
    public class StreamThing :  Thing, IStreamThing {
        
        //public StreamThing() : this(Common.Isaac.Long) { }
        //public StreamThing(Int64 id) { _id = id; }
        public StreamThing() : base (){}
        public StreamThing(Stream data) : base() {
            this.Data = data;
        }
        public StreamThing(Id id, Stream data) : base(id) {
            this.Data = data;
        }

        [Transient]
        private State _state = null;
        protected State state {
            get {
                if (_state == null) {
                    _state = new State();
                }
                return _state;
            }
            set { _state = value; }
        }

        #region Proxy-Handling

        [Transient]
        private IDataContainer<Id> _dataContainer = null;
        /// <summary>
        /// Remark: this design 
        /// </summary>
        public virtual IDataContainer<Id> DataContainer {
            get { return _dataContainer; }
            set { _dataContainer = value; }
        }

        [Transient]
        ByteStreamWrapper _streamWrapper = null;
        /// <summary>
        /// wraps the IRealData<byte[]>.Data over a MemoryStream
        /// </summary>
        internal virtual ByteStreamWrapper StreamWrapper {
            get {
                if (_streamWrapper == null) {
                    if (!state.Hollow && DataContainer!=null) {
                        IRealData<Id, Byte[]> realData =
                            DataContainer.GetById(this.Id) as IRealData<Id, Byte[]>;
                        if (realData != null) {
                            _streamWrapper = new ByteStreamWrapper (realData);
                        }
                    }
                }
                if (_streamWrapper == null) {
                    _streamWrapper = new ByteStreamWrapper(this.Id);
                    _streamWrapper.State.Hollow = true;
                }
                return _streamWrapper;
            }
        }

        public virtual void Flush() {
            if (_streamWrapper != null) {
                if (!StreamWrapper.State.Clean) {
                    StreamWrapper.Flush();
                    DataContainer.Add(this.StreamWrapper.InnerData);
                }
            } else {
                if (Compressable) {
                    Compress();
                } else if (_unCompressedStream != null) {
                    StreamWrapper.Stream = _unCompressedStream;
                }
                StreamWrapper.Flush();
                DataContainer.Add(this.StreamWrapper.InnerData);
            }
        }

        public virtual void ClearRealSubject(bool clean) {
            if (clean) {
                if (( _unCompressedStream != null ) && ( Compressable )) {
                    _unCompressedStream.Close ();
                }
            }
            _unCompressedStream = null;
            if (!state.Hollow && (_streamWrapper != null)) {
                if (StreamWrapper.State.Dirty) {
                    Flush();
                }
                StreamWrapper.Stream = null;
                _streamWrapper = null;
            }
        }

        public virtual void ClearRealSubject() {
            ClearRealSubject (true);
        }

        #endregion
        
        public virtual Stream Data {
            get {
                Stream result = null;
                if (_unCompressedStream != null) {
                    result = _unCompressedStream;
                } else {
                    if (StreamWrapper != null) {
                        result = _streamWrapper.Stream;
                        if (!Compressable) {
                            _unCompressedStream = result;
                        }
                    }
                }
                return result;
            }
            set {
                if (value == null) {
                    _streamWrapper = null;
                }
                _unCompressedStream = value;
                //if (!Compressable) {
                //    StreamWrapper.Stream = _unCompressedStream;
                //}
            }
        }

        public override void MakeEqual(IThing thing) {
            base.MakeEqual(thing);
            if (thing is IStreamThing) {
                this.StreamType = ( (IStreamThing) thing ).StreamType;
                this.Compression = ((IStreamThing)thing).Compression;
                ClearRealSubject ();
            }
        }
        object IThing.Data {
            get { return this.Data; }
            set {
                if (value is Stream)
                    Data = (Stream)value;
            }
        }

        public override string ToString() {
            return ((char)0x25A1).ToString(); // white square
        }

        #region Properties: Compression, StreamType
        
        int _compression = (int)CompressionType.None;
        
        [DataMember]
        public virtual CompressionType Compression {
            get { return (CompressionType)_compression; }
            set {
                int _value = (int)value;
                if (_compression != _value) {
                    state.Dirty = true;
                }
                _compression = _value;
            }
        }

        Id _streamType = -1;
        [DataMember]
        public virtual Id StreamType {
            get { return _streamType; }
            set {
                if (value != _streamType) {
                    state.Dirty = true;
                    _streamType = value;
                }
            }
        }

        #endregion

        #region Compression-Handling
        [Transient]
        Stream _unCompressedStream = null;
        
        [Transient]
        ICompressionWorker _compressionWorker = null;
        protected virtual ICompressionWorker CompressionWorker {
            get {
                if (_compressionWorker == null) {
                    _compressionWorker =
                        Registry.Pool.TryGetCreate<ICompressionWorker>();
                }
                return _compressionWorker;
            }
            set { _compressionWorker = value; }
        }

        protected virtual bool Compressable {
            get { return CompressionWorker.Compressable(Compression); }
            set { ; }
        }

        public virtual void Compress() {
            if (Compressable && (_unCompressedStream != null)) {
                Stream stream = _unCompressedStream;
                stream.Position = 0;
                if (StreamWrapper != null) {
                    StreamWrapper.Stream = CompressionWorker.Compress(stream, Compression);
                }
            }

        }

        public virtual void DeCompress() {
            if (Compressable && (_unCompressedStream == null) && (this.Data != null)) {
                this._unCompressedStream = null;
                _unCompressedStream = CompressionWorker.DeCompress(StreamWrapper.Stream, Compression);
                _streamWrapper.Clear();
                _streamWrapper = null;
                _unCompressedStream.Position = 0;
            }
        }


        #endregion
    }
}