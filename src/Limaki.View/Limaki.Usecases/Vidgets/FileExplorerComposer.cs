/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2015 Lytico
 *
 * http://www.limada.org
 * 
 */

using System;
using System.Linq;
using System.IO;
using Limaki.Common;
using Limaki.Common.Text;
using Limaki.Drawing;
using Limaki.Drawing.Styles;
using Limaki.Graphs;
using Limaki.Iconerias;
using Limaki.View;
using Limaki.View.Visuals;
using Limaki.View.Viz.Modelling;
using Limaki.View.Viz.UI.GraphScene;
using Limaki.View.Viz.Visuals;
using Xwt;
using Xwt.Drawing;
using Limaki.View.Viz.UI;

namespace Limaki.Usecases.Vidgets {

    public class FileExplorerComposer {

        AwesomeIconeria _ic = null;
        protected AwesomeIconeria Ic {
            get {
                if (_ic == null) {
                    _ic = new AwesomeIconeria ();
                    SetDefault (_ic);
                }
                return _ic;
            }
        }

        protected double Scale = 1;
        protected void SetDefault (Iconeria ic) {
            Scale = Desktop.PrimaryScreen.ScaleFactor;
            ic.Stroke = true;
            ic.StrokeColor = IconStrokeColor;
            ic.Fill = true;
            ic.FillColor = FolderIconFillColor;
            var w = 44;
            ic.DefaultSize = new Size (w, w);
            ic.LineWidth = 0;
        }

        public Color FolderIconFillColor = Colors.Wheat;
        public Color IconStrokeColor = Colors.Black;

        private Image _levelUpIcon = null;
        public Image LevelUpIcon {
            get {
                return _levelUpIcon ?? (_levelUpIcon =
                    new PaintingImage (
                        Ic.DefaultSize,
                        ctx => {
                            SetDefault (Ic);
                            Ic.FillColor = this.IconStrokeColor;
                            Ic.PaintIcon (ctx, Ic.DefaultSize.Width, 0, 0, Ic.FaLevelUp);
                        }
                        ));
            }
        }

        private Image _HomeIcon = null;
        public Image HomeIcon {
            get {
                return _HomeIcon ?? (_HomeIcon =
                    new PaintingImage (
                        Ic.DefaultSize,
                        ctx => {
                            SetDefault (Ic);
                            Ic.FillColor = IconStrokeColor;
                            Ic.PaintIcon (ctx, Ic.DefaultSize.Width, 0, 0, Ic.FaHome);
                        }
                        ));
            }
        }
        private Image _driveIcon = null;
        protected Image DriveIcon {
            get {
                return _driveIcon ?? (_driveIcon =
                    new PaintingImage (
                        Ic.DefaultSize,
                        ctx => {
                            SetDefault (Ic);
                            Ic.FillColor = IconStrokeColor;
                            Ic.PaintIcon (ctx, Ic.DefaultSize.Width, 0, 0, Ic.FaHddO);
                        }
                        ));
            }
        }

        private Image _folderIcon = null;
        protected Image FolderIcon {
            get {
                return _folderIcon ?? (_folderIcon =
                    new PaintingImage (Ic.DefaultSize,
                        ctx => {
                            SetDefault (Ic);
                            Ic.PaintIcon (ctx, Ic.DefaultSize.Width, 0, 0, Ic.FaFolder);
                        }
                        ));
            }
        }

        private Image _fileIcon = null;
        protected Image FileIcon {
            get {
                return _fileIcon ?? (_fileIcon =
                    new PaintingImage (Ic.DefaultSize,
                        ctx => {
                            SetDefault (Ic);
                            Ic.FillColor = Colors.WhiteSmoke;
                            Ic.PaintIcon (ctx, Ic.DefaultSize.Width, 0, 0, Ic.FaFile);
                        }
                        ));
            }
        }

        private Image _currentIcon = null;
        protected Image CurrentPathIcon {
            get {
                return _currentIcon ?? (_currentIcon =
                    new PaintingImage (
                        Ic.DefaultSize,
                        ctx => {
                            SetDefault (Ic);
                            Ic.FillColor = IconStrokeColor.WithAlpha (.7);
                            Ic.Stroke = false;
                            Ic.PaintIcon (ctx, Ic.DefaultSize.Width, 0, 0, Ic.FaFolderOpen);
                        }
                        ));
            }
        }

        IGraphSceneLayout<IVisual, IVisualEdge> _layout = null;
        public IGraphSceneLayout<IVisual, IVisualEdge> Layout {
            get {
                if (_layout == null) {
                    var styleSheet = Registry.Pooled<StyleSheets> ().PredefinedStyleSheets ("WhiteGlass");
                    styleSheet.ParentStyle.AutoSize = new Size (double.MaxValue, double.MaxValue);
                    styleSheet.ParentStyle.Padding = new Spacing (5, 2, 5, 2);
                    var style = styleSheet.ItemStyle.SelectedStyle;
                    style.FillColor = Colors.DarkGray;
                    style.TextColor = styleSheet.BackColor;
                    _layout =
                        Registry.Factory.Create<IGraphSceneLayout<IVisual, IVisualEdge>> (SceneGetter, styleSheet);
                    _layout.Distance = new Size (Scale, Scale);
                }
                return _layout;
            }
        }

        protected Func<IGraphScene<IVisual, IVisualEdge>> SceneGetter {
            get { return () => Scene; }
        }

        IGraphScene<IVisual, IVisualEdge> _scene = null;
        public IGraphScene<IVisual, IVisualEdge> Scene {
            get {
                if (_scene == null) {
                    _scene = new Scene {
                        Graph = new SubGraph<IVisual, IVisualEdge> (new VisualGraph (), new VisualGraph ())
                    };

                }
                return _scene;
            }
        }

        protected IVisual LevelUp { get; set; }
        protected IVisual Root { get; set; }

        public string CurrentPath { get; protected set; }
        public bool ShowCurrent { get; set; }
        public string FileFilter { get; set; }
        public Action<string> FileSelected { get; set; }

        public class VisualDir : Visual<object[]> {
            public VisualDir (Image icon, string name) {
                Data = new object[] { icon, name };
                this.Name = name;
                this.Icon = icon;
            }
            public string Name { get; protected set; }
            public Image Icon { get; protected set; }
        }

        public void ShowDir (string path) {
            var oldPath = CurrentPath;
            if (path != null) {
                this.CurrentPath = path;
                if (!path.EndsWith (Path.DirectorySeparatorChar.ToString ()))
                    path += Path.DirectorySeparatorChar;

                try {
                    if (!OS.Mono)
                        Directory.GetAccessControl (path);
                } catch (UnauthorizedAccessException) {
                    CurrentPath = oldPath;
                    return;
                }
            }

            var scene = this.Scene;
            scene.Clear ();
            LevelUp = null;
            Root = null;

            var graph = scene.Graph;

            var isRoot = path == null;
            if (isRoot) {
                foreach (var drive in DriveInfo.GetDrives ()) {
                    graph.Add (new VisualDir (DriveIcon,
                        string.Format ("{0} ({1})", drive.VolumeLabel, drive.Name)));
                }
            } else {

                if (ShowCurrent) {
                    graph.Add (new Visual<string> (this.CurrentPath)); // (this.CurrentIcon, this.CurrentPath));
                }
                LevelUp = new VisualDir (LevelUpIcon, "..");//ShowCurrent ? this.CurrentPath : "..");
                graph.Add (LevelUp);

                try {
                    foreach (var dir in Directory.EnumerateDirectories (path)) {
                        var name = Path.GetFileName (dir);
                        graph.Add (new VisualDir (this.FolderIcon, name));
                    }
                    var pattern = FileFilter ?? "*.*";
                    foreach (var file in Directory.EnumerateFiles (path, pattern)) {
                        var name = Path.GetFileName (file);
                        graph.Add (new VisualDir (this.FileIcon, name));
                    }
                } catch (UnauthorizedAccessException ex) {
                    CurrentPath = oldPath;
                    ShowDir (oldPath);
                    return;
                }
            }

            var layout = Layout;
            var aligner = new Aligner<IVisual, IVisualEdge> (scene, layout);
            var dd = layout.Distance.Height;
            var options = new AlignerOptions {
                Distance = new Size (dd, dd),
                AlignX = Alignment.Start,
                AlignY = Alignment.Start,
                Dimension = Dimension.X,
                PointOrderDelta = 1
            };

            aligner.OneColumn (scene.Graph, (Point) layout.Border, options);

            if (LevelUp != null) {
                var home = new VisualDir (HomeIcon, "");
                graph.Add (home);
                options.Distance = new Size (100 * Scale, 0);
                options.Dimension = Dimension.Y;
                options.AlignY = Alignment.End;
                aligner.Locator.SetLocation (home, new Point (int.MaxValue, int.MaxValue));
                aligner.OneColumn (new IVisual[] { LevelUp, home }, options);

            }
            aligner.Locator.Commit (scene.Requests);

        }

        public void ComposeDisplay (VisualsDisplay display) {
            display.StyleSheet = Layout.StyleSheet;
            display.Layout = Layout;
            display.Data = Scene;

            var disp = display.ActionDispatcher;
            disp.Remove (disp.GetAction<GraphItemMoveResizeAction<IVisual, IVisualEdge>> ());
            var edit = disp.Actions.FirstOrDefault (a => a is VisualsTextEditActionBase);
            if (edit != null)
                disp.Remove (edit);

            var select = disp.GetAction<DelegatingMouseAction> ();
            if (select == null) { //  only one action per action.gettype is allowed
                select = new DelegatingMouseAction ();
                disp.Add (select);
            };

            IVisual hitItem = null;
            var hitCount = 0;
            select.MouseDown += e => {
                var loc = display.Viewport.Camera.ToSource (e.Location);
                var isHit = Scene.Focused != null && Scene.Focused.Shape.IsHit (loc, 5);

                if (hitItem == Scene.Focused && isHit) {
                    var dirItem = Scene.Focused as VisualDir;
                    if (dirItem == null)
                        return;

                    if (dirItem.Icon == this.FileIcon) {
                        if (FileSelected != null)
                            FileSelected (Path.Combine (CurrentPath, dirItem.Name));
                        return;
                    }
                }
                if (hitItem != Scene.Focused) {
                    hitItem = Scene.Focused;
                    hitCount = 0;
                }

            };

            Action<string> showPath = (path) => {
                if (Directory.Exists (path)) {
                    ShowDir (path);
                    display.DataChanged ();
                }
            };

            display.SceneFocusChanged += (s, e) => {
                if (e.Item == LevelUp) {
                    var parent = Directory.GetParent (CurrentPath);
                    string path = null;
                    if (parent != null)
                        path = parent.FullName;
                    ShowDir (path);
                    display.Perform ();

                    return;
                }
                var dirItem = e.Item as VisualDir;
                if (dirItem != null) {
                    if (dirItem.Icon == this.FolderIcon) {
                        showPath (Path.Combine (CurrentPath, dirItem.Name));
                        return;
                    }
                    if (dirItem.Icon == this.HomeIcon) {
                        ShowDir (null);
                        display.Perform ();
                        return;
                    }
                    if (dirItem.Icon == this.DriveIcon) {
                        var drive = dirItem.Name.Between ("(", ")", 0);
                        showPath (drive);
                        return;
                    }
                }
            };
        }
    }
}