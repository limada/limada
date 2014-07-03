/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2013-2014 Lytico
 *
 * http://www.limada.org
 * 
 */

using System;
using System.Collections.Generic;
using System.Linq;
using Limaki.Actions;
using Limaki.Drawing;
using Limaki.Graphs;
using Limaki.Iconerias;
using Limaki.View.Vidgets;
using Limaki.View.Visuals;
using Limaki.View.Viz.Modelling;
using Xwt;
using Xwt.Backends;

namespace Limaki.View.Viz.Visualizers.ToolStrips {

    public interface IArrangerToolStripBackend : IDisplayToolStripBackend { }

    [BackendType (typeof (IArrangerToolStripBackend))]
    public class ArrangerToolStrip : DisplayToolStrip<IGraphSceneDisplay<IVisual, IVisualEdge>> {

        public IToolStripCommand LogicalLayoutCommand { get; set; }
        public IToolStripCommand LogicalLayoutLeafCommand { get; set; }
        public IToolStripCommand FullLayoutCommand { get; set; }
        public IToolStripCommand ColumnsCommand { get; set; }
        public IToolStripCommand OneColumnCommand { get; set; }
        public IToolStripCommand ArrangeLeftCommand { get; set; }
        public IToolStripCommand ArrangeCenterCommand { get; set; }
        public IToolStripCommand ArrangeRightCommand { get; set; }
        public IToolStripCommand ArrangeTopCommand { get; set; }
        public IToolStripCommand ArrangeCenterVCommand { get; set; }
        public IToolStripCommand ArrangeBottomCommand { get; set; }
        public IToolStripCommand DimensionXCommand { get; set; }
        public IToolStripCommand DimensionYCommand { get; set; }
        public IToolStripCommand UndoCommand { get; set; }

        public ArrangerToolStrip () {
            Compose ();
        }

        public override void Detach (object sender) {

        }

        public override void Attach (object sender) {
            base.Attach (sender);
        }

        public void Call (IGraphSceneDisplay<IVisual, IVisualEdge> display, Action<Aligner<IVisual, IVisualEdge>, IEnumerable<IVisual>> call) {
            if (display == null)
                return;

            Call (display, call, display.Data.Selected.Elements);
        }

        public void Call (IGraphSceneDisplay<IVisual, IVisualEdge> display, Action<Aligner<IVisual, IVisualEdge>, IEnumerable<IVisual>> call, IEnumerable<IVisual> items) {
            if (display == null)
                return;

            var aligner = new Aligner<IVisual, IVisualEdge> (display.Data, display.Layout);

            call (aligner, items);

            aligner.Locator.Commit (aligner.GraphScene.Requests);

            StoreUndo (display, aligner, items);

            display.Perform ();
        }

        private List<ICommand<IVisual>> _undo;
        private Int64 _undoID = 0;


        protected virtual void StoreUndo (IGraphSceneDisplay<IVisual, IVisualEdge> display, Aligner<IVisual, IVisualEdge> aligner, IEnumerable<IVisual> items) {
            _undo = new List<ICommand<IVisual>> ();
            _undoID = display.DataId;
            foreach (var item in aligner.GraphScene.Requests.Select (c => c.Subject)) {
                _undo.Add (new MoveCommand<IVisual> (item, i => i.Shape, item.Location));
            }
            foreach (var edge in aligner.Locator.AffectedEdges) {
                _undo.Add (new LayoutCommand<IVisual> (edge, LayoutActionType.Justify));
            }
        }

        public virtual void Undo (IGraphSceneDisplay<IVisual, IVisualEdge> display) {
            if (display == null)
                return;
            if (_undo != null && _undoID == display.DataId) {
                foreach (var comm in _undo)
                    display.Data.Requests.Add (comm);
                display.Perform ();
                _undo = null;
                _undoID = 0;
            }
        }

        public virtual void Undo () {
            Undo (CurrentDisplay);
        }

        public virtual void OptionsAndLayout (AlignerOptions options) {
            if (CurrentDisplay == null)
                return;
            options.Distance = CurrentDisplay.Layout.Distance;
            // TODO: CurrentDisplay.Layout.SetOptions(options);
        }

        public virtual void Columns (AlignerOptions options) {
            OptionsAndLayout (options);
            Call (CurrentDisplay, (aligner, items) => aligner.Columns (items, options));
        }

        public void OneColumn (AlignerOptions options) {
            OptionsAndLayout (options);
            options = new AlignerOptions (options);
            options.PointOrder = options.Dimension == Dimension.X ?
                PointOrder.YX : PointOrder.XY;
            options.PointOrderDelta = 0;
            Call (CurrentDisplay, (aligner, items) => aligner.OneColumn (items, options));
        }

        public virtual void FullLayout (AlignerOptions options) {
            OptionsAndLayout (options);
            var display = this.CurrentDisplay;
            if (display != null) {
                display.BackColor = display.StyleSheet.BackColor;
                display.Reset ();
                display.BackendRenderer.Render ();
            }
        }

        public virtual void LogicalLayout (AlignerOptions options) {
            OptionsAndLayout (options);
            var display = this.CurrentDisplay;
            if (display != null) {
                var selected = display.Data.Selected.Elements;
                var root = display.Data.Focused;
                if (selected.Count () == 1) {
                    selected = new Walker<IVisual, IVisualEdge> (display.Data.Graph).DeepWalk (root, 0).Select (l => l.Node);
                }
                Call (CurrentDisplay, (aligner, items) => aligner.Columns (root, items, options), selected);
            }
        }

        public virtual void LogicalLayoutLeaf (AlignerOptions options) {
            OptionsAndLayout (options);
            var display = this.CurrentDisplay;
            if (display != null) {
                var selected = display.Data.Selected.Elements;
                var root = display.Data.Focused;
                if (selected.Count () == 1) {
                    var walk = new Walker<IVisual, IVisualEdge> (display.Data.Graph)
                        .DeepWalk (root, 0, Walk.Leafs<IVisual, IVisualEdge> ())
                        .Where (l => !(l.Node is IVisualEdge))
                        .ToArray ();

                    var save = options.Collisions;

                    Call (CurrentDisplay, (aligner, items) => {
                        var bounds = new Rectangle (aligner.Locator.GetLocation (root), aligner.Locator.GetSize (root));
                        options.Collisions = Collisions.None;
                        var cols = aligner.MeasureWalk (walk, ref bounds, options);
                        aligner.DequeColumn (cols, ref bounds, options);
                        options.Collisions = Collisions.NextFree | Collisions.PerColumn | Collisions.Toggle;
                        aligner.LocateColumns (cols, ref bounds, options);
                    }
                        , walk.Select (l => l.Node));
                    options.Collisions = save;
                }
            }
        }

        protected virtual void Compose () {

            var options = new AlignerOptions {
                Dimension = Dimension.X,
                PointOrderDelta = 40,
                Collisions = Collisions.NextFree //| Collisions.Toggle
            };

            Action action = () => Columns (options);

            LogicalLayoutLeafCommand = new ToolStripCommand {
                Action = (s) => {
                    action = () => LogicalLayoutLeaf (options);
                    action ();
                },
                Image = Iconery.LogicalLayoutLeaf,
                Size = DefaultSize,
                ToolTipText = "arrange leaf of selected"
            };

            LogicalLayoutCommand = new ToolStripCommand {
                Action = (s) => {
                    action = () => LogicalLayout (options);
                    action ();
                },
                Image = Iconery.LogicalLayout,
                Size = DefaultSize,
                ToolTipText = "arrange siblings of selected"
            };

            FullLayoutCommand = new ToolStripCommand {
                Action = (s) => {
                    action = () => FullLayout (options);
                    action ();
                },
                Image = Iconery.FullLayout,
                Size = DefaultSize,
                ToolTipText = "arrange all"
            };

            ColumnsCommand = new ToolStripCommand {
                Action = (s) => {
                    action = () => Columns (options);
                    action ();
                },
                Image = Iconery.ArrageRows,
                Size = DefaultSize,
                ToolTipText = "arrange in columns"

            };

            OneColumnCommand = new ToolStripCommand {
                Action = (s) => {
                    action = () => OneColumn (options);
                    action ();
                },
                Image = Iconery.ArrangeOneRow,
                Size = DefaultSize,
                ToolTipText = "arrange in one column"
            };

            ArrangeLeftCommand = new ToolStripCommand {
                Action = (s) => {
                    options.AlignX = Alignment.Start;
                    action ();
                },
                Image = Iconery.ArrangeLeft,
                Size = DefaultSize,
                ToolTipText = "align left"
            };

            ArrangeCenterCommand = new ToolStripCommand {
                Action = (s) => {
                    options.AlignX = Alignment.Center;
                    action ();
                },
                Image = Iconery.ArrangeCenter,
                Size = DefaultSize,
                ToolTipText = "align center"
            };

            ArrangeRightCommand = new ToolStripCommand {
                Action = (s) => {
                    options.AlignX = Alignment.End;
                    action ();
                },
                Image = Iconery.ArrangeRight,
                Size = DefaultSize,
                ToolTipText = "align rigth"
            };

            ArrangeTopCommand = new ToolStripCommand {
                Action = (s) => {
                    options.AlignY = Alignment.Start;
                    action ();
                },
                Image = Iconery.ArrangeTop,
                Size = DefaultSize,
                ToolTipText = "align top"
            };

            ArrangeCenterVCommand = new ToolStripCommand {
                Action = (s) => {
                    options.AlignY = Alignment.Center;
                    action ();
                },
                Image = Iconery.ArrangeMiddle,
                Size = DefaultSize,
                ToolTipText = "align middle"
            };

            ArrangeBottomCommand = new ToolStripCommand {
                Action = (s) => {
                    options.AlignY = Alignment.End;
                    action ();
                },
                Image = Iconery.ArrangeBottom,
                Size = DefaultSize,
                ToolTipText = "align bottom"
            };

            DimensionXCommand = new ToolStripCommand {
                Action = (s) => {
                    options.Dimension = Dimension.X;
                    action ();
                },
                Image = Iconery.DimensionX,
                Size = DefaultSize,
                ToolTipText = "arrange left to right"
            };

            DimensionYCommand = new ToolStripCommand {
                Action = (s) => {
                    options.Dimension = Dimension.Y;
                    action ();
                },
                Image = Iconery.DimensionY,
                Size = DefaultSize,
                ToolTipText = "arrange top to bottom"
            };

            UndoCommand = new ToolStripCommand {
                Action = (s) => Undo (),
                Size = DefaultSize,
                Image = Iconery.Undo,
                ToolTipText = "undo last arrange"
            };

            var horizontalButton = new ToolStripDropDownButton (ArrangeLeftCommand);
            horizontalButton.AddItems (
                new ToolStripButton (ArrangeCenterCommand) { ToggleOnClick = horizontalButton },
                new ToolStripButton (ArrangeRightCommand) { ToggleOnClick = horizontalButton }
                );

            var verticalButton = new ToolStripDropDownButton (ArrangeTopCommand);
            verticalButton.AddItems (
                new ToolStripButton (ArrangeCenterVCommand) { ToggleOnClick = verticalButton },
                new ToolStripButton (ArrangeBottomCommand) { ToggleOnClick = verticalButton }
                );

            var layoutButton = new ToolStripDropDownButton (LogicalLayoutLeafCommand);
            layoutButton.AddItems (
                new ToolStripButton (LogicalLayoutCommand) { ToggleOnClick = layoutButton },
                new ToolStripButton (ColumnsCommand) { ToggleOnClick = layoutButton },
                new ToolStripButton (OneColumnCommand) { ToggleOnClick = layoutButton },
                new ToolStripButton (FullLayoutCommand)
                );

            var dimensionButton = new ToolStripDropDownButton (DimensionXCommand);
            dimensionButton.AddItems (
                new ToolStripButton (DimensionYCommand) { ToggleOnClick = dimensionButton }
                );

            this.AddItems (
                layoutButton,
                horizontalButton,
                verticalButton,
                dimensionButton,
                new ToolStripButton (UndoCommand)
                );
        }
    }
}