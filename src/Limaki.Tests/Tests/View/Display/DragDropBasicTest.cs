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


using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using NUnit.Framework;
using Limaki.Common;
using Limaki.Drawing;
using Limaki.Winform.DragDrop;
using Limaki.UnitTest;
using Limaki.Winform;
using Limaki.Actions;
using Limaki.Widgets;
using System.Xml.Serialization;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using Limaki.Graphs;

namespace Limaki.Tests.View.Display {
    public class DragDropBasicTest:DomainTest {
        private string widgetData = "Widget DragContent";

        private IGraph<IWidget, IEdgeWidget> _graph = null;
        public IGraph<IWidget, IEdgeWidget> Graph {
            get {
                if (_graph == null) {
                    _graph = new WidgetGraph ();
                }
                return _graph;
            }
            set { _graph = value; }
        }

        [Test]
        public void TestWidget () {
            SetWidget();
            Application.DoEvents();
            GetWidget();
        }

        

        public void GetWidget () {
            IDataObject dataObject = Clipboard.GetDataObject();
            Application.DoEvents();

            DataObjectHandlerChain chain = new DataObjectHandlerChain();
            chain.InitDataObjectHanders();
           
            IWidget widget = chain.GetWidget(dataObject,Graph,false);
            Assert.AreEqual(widget.Data.ToString(),widgetData);

            dataObject.SetData("another format", new Rectangle());
        }

        public void SetWidget() {
            IWidget widget = new Widget<string>(widgetData);

            DataObjectHandlerChain chain = new DataObjectHandlerChain();
            chain.InitDataObjectHanders();

            DataObject dataObject = new DataObject();
            chain.SetWidget(dataObject, Graph,widget);

            System.Runtime.InteropServices.ComTypes.IDataObject comData =
                 dataObject as System.Runtime.InteropServices.ComTypes.IDataObject;

            string s = comData.ToString();

            Clipboard.SetDataObject(dataObject);
        }

        [Test]
        public void SetWidgetXML() {
            IWidget widget = new Widget<string>(widgetData);
            XmlSerializer ser = new XmlSerializer(widget.GetType());
            TextWriter writer = new StringWriter();
            ser.Serialize(writer, widget);
            string s = writer.ToString();
            writer.Close();



        }

        [Test]
        public void SetWidgetStream()
        {
            IWidget widget = new Widget<string>(widgetData);
            IFormatter formatter = new BinaryFormatter();
            Stream stream = new MemoryStream();
            formatter.Serialize(stream, widget);
            //stream.Close();
            stream.Position = 0;

            IWidget widget2 = (IWidget)formatter.Deserialize(stream);
            stream.Close();


        }
    }
}
