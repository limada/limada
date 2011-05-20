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

using System.Xml.Linq;
using Limaki.Common;
using Limaki.Drawing;
using NUnit.Framework;

namespace Limaki.Tests.View.Drawing.Shapes {
    public class ViewSerialisationTest:DomainTest {

        [Test]
        public void FontTest() {
            var ser = new DrawingPrimitivesSerializer ();
            var font = new Font ();
            font.FontFamily = "Tahoma";
            var elem = ser.Write (font);
            this.ReportDetail (elem.ToString());

            var newFont = ser.ReadFont (elem);
            
            Assert.AreEqual (font.FontFamily, newFont.FontFamily);
            Assert.AreEqual(font.Size, newFont.Size);
            Assert.AreEqual(font.Style, newFont.Style);
        }

        [Test]
        public void ColorTest() {
            var ser = new DrawingPrimitivesSerializer();
            var item = Color.Black;


            var node = new XElement ("ColorTest");
            node.Add (ser.Write (item,"Color"));
            this.ReportDetail(node.ToString());

            var newItem = ser.ReadColor(node,"Color");
            Assert.AreEqual(item, newItem);

        }

        [Test]
        public void PenTest() {
            var ser = new DrawingPrimitivesSerializer();
            var item = new Pen(Color.Red);
            item.Thickness = 5;
            item.LineJoin = PenLineJoin.Miter;
            item.StartCap = PenLineCap.Flat;
            item.EndCap = PenLineCap.Round;

            var node = ser.Write(item);
            this.ReportDetail(node.ToString());

            var newItem = ser.ReadPen(node);
            Assert.AreEqual(item, newItem);

        }

        [Test]
        public void TupleTest() {
            var ser = new DrawingPrimitivesSerializer();
            var item = new Pair<string, string> ("a","b");
            ser.Write (item);
            var node = new XElement ("tupletest");
            node.Add    (ser.Write("tuple",item));

            this.ReportDetail(node.ToString());

            var newItem = ser.ReadTuple(node, "tuple");
            Assert.AreEqual(item, newItem);
        }
        [Test]
        public void SizeTest() {
            var ser = new DrawingPrimitivesSerializer();
            var item = new SizeI(10,10);
            var node = new XElement("itemTest");
            node.Add(ser.Write("size", new Pair<int,int>(item.Width,item.Height)));

            this.ReportDetail(node.ToString());

            var newItem = ser.ReadSize(node, "size");
            Assert.AreEqual(item, newItem);
        }
        [Test]
        public void StyleTest() {
            var ser = new DrawingPrimitivesSerializer();
            var item = StyleSheet.CreateDefaultStyleSheet().DefaultStyle;

            item.AutoSize = new SizeI (10, 10);
            item.PaintData = false;

            var node = ser.Write (item);
            
            this.ReportDetail(node.ToString());

            var newItem = ser.ReadStyle(node,item.ParentStyle);
            Assert.IsTrue(item.Equals(newItem));

        }

        [Test]
        public void StyleSheetTest() {
            var ser = new DrawingPrimitivesSerializer();
            var item = StyleSheet.CreateDefaultStyleSheet();

            var node = ser.Write(item);

            this.ReportDetail(node.ToString());

            var newItem = ser.ReadStyleSheet(node);
            Assert.AreEqual(item, newItem);

        }
    }
}
