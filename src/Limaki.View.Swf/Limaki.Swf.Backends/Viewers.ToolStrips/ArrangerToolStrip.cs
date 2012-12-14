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
 * http://www.limada.org
 * 
 */

using System;
using System.ComponentModel;
using System.Windows.Forms;
using Limaki.View.Layout;
using Limaki.Viewers.ToolStripViewers;
using Alignment = Xwt.Alignment;
using Dimension = Limaki.Drawing.Dimension;

namespace Limaki.Swf.Backends.Viewers.ToolStrips {

    public partial class ArrangerToolStrip : ToolStrip, IArrangerTool {

        ArrangerToolController _controller = null;
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ArrangerToolController Controller {
            get { return _controller ?? (_controller = new ArrangerToolController { Tool = this }); }
        }

        public ArrangerToolStrip () {
            InitializeComponent();
            Compose();
            Controller.Tool = this;
        }

        protected virtual void Compose () {
            var options = new AlignerOptions {
                Dimension = Dimension.X,
                PointOrderDelta=40,
                Collisions = Collisions.NextFree //| Collisions.Toggle
            };
            
            var size = new System.Drawing.Size(36, 36);
            Action action = () => Columns(options);

            var logicalLayout = new ToolStripCommand {
                Action = (s) => {
                    action = () => LogicalLayout(options);
                    action();
                },
                Image = Limaki.View.Properties.Iconery.LogicalLayout,
                Size = size,
            };

            var fullLayout = new ToolStripCommand {
                Action = (s) => {
                    action = () => FullLayout(options);
                    action();
                },
                Image = Limaki.View.Properties.Iconery.FullLayout,
                Size = size,
            };

            var columns = new ToolStripCommand {
                Action = (s) => {
                    action = () => Columns(options);
                    action();
                },
                Image = Limaki.View.Properties.Iconery.ArrageRows,
                Size = size,
            };
            var oneColumn = new ToolStripCommand {
                Action = (s) => {
                    action = () => OneColumn(options);
                    action();
                },
                Image = Limaki.View.Properties.Iconery.ArrangeOneRow,
                Size = size,
            };
            var arrangeLeft = new ToolStripCommand {
                Action = (s) => {
                    options.AlignX = Alignment.Start;
                    action();
                },
                Image = Limaki.View.Properties.Iconery.ArrangeLeft,
                Size = size,
            };
            var arrangeCenter = new ToolStripCommand {
                Action = (s) => {
                    options.AlignX = Alignment.Center;
                    action();
                },
                Image = Limaki.View.Properties.Iconery.ArrangeCenter,
                Size = size,
            };
            var arrangeRight = new ToolStripCommand {
                Action = (s) => {
                    options.AlignX = Alignment.End;
                    action();
                },
                Image = Limaki.View.Properties.Iconery.ArrangeRight,
                Size = size,
            };

            var arrangeTop = new ToolStripCommand {
                Action = (s) => {
                    options.AlignY = Alignment.Start;
                    action();
                },
                Image = Limaki.View.Properties.Iconery.ArrangeTop,
                Size = size,
            };
            var arrangeCenterV = new ToolStripCommand {
                Action = (s) => {
                    options.AlignY = Alignment.Center;
                    action();
                },
                Image = Limaki.View.Properties.Iconery.ArrangeMiddle,
                Size = size,
            };
            var arrangeBottom = new ToolStripCommand {
                Action = (s) => {
                    options.AlignY = Alignment.End;
                    action();
                },
                Image = Limaki.View.Properties.Iconery.ArrangeBottom,
                Size = size,
            };

            var dimensionX = new ToolStripCommand {
                Action = (s) => {
                    options.Dimension = Dimension.X;
                    action();
                },
                Image = Limaki.View.Properties.Iconery.DimensionX,
                Size = size,
            };

            var dimensionY = new ToolStripCommand {
                Action = (s) => {
                    options.Dimension = Dimension.Y;
                    action();
                },
                Image = Limaki.View.Properties.Iconery.DimensionY,
                Size = size,
            };

            var undo = new ToolStripCommand {
                Action = (s) => Undo(),
                Size = size,
                Image = Limaki.View.Properties.Iconery.Undo,
            };
            var horizontalButton = new ToolStripDropDownButtonEx { Command = arrangeLeft };
            horizontalButton.DropDownItems.AddRange(new ToolStripItem[] {
                  new ToolStripMenuItemEx {Command=arrangeCenter,ToggleOnClick=horizontalButton},
                  new ToolStripMenuItemEx {Command=arrangeRight,ToggleOnClick=horizontalButton},
            });
            var verticalButton = new ToolStripDropDownButtonEx { Command = arrangeTop };
            verticalButton.DropDownItems.AddRange(new ToolStripItem[] {
                  new ToolStripMenuItemEx {Command=arrangeCenterV,ToggleOnClick=verticalButton},
                  new ToolStripMenuItemEx {Command=arrangeBottom,ToggleOnClick=verticalButton},
            });

            var layoutButton = new ToolStripDropDownButtonEx { Command = logicalLayout };
            layoutButton.DropDownItems.AddRange(new ToolStripItem[] {
                new ToolStripMenuItemEx {Command=columns,ToggleOnClick=layoutButton},    
                new ToolStripMenuItemEx {Command=oneColumn,ToggleOnClick=layoutButton},          
                new ToolStripMenuItemEx {Command=fullLayout}, 
            });

            var dimensionButton = new ToolStripDropDownButtonEx { Command = dimensionX };
            dimensionButton.DropDownItems.AddRange(new ToolStripItem[] {
                new ToolStripMenuItemEx {Command=dimensionY,ToggleOnClick=dimensionButton},    
            });

            this.Items.AddRange(new ToolStripItem[] {
                layoutButton,
                horizontalButton,
                verticalButton,
                dimensionButton,
                new ToolStripButtonEx {Command=undo},
            });
        }

        public virtual void Undo () {
            Controller.Undo();
        }

        public virtual void Columns (AlignerOptions options) {
            Controller.Columns(options);
        }
        public virtual void OneColumn (AlignerOptions options) {
            Controller.OneColumn(options);
        }
        public virtual void LogicalLayout (AlignerOptions options) {
            Controller.LogicalLayout(options);
        }
        public virtual void FullLayout (AlignerOptions options) {
            Controller.FullLayout(options);
        }
    }
}