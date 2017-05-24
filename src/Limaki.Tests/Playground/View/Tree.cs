/*
 * Limaki 
 * 
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

using System.Collections.Generic;
using System;
using System.Collections;
using System.Linq;

namespace Limaki.Common.Collections {

    /// <summary>
    /// a tree based on one? linkedlist
    /// </summary>
    public class Tree<T>:IEnumerable<TreeNode<T>>  {



        public TreeNode<T> Head { get { return List.Any () ? List.First () : null; } }

        public LinkedList<TreeNode<T>> List { get; } = new LinkedList<TreeNode<T>> ();

        public IEnumerator<TreeNode<T>> GetEnumerator () {
            return List.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator () => this.GetEnumerator ();

        public TreeNode<T>  AddHead (T item) {
            var treeNode = new TreeNode<T> { Value = item };
            treeNode.Node =  List.AddFirst(treeNode);
            return treeNode;

        }

        public TreeNode<T> AddLeaf (T item, TreeNode<T> root, bool append = false) {

            if (root == null)
                throw new ArgumentException ();
            
            var treeNode = new TreeNode<T> { Value = item, Root = root.Node };
            var cursor = root.Node;
            if (append) {
                var lastLeaf = Where (l => l.Root!=null && l.Root.Value.Equals (root), cursor.Next?.Value, false).LastOrDefault()?.Node;
                cursor = lastLeaf ?? cursor;
            }
            treeNode.Node = List.AddAfter (cursor, treeNode);

            return treeNode;
        }

        public long WhereLoops { get; protected set; }
        public IEnumerable<TreeNode<T>> Where (Func<TreeNode<T>, bool> where, TreeNode<T> current, bool goback = true, bool all = true) {
            if (current == null)
                yield break;
            var cursor = current;
            var end = goback ? List.Last : List.First;
            while (cursor != null) {
                WhereLoops++;
                if (where (cursor))
                    yield return cursor;
                cursor = goback ? cursor.Node.Previous?.Value : cursor.Node.Next?.Value;
            }
            if (!all)
                yield break;
            cursor = goback ? current.Node.Next?.Value : current.Node.Previous?.Value;
            goback = !goback;
            while (cursor != null) {
                WhereLoops++;
                if (where (cursor))
                    yield return cursor;
                cursor = goback ? cursor.Node.Previous?.Value : cursor.Node.Next?.Value;
            }
        }
    }

    public class TreeNode<T> {

        internal TreeNode () { }

        public T Value { get; set; }

        public LinkedListNode<TreeNode<T>> Node { get; internal set; }

        public LinkedListNode<TreeNode<T>> Root { get; internal set; }

        public bool Visited { get; set; }

        public override string ToString () {
            var root = default (T);
            if (Root != null && Root.Value != null)
                root = Root.Value.Value;
            return $"{{Node={Node.Value.Value} Root={root}}}";
        }

    }
}

