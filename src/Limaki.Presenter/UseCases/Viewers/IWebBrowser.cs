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
 */

#define nowinform
using System;
using System.ComponentModel;
using System.IO;
using Limaki.Drawing;

#if winform
using System.Windows.Forms;
#endif

namespace Limaki.UseCases.Viewers {
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

        

        [BrowsableAttribute(false)]
        event EventHandler DocumentTitleChanged;

        event EventHandler FileDownload;
        
        event CancelEventHandler NewWindow;
        

        [BrowsableAttribute(false)]
        event EventHandler StatusTextChanged;

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        event EventHandler PaddingChanged;


        #region Winform
#if winform
        [BrowsableAttribute(false)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Hidden)]
        HtmlDocument Document { get; }


        void Refresh(WebBrowserRefreshOption opt);


        [BrowsableAttribute(false)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Hidden)]
        WebBrowserReadyState ReadyState { get; }


        event WebBrowserDocumentCompletedEventHandler DocumentCompleted;
        event WebBrowserNavigatedEventHandler Navigated;
        event WebBrowserNavigatingEventHandler Navigating;
        event WebBrowserProgressChangedEventHandler ProgressChanged;
#endif
        #endregion
    }
}