/*
 * Limaki 
 * Version 0.071
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
using System.Collections.Generic;
using Limaki.Common;
using Limaki.Common.Collections;

namespace Limaki.Graphs {
    /// <summary>
    /// Converts one graph into an other graph
    /// in both directions (bidirectional)
    /// </summary>
    /// <typeparam name="TItemOne"></typeparam>
    /// <typeparam name="TItemTwo"></typeparam>
    /// <typeparam name="TEdgeOne"></typeparam>
    /// <typeparam name="TEdgeTwo"></typeparam>
    public abstract class GraphConverter<TItemOne, TItemTwo, TEdgeOne, TEdgeTwo>:
        IFactoryListener<TItemOne>
        where TEdgeOne : IEdge<TItemOne>, TItemOne
        where TEdgeTwo : IEdge<TItemTwo>, TItemTwo {

        public GraphConverter() {
            InitFactoryMethods();
        }

        public GraphConverter( IGraph<TItemOne, TEdgeOne> graphA, IGraph<TItemTwo, TEdgeTwo> graphB )
            : this() {
            this.One = graphA;
            this.Two = graphB;
        }
        GraphUniConverter<TItemOne, TItemTwo, TEdgeOne, TEdgeTwo> _oneTwoConverter = null;
        GraphUniConverter<TItemOne, TItemTwo, TEdgeOne, TEdgeTwo> OneTwoConverter {
            get {
                if (_oneTwoConverter == null) {
                    _oneTwoConverter = new GraphUniConverter<TItemOne, TItemTwo, TEdgeOne, TEdgeTwo>();
                    _oneTwoConverter.CreateItem = this.CreateItemTwo;
                    _oneTwoConverter.CreateEdge = this.CreateEdgeTwo;
                    _oneTwoConverter.RegisterEdge = this.RegisterOneTwo;
                    _oneTwoConverter.RegisterPair = this.RegisterPair;
                }
                return _oneTwoConverter;
            }
        }
        GraphUniConverter<TItemTwo, TItemOne, TEdgeTwo, TEdgeOne> _twoOneConverter;
        GraphUniConverter<TItemTwo, TItemOne, TEdgeTwo, TEdgeOne> TwoOneConverter {
            get {
                if (_twoOneConverter == null) {
                    _twoOneConverter = new GraphUniConverter<TItemTwo, TItemOne, TEdgeTwo, TEdgeOne>();
                    _twoOneConverter.CreateItem = this.CreateItemOne;
                    _twoOneConverter.CreateEdge = this.CreateEdgeOne;
                    _twoOneConverter.RegisterEdge = this.RegisterTwoOne;
                    _twoOneConverter.RegisterPair = this.RegisterPair;
                }
                return _twoOneConverter;
            }
        }

        public virtual IGraph<TItemOne, TEdgeOne> One {
            get { return OneTwoConverter.One; }
            set {
                OneTwoConverter.One = value;
                TwoOneConverter.Two = value;
            }
        }

        public virtual IGraph<TItemTwo, TEdgeTwo> Two {
            get { return OneTwoConverter.Two; }
            set {
                OneTwoConverter.Two = value;
                TwoOneConverter.One = value;
            }
        }

        public IDictionary<TItemOne, TItemTwo> One2Two {
            get { return OneTwoConverter.Dict; }
            set { OneTwoConverter.Dict = value; }
        }

        public virtual void RegisterPair( TItemOne a, TItemTwo b ) {
            if (!OneTwoConverter.Dict.ContainsKey(a))
                OneTwoConverter.Dict.Add(a, b);
            if (!TwoOneConverter.Dict.ContainsKey(b))
                TwoOneConverter.Dict.Add(b, a);
        }

        public virtual void RegisterPair( TItemTwo b, TItemOne a ) {
            RegisterPair(a, b);
        }

        #region OneTwo

        public IDictionary<TItemTwo,TItemOne> Two2One {
            get { return TwoOneConverter.Dict; }
            set { TwoOneConverter.Dict = value; }
        }

        public virtual TItemTwo Convert( TItemOne a ) {
            return OneTwoConverter.Convert(a);
        }

        public virtual void ConvertOneTwo() {
            OneTwoConverter.Convert();
        }

        public virtual TItemTwo Get( TItemOne a ) {
            TItemTwo result = default(TItemTwo);
            if (a == null)
                return result;
            OneTwoConverter.Dict.TryGetValue(a, out result);
            return result;

        }
        #endregion

        #region TwoOne

        public virtual TItemOne Convert( TItemTwo b ) {
            return TwoOneConverter.Convert(b);
        }

        public virtual TItemOne Get( TItemTwo b ) {
            TItemOne result = default(TItemOne);
            TwoOneConverter.Dict.TryGetValue(b, out result);
            return result;

        }
        public virtual void ConvertTwoOne() {
            TwoOneConverter.Convert();
        }

        public virtual void Clear() {
            OneTwoConverter.Dict.Clear();
            TwoOneConverter.Dict.Clear();
        }
        #endregion

        #region Factory-Methods

        public abstract void InitFactoryMethods();

        public Converter<TItemOne, TItemTwo> CreateItemTwo = null;
        public Converter<TEdgeOne, TEdgeTwo> CreateEdgeTwo = null;
        public Act<TEdgeOne, TEdgeTwo> RegisterOneTwo=null;

        public Converter<TItemTwo,TItemOne> CreateItemOne = null;
        public Converter<TEdgeTwo,TEdgeOne> CreateEdgeOne = null;
        public Act<TEdgeTwo,TEdgeOne> RegisterTwoOne=null;

        #endregion



        #region IFactoryListener<TItemOne> Member

        public virtual Action<TItemOne> CreateListener {
            get {
                return TwoOneConverter.CreateListener;
            }
            set {
                TwoOneConverter.CreateListener = value;
            }
        }

        #endregion
    }
}
