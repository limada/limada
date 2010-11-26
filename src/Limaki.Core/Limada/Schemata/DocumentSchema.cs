/*
 * Limada
 * Version 0.081
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

using Limada.Model;
using Limada.Schemata;

namespace Limada.Schemata {
    /// <summary>
    /// DocumentSchema describes a Schema
    /// 
    /// where  
    /// 
    /// someLink.Marker == DocumentSchema.Document
    /// someLink.Leaf == aDocument
    /// 
    /// aDocument has
    ///
    /// theTitleLink.Leaf = theTitle
    /// theTitleLink.Root = aDocument
    /// theTitleLink.Marker = DocumentSchema.Title
    ///
    /// someAuthorLink.Leaf = someAuthor;
    /// someAuthorLink.Root = aDocument;
    /// someAuthorLink.Marker = DocumentSchema.Author;
    /// 
    /// somePageLink.Leaf = somePage;
    /// somePageLink.Root = aDocument;
    /// somePageLink.Marker = DocumentSchema.Page;
    /// somePageNumberLink.Root = somePageLink;
    /// somePageNumberLink.Leaf = somePageNumber;
    /// somePageNumberLink.Root = DocumentSchema.PageNumber;
    /// 
    /// <see cref="MetaSchema"/>:
    /// 
    /// DocumentSchema.Document2TitleLink.Root = DocumentSchema.Document
    /// DocumentSchema.Document2TitleLink.Leaf = DocumentSchema.Title
    /// DocumentSchema.Document2TitleLink.Marker = MetaSchema.DescriptionMarker
    /// 
    /// DocumentSchema.Document2AuthorLink.Root = DocumentSchema.Document
    /// DocumentSchema.Document2AuthorLink.Leaf = DocumentSchema.Author
    /// DocumentSchema.Document2AuthorLink.Marker = MetaSchema.Root;
    /// 
    /// DocumentSchema.Page2PageNumber.Root = DocumentSchema.Page
    /// DocumentSchema.Page2PageNumber.Leaf = DocumentSchema.PageNr
    /// DocumentSchema.Page2PageNumber.Marker = MetaSchema.DescriptionMarker;
    /// 
    /// 
    /// </summary>
    public class DocumentSchema : Schema {
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
        /// link.root == DocumentSchema.Document 
        /// link.leaf == DocumentSchema.Title
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
        //			pidDocumentGUIDocumentToTitleEdge:txbID = $68C5691BF8B83A95;
        //			pidDocumentGUIDocumentToAutorEdge:txbID = $EE75EE5BA8CE6A29;
        //			pidDocumentGUIDocumentToPageEdge:txbID = $3D044A4D1A3D49E1;
        //			pidDocumentGUIPageEdgeToNumberEdge:txbID = $6725F26A6770C0E0;

        #region Data-Handling
        
        public DocumentSchema():base() {}
        public DocumentSchema(IThingGraph graph, IThing document) : base(graph,document) { }


        public IThing Title {
            get { return GetTheLeaf ( DocumentTitle ); }
            set { SetTheLeaf(DocumentTitle, value); }
        }

        #endregion
    }
}