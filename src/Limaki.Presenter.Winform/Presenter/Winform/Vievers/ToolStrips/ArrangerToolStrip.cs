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

using System.Windows.Forms;
using Limaki.Common;
using Limaki.Drawing;
using Limaki.UseCases.Viewers.ToolStripViewers;
using System.ComponentModel;
using Limaki.Presenter.Layout;

namespace Limaki.UseCases.Winform.Viewers.ToolStripViewers {
    

    public partial class ArrangerToolStrip : ToolStrip, IArrangerTool {

        ArrangerToolController _controller = null;
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ArrangerToolController Controller {
            get { return _controller ?? (_controller = new ArrangerToolController { Tool = this }); }
        }

        public ArrangerToolStrip() {
            InitializeComponent();
            Compose();
            Controller.Tool = this;
        }
        
        protected virtual void Compose() {
            var options = new AllignerOptions();
            var size = new System.Drawing.Size(36, 36);
            var fullLayout = new ToolStripCommand {
                Action = (s) => FullLayout(options),
                Image = Limaki.Presenter.Properties.Resources.ModifyLayout24,
                Size = size,
            };
            var arrangeLeft = new ToolStripCommand {
                Action = (s) => {
                    options.HorizontalAlignment = Limaki.Drawing.HorizontalAlignment.Left;
                    Align(options);
                },
                Image = Limaki.Presenter.Properties.Resources.ArrangeLeft,
                Size = size,
            };
            var arrangeCenter = new ToolStripCommand {
                Action = (s) => {
                    options.HorizontalAlignment = Limaki.Drawing.HorizontalAlignment.Center;
                    Align(options);
                },
                Image = Limaki.Presenter.Properties.Resources.ArrangeCenter,
                Size = size,
            };
            var arrangeRight = new ToolStripCommand {
                Action = (s) => {
                    options.HorizontalAlignment = Limaki.Drawing.HorizontalAlignment.Right;
                    Align(options);
                },
                Image = Limaki.Presenter.Properties.Resources.ArrangeRight,
                Size = size,
            };
            var arrageRows = new ToolStripCommand {
                Action = (s) => Rows(options),
                Image = Limaki.Presenter.Properties.Resources.ArrageRows,
                Size = size,
            };
            var undo = new ToolStripCommand {
                Action = (s) => Undo(),
                Size = size,
                Image = Limaki.Presenter.Properties.Resources.Undo,
            };
            var arrangeButton = new ToolStripDropDownButtonEx {Command = arrangeLeft};
            arrangeButton.DropDownItems.AddRange(new ToolStripItem[] {
                  new ToolStripMenuItemEx {Command=arrangeCenter,ToggleOnClick=arrangeButton},
                  new ToolStripMenuItemEx {Command=arrangeRight,ToggleOnClick=arrangeButton},
            });
            this.Items.AddRange(new ToolStripItem[] {
                new ToolStripButtonEx { Command=fullLayout},
                arrangeButton,
                new ToolStripButtonEx {Command=arrageRows},
                new ToolStripButtonEx {Command=undo},
            });
        }

        public virtual void Undo() {
            Controller.Undo();
        }
        public virtual void Align(AllignerOptions options) {
            Controller.Align(options);
        }
        public virtual void Rows(AllignerOptions options) {
            Controller.Rows(options);
        }
        public virtual void FullLayout(AllignerOptions options) {
            Controller.FullLayout(options);
        }

    }
}