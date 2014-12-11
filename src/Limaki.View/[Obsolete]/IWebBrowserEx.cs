using System;
using System.ComponentModel;

namespace Limaki.View.Vidgets {

    [Obsolete]
    public interface IWebBrowserEx : IWebBrowser {

        bool AllowNavigation { get; set; }
        bool AllowWebBrowserDrop { get; set; }

        bool CanGoBack { get; }
        bool CanGoForward { get; }

        string DocumentTitle { get; }
        string DocumentType { get; }

        bool IsBusy { get; }

        bool IsOffline { get; }

        string StatusText { get; }

        bool GoBack ();
        bool GoForward ();
        void GoHome ();

        void Navigate (Uri url);
        void Navigate (string urlString, bool newWindow);
        void Navigate (string urlString, string targetFrameName);
        void Navigate (Uri url, bool newWindow);
        void Navigate (Uri url, string targetFrameName);
        void Navigate (string urlString, string targetFrameName, byte[] postData, string additionalHeaders);
        void Navigate (Uri url, string targetFrameName, byte[] postData, string additionalHeaders);
        void Refresh ();

        void Stop ();
        void GoSearch ();

        void ShowPageSetupDialog ();
        void ShowPrintPreviewDialog ();
        void ShowPropertiesDialog ();
        void ShowSaveAsDialog ();

        event EventHandler CanGoBackChanged;
        event EventHandler CanGoForwardChanged;
        event EventHandler DocumentTitleChanged;
        event EventHandler FileDownload;
        event CancelEventHandler NewWindow;
        event EventHandler StatusTextChanged;
        event EventHandler PaddingChanged;
    }

    public interface IWebBrowserExBackend : IWebBrowserBackend, IWebBrowserEx {

    }
}