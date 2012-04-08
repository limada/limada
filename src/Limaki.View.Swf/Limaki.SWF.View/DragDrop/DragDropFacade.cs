/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2006-2011 Lytico
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
using System.Text;
using Limaki.View.Visuals.UI;
using Limaki.Visuals;
using Xwt;

namespace Limaki.View.Swf.DragDrop {
    /// <summary>
    /// encapsulates operations to realise DragDrop or CopyPaste events
    /// </summary>
    public class DragDropFacade {
        #region DataObject handling
        private DataObjectHandlerChain chain = new DataObjectHandlerChain();

        public DragDropFacade() {
            chain.InitDataObjectHanders();
        }

        public IVisual GetVisual(IDataObject dataObject, IGraph<IVisual, IVisualEdge> graph, bool inProcess) {
            var visual = chain.GetVisual(dataObject, graph, inProcess);
            return visual;
        }

        public bool IsValidData(IDataObject data) {
            return chain.IsValidData(data);
        }

        public IDataObject SetVisual(IGraph<IVisual, IVisualEdge> graph, IVisual visual, IDataObject dataObject) {
            if (dataObject == null) throw new ArgumentNullException("dataObject");
            chain.SetVisual(dataObject, graph,visual);
            return dataObject;
        }

        public IDataObject SetVisual(IGraph<IVisual, IVisualEdge> graph, IVisual visual) {
            return SetVisual(graph,visual, new DataObject());
        }

        public IDataObject SetVisual(IWidgetBackend control, IGraph<IVisual, IVisualEdge> graph, IVisual visual) {
            ControlDataObject dataObject = new ControlDataObject();
            dataObject.control = control;
            return SetVisual(graph,visual, dataObject);
        }

        public IVisual Clone(IGraph<IVisual, IVisualEdge> graph, IVisual item) {
            return new SerializedVisualsDataObjectHandler().Clone(graph,item);
        }
        #endregion

        #region Scene handling

        public virtual void LinkItem(IGraphScene<IVisual,IVisualEdge> scene,IVisual item, Point pt, int hitSize, bool itemIsRoot) {
            if (item != null) {
                IVisual target = scene.Hovered;
                if (target == null && scene.Focused != null && scene.Focused.Shape.IsHit(pt, hitSize)) {
                    target = scene.Focused;
                }
                if (item != target) {
                    if (itemIsRoot)
                        SceneExtensions.CreateEdge (scene, item, target);
                    else
                        SceneExtensions.CreateEdge(scene, target, item);
                }
            }
        }

        public IVisual PlaceVisual(IDataObject dataObject, IGraphScene<IVisual,IVisualEdge> scene, IGraphLayout<IVisual,IVisualEdge> layout) {
            var visual = GetVisual (dataObject, scene.Graph,false);
            SceneExtensions.PlaceVisual(scene,scene.Focused, visual, layout);
            //scene.Selected.Clear();
            //scene.Focused = visual;
            return visual;

        }



       public virtual bool DoDragDrop(IGraphScene<IVisual,IVisualEdge> scene, IWidgetBackend control, IDataObject dataObject, 
           IGraphLayout<IVisual,IVisualEdge> layout, Point pt, int hitsize) {
            IVisual item = null;
            bool itemIsRoot = false;

            if (dataObject is ControlDataObject) { // data is sent from same application
                ControlDataObject data = (ControlDataObject)dataObject;
                if (data.control == control) { // data is sent from same control, so get the item as native instance
                    item = GetVisual(dataObject, scene.Graph,true);
                    itemIsRoot = true;
                }

                // data is sent from another control
                else if (data.control is IDisplayBackend<IGraphScene<IVisual,IVisualEdge>>) {// the other control has a Scene
                    var targetGraph =
                        scene.Graph as IGraphPair<IVisual, IVisual, IVisualEdge, IVisualEdge>;

					var display = ((IDisplayBackend)data.control).Display as IDisplay<IGraphScene<IVisual,IVisualEdge>>;
                    var sourceGraph =
                        display.Data.Graph as IGraphPair<IVisual, IVisual, IVisualEdge, IVisualEdge>;

                    if (targetGraph != null && sourceGraph != null) {
                        var sourceitem = GetVisual(dataObject, sourceGraph, true);

                        item = GraphMapping.Mapping.LookUp<IVisual, IVisualEdge>(sourceGraph, targetGraph, sourceitem);

                        SceneExtensions.AddItem(scene, item, layout, pt);


                    } else {
                        item = GetVisual(dataObject, scene.Graph,false);
                        SceneExtensions.AddItem(scene, item, layout, pt);
                    }
                } else {// the other control is not a widget control
                    item = GetVisual(dataObject, scene.Graph,false);
                    SceneExtensions.AddItem(scene, item, layout, pt);
                }
            } else { // data is sent form another application
                item = GetVisual(dataObject, scene.Graph,false);
                SceneExtensions.AddItem(scene, item, layout, pt);
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