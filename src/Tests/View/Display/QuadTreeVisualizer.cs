/*
 * Limaki 
 * Version 0.07
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
using System.Drawing;
using System.Windows.Forms;
using Limaki.Actions;
using Limaki.Winform.Displays;
using Limaki.Drawing;
using Limaki.Drawing.Indexing.QuadTrees;
using Limaki.Drawing.Shapes;
using Limaki.Widgets;

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

        private void PopulateDisplay () {
            //((SceneCommandAction<Scene, IWidget>)widgetDisplay.CommandAction).Layout =
            //    new BasicWidgetLayout<Scene, IWidget>(
            //        widgetDisplay.displayKit.dataHandler,
            //        ((WidgetKit)widgetDisplay.displayKit).StyleSheet);

            widgetDisplay.SelectAction.Enabled = true;

            if ( Data != null ) {

                IStyle style = ( (WidgetKit) widgetDisplay.displayKit ).StyleSheet[StyleNames.DefaultStyle];
                string s = new PointF(float.MaxValue, float.MaxValue).ToString() + "\n\n" +
                           new RectangleF(float.MaxValue, float.MaxValue, float.MaxValue, float.MaxValue).ToString() + "\n";
                style.AutoSize = ShapeUtils.GetTextDimension(style.Font, s, new Size()).ToSize();

                this.widgetDisplay.ZoomState = ZoomState.Original;

                this.widgetDisplay.Data = new Scene();

                IDictionary<Node<IWidget>, IWidget> nodesDone = new Dictionary<Node<IWidget>, IWidget>();
                IDictionary<IWidget, IWidget> itemsDone = new Dictionary<IWidget, IWidget>();

                Node<IWidget> rootNode = new Node<IWidget>(new RectangleF(), 0);
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

                result.Shape = new RectangleShape(Rectangle.Ceiling(node.Envelope));
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

                    string ds = widget.Data.ToString() + "\n" +
                               widget.Shape.BoundsRect.ToString();
                    if ( !node.Envelope.Contains(widget.Shape.BoundsRect) )
                        ds = "! " + ds;
                    childWidget = new Widget<string>(ds);
                    childWidget.Shape = new RoundedRectangleShape(widget.Shape.BoundsRect);
                    this.widgetDisplay.Data.Add(childWidget);
                    itemsDone.Add(widget, childWidget);
                }

                IEdgeWidget edge = 
                    new EdgeWidget<string>(string.Empty, 
                    childWidget, nodeWidget);

                edge.Shape = new VectorShape();
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