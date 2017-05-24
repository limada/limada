/*
* This code is free software; you can redistribute it and/or modify it
* under the terms of the GNU General Public License version 2 only, as
* published by the Free Software Foundation.
*
* Author: Lytico
* Copyright (C) 2017 Lytico
*
* http://www.limada.org
*
*/

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Limaki.Common;
using Limaki.Common.Collections;
using Limaki.Common.Linqish;
using Limaki.Drawing;
using Limaki.Graphs;
using Limaki.View.Visuals;
using Limaki.View.Viz.Modelling;
using Limaki.View.Viz.Visuals;
using Limaki.View.XwtBackend.Viz;
using NUnit.Framework;
using Xwt;
using Xwt.Drawing;

namespace Limaki.Playground.View {


    public class TreeAlignerPrototyper : BlockAlignerPrototyper {

        public override Rectangle AlignByPath<TItem, TEdge> (IEnumerable<LevelItem<TItem>> walk, AlignerOptions options, Aligner<TItem, TEdge> aligner, Point startPoint) {
            
            TrackBounds = new Rectangle [0];

            var tree = new Tree<TItem> ();
            TreeNode<TItem> cursor = null;
            TreeNode<TItem> pathCursor = null;
            foreach (var step in walk) {
                ReportDetail ($"{step}");
                if (tree.Head == null) {
                    cursor = tree.AddHead (step.Node);
                    pathCursor = cursor;
                    continue;
                }
                pathCursor = tree.Where (t => t.Value.Equals (step.Path), pathCursor, false).First ();
                // TODO: append = false is not working!
                cursor = tree.AddLeaf (step.Node, pathCursor, true);
            }

            ReportDetail ($"->\t{nameof(tree)}\t{tree.WhereLoops}");
            foreach (var t in tree) {
                ReportDetail ($"{t}");
            }
            return new Rectangle (startPoint, Size.Zero);
        }
    }
}