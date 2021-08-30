/*
 * Limada
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

using Limada.Model;
using Limada.Schemata;
using System.IO;
using Limaki.Contents;
using System.Collections.Generic;
using System.Linq;
using Limaki.Graphs;
using Limaki.Common.Linqish;
using System;
using System.Diagnostics;

namespace Limada.Schemata {
    /// <summary>
    /// 
    /// a Digidoc is a document with pages
    /// it can be a scan, a fax, or the result of a parsed pdf-document
    /// 
    /// DigidocSchema describes a Schema
    /// 
    /// where  
    /// 
    /// someLink.Marker == DigidocSchema.Document
    /// someLink.Leaf == aDocument
    /// 
    /// aDocument has
    ///
    /// theTitleLink.Leaf = theTitle
    /// theTitleLink.Root = aDocument
    /// theTitleLink.Marker = DigidocSchema.Title
    ///
    /// someAuthorLink.Leaf = someAuthor;
    /// someAuthorLink.Root = aDocument;
    /// someAuthorLink.Marker = DigidocSchema.Author;
    /// 
    /// somePageLink.Leaf = somePage;
    /// somePageLink.Root = aDocument;
    /// somePageLink.Marker = DigidocSchema.Page;
    /// somePageNumberLink.Root = somePageLink;
    /// somePageNumberLink.Leaf = somePageNumber;
    /// somePageNumberLink.Root = DigidocSchema.PageNumber;
    /// 
    /// <see cref="MetaSchema"/>:
    /// 
    /// DigidocSchema.Document2TitleLink.Root = DigidocSchema.Document
    /// DigidocSchema.Document2TitleLink.Leaf = DigidocSchema.Title
    /// DigidocSchema.Document2TitleLink.Marker = MetaSchema.DescriptionMarker
    /// 
    /// DigidocSchema.Document2AuthorLink.Root = DigidocSchema.Document
    /// DigidocSchema.Document2AuthorLink.Leaf = DigidocSchema.Author
    /// DigidocSchema.Document2AuthorLink.Marker = MetaSchema.Root;
    /// 
    /// DigidocSchema.Page2PageNumber.Root = DigidocSchema.Page
    /// DigidocSchema.Page2PageNumber.Leaf = DigidocSchema.PageNr
    /// DigidocSchema.Page2PageNumber.Marker = MetaSchema.DescriptionMarker;
    /// 
    /// 
    /// </summary>
    public class DigidocSchema : Schema {
        /// <summary>
        /// a <see cref="TopicSchema.Topic"/> to gather all Documents
        /// </summary>
        public static readonly IThing DocumentsRoot = Thing<string>("Documents", 0xDA989F8F8DE3E245);

        /// <summary>
        /// marker for document
        /// </summary>
        public static readonly IThing Document = Thing<string>("Document", 0x21F92B31EFED5A67);
        /// <summary>
        /// marker for title of a document
        /// </summary>
        public static readonly IThing DocumentTitle = Thing<string>("Title", 0xA769ACA5A26200E7);
        /// <summary>
        /// marker for author of a document
        /// </summary>
        public static readonly IThing Author = Thing<string>("Autor", 0x5212AC00038D58A8);
        /// <summary>
        /// marker for pages of a document
        /// </summary>
        public static readonly IThing DocumentPage = Thing<string>("Page", 0x19FDE85FF9EA7D77);

        /// <summary>
        /// marker for a number of a page of a document
        /// </summary>
        public static readonly IThing PageNumber = Thing<string>("Number", 0x57D229022C78F4F8);

        #region Document.MetaSchema
        /// <summary>
        /// defines Document.Title as Default
        /// link.root == DigidocSchema.Document 
        /// link.leaf == DigidocSchema.Title
        /// link.marker == MetaSchema.Description
        /// </summary>
        public static readonly IThing DocumentDefaultLink = Link(Document, DocumentTitle, MetaSchema.DescriptionMarker, 0xE62EB6C672E3A7D8);

        /// <summary>
        /// defines Page.PageNumber as Default
        /// link.root == Page 
        /// link.leaf == PageNumber
        /// link.marker == MetaSchema.Description
        /// </summary>
        public static readonly IThing PageDefaultLink = Link(DocumentPage, PageNumber, MetaSchema.DescriptionMarker, 0xB012D15B9121E849);

        /// <summary>
        /// hides pages of document in views
        /// link.root == Document 
        /// link.leaf == Page
        /// link.marker == ViewMetaSchema.Hide
        /// </summary>     
        public static readonly IThing HidePagesLink = Link(Document, DocumentPage, ViewMetaSchema.Hide, 0x1FA8426299439E81);

        /// <summary>
        /// TODO: this has the wrong semantic;
        /// move to PartsSchema and make a new one
        /// </summary>
        public static readonly IThing AuthorHasDocument = Link(Author, Document, MetaSchema.Root, 0xEE75EE5BA8CE6A29);

        #endregion
        // TODO:
        //			pidAutoCreateDocumentToTitleEdge:txbID = $68C5691BF8B83A95;
        //			pidAutoCreateDocumentToAutorEdge:txbID = $EE75EE5BA8CE6A29;
        //			pidAutoCreateDocumentToPageEdge:txbID = $3D044A4D1A3D49E1;
        //			pidAutoCreatePageEdgeToNumberEdge:txbID = $6725F26A6770C0E0;

        #region Data-Handling
        
        public DigidocSchema():base() { }

        public DigidocSchema(IThingGraph graph, IThing document) : base(graph,document) { }

        static DigidocSchema () {
            ComposeDependencies ();
        }

        public IThing Title {
            get { return GetTheLeaf ( DocumentTitle ); }
            set { SetTheLeaf(DocumentTitle, value); }
        }

        public IThing CreatePage (Content<Stream> content, object pageNr) {
            return CreatePage(this.Graph, this.Subject, content, pageNr);
        }

        public IThing CreatePage(IThingGraph graph, IThing document, Content<Stream> stream, object pageNr) {
            if (graph == null || document == null)
                return null;
            var page = Factory.CreateItem<Stream>(null) as IStreamThing;
            page.ContentContainer = graph.ContentContainer;
            if (stream != null) {
                new ThingContentFacade(Factory).AssignContent(page, stream);
            }
            var pageEdge = Factory.CreateEdge(document, page, DigidocSchema.DocumentPage);
            graph.Add(pageEdge);

            var number = Factory.CreateItem(pageNr);
            var numberEdge = Factory.CreateEdge(pageEdge, number, DigidocSchema.PageNumber);
            graph.Add(number);
            graph.Add(numberEdge);
            return page;
        }

        public IThing CreateDocument(string title) {
            this.Subject = CreateDocument(this.Graph, title);
            return this.Subject;
        }

        public IThing CreateDocument(IThingGraph graph, string title) {
            if (graph == null) return null;

            var document = Factory.CreateItem();
            graph.Add(document);
            if (title == null)
                title = "Document " + document.Id.ToString("X");
            graph.Add(Factory.CreateEdge(document, Factory.CreateItem(title), DigidocSchema.DocumentTitle));
            return document;
        }

        public IEnumerable<IThing> Pages() {
            return this.Pages(this.Graph, this.Subject);
        }

        public IEnumerable<IThing> OrderedPages() {
            return this.OrderedPages(this.Graph, this.Subject);
        }

        public IEnumerable<IThing> Pages (IThingGraph graph, IThing document) {
            return PageLinks(graph, document)
                .Select(link => link.Leaf);
        }

        public IEnumerable<ILink> PageLinks (IThingGraph graph, IThing document) {
            graph = graph.Unwrap() as IThingGraph;
            if (graph == null || document == null)
                return new ILink[0];
            return graph.Edges(document)
                .Where(link => link.Marker.Id == DocumentPage.Id);
        }

        public IEnumerable<IThing> OrderedPages(IThingGraph graph, IThing document) {
            var pages = Pages(graph, document);
            Func<IThing, IThing> order = t => t;
            var schemaGraph = graph as SchemaThingGraph;
            if (schemaGraph != null) {
                order = t => schemaGraph.ThingToDisplay(t);
            }
            return pages.Yield().OrderBy(e => order(e), new ThingComparer());
        }

        public bool HasPages() {
            return HasPages(this.Graph, this.Subject);
        }

        public bool HasPages (GraphCursor<IThing, ILink> cursor) {
            return HasPages(cursor.Graph , cursor.Cursor);
        }

        public bool HasPages (IGraph<IThing, ILink> graph, IThing document) {
            graph = graph.Unwrap();
            if (graph == null || document == null)
                return false;

            return graph.Edges (document)
                .Any (link => link.Marker != null && link.Marker.Id == DigidocSchema.DocumentPage.Id && link.Root != null && link.Root == document);
        }

        public IEnumerable<Content<Stream>> PageStreams() {
            return this.PageStreams(this.Graph, this.Subject);
        }

        public IEnumerable<Content<Stream>> PageStreams(IThingGraph graph, IThing document) {
            if (graph == null || document == null)
                return new Content<Stream>[0];

            return Pages(graph, document)
                .Where(page => page is IStreamThing)
                .Select(page => graph.ContentOf(page));
        }

        public IThing PageNumberOf (IGraph<IThing, ILink> graph, IThing document, IThing page) {
            graph = graph.Unwrap ();
            var pageLink = graph.Edges (page)
                .FirstOrDefault (l => l.Root == document && l.Leaf == page && l.Marker.Id == DocumentPage.Id);
            if (pageLink != null) {
                return GetTheLeaf (graph, pageLink, PageNumber);
            }
            return null;
        }

        public IThing PageNumberOf (IThing page) { return PageNumberOf (this.Graph, this.Subject, page); }

        #endregion

        #region Dependencies

        protected static void ComposeDependencies () {
            SchemaFacade.Dependencies.Visitor += (source, action, changeType) => {
                new DigidocSchema ().DependsOn (source, changeType).ForEach (action);
            };
        }

        public virtual IEnumerable<IThing> DependsOn (GraphCursor<IThing, ILink> source, GraphEventType eventType) {
            if (HasPages (source)) {
                var doc = source.Cursor;
                var graph = source.Graph.Unwrap();

                foreach (var link in PageLinks (graph as IThingGraph, doc).ToArray ()) {
                    var page = link.Leaf;
                    var singleEdge = graph.HasSingleEdge (page);
                    yield return link;

                    if (singleEdge || eventType != GraphEventType.Remove)
                        yield return page;
                }

            }
        }

        #endregion

    }
}