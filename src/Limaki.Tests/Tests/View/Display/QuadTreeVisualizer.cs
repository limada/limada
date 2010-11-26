/*
 * Limaki 
 * Version 0.08
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

using System.Collections.Generic;
using System.Windows.Forms;
using Limaki.Drawing.Painters;
using Limaki.Graphs;
using Limaki.Winform.Displays;
using Limaki.Drawing;
using Limaki.Drawing.Indexing.QuadTrees;
using Limaki.Drawing.Shapes;
using Limaki.Widgets;
using Limaki.Tests.Graph;
using Limaki.Common;

namespace Limaki.Tests.Display {
    public partial class QuadTreeVisualizer : Form {
        public QuadTreeVisualizer () {
            InitializeComponent();
        }

        private Quadtree<IWidget> _data = null;

        public Quadtree<IWidget> Data {
            get { return _data; }
            set {
                _data = value;
                PopulateDisplay();


            }
        }

        static IDrawingUtils _drawingUtils = null;
        protected static IDrawingUtils drawingUtils {
            get {
                if (_drawingUtils == null) {
                    _drawingUtils = Registry.Factory.One<IDrawingUtils>();
                }
                return _drawingUtils;
            }
        }

        private void PopulateDisplay () {
            widgetDisplay.SelectAction.Enabled = true;

            if ( Data != null ) {

                IStyle style = ( (WidgetKit) widgetDisplay.DisplayKit ).StyleSheet[StyleNames.DefaultStyle];
                string s = new PointS(float.MaxValue, float.MaxValue).ToString() + "\n\n" +
                           new RectangleS(float.MaxValue, float.MaxValue, float.MaxValue, float.MaxValue).ToString() + "\n";
                style.AutoSize =
                    drawingUtils.GetTextDimension (s, style).ToSize();
                    

                this.widgetDisplay.ZoomState = ZoomState.Original;

                this.widgetDisplay.Data = new Scene();

                IDictionary<Node<IWidget>, IWidget> nodesDone = new Dictionary<Node<IWidget>, IWidget>();
                IDictionary<IWidget, IWidget> itemsDone = new Dictionary<IWidget, IWidget>();

                Node<IWidget> rootNode = new Node<IWidget>(new RectangleS(), 0);
                rootNode.Items = Data.Root.Items;
                for ( int i = 0; i < Data.Root.subnode.Length; i++ )
                    rootNode.subnode[i] = Data.Root.subnode[i];
                IWidget root = NodeWidget(rootNode, nodesDone, itemsDone);


                this.widgetDisplay.Data.Focused = root;
                this.widgetDisplay.CommandsInvoke();
                this.widgetDisplay.Invalidate();
            }
        }

        private IWidget NodeWidget ( Node<IWidget> node,
            IDictionary<Node<IWidget>, IWidget> nodesDone,
            IDictionary<IWidget, IWidget> itemsDone ) {
            if ( node != null ) {
                IWidget result = new Widget<string>(node.Centre.ToString() + "\n" + node.Envelope.ToString());

                result.Shape = new RectangleShape(RectangleI.Ceiling(node.Envelope));
                this.widgetDisplay.Data.Add(result);
                nodesDone.Add(node, result);
                NodeItems(node, result, itemsDone);

                if ( node.HasSubNodes ) {
                    IWidget subRoot = new Widget<string>(" ° ");
                    this.widgetDisplay.Data.Add(
                        new EdgeWidget<string>(string.Empty, result, subRoot));

                    foreach (Node<IWidget> sub in node.subnode) {
                        if (sub != null) {
                            IWidget subWidget = NodeWidget(sub, nodesDone, itemsDone);
                            this.widgetDisplay.Data.Add(
                                new EdgeWidget<string>(string.Empty,
                                                       subRoot, subWidget));
                        }
                    }
                }
                return result;
            } else
                return null;
        }

        private void NodeItems ( Node<IWidget> node, IWidget nodeWidget,
            IDictionary<IWidget, IWidget> itemsDone ) {
            foreach ( IWidget widget in node.Items ) {
                IWidget childWidget = null;
                itemsDone.TryGetValue(widget, out childWidget);
                if ( childWidget == null ) {
                    string ws = widget.Data.ToString ();
                    if (widget is IEdgeWidget) {
                        ws = GraphUtils.EdgeString<IWidget,IEdgeWidget>((IEdgeWidget) widget);
                    }
                    string ds = ws + "\n" +
                               widget.Shape.BoundsRect.ToString();

                    if ( !node.Envelope.Contains(widget.Shape.BoundsRect) )
                        ds = "! " + ds;
                    childWidget = new Widget<string>(ds);
                    childWidget.Shape =
                        this.widgetDisplay.DataLayout.ShapeFactory.One<IRoundedRectangleShape> ();
                    childWidget.Location = widget.Location;
                    childWidget.Size = widget.Size;
                    
                    this.widgetDisplay.Data.Add(childWidget);
                    itemsDone.Add(widget, childWidget);
                }

                IEdgeWidget edge = 
                    new EdgeWidget<string>(string.Empty, 
                    childWidget, nodeWidget);

                edge.Shape = this.widgetDisplay.DataLayout.CreateShape(edge);
                this.widgetDisplay.Data.Add(edge);
            }


        }




        private void widgetDisplay_KeyPress ( object sender, KeyPressEventArgs e ) {
            if ( e.KeyChar == 'l' ) {
                this.widgetDisplay.CommandsInvoke();
                this.widgetDisplay.Invalidate();
                e.Handled = true;
            }
        }
    }
}