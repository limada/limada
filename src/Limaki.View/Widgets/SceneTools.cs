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
using Limaki.Actions;
using Limaki.Common;
using Limaki.Drawing;
using Limaki.Drawing.Shapes;
using Limaki.Graphs;
using Limaki.Drawing.UI;
using Limaki.Widgets.Layout;


namespace Limaki.Widgets {
    public class SceneTools {
        public static void ChangeShape(Scene scene, IWidget widget, IShape newShape) {
            if (widget != null && !(widget is IEdgeWidget)) {
                if (newShape != null) {
                    newShape = (IShape)newShape.Clone();
                    newShape.Location = widget.Shape.Location;
                    newShape.Size = widget.Shape.Size;
                    var changeShape =
                        new ActionCommand<IWidget, IShape>(
                            widget,
                            newShape,
                            delegate(IWidget target, IShape shape) { target.Shape = shape; });
                    scene.Commands.Add(changeShape);
                    if (widget.Shape is VectorShape || newShape is VectorShape) {
                        scene.Commands.Add(new LayoutCommand<IWidget>(widget, LayoutActionType.Justify));
                    }
                    foreach (IEdgeWidget edge in scene.Twig(widget)) {
                        scene.Commands.Add(new LayoutCommand<IWidget>(edge, LayoutActionType.Justify));
                    }
                }
            }
        }

        public static void ChangeMarkers(Scene scene, IEnumerable<IWidget> elements, string text) {
            if (scene.Markers != null) {
                scene.Markers.ChangeMarkers (elements, text);
                foreach (IWidget widget in elements) {
                    scene.Commands.Add(new LayoutCommand<IWidget>(widget, LayoutActionType.Justify));
                }
            }
        }

        public static IEdgeWidget CreateEdge(Scene scene) {
            IEdgeWidget edge = null;
            if (scene != null && scene.Markers != null) {
                edge = scene.Markers.CreateDefaultEdge() as IEdgeWidget;
            } 
            if (edge == null){
                var factory = Registry.Factory.One<IWidgetFactory> ();
                edge = factory.CreateEdgeWidget ("�");
            }
            return edge;
        }

        public static void CreateEdge(Scene scene, IWidget root, IWidget leaf) {
            if (scene != null && leaf != null && root != null && root != leaf) {
                IEdgeWidget edge = CreateEdge (scene);
                
                edge.Root = root;
                edge.Leaf = leaf;
                scene.Add(edge);
                if (scene.Markers != null) {
                    object marker = scene.Markers.DefaultMarker;
                    scene.Graph.OnChangeData (edge, marker);
                }
                scene.Graph.OnGraphChanged(edge, GraphChangeType.Add);
                scene.Commands.Add(new LayoutCommand<IWidget>(edge, LayoutActionType.Invoke));
                scene.Commands.Add(new LayoutCommand<IWidget>(edge, LayoutActionType.Justify));
            }
        }

        public static void AddItem(Scene scene, IWidget item, ILayout<Scene, IWidget> layout, PointI pt) {
            bool allowAdd = true;
            if (scene == null)
                return;
            if (item is IEdgeWidget) {
                IEdgeWidget edge = (IEdgeWidget)item;
                allowAdd = scene.Contains(edge.Root) && scene.Contains(edge.Leaf);
            }
            if (allowAdd) {
                SceneFacade facade =
                    new SceneFacade(delegate() { return scene; }, layout);
                facade.Add(item, pt);
            }
        }

        public static IWidget PlaceWidget(IWidget root, IWidget widget, Scene scene, ILayout<Scene, IWidget> layout) {
            if (widget != null && scene !=null) {
                PointI pt = (PointI)layout.Distance;
                if (root != null) {
                    pt = root.Shape[Anchor.LeftBottom];
                }
                AddItem(scene, widget, layout, pt);
                SceneTools.CreateEdge(scene, root, widget);
            }

            return widget;
        }

        public static void CleanScene(Scene scene) {
            if (scene != null) {
                if (scene.Graph is GraphView<IWidget, IEdgeWidget>) {
                    ( (GraphView<IWidget, IEdgeWidget>) scene.Graph ).One.Clear ();
                    scene.ClearView ();
                    Registry.ApplyProperties<MarkerContextProcessor, Scene> (scene);
                } else {
                    throw new ArgumentException ("scene.Graph must be a GraphView to load Sheets");
                }
            }
        }
    }
}
