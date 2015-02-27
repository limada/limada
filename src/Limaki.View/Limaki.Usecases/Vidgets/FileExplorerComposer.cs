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
        protected AwesomeIconeria ic {
            get {
                return _ic ?? (_ic =
                    new AwesomeIconeria {
                        Stroke = true,
                        StrokeColor = IconStrokeColor,
                        Fill = true,
                        FillColor = FolderIconFillColor,
                        DefaultSize = new Size (20, 20),
                    });
            }
        }

        protected void SetDefault (Iconeria ic) {
            ic.Stroke = true;
            ic.StrokeColor = IconStrokeColor;
            ic.Fill = true;
            ic.FillColor = FolderIconFillColor;
            ic.DefaultSize = new Size (20, 20);
            ic.LineWidth = 0;
        }

        public Color FolderIconFillColor = Colors.Wheat;
        public Color IconStrokeColor = Colors.Black;

        private Image _levelUpIcon = null;
        public Image LevelUpIcon {
            get {
                return _levelUpIcon ?? (_levelUpIcon =
                    new PaintingImage (
                        ic.DefaultSize,
                        ctx => {
                            SetDefault (ic);
                            ic.FillColor = this.IconStrokeColor;
                            ic.PaintIcon (ctx, ic.DefaultSize.Width, 0, 0, ic.FaLevelUp);
                        }
                        ));
            }
        }

        private Image _driveIcon = null;
        protected Image DriveIcon {
            get {
                return _driveIcon ?? (_driveIcon =
                    new PaintingImage (
                        ic.DefaultSize,
                        ctx => {
                            SetDefault (ic);
                            ic.FillColor = IconStrokeColor;
                            ic.PaintIcon (ctx, ic.DefaultSize.Width, 0, 0, ic.FaHddO);
                        }
                        ));
            }
        }
        
        private Image _folderIcon = null;
        protected Image FolderIcon {
            get {
                return _folderIcon ?? (_folderIcon =
                    new PaintingImage (ic.DefaultSize,
                        ctx => {
                            SetDefault (ic);
                            ic.PaintIcon (ctx, ic.DefaultSize.Width, 0, 0, ic.FaFolder);
                        }
                        ));
            }
        }

        private Image _fileIcon = null;
        protected Image FileIcon {
            get {
                return _fileIcon ?? (_fileIcon =
                    new PaintingImage (ic.DefaultSize,
                        ctx => {
                            SetDefault (ic);
                            ic.FillColor = Colors.WhiteSmoke;
                            ic.PaintIcon (ctx, ic.DefaultSize.Width, 0, 0, ic.FaFile);
                        }
                        ));
            }
        }

        private Image _currentIcon = null;
        protected Image CurrentPathIcon {
            get {
                return _currentIcon ?? (_currentIcon =
                    new PaintingImage (
                        ic.DefaultSize,
                        ctx => {
                            SetDefault (ic);
                            ic.FillColor = IconStrokeColor.WithAlpha(.7);
                            ic.Stroke = false;
                            ic.PaintIcon (ctx, ic.DefaultSize.Width, 0, 0, ic.FaFolderOpen);
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
                    _layout =
                        Registry.Factory.Create<IGraphSceneLayout<IVisual, IVisualEdge>> (SceneGetter, styleSheet);
                    _layout.Distance = new Size (-5, -5);
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
					if(!OS.Mono)
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
                foreach (var drive in DriveInfo.GetDrives()) {
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
                    foreach (var file in Directory.EnumerateFiles (path,pattern)) {
                        var name = Path.GetFileName (file);
                        graph.Add (new VisualDir (this.FileIcon, name));
                    }
                } catch (UnauthorizedAccessException) {
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
            aligner.Locator.Commit (scene.Requests);

        }

        public void ComposeDisplay (VisualsDisplay display) {
            display.StyleSheet = Layout.StyleSheet;
            display.Layout = Layout;
			display.Data = Scene;

            var disp = display.ActionDispatcher;
            disp.Remove (disp.GetAction<GraphItemMoveResizeAction<IVisual, IVisualEdge>> ());

            //var select = new DelegatingMouseAction { };
            //disp.Add (select); only one action per action.gettype is allowed

            display.SceneFocusChanged += (s,e) => {
                if (e.Item == LevelUp) {
                    var parent = Directory.GetParent (CurrentPath);
                    string path = null;
                    if (parent != null)
                        path = parent.FullName;
                    var isRoot = Directory.GetDirectoryRoot (CurrentPath) == path;
                    ShowDir (path);
                    display.Perform ();
                    
                    return;
                }
                var dirItem = e.Item as VisualDir;
                if (dirItem != null) {
                    Action<string> showPath = (path) => {
                        if (Directory.Exists (path)) {
                            ShowDir (path);
                            display.DataChanged ();
                        }
                    };
                    if (dirItem.Icon == this.FolderIcon ) {
                        showPath (Path.Combine (CurrentPath, dirItem.Name));
                        return;
                    }
                    if (dirItem.Icon == this.FileIcon) {
                        if (FileSelected != null)
                            FileSelected (Path.Combine (CurrentPath, dirItem.Name));
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