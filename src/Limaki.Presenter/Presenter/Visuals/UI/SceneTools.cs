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
using Limaki.Presenter.Layout;
using Limaki.Visuals;
using Limaki.Presenter.UI;



namespace Limaki.Presenter.Visuals.UI {
    public class SceneTools {
        public static void ChangeShape(Scene scene, IVisual visual, IShape newShape) {
            if (visual != null && !(visual is IVisualEdge)) {
                if (newShape != null) {
                    newShape = (IShape)newShape.Clone();
                    newShape.Location = visual.Shape.Location;
                    newShape.Size = visual.Shape.Size;
                    var changeShape =
                        new ActionCommand<IVisual, IShape>(
                            visual,
                            newShape,
                            delegate(IVisual target, IShape shape) { target.Shape = shape; });
                    scene.Requests.Add(changeShape);
                    if (visual.Shape is VectorShape || newShape is VectorShape) {
                        scene.Requests.Add(new LayoutCommand<IVisual>(visual, LayoutActionType.Justify));
                    }
                    foreach (IVisualEdge edge in scene.Twig(visual)) {
                        scene.Requests.Add(new LayoutCommand<IVisual>(edge, LayoutActionType.Justify));
                    }
                }
            }
        }

        public static void ChangeMarkers(Scene scene, IEnumerable<IVisual> elements, string text) {
            if (scene.Markers != null) {
                scene.Markers.ChangeMarkers (elements, text);
                foreach (var visual in elements) {
                    scene.Requests.Add(new LayoutCommand<IVisual>(visual, LayoutActionType.Justify));
                }
            }
        }

        public static IVisualEdge CreateEdge(Scene scene) {
            IVisualEdge edge = null;
            if (scene != null && scene.Markers != null) {
                edge = scene.Markers.CreateDefaultEdge() as IVisualEdge;
            } 
            if (edge == null){
                var factory = Registry.Factory.Create<IVisualFactory> ();
                edge = factory.CreateEdge ("°");
            }
            return edge;
        }

        public static void CreateEdge(Scene scene, IVisual root, IVisual leaf) {
            if (scene != null && leaf != null && root != null && root != leaf) {
                IVisualEdge edge = CreateEdge (scene);
                
                edge.Root = root;
                edge.Leaf = leaf;
                scene.Add(edge);
                if (scene.Markers != null) {
                    object marker = scene.Markers.DefaultMarker;
                    scene.Graph.OnChangeData (edge, marker);
                }
                scene.Graph.OnGraphChanged(edge, GraphChangeType.Add);
                scene.Requests.Add(new LayoutCommand<IVisual>(edge, LayoutActionType.Invoke));
                scene.Requests.Add(new LayoutCommand<IVisual>(edge, LayoutActionType.Justify));
            }
        }

        public static void AddItem(Scene scene, IVisual item, IGraphLayout<IVisual,IVisualEdge> layout, PointI pt) {
            bool allowAdd = true;
            if (scene == null)
                return;
            if (item is IVisualEdge) {
                IVisualEdge edge = (IVisualEdge)item;
                allowAdd = scene.Contains(edge.Root) && scene.Contains(edge.Leaf);
            }
            if (allowAdd) {
                GraphSceneFacade<IVisual,IVisualEdge> facade =
                    new GraphSceneFacade<IVisual, IVisualEdge>(delegate() { return scene; }, layout);
                facade.Add(item, pt);
            }
        }

        public static IVisual PlaceVisual(IVisual root, IVisual visual, Scene scene, IGraphLayout<IVisual,IVisualEdge> layout) {
            if (visual != null && scene !=null) {
                PointI pt = (PointI)layout.Distance;
                if (root != null) {
                    pt = root.Shape[Anchor.LeftBottom];
                }
                AddItem(scene, visual, layout, pt);
                SceneTools.CreateEdge(scene, root, visual);
            }

            return visual;
        }

        public static void CleanScene(IGraphScene<IVisual, IVisualEdge> scene) {
            if (scene != null) {
                if (scene.Graph is GraphView<IVisual, IVisualEdge>) {
                    ( (GraphView<IVisual, IVisualEdge>) scene.Graph ).One.Clear ();
                    scene.ClearView ();
                    Registry.ApplyProperties<MarkerContextProcessor, IGraphScene<IVisual, IVisualEdge>>(scene);
                } else {
                    throw new ArgumentException ("scene.Graph must be a GraphView to load Sheets");
                }
            }
        }
    }
}