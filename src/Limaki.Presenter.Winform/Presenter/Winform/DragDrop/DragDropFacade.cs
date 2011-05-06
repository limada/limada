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
using System.IO;
using System.Windows.Forms;
using Limaki.Drawing;
using Limaki.Graphs;
using Limaki.Graphs.Extensions;
using Limaki.Widgets;
using System.Text;
using Limaki.Presenter.Widgets.UI;

namespace Limaki.Presenter.Winform.DragDrop {
    /// <summary>
    /// encapsulates operations to realise DragDrop or CopyPaste events
    /// </summary>
    public class DragDropFacade {
        #region DataObject handling
        private DataObjectHandlerChain chain = new DataObjectHandlerChain();

        public DragDropFacade() {
            chain.InitDataObjectHanders();
        }

        public IWidget GetWidget(IDataObject dataObject, IGraph<IWidget, IEdgeWidget> graph, bool inProcess) {
            IWidget widget = chain.GetWidget(dataObject, graph, inProcess);
            return widget;
        }

        public bool IsValidData(IDataObject data) {
            return chain.IsValidData(data);
        }

        public IDataObject SetWidget(IGraph<IWidget, IEdgeWidget> graph, IWidget widget, IDataObject dataObject) {
            if (dataObject == null) throw new ArgumentNullException("dataObject");
            chain.SetWidget(dataObject, graph,widget);
            return dataObject;
        }

        public IDataObject SetWidget(IGraph<IWidget, IEdgeWidget> graph, IWidget widget) {
            return SetWidget(graph,widget, new DataObject());
        }

        public IDataObject SetWidget(IControl control, IGraph<IWidget, IEdgeWidget> graph, IWidget widget) {
            ControlDataObject dataObject = new ControlDataObject();
            dataObject.control = control;
            return SetWidget(graph,widget, dataObject);
        }

        public IWidget Clone(IGraph<IWidget, IEdgeWidget> graph, IWidget item) {
            return new SerializedWidgetDataObjectHandler().Clone(graph,item);
        }
        #endregion

        #region Scene handling

        public virtual void LinkItem(Scene scene,IWidget item, PointI pt, int hitSize, bool itemIsRoot) {
            if (item != null) {
                IWidget target = scene.Hovered;
                if (target == null && scene.Focused != null && scene.Focused.Shape.IsHit(pt, hitSize)) {
                    target = scene.Focused;
                }
                if (item != target) {
                    if (itemIsRoot)
                        SceneTools.CreateEdge (scene, item, target);
                    else
                        SceneTools.CreateEdge(scene, target, item);
                }
            }
        }

        public IWidget PlaceWidget(IDataObject dataObject, IGraphScene<IWidget,IEdgeWidget> scene, IGraphLayout<IWidget,IEdgeWidget> layout) {
            IWidget widget = GetWidget (dataObject, scene.Graph,false);
            SceneTools.PlaceWidget(scene.Focused,widget,scene as Scene,layout);
            //scene.Selected.Clear();
            //scene.Focused = widget;
            return widget;

        }



       public virtual bool DoDragDrop(IGraphScene<IWidget,IEdgeWidget> ascene, IControl control, IDataObject dataObject, IGraphLayout<IWidget,IEdgeWidget> layout, PointI pt, int hitsize) {
           Scene scene = ascene as Scene;
           
            IWidget item = null;
            bool itemIsRoot = false;

            if (dataObject is ControlDataObject) { // data is sent from same application
                ControlDataObject data = (ControlDataObject)dataObject;
                if (data.control == control) { // data is sent from same control, so get the item as native instance
                    item = GetWidget(dataObject, scene.Graph,true);
                    itemIsRoot = true;
                }

                // data is sent from another control
                else if (data.control is IDisplayDevice<IGraphScene<IWidget,IEdgeWidget>>) {// the other control has a Scene
                    var targetGraph =
                        scene.Graph as IGraphPair<IWidget, IWidget, IEdgeWidget, IEdgeWidget>;

					var display = ((IDisplayDevice)data.control).Display as IDisplay<IGraphScene<IWidget,IEdgeWidget>>;
                    var sourceGraph =
                        display.Data.Graph as IGraphPair<IWidget, IWidget, IEdgeWidget, IEdgeWidget>;

                    if (targetGraph != null && sourceGraph != null) {
                        var sourceitem = GetWidget(dataObject, sourceGraph, true);

                        item = GraphMapping.Mapping.LookUp<IWidget, IEdgeWidget>(sourceGraph, targetGraph, sourceitem);

                        SceneTools.AddItem(scene, item, layout,pt);


                    } else {
                        item = GetWidget(dataObject, scene.Graph,false);
                        SceneTools.AddItem(scene, item, layout, pt);
                    }
                } else {// the other control is not a widget control
                    item = GetWidget(dataObject, scene.Graph,false);
                    SceneTools.AddItem(scene, item, layout, pt);
                }
            } else { // data is sent form another application
                item = GetWidget(dataObject, scene.Graph,false);
                SceneTools.AddItem(scene, item, layout, pt);
            }
            if (item == null) {
               return false;
            }
            LinkItem(scene, item, pt, hitsize, itemIsRoot);
            return true;
        }
        #endregion

        public static Uri GetUri(IDataObject dataObject) {
            Uri result = null;
            string uriFormat = "UniformResourceLocator";
            if (dataObject.GetDataPresent(uriFormat)) {
                Stream s = dataObject.GetData (uriFormat) as Stream;
                string uri = new StreamReader (s).ReadToEnd();
                if (uri.EndsWith("\0"))
                    uri = uri.Remove (uri.Length - 1);
                result = new Uri (uri);
            }
            uriFormat = "text/html";
            if (dataObject.GetDataPresent(uriFormat)) {
                Stream s = dataObject.GetData(uriFormat) as Stream;
                string uri = new StreamReader(s,Encoding.Unicode).ReadToEnd();
                if (uri.EndsWith("\0"))
                    uri = uri.Remove(uri.Length - 1);
                var pos = uri.IndexOf ("img src=");
                if (pos != -1) {
                    pos += 9;
                    var endPos = uri.IndexOf ('"', pos);
                    if (endPos != -1) {
                        uri = uri.Substring (pos, endPos - pos);
                        result = new Uri (uri);
                    }
                }
            }
            return result;
        }
    }
}