/*
 * Limaki 
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
 */

using System;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;
using Limaki.Drawing;

namespace Limaki.Winform.Controls {
    public interface IWebBrowser {
        [DefaultValue(true)]
        bool AllowNavigation { get; set; }

        [DefaultValue(true)]
        bool AllowWebBrowserDrop { get; set; }

        [BrowsableAttribute(false)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Hidden)]
        bool CanGoBack { get; }

        [BrowsableAttribute(false)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Hidden)]
        bool CanGoForward { get; }

        [BrowsableAttribute(false)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Hidden)]
        HtmlDocument Document { get; }

        [BrowsableAttribute(false)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Hidden)]
        Stream DocumentStream { get; set; }

        [BrowsableAttribute(false)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Hidden)]
        string DocumentText { get; set; }

        [BrowsableAttribute(false)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Hidden)]
        string DocumentTitle { get;  }

        [BrowsableAttribute(false)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Hidden)]
        string DocumentType { get; }

        [BrowsableAttribute(false)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Hidden)]
        bool IsBusy { get; }

        [BrowsableAttribute(false)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Hidden)]
        bool IsOffline { get; }


        [BrowsableAttribute(false)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Hidden)]
        WebBrowserReadyState ReadyState { get; }

        [BrowsableAttribute(false)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Hidden)]
        string StatusText { get; }

        [BindableAttribute(true)]
        [DefaultValue(null)]
        //[TypeConverter(typeof(WebBrowserUriTypeConverter))]
        Uri Url { get; set; }

        bool GoBack();
        bool GoForward();
        void GoHome();
        void MakeReady();
        void Navigate(string urlString);
        void Navigate(Uri url);
        void Navigate(string urlString, bool newWindow);
        void Navigate(string urlString, string targetFrameName);
        void Navigate(Uri url, bool newWindow);
        void Navigate(Uri url, string targetFrameName);
        void Navigate(string urlString, string targetFrameName, byte[] postData, string additionalHeaders);
        void Navigate(Uri url, string targetFrameName, byte[] postData, string additionalHeaders);
        void Refresh();
        void Refresh(WebBrowserRefreshOption opt);
        void Stop();
        void GoSearch();
        void ShowPageSetupDialog();
        void ShowPrintPreviewDialog();
        void ShowPropertiesDialog();
        void ShowSaveAsDialog();

        [BrowsableAttribute(false)]
        event EventHandler CanGoBackChanged;

        [BrowsableAttribute(false)]
        event EventHandler CanGoForwardChanged;

        event WebBrowserDocumentCompletedEventHandler DocumentCompleted;

        [BrowsableAttribute(false)]
        event EventHandler DocumentTitleChanged;

        event EventHandler FileDownload;
        event WebBrowserNavigatedEventHandler Navigated;
        event WebBrowserNavigatingEventHandler Navigating;
        event CancelEventHandler NewWindow;
        event WebBrowserProgressChangedEventHandler ProgressChanged;

        [BrowsableAttribute(false)]
        event EventHandler StatusTextChanged;

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        event EventHandler PaddingChanged;
    }
}