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
using Limada.Model;
using Limada.Schemata;
using Limaki.Common;
using Limaki.Drawing;
using Limaki.Graphs;
using Limaki.Model.Streams;
using Limaki.Widgets;

namespace Limada.View {
    public class WidgetThingAdapter:GraphModelAdapter<IWidget,IThing,IEdgeWidget,ILink> {
        private IThingFactory _thingFactory = null;
        public IThingFactory ThingFactory {
            get {
                if (_thingFactory == null) {
                    _thingFactory = new ThingFactory();
                }
                return _thingFactory;
            }
            set { _thingFactory = value; }
        }

        private IWidgetFactory _widgetFactory = null;
        public IWidgetFactory WidgetFactory {
            get {
                if (_widgetFactory == null) {
                    _widgetFactory = Registry.Factory.One<IWidgetFactory> ();
                }
                return _widgetFactory;
            }
            set { _widgetFactory = value; }
        }

        private IShapeFactory _shapeFactory = null;
        public IShapeFactory ShapeFactory {
            get {
                if (_shapeFactory == null) {
                    _shapeFactory = Registry.Factory.One<IShapeFactory>();
                }
                return _shapeFactory;
            }
            set { _shapeFactory = value; }
        }

        public override IWidget CreateItemOne(IGraph<IThing, ILink> sender,
            IGraph<IWidget, IEdgeWidget> target, IThing a) {
            
            IWidget result = WidgetFactory.CreateWidget(ThingDataToDisplay(sender,a));
            if (SchemaFacade.DescriptionableThing(a)) {
                if (a is IThing<Stream>) {
                    result.Shape = ShapeFactory.One<IBezierShape>();
                } else {
                    result.Shape = ShapeFactory.One<IRectangleShape>();
                }
            }

            return result;
        }

        public override IEdgeWidget CreateEdgeOne(IGraph<IThing, ILink> sender, 
            IGraph<IWidget, IEdgeWidget>target, ILink a) {
            //if (a.Marker == null) { // this should never happen!:
            //    a.Marker = CommonSchema.EmptyMarker;
            //}
            return WidgetFactory.CreateEdgeWidget(ThingDataToDisplay(sender,a.Marker));

        }

        public override IThing CreateItemTwo(IGraph<IWidget, IEdgeWidget> sender, 
            IGraph<IThing, ILink> target, IWidget b) {
            
            IThing result =  ThingFactory.CreateThing(target as IThingGraph, b.Data);
            
            return result;
        }

        public override ILink CreateEdgeTwo(IGraph<IWidget, IEdgeWidget> sender,
            IGraph<IThing, ILink> target, IEdgeWidget b) {
            return ThingFactory.CreateLink(target as IThingGraph, b.Data);
        }

        public virtual void SetWidgetByThing(IGraph<IThing, ILink> graph, IWidget target, IThing source) {
            target.Data = ThingDataToDisplay (graph, source);
        }

        public object ThingDataToDisplay(IGraph<IThing, ILink> graph, IThing thing) {
            if (thing == null)
                return CommonSchema.NullString;

            thing = ThingToDisplay (graph, thing);
            object result = null;
            if (thing is IProxy) {
                result = CommonSchema.ProxyString;
            } else {
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


        public IThing ThingToDisplay(IGraph<IThing, ILink> graph, IThing thing) {
            if (graph is SchemaThingGraph) {
                return( (SchemaThingGraph) graph ).ThingToDisplay (thing);
            } else {
                return thing;
            }
        }

        public virtual IThing SetThingByData(IGraph<IThing, ILink> graph, IThing thing, object data) {
            IThing itemToChange = ThingToDisplay(graph,thing);
            if (thing != null && itemToChange == thing && SchemaFacade.DescriptionableThing(thing)) {
                itemToChange = ThingFactory.CreateThing(data);
                graph.Add(new Link(thing, itemToChange, CommonSchema.DescriptionMarker));

            } else {
                itemToChange.Data = data;
                // this is necessary because db4o.graph does not save by itself
                graph.Add (itemToChange);
            }
            return itemToChange;
        }

        public override void ChangeData(IGraph<IThing, ILink> graph, IThing item, object data) {
            throw new Exception("The method or operation is not implemented.");
        }

        public override void ChangeData(IGraph<IWidget, IEdgeWidget> sender, IWidget widget, object data) {
            WidgetThingGraph graph = sender as WidgetThingGraph;
            
            if (graph == null)
                throw new ArgumentException();

            IThing thing = graph.Get(widget);
            if ( thing != null ) {
                if (thing is ILink) {
                    ILink link = (ILink) thing;
                    if (data is IThing) {
                        
                        link.Marker = (IThing) data;
                        // this is necessary because db4o.graph does not save by itself
                        graph.Two.Add (link.Marker);

                        SetWidgetByThing(graph.Two, widget, link.Marker);

                    }
                } else {
                    thing = SetThingByData (graph.Two,thing, data);
                    SetWidgetByThing (graph.Two, widget, thing);
                }
            }
        }
    }
}