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
using Limada.Model;
using Limada.Schemata;
using Limaki.Common;
using Limaki.Drawing;
using Limaki.Drawing.Shapes;
using Limaki.Graphs;
using Limaki.Model.Content;
using Limaki.Visuals;

namespace Limada.VisualThings {

    /// <summary>
    /// transforms a IThing into a IVisual
    /// </summary>
    public class VisualThingTransformer:GraphItemTransformer<IVisual,IThing,IVisualEdge,ILink> {
        private IThingFactory _thingFactory = null;
        public IThingFactory ThingFactory {
            get {
                if (_thingFactory == null) {
                    _thingFactory = Registry.Factory.Create<IThingFactory>();
                }
                return _thingFactory;
            }
            set { _thingFactory = value; }
        }

        private IVisualFactory _visualFactory = null;
        public IVisualFactory VisualFactory {
            get {
                if (_visualFactory == null) {
                    _visualFactory = Registry.Factory.Create<IVisualFactory> ();
                }
                return _visualFactory;
            }
            set { _visualFactory = value; }
        }

        private IShapeFactory _shapeFactory = null;
        public IShapeFactory ShapeFactory {
            get {
                if (_shapeFactory == null) {
                    _shapeFactory = Registry.Factory.Create<IShapeFactory>();
                }
                return _shapeFactory;
            }
            set { _shapeFactory = value; }
        }

        public override IVisual CreateSinkItem(IGraph<IThing, ILink> source,
            IGraph<IVisual, IVisualEdge> sink, IThing a) {
            
            var result = VisualFactory.CreateItem(ThingDataToDisplay(source,a));
            if (SchemaFacade.DescriptionableThing(a)) {
                if (a is IThing<Stream>) {
                    result.Shape = ShapeFactory.Create<IBezierRectangleShape>();
                } else {
                    result.Shape = ShapeFactory.Create<IRectangleShape>();
                }
            } else {
                result.Shape = ShapeFactory.Create<IRoundedRectangleShape>();
            }

            return result;
        }

        public override IVisualEdge CreateSinkEdge (IGraph<IThing, ILink> source, IGraph<IVisual, IVisualEdge> sink, ILink a) {
            //if (a.Marker == null) { // this should never happen!:
            //    a.Marker = CommonSchema.EmptyMarker;
            //}
            return VisualFactory.CreateEdge(ThingDataToDisplay(source,a.Marker));

        }

        public override IThing CreateSourceItem(IGraph<IVisual, IVisualEdge> sink, 
            IGraph<IThing, ILink> source, IVisual b) {
            
            var result =  ThingFactory.CreateItem(source as IThingGraph, b.Data);
            
            return result;
        }

        public override ILink CreateSourceEdge(IGraph<IVisual, IVisualEdge> sink,
            IGraph<IThing, ILink> source, IVisualEdge b) {
            return ThingFactory.CreateEdge(source as IThingGraph, b.Data);
        }

        public object ThingDataToDisplay(IGraph<IThing, ILink> graph, IThing thing) {
            if (thing == null)
                return CommonSchema.NullString;

            thing = graph.ThingToDisplay (thing);
            object result = null;
            if (thing is IProxy) {
                result = CommonSchema.ProxyString;
            } else {
                if (thing is INumberThing)
                    result = ((INumberThing) thing).Number;
                else
                    result = thing.Data;
            }

            if (thing is ILink)
                result = thing.ToString ();
            else if (thing.Id == CommonSchema.EmptyMarker.Id) {
                result = CommonSchema.EmtpyMarkerString;
            } else if (thing.GetType() == typeof(Thing))
                result = CommonSchema.ThingString;

            return result;
        }

        public virtual IThing SetThingByData(IGraph<IThing, ILink> graph, IThing thing, object data) {
            var itemToChange = graph.ThingToDisplay (thing);
            if (thing != null && itemToChange == thing && SchemaFacade.DescriptionableThing(thing)) {
                itemToChange = ThingFactory.CreateItem(data);
                graph.Add(new Link(thing, itemToChange, CommonSchema.DescriptionMarker));

            } else {
                graph.DoChangeData (itemToChange, data);
            }
            return itemToChange;
        }

        /// <summary>
        /// sets Target.Data to ThingDataToDisplay of source
        /// </summary>
        /// <param name="thingGraph"></param>
        /// <param name="visual"></param>
        /// <param name="thing"></param>
        public virtual void SetVisualByThing (IGraph<IThing, ILink> thingGraph, IVisual visual, IThing thing) {
            visual.Data = ThingDataToDisplay(thingGraph, thing);
        }

        public override void ChangeData(IGraph<IVisual, IVisualEdge> sink, IVisual visual, object data) {
            var graph = sink as VisualThingGraph;
            
            if (graph == null)
                throw new ArgumentException();

            var thing = graph.Get(visual);
            if ( thing != null ) {
                if (thing is ILink) {
                    var link = (ILink) thing;
                    if (data is IThing) {
                        graph.Source.DoChangeData (link.Marker, data);
                        SetVisualByThing(graph.Source, visual, link.Marker);
                    }
                } else {
                    thing = SetThingByData (graph.Source, thing, data);
                    SetVisualByThing (graph.Source, visual, thing);
                }
            }
        }

        public override void ChangeData (IGraph<IThing, ILink> graph, IThing item, object data) {
            throw new Exception("The method or operation is not implemented.");
        }
    }
}