/*
 * Limaki 
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
using System.Collections.Generic;

namespace Limaki.Graphs {
    /// <summary>
    /// Converts one graph into an other graph
    /// in both directions (bidirectional)
    /// </summary>
    /// <typeparam name="TItemOne"></typeparam>
    /// <typeparam name="TItemTwo"></typeparam>
    /// <typeparam name="TEdgeOne"></typeparam>
    /// <typeparam name="TEdgeTwo"></typeparam>
    public class GraphMapper<TItemOne, TItemTwo, TEdgeOne, TEdgeTwo>:
        IFactoryListener<TItemOne>
        where TEdgeOne : IEdge<TItemOne>, TItemOne
        where TEdgeTwo : IEdge<TItemTwo>, TItemTwo {

        public GraphMapper(GraphModelAdapter<TItemOne, TItemTwo, TEdgeOne, TEdgeTwo> adapter) {
            this.Adapter = adapter;    
        }

        public GraphMapper() {}

        public GraphMapper( IGraph<TItemOne, TEdgeOne> graphA, IGraph<TItemTwo, TEdgeTwo> graphB,
            GraphModelAdapter<TItemOne, TItemTwo, TEdgeOne, TEdgeTwo> adapter)
            : this() {
            this.Adapter = adapter;
            this.One = graphA;
            this.Two = graphB;
        }

        private GraphModelAdapter<TItemOne, TItemTwo, TEdgeOne, TEdgeTwo> _adapter = null;
        public GraphModelAdapter<TItemOne, TItemTwo, TEdgeOne, TEdgeTwo> Adapter {
            get { return _adapter; }
            set {
                _adapter = value;
                CreateItemOne = Adapter.CreateItemOne;
                CreateEdgeOne = Adapter.CreateEdgeOne;
                CreateItemTwo = Adapter.CreateItemTwo;
                CreateEdgeTwo = Adapter.CreateEdgeTwo;
                EdgeCreatedOneTwo = Adapter.EdgeCreated;
                EdgeCreatedTwoOne = Adapter.EdgeCreated;
            }
        }

        protected GraphMapper(GraphMapperOneTwo<TItemOne, TItemTwo, TEdgeOne, TEdgeTwo> oneTwoMapper,
            GraphMapperOneTwo<TItemTwo, TItemOne, TEdgeTwo, TEdgeOne> twoOneMapper)
            : this() {
            this._oneTwoMapper = oneTwoMapper;
            this._twoOneMapper = twoOneMapper;
        }

        GraphMapperOneTwo<TItemOne, TItemTwo, TEdgeOne, TEdgeTwo> _oneTwoMapper = null;
        GraphMapperOneTwo<TItemOne, TItemTwo, TEdgeOne, TEdgeTwo> OneTwoMapper {
            get {
                if (_oneTwoMapper == null) {
                    _oneTwoMapper = new GraphMapperOneTwo<TItemOne, TItemTwo, TEdgeOne, TEdgeTwo>();
                    _oneTwoMapper.CreateItem = this.CreateItemTwo;
                    _oneTwoMapper.CreateEdge = this.CreateEdgeTwo;
                    _oneTwoMapper.EdgeCreated = this.EdgeCreatedOneTwo;
                    _oneTwoMapper.RegisterPair = this.RegisterPair;
                }
                return _oneTwoMapper;
            }
        }

        GraphMapperOneTwo<TItemTwo, TItemOne, TEdgeTwo, TEdgeOne> _twoOneMapper;
        GraphMapperOneTwo<TItemTwo, TItemOne, TEdgeTwo, TEdgeOne> TwoOneMapper {
            get {
                if (_twoOneMapper == null) {
                    _twoOneMapper = new GraphMapperOneTwo<TItemTwo, TItemOne, TEdgeTwo, TEdgeOne>();
                    _twoOneMapper.CreateItem = this.CreateItemOne;
                    _twoOneMapper.CreateEdge = this.CreateEdgeOne;
                    _twoOneMapper.EdgeCreated = this.EdgeCreatedTwoOne;
                    _twoOneMapper.RegisterPair = this.RegisterPair;
                }
                return _twoOneMapper;
            }
        }

        public virtual IGraph<TItemOne, TEdgeOne> One {
            get { return OneTwoMapper.One; }
            set {
                OneTwoMapper.One = value;
                TwoOneMapper.Two = value;
            }
        }

        public virtual IGraph<TItemTwo, TEdgeTwo> Two {
            get { return OneTwoMapper.Two; }
            set {
                OneTwoMapper.Two = value;
                TwoOneMapper.One = value;
            }
        }

        public IDictionary<TItemOne, TItemTwo> One2Two {
            get { return OneTwoMapper.Dict; }
            set { OneTwoMapper.Dict = value; }
        }

        public virtual void RegisterPair( TItemOne a, TItemTwo b ) {
            OneTwoMapper.Dict[a] = b;
            TwoOneMapper.Dict[b] = a;
            //if (!OneTwoMapper.Dict.ContainsKey(a))
            //    OneTwoMapper.Dict.Add(a, b);
            //if (!TwoOneMapper.Dict.ContainsKey(b))
            //    TwoOneMapper.Dict.Add(b, a);
        }

        public virtual void RegisterPair( TItemTwo b, TItemOne a ) {
            RegisterPair(a, b);
        }

        #region OneTwo

        public IDictionary<TItemTwo,TItemOne> Two2One {
            get { return TwoOneMapper.Dict; }
            set { TwoOneMapper.Dict = value; }
        }

        public virtual TItemTwo TryGetCreate( TItemOne a ) {
            return OneTwoMapper.TryGetCreate(a);
        }

        public virtual void ConvertOneTwo() {
            OneTwoMapper.Convert();
        }

        public virtual TItemTwo Get( TItemOne a ) {
            TItemTwo result = default(TItemTwo);
            if (a == null)
                return result;
            OneTwoMapper.Dict.TryGetValue(a, out result);
            return result;

        }
        #endregion

        #region TwoOne

        public virtual TItemOne TryGetCreate( TItemTwo b ) {
            return TwoOneMapper.TryGetCreate(b);
        }

        public virtual TItemOne Get( TItemTwo b ) {
            TItemOne result = default(TItemOne);
            if (b == null)
                return result;
            TwoOneMapper.Dict.TryGetValue(b, out result);
            return result;

        }
        public virtual void ConvertTwoOne() {
            TwoOneMapper.Convert();
        }

        public virtual void Clear() {
            OneTwoMapper.Dict = null;
            TwoOneMapper.Dict = null;
        }

        #endregion

        #region Factory-Methods

        public Func<IGraph<TItemOne, TEdgeOne>, IGraph<TItemTwo, TEdgeTwo>, TItemOne, TItemTwo> 
            CreateItemTwo = null;
        public Func<IGraph<TItemOne, TEdgeOne>, IGraph<TItemTwo, TEdgeTwo>, TEdgeOne, TEdgeTwo> 
            CreateEdgeTwo = null;
        public Action<TEdgeOne, TEdgeTwo> EdgeCreatedOneTwo=null;

        public Func<IGraph<TItemTwo, TEdgeTwo>, IGraph<TItemOne, TEdgeOne>, TItemTwo, TItemOne> 
            CreateItemOne = null;
        public Func<IGraph<TItemTwo, TEdgeTwo>, IGraph<TItemOne, TEdgeOne>, TEdgeTwo, TEdgeOne> 
            CreateEdgeOne = null;
        public Action<TEdgeTwo,TEdgeOne> EdgeCreatedTwoOne=null;

        #endregion

        #region IFactoryListener<TItemOne> Member
        public virtual Action<TItemOne> ItemCreated {
            get { return TwoOneMapper.ItemCreated; }
            set { TwoOneMapper.ItemCreated = value; }
        }

        #endregion

        public GraphMapper<TItemTwo, TItemOne, TEdgeTwo, TEdgeOne> ReverseMapper() {
            GraphMapper<TItemTwo, TItemOne, TEdgeTwo, TEdgeOne> result =
                new GraphMapper<TItemTwo, TItemOne, TEdgeTwo, TEdgeOne>(
                this.TwoOneMapper, 
                this.OneTwoMapper);

            result.Adapter = this.Adapter.ReverseAdapter ();
            result.One = this.Two;
            result.Two = this.One;

            return result;
        }
    }
}
