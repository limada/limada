using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using Gecko.Events;
using Gecko.Interop;

namespace Gecko
{

	partial class GeckoWebBrowser
	{

        public event EventHandler<DomEventArgs> Load;

        #region public event EventHandler DocumentCompleted<GeckoDocumentCompletedEventArgs>

	    /// <summary>
	    /// Occurs after the browser has finished parsing a new page and updated the <see cref="Document"/> property.
	    /// </summary>
	    [Category ("Navigation")]
	    [Description ("Occurs after the browser has finished parsing a new page and updated the Document property.")]
	    public event EventHandler<GeckoDocumentCompletedEventArgs> DocumentCompleted;

        /// <summary>Raises the <see cref="DocumentCompleted"/> event.</summary>
        /// <param name="e">The data for the event.</param>
        protected virtual void OnDocumentCompleted (GeckoDocumentCompletedEventArgs e) {
            if (DocumentCompleted != null)
                DocumentCompleted (this, e);
        }

        #endregion

        public bool IsBusy { get; set; }
        public string StatusText { get; set; }

        void OnStateChange (nsIWebProgress aWebProgress, nsIRequest aRequest, uint aStateFlags, int aStatus) {
            const int NS_BINDING_ABORTED = unchecked ((int)0x804B0002);

            bool stateIsStop = ((aStateFlags & nsIWebProgressListenerConstants.STATE_STOP) != 0);
            bool stateIsStart = ((aStateFlags & nsIWebProgressListenerConstants.STATE_START) != 0);
            bool stateIsRedirecting = ((aStateFlags & nsIWebProgressListenerConstants.STATE_REDIRECTING) != 0);
            bool stateIsTransfering = ((aStateFlags & nsIWebProgressListenerConstants.STATE_TRANSFERRING) != 0);
            bool stateIsBroken = ((aStateFlags & nsIWebProgressListenerConstants.STATE_IS_BROKEN) != 0);

            #region request parameters

            /* This flag indicates that the state transition is for a request, which includes but is not limited to document requests.
             * Other types of requests, such as requests for inline content (for example images and stylesheets) are considered normal requests.
             */
            bool stateIsRequest = ((aStateFlags & nsIWebProgressListenerConstants.STATE_IS_REQUEST) != 0);

            /* This flag indicates that the state transition is for a document request. This flag is set in addition to STATE_IS_REQUEST.
             * A document request supports the nsIChannel interface and its loadFlags attribute includes the nsIChannel ::LOAD_DOCUMENT_URI flag.
             * A document request does not complete until all requests associated with the loading of its corresponding document have completed.
             * This includes other document requests (for example corresponding to HTML <iframe> elements).
             * The document corresponding to a document request is available via the DOMWindow attribute of onStateChange()'s aWebProgress parameter.
             */
            bool stateIsDocument = ((aStateFlags & nsIWebProgressListenerConstants.STATE_IS_DOCUMENT) != 0);

            /* This flag indicates that the state transition corresponds to the start or stop of activity in the indicated nsIWebProgress instance.
             * This flag is accompanied by either STATE_START or STATE_STOP, and it may be combined with other State Type Flags.
             * 
             * Unlike STATE_IS_WINDOW, this flag is only set when activity within the nsIWebProgress instance being observed starts or stops.
             * If activity only occurs in a child nsIWebProgress instance, then this flag will be set to indicate the start and stop of that activity.
             * For example, in the case of navigation within a single frame of a HTML frameset, a nsIWebProgressListener instance attached to the
             * nsIWebProgress of the frameset window will receive onStateChange() calls with the STATE_IS_NETWORK flag set to indicate the start and
             * stop of said navigation. In other words, an observer of an outer window can determine when activity, that may be constrained to a
             * child window or set of child windows, starts and stops.
             */
            bool stateIsNetwork = ((aStateFlags & nsIWebProgressListenerConstants.STATE_IS_NETWORK) != 0);

            /* This flag indicates that the state transition corresponds to the start or stop of activity in the indicated nsIWebProgress instance.
             * This flag is accompanied by either STATE_START or STATE_STOP, and it may be combined with other State Type Flags.
             * This flag is similar to STATE_IS_DOCUMENT. However, when a document request completes, two onStateChange() calls with STATE_STOP are generated.
             * The document request is passed as aRequest to both calls. The first has STATE_IS_REQUEST and STATE_IS_DOCUMENT set, and the second has
             * the STATE_IS_WINDOW flag set (and possibly the STATE_IS_NETWORK flag set as well -- see above for a description of when the STATE_IS_NETWORK
             * flag may be set). This second STATE_STOP event may be useful as a way to partition the work that occurs when a document request completes.
             */
            bool stateIsWindow = ((aStateFlags & nsIWebProgressListenerConstants.STATE_IS_WINDOW) != 0);

            bool requesterr = stateIsRequest && (aStatus & 0xff0000) == ((GeckoError.NS_ERROR_MODULE_SECURITY + GeckoError.NS_ERROR_MODULE_BASE_OFFSET) << 16);

            #endregion request parameters

            #region validity checks
            // The request parametere may be null
            if (aRequest == null)
                return;

            if ((stateIsStop && !stateIsNetwork && !requesterr) || stateIsBroken)
                return;

            // Ignore ViewSource requests, they don't provide the URL
            // see: http://mxr.mozilla.org/mozilla-central/source/netwerk/protocol/viewsource/nsViewSourceChannel.cpp#114
            if (!(stateIsStop|| stateIsBroken)) 
            {
                var viewSource = Xpcom.QueryInterface<nsIViewSourceChannel> (aRequest);
                if (viewSource != null) {
                    Marshal.ReleaseComObject (viewSource);
                    return;
                }
            }

            #endregion validity checks

            var request = Gecko.Net.Request.CreateRequest (aRequest);
            Uri destUri = null;
            Uri.TryCreate (request.Name, UriKind.Absolute, out destUri);
            var domWindow = aWebProgress.GetDOMWindowAttribute ().Wrap (x => new GeckoWindow (x));

            #region STATE_START
            /* This flag indicates the start of a request.
			 * This flag is set when a request is initiated.
			 * The request is complete when onStateChange() is called for the same request with the STATE_STOP flag set.
			 */
            if (stateIsStart) {

                // TODO: replace to aWebProgress.GetIsTopLevelAttribute() // Gecko 24+
                if (stateIsNetwork && domWindow.IsTopWindow ()) {
                    IsBusy = true;

                    GeckoNavigatingEventArgs ea = new GeckoNavigatingEventArgs (destUri, domWindow);
                    OnNavigating (ea);

                    if (ea.Cancel) {
                        aRequest.Cancel (NS_BINDING_ABORTED);
                        //TODO: change the following handling of cancelled request

                        // clear busy state
                        IsBusy = false;

                        // clear progress bar
                        OnProgressChanged (new GeckoProgressEventArgs (100, 100));

                        // clear status bar
                        StatusText = "";
                    }
                } else if (stateIsDocument) {
                    GeckoNavigatingEventArgs ea = new GeckoNavigatingEventArgs (destUri, domWindow);
                    OnFrameNavigating (ea);

                    if (ea.Cancel) {
                        // TODO: test it on Linux
                        if (!Xpcom.IsLinux)
                            aRequest.Cancel (NS_BINDING_ABORTED);
                    }
                }
            }
            #endregion STATE_START

            #region STATE_REDIRECTING
                /* This flag indicates that a request is being redirected.
			 * The request passed to onStateChange() is the request that is being redirected.
			 * When a redirect occurs, a new request is generated automatically to process the new request.
			 * Expect a corresponding STATE_START event for the new request, and a STATE_STOP for the redirected request.
			 */
            else if (stateIsRedirecting) {

                // make sure we're loading the top-level window
                GeckoRedirectingEventArgs ea = new GeckoRedirectingEventArgs (destUri, domWindow);
                OnRedirecting (ea);

                if (ea.Cancel) {
                    aRequest.Cancel (NS_BINDING_ABORTED);
                }
            }
            #endregion STATE_REDIRECTING

            #region STATE_TRANSFERRING
                /* This flag indicates that data for a request is being transferred to an end consumer.
			 * This flag indicates that the request has been targeted, and that the user may start seeing content corresponding to the request.
			 */
            else if (stateIsTransfering) {

            }
            #endregion STATE_TRANSFERRING

            #region STATE_STOP
                /* This flag indicates the completion of a request.
			 * The aStatus parameter to onStateChange() indicates the final status of the request.
			 */
            else if (stateIsStop) {
                /* aStatus
                 * Error status code associated with the state change.
                 * This parameter should be ignored unless aStateFlags includes the STATE_STOP bit.
                 * The status code indicates success or failure of the request associated with the state change.
                 * 
                 * Note: aStatus may be a success code even for server generated errors, such as the HTTP 404 File Not Found error.
                 * In such cases, the request itself should be queried for extended error information (for example for HTTP requests see nsIHttpChannel).
                 */

                if (stateIsNetwork) {
                    // clear busy state
                    IsBusy = false;
                    if (aStatus == 0) {
                        // kill any cached document and raise DocumentCompleted event
                        OnDocumentCompleted (new GeckoDocumentCompletedEventArgs (destUri, domWindow));

                        // clear progress bar
                        OnProgressChanged (new GeckoProgressEventArgs (100, 100));
                    } else {
                        OnNavigationError (new GeckoNavigationErrorEventArgs (request.Name, domWindow, aStatus));
                    }
                    // clear status bar
                    StatusText = "";
                }

                if (stateIsRequest) {
                    if ((aStatus & 0xff0000) == ((GeckoError.NS_ERROR_MODULE_SECURITY + GeckoError.NS_ERROR_MODULE_BASE_OFFSET) << 16)) {
                        var ea = new GeckoNSSErrorEventArgs (destUri, aStatus);
                        OnNSSError (ea);
                        if (ea.Handled) {
                            aRequest.Cancel (GeckoError.NS_BINDING_ABORTED);
                        }
                    }
                }
            }
            #endregion STATE_STOP

            if (domWindow != null) {
                domWindow.Dispose ();
            }
        }

        private void OnNSSError (GeckoNSSErrorEventArgs ea) {
            
        }

        private void OnNavigationError (GeckoNavigationErrorEventArgs geckoNavigationErrorEventArgs) {
            
        }

        private void OnRedirecting (GeckoRedirectingEventArgs ea) {
            
        }

        private void OnFrameNavigating (GeckoNavigatingEventArgs ea) {
            
        }

        private void OnProgressChanged (GeckoProgressEventArgs geckoProgressEventArgs) {
            
        }

        private void OnNavigating (GeckoNavigatingEventArgs ea) {
            
        }

	}

    class GeckoRedirectingEventArgs {
        private Uri destUri;
        private GeckoWindow domWindow;

        public GeckoRedirectingEventArgs (Uri destUri, GeckoWindow domWindow) {
            // TODO: Complete member initialization
            this.destUri = destUri;
            this.domWindow = domWindow;
        }
        public bool Cancel { get; set; }
    }

    class GeckoProgressEventArgs {
        private int p1;
        private int p2;

        public GeckoProgressEventArgs (int p1, int p2) {
            // TODO: Complete member initialization
            this.p1 = p1;
            this.p2 = p2;
        }
    }
}
