using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Gecko.Events;
using Gecko.Interop;
using Gecko.Net;
using System.Collections.Generic;

//EventHandlerList http://msdn.microsoft.com/en-us/library/system.componentmodel.eventhandlerlist.aspx
//Generic EventHandler http://msdn.microsoft.com/en-us/library/db0etb8x.aspx
//C# 3.0 in a nutshell http://books.google.com/books?id=_Y0rWd-Q2xkC&pg=PA113&lpg=PA113&dq=readonly+field+eventargs&source=bl&ots=R8CCSaWH-j&sig=s6ImztjXLnI2rrL-usLfoGmVR8g&hl=en&sa=X&ei=EctAT5baOM3Zsga9ocDYBA&ved=0CFkQ6AEwBw


namespace Gecko
{
	partial class GeckoWebBrowser
	{
		// TODO: replace to GeckoEventList (with binary search algorithm) after all tests
		private GeckoEventListTmp _eventList = new GeckoEventListTmp();
		// int keys are faster than objects
		#region Event Keys
		// Navigation
		private const int NavigatingEvent =0x10001;
		private const int NavigatedEvent = 0x10002;
		private const int FrameNavigatingEvent = 0x10003;
		private const int DocumentCompletedEvent = 0x10004;
		private const int RedirectingEvent = 0x10005;
		private const int CanGoBackChangedEvent = 0x10006;
		private const int CanGoForwardChangedEvent = 0x10007;
		// ProgressChanged
		private const int RequestProgressChangedEvent = 0x20001;
		private const int ProgressChangedEvent = 0x20002;
		// History
		private const int HistoryNewEntryEvent = 0x30001;
		private const int HistoryGoBackEvent = 0x30002;
		private const int HistoryGoForwardEvent = 0x30003;
		private const int HistoryReloadEvent = 0x30004;
		private const int HistoryGotoIndexEvent = 0x30005;
		private const int HistoryPurgeEvent = 0x30006;
		// Windows
		private const int CreateWindowEvent = 0x40001;
		private const int CreateWindow2Event = 0x40002;
		private const int WindowSetBoundsEvent = 0x40003;
		private const int WindowClosedEvent = 0x40004;
		// StatusTextChanged
		private const int StatusTextChangedEvent = 0x50001;
		// DocumentTitleChanged
		private const int DocumentTitleChangedEvent = 0x60001;
		// ShowContextMenu
		private const int ShowContextMenuEvent = 0x70001;
		// ObserveHttpModifyRequest
		private const int ObserveHttpModifyRequestEvent = 0x80001;
		// Dom
		private const int DomKeyDownEvent = 0x90001;
		private const int DomKeyUpEvent = 0x90002;
		private const int DomKeyPressEvent = 0x90003;
		private const int DomMouseDownEvent = 0x90004;
		private const int DomMouseUpEvent = 0x90005;
		private const int DomMouseOverEvent = 0x90006;
		private const int DomMouseOutEvent = 0x90007;
		private const int DomMouseMoveEvent = 0x90008;
		private const int DomContextMenuEvent = 0x90009;
		private const int DomMouseScrollEvent = 0x9000A;
		private const int DomSubmitEvent = 0x9000B;
		private const int DomCompositionStartEvent = 0x9000C;
		private const int DomCompositionEndEvent = 0x9000D;
		private const int DomFocusEvent = 0x9000E;
		private const int DomBlurEvent = 0x9000F;
		private const int LoadEvent = 0x90010;
		private const int DOMContentLoadedEvent = 0x90011;
		private const int ReadyStateChangeEvent = 0x90012;
		private const int HashChangeEvent = 0x90013;
		private const int DomContentChangedEvent = 0x90014;
		private const int DomClickEvent = 0x90015;
		private const int DomDoubleClickEvent = 0x90016;
		private const int DomDragStartEvent = 0x90017;
		private const int DomDragEnterEvent = 0x90018;
		private const int DomDragOverEvent = 0x90019;
		private const int DomDragLeaveEvent = 0x9001A;
		private const int DomDragEvent = 0x9001B;
		private const int DomDropEvent = 0x9001C;
		private const int DomDragEndEvent = 0x90010D;
		#endregion

		#region Navigation Events

		#region public event EventHandler<GeckoNavigatingEventArgs> Navigating

		/// <summary>
		/// Occurs before the browser navigates to a new page.
		/// </summary>
		[Category( "Navigation" )]
		[Description( "Occurs before the browser navigates to a new page." )]
		public event EventHandler<GeckoNavigatingEventArgs> Navigating
		{
			add { _eventList.AddHandler(NavigatingEvent, value); }
			remove { _eventList.RemoveHandler(NavigatingEvent, value); }
		}

		/// <summary>Raises the <see cref="Navigating"/> event.</summary>
		/// <param name="e">The data for the event.</param>
		protected virtual void OnNavigating( GeckoNavigatingEventArgs e )
		{
			var evnt = ( ( EventHandler<GeckoNavigatingEventArgs> ) _eventList[ NavigatingEvent ] );
			if ( evnt != null ) evnt( this, e );
		}

		#endregion

		#region public event EventHandler<GeckoNavigatedEventArgs> Navigated

		/// <summary>
		/// Occurs after the browser has navigated to a new page.
		/// </summary>
		[Category( "Navigation" )]
		[Description( "Occurs after the browser has navigated to a new page." )]
		public event EventHandler<GeckoNavigatedEventArgs> Navigated
		{
			add { _eventList.AddHandler( NavigatedEvent, value ); }
			remove { _eventList.RemoveHandler( NavigatedEvent, value ); }
		}

		/// <summary>Raises the <see cref="Navigated"/> event.</summary>
		/// <param name="e">The data for the event.</param>
		protected virtual void OnNavigated( GeckoNavigatedEventArgs e )
		{
			var evnt = ( EventHandler<GeckoNavigatedEventArgs> ) _eventList[ NavigatedEvent ];
			if ( evnt != null ) evnt( this, e );
		}

		#endregion

		#region public event EventHandler<GeckoRedirectingEventArgs> Redirecting

		/// <summary>
		/// Occurs before the browser redirects to a new page.
		/// </summary>
		[Category("Navigation")]
		[Description("Occurs before the browser redirects to a new page.")]
		public event EventHandler<GeckoRedirectingEventArgs> Redirecting
		{
			add
			{
				_eventList.AddHandler(RedirectingEvent, value);
			}
			remove
			{
				_eventList.RemoveHandler(RedirectingEvent, value);
			}
		}

		/// <summary>Raises the <see cref="Redirecting"/> event.</summary>
		/// <param name="e">The data for the event.</param>
		protected virtual void OnRedirecting(GeckoRedirectingEventArgs e)
		{
			var evnt = ((EventHandler<GeckoRedirectingEventArgs>)_eventList[RedirectingEvent]);
			if (evnt != null)
				evnt(this, e);
		}

		#endregion

		#region public event EventHandler<GeckoNavigatingEventArgs> FrameNavigating

		/// <summary>
		/// Occurs before the browser navigates to a new frame.
		/// </summary>
		[Category("Navigation")]
		[Description("Occurs before the browser navigates to a new frame.")]
		public event EventHandler<GeckoNavigatingEventArgs> FrameNavigating
		{
			add { _eventList.AddHandler(FrameNavigatingEvent, value); }
			remove { _eventList.RemoveHandler(FrameNavigatingEvent, value); }
		}

		/// <summary>Raises the <see cref="FrameNavigating"/> event.</summary>
		/// <param name="e">The data for the event.</param>
		protected virtual void OnFrameNavigating(GeckoNavigatingEventArgs e)
		{
			var evnt = ((EventHandler<GeckoNavigatingEventArgs>)_eventList[FrameNavigatingEvent]);
			if (evnt != null) evnt(this, e);
		}

		#endregion


		#region public event EventHandler DocumentCompleted

		/// <summary>
		/// Occurs after the browser has finished parsing a new page and updated the <see cref="Document"/> property.
		/// </summary>
		[Category( "Navigation" )]
		[Description( "Occurs after the browser has finished parsing a new page and updated the Document property." )]
		public event EventHandler DocumentCompleted
		{
			add { _eventList.AddHandler( DocumentCompletedEvent, value ); }
			remove { _eventList.RemoveHandler( DocumentCompletedEvent, value ); }
		}

		/// <summary>Raises the <see cref="DocumentCompleted"/> event.</summary>
		/// <param name="e">The data for the event.</param>
		protected virtual void OnDocumentCompleted( EventArgs e )
		{
			var evnt = ( EventHandler ) _eventList[ DocumentCompletedEvent ];
			if ( evnt != null ) evnt( this, e );
		}

		#endregion

		#region public event EventHandler CanGoBackChanged

		/// <summary>
		/// Occurs when the value of the <see cref="CanGoBack"/> property is changed.
		/// </summary>
		[Category( "Property Changed" )]
		[Description( "Occurs when the value of the CanGoBack property is changed." )]
		public event EventHandler CanGoBackChanged
		{
			add { _eventList.AddHandler( CanGoBackChangedEvent, value ); }
			remove { _eventList.RemoveHandler( CanGoBackChangedEvent, value ); }
		}

		/// <summary>Raises the <see cref="CanGoBackChanged"/> event.</summary>
		/// <param name="e">The data for the event.</param>
		protected virtual void OnCanGoBackChanged( EventArgs e )
		{
			var evnt = ( EventHandler ) _eventList[ CanGoBackChangedEvent ];
			if ( evnt != null ) evnt( this, e );
		}

		#endregion

		#region public event EventHandler CanGoForwardChanged

		/// <summary>
		/// Occurs when the value of the <see cref="CanGoForward"/> property is changed.
		/// </summary>
		[Category( "Property Changed" )]
		[Description( "Occurs when the value of the CanGoForward property is changed." )]
		public event EventHandler CanGoForwardChanged
		{
			add { _eventList.AddHandler( CanGoForwardChangedEvent, value ); }
			remove { _eventList.RemoveHandler( CanGoForwardChangedEvent, value ); }
		}

		/// <summary>Raises the <see cref="CanGoForwardChanged"/> event.</summary>
		/// <param name="e">The data for the event.</param>
		protected virtual void OnCanGoForwardChanged( EventArgs e )
		{
			var evnt = ( EventHandler ) _eventList[ CanGoForwardChangedEvent ];
			if ( evnt != null ) evnt( this, e );
		}

		#endregion

		#endregion

		#region History Events

		#region public event EventHandler<GeckoHistoryEventArgs> HistoryNewEntry

		[Category( "History" )]
		public event EventHandler<GeckoHistoryEventArgs> HistoryNewEntry
		{
			add { _eventList.AddHandler( HistoryNewEntryEvent, value ); }
			remove { _eventList.RemoveHandler( HistoryNewEntryEvent, value ); }
		}

		/// <summary>Raises the <see cref="HistoryNewEntry"/> event.</summary>
		/// <param name="e">The data for the event.</param>
		protected virtual void OnHistoryNewEntry( GeckoHistoryEventArgs e )
		{
			var evnt = ( EventHandler<GeckoHistoryEventArgs> ) _eventList[ HistoryNewEntryEvent ];
			if ( evnt != null ) evnt( this, e );
		}

		#endregion

		#region public event EventHandler<GeckoHistoryEventArgs> HistoryGoBack

		[Category( "History" )]
		public event EventHandler<GeckoHistoryEventArgs> HistoryGoBack
		{
			add { _eventList.AddHandler( HistoryGoBackEvent, value ); }
			remove { _eventList.RemoveHandler( HistoryGoBackEvent, value ); }
		}

		/// <summary>Raises the <see cref="HistoryGoBack"/> event.</summary>
		/// <param name="e">The data for the event.</param>
		protected virtual void OnHistoryGoBack( GeckoHistoryEventArgs e )
		{
			var evnt = ( EventHandler<GeckoHistoryEventArgs> ) _eventList[ HistoryGoBackEvent ];
			if ( evnt != null ) evnt( this, e );
		}

		#endregion

		#region public event EventHandler<GeckoHistoryEventArgs> HistoryGoForward

		[Category( "History" )]
		public event EventHandler<GeckoHistoryEventArgs> HistoryGoForward
		{
			add { _eventList.AddHandler( HistoryGoForwardEvent, value ); }
			remove { _eventList.RemoveHandler( HistoryGoForwardEvent, value ); }
		}

		/// <summary>Raises the <see cref="HistoryGoForward"/> event.</summary>
		/// <param name="e">The data for the event.</param>
		protected virtual void OnHistoryGoForward( GeckoHistoryEventArgs e )
		{
			var evnt = ( EventHandler<GeckoHistoryEventArgs> ) _eventList[ HistoryGoForwardEvent ];
			if ( evnt != null ) evnt( this, e );
		}

		#endregion

		#region public event EventHandler<GeckoHistoryEventArgs> HistoryReload

		[Category( "History" )]
		public event EventHandler<GeckoHistoryEventArgs> HistoryReload
		{
			add { _eventList.AddHandler( HistoryReloadEvent, value ); }
			remove { _eventList.RemoveHandler( HistoryReloadEvent, value ); }
		}

		/// <summary>Raises the <see cref="HistoryReload"/> event.</summary>
		/// <param name="e">The data for the event.</param>
		protected virtual void OnHistoryReload( GeckoHistoryEventArgs e )
		{
			var evnt = ( EventHandler<GeckoHistoryEventArgs> ) _eventList[ HistoryReloadEvent ];
			if ( evnt != null ) evnt( this, e );
		}

		#endregion

		#region public event GeckoHistoryGotoIndexEventHandler HistoryGotoIndex

		[Category( "History" )]
		public event EventHandler<GeckoHistoryGotoIndexEventArgs> HistoryGotoIndex
		{
			add { _eventList.AddHandler( HistoryGotoIndexEvent, value ); }
			remove { _eventList.RemoveHandler( HistoryGotoIndexEvent, value ); }
		}

		/// <summary>Raises the <see cref="HistoryGotoIndex"/> event.</summary>
		/// <param name="e">The data for the event.</param>
		protected virtual void OnHistoryGotoIndex( GeckoHistoryGotoIndexEventArgs e )
		{
			var evnt = ( EventHandler<GeckoHistoryGotoIndexEventArgs> ) _eventList[ HistoryGotoIndexEvent ];
			if ( evnt != null ) evnt( this, e );
		}

		#endregion

		#region public event GeckoHistoryPurgeEventHandler HistoryPurge

		[Category( "History" )]
		public event EventHandler<GeckoHistoryPurgeEventArgs> HistoryPurge
		{
			add { _eventList.AddHandler( HistoryPurgeEvent, value ); }
			remove { _eventList.RemoveHandler( HistoryPurgeEvent, value ); }
		}

		/// <summary>Raises the <see cref="HistoryPurge"/> event.</summary>
		/// <param name="e">The data for the event.</param>
		protected virtual void OnHistoryPurge( GeckoHistoryPurgeEventArgs e )
		{
			var evnt = ( EventHandler<GeckoHistoryPurgeEventArgs> ) _eventList[ HistoryPurgeEvent ];
			if ( evnt != null ) evnt( this, e );
		}

		#endregion

		#endregion

		#region public event GeckoRequestProgressEventHandler ProgressChanged

		/// <summary>
		/// Occurs when the control has updated progress information.
		/// </summary>
		[Category("Navigation")]
		[Description("Occurs when the Request has updated progress information.")]
		public event EventHandler<GeckoRequestProgressEventArgs> RequestProgressChanged
		{
			add
			{
				_eventList.AddHandler(RequestProgressChangedEvent, value);
			}
			remove
			{
				_eventList.RemoveHandler(RequestProgressChangedEvent, value);
			}
		}

		/// <summary>Raises the <see cref="ProgressChanged"/> event.</summary>
		/// <param name="e">The data for the event.</param>
		protected virtual void OnRequestProgressChanged(GeckoRequestProgressEventArgs e)
		{
			var evnt = (EventHandler<GeckoRequestProgressEventArgs>)_eventList[RequestProgressChangedEvent];
			if (evnt != null)
				evnt(this, e);
		}

		#endregion

		#region public event GeckoRequestProgressEventHandler ProgressChanged

		/// <summary>
		/// Occurs when the control has updated progress information.
		/// </summary>
		[Category( "Navigation" )]
		[Description( "Occurs when the control has updated progress information." )]
		public event EventHandler<GeckoProgressEventArgs> ProgressChanged
		{
			add { _eventList.AddHandler( ProgressChangedEvent, value ); }
			remove { _eventList.RemoveHandler( ProgressChangedEvent, value ); }
		}


		/// <summary>Raises the <see cref="ProgressChanged"/> event.</summary>
		/// <param name="e">The data for the event.</param>
		protected virtual void OnProgressChanged( GeckoProgressEventArgs e )
		{
			var evnt = ( EventHandler<GeckoProgressEventArgs> ) _eventList[ ProgressChangedEvent ];
			if ( evnt != null ) evnt( this, e );
		}

		#endregion

		#region Window Events

		#region public event GeckoCreateWindowEventHandler CreateWindow

		public event EventHandler<GeckoCreateWindowEventArgs> CreateWindow
		{
			add { _eventList.AddHandler( CreateWindowEvent, value ); }
			remove { _eventList.RemoveHandler( CreateWindowEvent, value ); }
		}

		/// <summary>Raises the <see cref="CreateWindow"/> event.</summary>
		/// <param name="e">The data for the event.</param>
		protected virtual void OnCreateWindow( GeckoCreateWindowEventArgs e )
		{
			var evnt = ( EventHandler<GeckoCreateWindowEventArgs> ) _eventList[ CreateWindowEvent ];
			if ( evnt != null ) evnt( this, e );
		}

		#endregion

		#region public event GeckoCreateWindow2EventHandler CreateWindow2

		public event EventHandler<GeckoCreateWindow2EventArgs> CreateWindow2
		{
			add { _eventList.AddHandler( CreateWindow2Event, value ); }
			remove { _eventList.RemoveHandler( CreateWindow2Event, value ); }
		}

		/// <summary>Raises the <see cref="CreateWindow"/> event.</summary>
		/// <param name="e">The data for the event.</param>
		protected virtual void OnCreateWindow2( GeckoCreateWindow2EventArgs e )
		{
			var evnt = ( EventHandler<GeckoCreateWindow2EventArgs> ) _eventList[ CreateWindow2Event ];
			if ( evnt != null ) evnt( this, e );
		}

		#endregion

		#region public event GeckoWindowSetBoundsEventHandler WindowSetBounds



		public event EventHandler<GeckoWindowSetBoundsEventArgs> WindowSetBounds
		{
			add { _eventList.AddHandler( WindowSetBoundsEvent, value ); }
			remove { _eventList.RemoveHandler( WindowSetBoundsEvent, value ); }
		}

		/// <summary>Raises the <see cref="WindowSetBounds"/> event.</summary>
		/// <param name="e">The data for the event.</param>
		protected virtual void OnWindowSetBounds( GeckoWindowSetBoundsEventArgs e )
		{
			var evnt = (EventHandler<GeckoWindowSetBoundsEventArgs>)_eventList[WindowSetBoundsEvent];
			if ( evnt != null ) evnt( this, e );
		}

		#endregion


		#region public event EventHandler WindowClosed
		public event EventHandler WindowClosed
		{
			add { _eventList.AddHandler(WindowClosedEvent, value); }
			remove { _eventList.RemoveHandler(WindowClosedEvent, value); }
		}
		

		/// <summary>Raises the <see cref="WindowClosed"/> event.</summary>
		/// <param name="e">The data for the event.</param>
		protected virtual void OnWindowClosed(EventArgs e)
		{
			var evnt = ( EventHandler ) _eventList[ WindowClosedEvent ];
			if (evnt != null) evnt(this, e);
		}
		#endregion
		#endregion

		#region public event EventHandler StatusTextChanged

		/// <summary>
		/// Occurs when the value of the <see cref="StatusText"/> property is changed.
		/// </summary>
		[Category( "Property Changed" )]
		[Description( "Occurs when the value of the StatusText property is changed." )]
		public event EventHandler StatusTextChanged
		{
			add { _eventList.AddHandler( StatusTextChangedEvent, value ); }
			remove { _eventList.RemoveHandler( StatusTextChangedEvent, value ); }
		}

		/// <summary>Raises the <see cref="StatusTextChanged"/> event.</summary>
		/// <param name="e">The data for the event.</param>
		protected virtual void OnStatusTextChanged( EventArgs e )
		{
			var evnt = ( EventHandler ) _eventList[ StatusTextChangedEvent ];
			if ( evnt != null ) evnt( this, e );
		}

		#endregion

		#region public event EventHandler DocumentTitleChanged

		/// <summary>
		/// Occurs when the value of the <see cref="DocumentTitle"/> property is changed.
		/// </summary>
		[Category( "Property Changed" )]
		[Description( "Occurs when the value of the DocumentTitle property is changed." )]
		public event EventHandler DocumentTitleChanged
		{
			add { _eventList.AddHandler( DocumentTitleChangedEvent, value ); }
			remove { _eventList.RemoveHandler( DocumentTitleChangedEvent, value ); }
		}

		/// <summary>Raises the <see cref="DocumentTitleChanged"/> event.</summary>
		/// <param name="e">The data for the event.</param>
		protected virtual void OnDocumentTitleChanged( EventArgs e )
		{
			var evnt = ( EventHandler ) _eventList[ DocumentTitleChangedEvent ];
			if ( evnt != null ) evnt( this, e );
		}

		#endregion

		#region public event GeckoContextMenuEventHandler ShowContextMenu

		public event EventHandler<GeckoContextMenuEventArgs> ShowContextMenu
		{
			add { _eventList.AddHandler( ShowContextMenuEvent, value ); }
			remove { _eventList.RemoveHandler( ShowContextMenuEvent, value ); }
		}

		/// <summary>Raises the <see cref="ShowContextMenu"/> event.</summary>
		/// <param name="e">The data for the event.</param>
		protected virtual void OnShowContextMenu( GeckoContextMenuEventArgs e )
		{
			var evnt = ( EventHandler<GeckoContextMenuEventArgs> ) _eventList[ ShowContextMenuEvent ];
			if ( evnt != null ) evnt( this, e );
		}

		#endregion

		#region public event GeckoObserveHttpModifyRequestEventHandler Navigated

		/// <summary>
		/// Occurs after the browser has send a http request to the web
		/// </summary>
		[Category( "Observe" )]
		[Description( "Occurs after the browser has navigated to a new page." )]
		public event EventHandler<GeckoObserveHttpModifyRequestEventArgs> ObserveHttpModifyRequest
		{
			add { _eventList.AddHandler( ObserveHttpModifyRequestEvent, value ); }
			remove { _eventList.RemoveHandler( ObserveHttpModifyRequestEvent, value ); }
		}

		/// <summary>Raises the <see cref="ObserveHttpModify"/> event.</summary>
		/// <param name="e">The data for the event.</param>
		protected virtual void OnObserveHttpModifyRequest(GeckoObserveHttpModifyRequestEventArgs e)
		{
			var evnt = ( EventHandler<GeckoObserveHttpModifyRequestEventArgs> ) _eventList[ ObserveHttpModifyRequestEvent ];
			if ( evnt != null ) evnt( this, e );
		}

		#endregion

		#region Dom EventHandlers

		#region Dom keyboard _eventList
		#region public event GeckoDomKeyEventHandler DomKeyDown
		[Category("DOM Events")]
		public event EventHandler<DomKeyEventArgs> DomKeyDown
		{
			add { _eventList.AddHandler(DomKeyDownEvent, value); }
			remove { _eventList.RemoveHandler(DomKeyDownEvent, value); }
		}

		/// <summary>Raises the <see cref="DomKeyDown"/> event.</summary>
		/// <param name="e">The data for the event.</param>
		protected virtual void OnDomKeyDown(DomKeyEventArgs e)
		{
			var evnt = (EventHandler<DomKeyEventArgs>)_eventList[DomKeyDownEvent];
			if (evnt != null) evnt(this, e);
		}
		#endregion

		#region public event GeckoDomKeyEventHandler DomKeyUp
		[Category("DOM Events")]
		public event EventHandler<DomKeyEventArgs> DomKeyUp
		{
			add { _eventList.AddHandler(DomKeyUpEvent, value); }
			remove { _eventList.RemoveHandler(DomKeyUpEvent, value); }
		}

		/// <summary>Raises the <see cref="DomKeyUp"/> event.</summary>
		/// <param name="e">The data for the event.</param>
		protected virtual void OnDomKeyUp(DomKeyEventArgs e)
		{
			var evnt = (EventHandler<DomKeyEventArgs>)_eventList[DomKeyUpEvent];
			if (evnt != null) evnt(this, e);
		}
		#endregion

		#region public event GeckoDomKeyEventHandler DomKeyPress
		[Category("DOM Events")]
		public event EventHandler<DomKeyEventArgs> DomKeyPress
		{
			add { _eventList.AddHandler(DomKeyPressEvent, value); }
			remove { _eventList.RemoveHandler(DomKeyPressEvent, value); }
		}

		/// <summary>Raises the <see cref="DomKeyPress"/> event.</summary>
		/// <param name="e">The data for the event.</param>
		protected virtual void OnDomKeyPress(DomKeyEventArgs e)
		{
			var evnt = (EventHandler<DomKeyEventArgs>)_eventList[DomKeyPressEvent];
			if (evnt != null) evnt(this, e);
		}
		#endregion
		#endregion

		#region Dom mouse Events
		#region public event GeckoDomMouseEventHandler DomMouseDown
		[Category("DOM Events")]
		public event EventHandler<DomMouseEventArgs> DomMouseDown
		{
			add { _eventList.AddHandler(DomMouseDownEvent, value); }
			remove { _eventList.RemoveHandler(DomMouseDownEvent, value); }
		}
		
		/// <summary>Raises the <see cref="DomMouseDown"/> event.</summary>
		/// <param name="e">The data for the event.</param>
		protected virtual void OnDomMouseDown(DomMouseEventArgs e)
		{
			var evnt = (EventHandler<DomMouseEventArgs>)_eventList[DomMouseDownEvent];
			if (evnt != null) evnt(this, e);
		}
		#endregion

		#region public event GeckoDomMouseEventHandler DomMouseUp
		[Category("DOM Events")]
		public event EventHandler<DomMouseEventArgs> DomMouseUp
		{
			add { _eventList.AddHandler(DomMouseUpEvent, value); }
			remove { _eventList.RemoveHandler(DomMouseUpEvent, value); }
		}

		/// <summary>Raises the <see cref="DomMouseUp"/> event.</summary>
		/// <param name="e">The data for the event.</param>
		protected virtual void OnDomMouseUp(DomMouseEventArgs e)
		{
			var evnt = (EventHandler<DomMouseEventArgs>)_eventList[DomMouseUpEvent];
			if (evnt != null) evnt(this, e);
		}
		#endregion

		#region public event GeckoDomMouseEventHandler DomMouseOver
		[Category("DOM Events")]
		public event EventHandler<DomMouseEventArgs> DomMouseOver
		{
			add { _eventList.AddHandler(DomMouseOverEvent, value); }
			remove { _eventList.RemoveHandler(DomMouseOverEvent, value); }
		}
		

		/// <summary>Raises the <see cref="DomMouseOver"/> event.</summary>
		/// <param name="e">The data for the event.</param>
		protected virtual void OnDomMouseOver(DomMouseEventArgs e)
		{
			var evnt = (EventHandler<DomMouseEventArgs>)this._eventList[DomMouseOverEvent];
			if (evnt != null) evnt(this, e);
		}
		#endregion

		#region public event GeckoDomMouseEventHandler DomMouseOut
		[Category("DOM Events")]
		public event EventHandler<DomMouseEventArgs> DomMouseOut
		{
			add { _eventList.AddHandler(DomMouseOutEvent, value); }
			remove { _eventList.RemoveHandler(DomMouseOutEvent, value); }
		}
		
		/// <summary>Raises the <see cref="DomMouseOut"/> event.</summary>
		/// <param name="e">The data for the event.</param>
		protected virtual void OnDomMouseOut(DomMouseEventArgs e)
		{
			var evnt = (EventHandler<DomMouseEventArgs>)_eventList[DomMouseOutEvent];
			if (evnt != null) evnt( this, e );
		}
		#endregion

		#region public event GeckoDomMouseEventHandler DomMouseMove
		[Category("DOM Events")]
		public event EventHandler<DomMouseEventArgs> DomMouseMove
		{
			add { _eventList.AddHandler(DomMouseMoveEvent, value); }
			remove { _eventList.RemoveHandler(DomMouseMoveEvent, value); }
		}
		
		/// <summary>Raises the <see cref="DomMouseMove"/> event.</summary>
		/// <param name="e">The data for the event.</param>
		protected virtual void OnDomMouseMove(DomMouseEventArgs e)
		{
			var evnt = (EventHandler<DomMouseEventArgs>)_eventList[DomMouseMoveEvent];
			if (evnt != null) evnt(this, e);
		}
		#endregion

		#region public event GeckoDomMouseEventHandler DomContextMenu
		[Category("DOM Events")]
		public event EventHandler<DomMouseEventArgs> DomContextMenu
		{
			add { _eventList.AddHandler(DomContextMenuEvent, value); }
			remove { _eventList.RemoveHandler(DomContextMenuEvent, value); }
		}
		
		/// <summary>Raises the <see cref="DomContextMenu"/> event.</summary>
		/// <param name="e">The data for the event.</param>
		protected virtual void OnDomContextMenu(DomMouseEventArgs e)
		{
			var evnt = (EventHandler<DomMouseEventArgs>)_eventList[DomContextMenuEvent];
			if (evnt != null)evnt(this, e);
		}
		#endregion

		#region public event GeckoDomMouseEventHandler DOMMouseScroll
		[Category("DOM Events")]
		public event EventHandler<DomMouseEventArgs> DomMouseScroll
		{
			add { _eventList.AddHandler(DomMouseScrollEvent, value); }
			remove { _eventList.RemoveHandler(DomMouseScrollEvent, value); }
		}

		/// <summary>Raises the <see cref="DomMouseScroll"/> event.</summary>
		/// <param name="e">The data for the event.</param>
		protected virtual void OnDomMouseScroll(DomMouseEventArgs e)
		{
			var evnt = (EventHandler<DomMouseEventArgs>)_eventList[DomMouseScrollEvent];
			if (evnt != null) evnt(this, e);
		}
		#endregion
		#endregion

		#region public event GeckoDomEventHandler DomSubmit
		[Category("DOM Events")]
		public event EventHandler<DomEventArgs> DomSubmit
		{
			add { _eventList.AddHandler(DomSubmitEvent, value); }
			remove { _eventList.RemoveHandler(DomSubmitEvent, value); }
		}
		
		/// <summary>Raises the <see cref="DomSubmit"/> event.</summary>
		/// <param name="e">The data for the event.</param>
		protected virtual void OnDomSubmit(DomEventArgs e)
		{
			var evnt = (EventHandler<DomEventArgs>)_eventList[DomSubmitEvent];
			if (evnt != null) evnt(this, e);
		}
		#endregion

		#region public event GeckoDomEventHandler DomCompositionStart
		[Category("DOM Events")]
		public event EventHandler<DomEventArgs> DomCompositionStart
		{
			add { _eventList.AddHandler(DomCompositionStartEvent, value); }
			remove { _eventList.RemoveHandler(DomCompositionStartEvent, value); }
		}
		
		/// <summary>Raises the <see cref="DomCompositionStart"/> event.</summary>
		/// <param name="e">The data for the event.</param>
		protected virtual void OnDomCompositionStart(DomEventArgs e)
		{
			var evnt = (EventHandler<DomEventArgs>)_eventList[DomCompositionStartEvent];
			if (evnt != null) evnt(this, e);
		}
		#endregion

		#region public event GeckoDomEventHandler DomCompositionEnd
		[Category("DOM Events")]
		public event EventHandler<DomEventArgs> DomCompositionEnd
		{
			add { _eventList.AddHandler(DomCompositionEndEvent, value); }
			remove { _eventList.RemoveHandler(DomCompositionEndEvent, value); }
		}
		
		/// <summary>Raises the <see cref="DomCompositionEnd"/> event.</summary>
		/// <param name="e">The data for the event.</param>
		protected virtual void OnDomCompositionEnd(DomEventArgs e)
		{
			var evnt = (EventHandler<DomEventArgs>)_eventList[DomCompositionEndEvent];
			if (evnt != null) evnt(this, e);
		}
		#endregion

		#region public event GeckoDomEventHandler DomFocus
		[Category("DOM Events")]
		public event EventHandler<DomEventArgs> DomFocus
		{
			add { _eventList.AddHandler(DomFocusEvent, value); }
			remove { _eventList.RemoveHandler(DomFocusEvent, value); }
		}
		
		/// <summary>Raises the <see cref="DomFocus"/> event.</summary>
		/// <param name="e">The data for the event.</param>
		protected virtual void OnDomFocus(DomEventArgs e)
		{
			var evnt = (EventHandler<DomEventArgs>)_eventList[DomFocusEvent];
			if (evnt != null) evnt(this, e);
		}
		#endregion

		#region public event GeckoDomEventHandler DomBlur
		[Category("DOM Events")]
		public event EventHandler<DomEventArgs> DomBlur
		{
			add { _eventList.AddHandler(DomBlurEvent, value); }
			remove { _eventList.RemoveHandler(DomBlurEvent, value); }
		}

		/// <summary>Raises the <see cref="DomBlur"/> event.</summary>
		/// <param name="e">The data for the event.</param>
		protected virtual void OnDomBlur(DomEventArgs e)
		{
			var evnt = (EventHandler<DomEventArgs>)_eventList[DomBlurEvent];
			if (evnt != null) evnt(this, e);
		}
		#endregion

		#region public event GeckoDomEventHandler Load
		[Category("DOM Events")]
		public event EventHandler<DomEventArgs> Load
		{
			add { _eventList.AddHandler(LoadEvent, value); }
			remove { _eventList.RemoveHandler(LoadEvent, value); }
		}
		
		/// <summary>Raises the <see cref="LoadEvent"/> event.</summary>
		/// <param name="e">The data for the event.</param>
		protected virtual void OnLoad(DomEventArgs e)
		{
			var evnt = (EventHandler<DomEventArgs>)_eventList[LoadEvent];
			if (evnt != null) evnt(this, e);
		}
		#endregion

		#region public event GeckoDomEventHandler DOMContentLoaded
		[Category("DOM Events")]
		public event EventHandler<DomEventArgs> DOMContentLoaded {
			add {
				_eventList.AddHandler(DOMContentLoadedEvent, value);
			}
			remove {
				_eventList.RemoveHandler(DOMContentLoadedEvent, value);
			}
		}

		/// <summary>Raises the <see cref="LoadEvent"/> event.</summary>
		/// <param name="e">The data for the event.</param>
		protected virtual void OnDOMContentLoaded(DomEventArgs e) {
			var evnt = (EventHandler<DomEventArgs>)_eventList[DOMContentLoadedEvent];
			if (evnt != null)
				evnt(this, e);
		}
		#endregion

		#region public event GeckoDomEventHandler ReadyStateChange
		[Category("DOM Events")]
		public event EventHandler<DomEventArgs> ReadyStateChange {
			add {
				_eventList.AddHandler(ReadyStateChangeEvent, value);
			}
			remove {
				_eventList.RemoveHandler(ReadyStateChangeEvent, value);
			}
		}

		/// <summary>Raises the <see cref="LoadEvent"/> event.</summary>
		/// <param name="e">The data for the event.</param>
		protected virtual void OnReadyStateChange(DomEventArgs e) {
			var evnt = (EventHandler<DomEventArgs>)_eventList[ReadyStateChangeEvent];
			if (evnt != null)
				evnt(this, e);
		}
		#endregion

        #region public event GeckoDomEventHandler HashChange
        [Category("DOM Events")]
		public event EventHandler<DomHashChangeEventArgs> HashChange
        {
            add { _eventList.AddHandler(HashChangeEvent, value); }
            remove { _eventList.RemoveHandler(HashChangeEvent, value); }
        }

        /// <summary>Raises the <see cref="HashChangeEvent"/> event.</summary>
        /// <param name="e">The data for the event.</param>
		protected virtual void OnHashChange(DomHashChangeEventArgs e)
        {
            var evnt = (EventHandler<DomHashChangeEventArgs>)_eventList[HashChangeEvent];
            if (evnt != null) evnt(this, e);
        }
        #endregion

		#region drag _eventList

		// DragStart

		public event EventHandler<DomDragEventArgs> DomDragStart
		{
			add { _eventList.AddHandler(DomDragStartEvent, value); }
			remove { _eventList.RemoveHandler(DomDragStartEvent, value); }
		}

		/// <summary>Raises the <see cref="DomDragStart"/> event.</summary>
		/// <param name="e">The data for the event.</param>
		protected virtual void OnDomDragStart(DomDragEventArgs e)
		{
			var evnt = (EventHandler<DomDragEventArgs>)_eventList[DomDragStartEvent];
			if (evnt != null) evnt(this, e);
		}

		// DragEnter

		public event EventHandler<DomDragEventArgs> DomDragEnter
		{
			add { _eventList.AddHandler(DomDragEnterEvent, value); }
			remove { _eventList.RemoveHandler(DomDragEnterEvent, value); }
		}

		/// <summary>Raises the <see cref="DomDragEnter"/> event.</summary>
		/// <param name="e">The data for the event.</param>
		protected virtual void OnDomDragEnter(DomDragEventArgs e)
		{
			var evnt = (EventHandler<DomDragEventArgs>)_eventList[DomDragEnterEvent];
			if (evnt != null) evnt(this, e);
		}

		// DragOver

		public event EventHandler<DomDragEventArgs> DomDragOver
		{
			add { _eventList.AddHandler(DomDragOverEvent, value); }
			remove { _eventList.RemoveHandler(DomDragOverEvent, value); }
		}

		/// <summary>Raises the <see cref="DomDragOver"/> event.</summary>
		/// <param name="e">The data for the event.</param>
		protected virtual void OnDomDragOver(DomDragEventArgs e)
		{
			var evnt = (EventHandler<DomDragEventArgs>)_eventList[DomDragOverEvent];
			if (evnt != null) evnt(this, e);
		}

		// DragLeave

		public event EventHandler<DomDragEventArgs> DomDragLeave
		{
			add { _eventList.AddHandler(DomDragLeaveEvent, value); }
			remove { _eventList.RemoveHandler(DomDragLeaveEvent, value); }
		}

		/// <summary>Raises the <see cref="DomDragLeave"/> event.</summary>
		/// <param name="e">The data for the event.</param>
		protected virtual void OnDomDragLeave(DomDragEventArgs e)
		{
			var evnt = (EventHandler<DomDragEventArgs>)_eventList[DomDragLeaveEvent];
			if (evnt != null) evnt(this, e);
		}

		// Drag

		public event EventHandler<DomDragEventArgs> DomDrag
		{
			add { _eventList.AddHandler(DomDragEvent, value); }
			remove { _eventList.RemoveHandler(DomDragEvent, value); }
		}

		/// <summary>Raises the <see cref="DomDrag"/> event.</summary>
		/// <param name="e">The data for the event.</param>
		protected virtual void OnDomDrag(DomDragEventArgs e)
		{
			var evnt = (EventHandler<DomDragEventArgs>)_eventList[DomDragEvent];
			if (evnt != null) evnt(this, e);
		}

		// Drop

		public event EventHandler<DomDragEventArgs> DomDrop
		{
			add { _eventList.AddHandler(DomDropEvent, value); }
			remove { _eventList.RemoveHandler(DomDropEvent, value); }
		}

		/// <summary>Raises the <see cref="DomDrop"/> event.</summary>
		/// <param name="e">The data for the event.</param>
		protected virtual void OnDomDrop(DomDragEventArgs e)
		{
			var evnt = (EventHandler<DomDragEventArgs>)_eventList[DomDropEvent];
			if (evnt != null) evnt(this, e);
		}

		// DragEnd

		public event EventHandler<DomDragEventArgs> DomDragEnd
		{
			add { _eventList.AddHandler(DomDragEndEvent, value); }
			remove { _eventList.RemoveHandler(DomDragEndEvent, value); }
		}

		/// <summary>Raises the <see cref="DomDragEnd"/> event.</summary>
		/// <param name="e">The data for the event.</param>
		protected virtual void OnDomDragEnd(DomDragEventArgs e)
		{
			var evnt = (EventHandler<DomDragEventArgs>)_eventList[DomDragEndEvent];
			if (evnt != null) evnt(this, e);
		}

		#endregion

		#region public event GeckoDomEventHandler DomContentChanged
		[Category("DOM Events")]
		public event EventHandler<DomEventArgs> DomContentChanged
		{
			add { _eventList.AddHandler(DomContentChangedEvent, value); }
			remove { _eventList.RemoveHandler(DomContentChangedEvent, value); }
		}

		/// <summary>Raises the <see cref="DomContentChanged"/> event.</summary>
		/// <param name="e">The data for the event.</param>
		protected virtual void OnDomContentChanged(DomEventArgs e)
		{
			var evnt = (EventHandler<DomEventArgs>)_eventList[DomContentChangedEvent];
			if (evnt != null) evnt(this, e);
		}
		#endregion

		#region public event GeckoDomEventHandler DomClick
		[Category("DOM Events")]
		public event EventHandler<DomEventArgs> DomClick
		{
			add { _eventList.AddHandler(DomClickEvent, value); }
			remove { _eventList.RemoveHandler(DomClickEvent, value); }
		}
		
		/// <summary>Raises the <see cref="DomClick"/> event.</summary>
		/// <param name="e">The data for the event.</param>
		protected virtual void OnDomClick(DomEventArgs e)
		{
			var evnt = (EventHandler<DomEventArgs>)_eventList[DomClickEvent];
			if (evnt != null) evnt(this, e);
		}
		#endregion

		#region public event GeckoDomEventHandler DomDoubleClick

		[Category("DOM Events")]
		public event EventHandler<DomEventArgs> DomDoubleClick
		{
			add { _eventList.AddHandler(DomDoubleClickEvent, value); }
			remove { _eventList.RemoveHandler(DomDoubleClickEvent, value); }
		}
		
		/// <summary>Raises the <see cref="DomDoubleClick"/> event.</summary>
		/// <param name="e">The data for the event.</param>
		protected virtual void OnDomDoubleClick(DomEventArgs e)
		{
			var evnt = (EventHandler<DomEventArgs>)_eventList[DomDoubleClickEvent];
			if (evnt != null) evnt(this, e);
		}

		#endregion public event GeckoDomEventHandler DomDoubleClick

		#endregion

		#region event JavascriptErrorEventHandler JavascriptError

		internal class JSErrorHandler : jsdIErrorHook
		{
			GeckoWebBrowser m_browser;

			internal JSErrorHandler(GeckoWebBrowser browser)
			{
				m_browser = browser;
			}

			public bool OnError(nsAUTF8StringBase message, nsAUTF8StringBase fileName, uint line, uint pos, uint flags, uint errnum, jsdIValue exc)
			{
				if (m_browser.IsDisposed) return true;
				var eventArgs = new JavascriptErrorEventArgs(message.ToString(), fileName.ToString(), line, pos, flags, errnum);
				m_browser.OnJavascriptError(eventArgs);
				return true;
			}
		}

		public void EnableJavascriptDebugger()
		{
			if (m_javascriptDebuggingEnabled)
				return;

			//using (var a = new AutoJSContext(JSContext))
			{
				using (var jsd = Xpcom.GetService2<jsdIDebuggerService>("@mozilla.org/js/jsd/debugger-service;1"))
				{
					jsd.Instance.SetErrorHookAttribute( new JSErrorHandler( this ) );
					using (var runtime = Xpcom.GetService2<nsIJSRuntimeService>("@mozilla.org/js/xpc/RuntimeService;1"))
					{
						jsd.Instance.ActivateDebugger( runtime.Instance.GetRuntimeAttribute() );
					}
				}
			}
			m_javascriptDebuggingEnabled = true;
		}

		public delegate void JavascriptErrorEventHandler(object sender, JavascriptErrorEventArgs e);

		private JavascriptErrorEventHandler _JavascriptError;

		public event JavascriptErrorEventHandler JavascriptError
		{
			add
			{
				EnableJavascriptDebugger();
				_JavascriptError += value;
			}
			remove { _JavascriptError -= value; }
		}

		protected virtual void OnJavascriptError(JavascriptErrorEventArgs e)
		{
			if (_JavascriptError != null)
				_JavascriptError(this, e);
		}

		#endregion

		#region event EventHandler<ConsoleMessageEventArgs> ConsoleMessage

		public sealed class ConsoleListener
			: nsIConsoleListener
		{
			GeckoWebBrowser m_browser;

			public ConsoleListener(GeckoWebBrowser browser)
			{
				m_browser = browser;
			}

			public void Observe(nsIConsoleMessage aMessage)
			{
				if (m_browser.IsDisposed) return;
				var e = new ConsoleMessageEventArgs(aMessage.GetMessageAttribute());
				m_browser.OnConsoleMessage(e);
			}
		}

		public void EnableConsoleMessageNotfication()
		{
			using (var consoleService = Xpcom.GetService2<nsIConsoleService>(Contracts.ConsoleService)) 
			{
				consoleService.Instance.RegisterListener(new ConsoleListener(this));
			}			
		}

		private EventHandler<ConsoleMessageEventArgs> _ConsoleMessage;

		public event EventHandler<ConsoleMessageEventArgs> ConsoleMessage
		{
			add
			{
				EnableConsoleMessageNotfication();
				_ConsoleMessage += value;
			}
			remove { _ConsoleMessage -= value; }
		}


		protected virtual void OnConsoleMessage(ConsoleMessageEventArgs e)
		{
			if (_ConsoleMessage != null)
				_ConsoleMessage(this, e);
		}

		#endregion
	}

	#region EventArgs Classes

	#region GeckoHistoryEventArgs
	/// <summary>Provides data for event.</summary>
	public class GeckoHistoryEventArgs
		: CancelEventArgs
	{
		/// <summary>
		/// Gets the URL of the history entry.
		/// </summary>
		public readonly Uri Url;

		/// <summary>Creates a new instance of a <see cref="GeckoHistoryEventArgs"/> object.</summary>
		/// <param name="url"></param>
		public GeckoHistoryEventArgs(Uri url)
		{
			Url = url;
		}
	}
	#endregion

	#region GeckoHistoryGotoIndexEventArgs

	/// <summary>Provides data for event.</summary>
	public class GeckoHistoryGotoIndexEventArgs
		: GeckoHistoryEventArgs
	{
		/// <summary>
		/// Gets the index in history of the document to be loaded.
		/// </summary>
		public readonly int Index;

		/// <summary>Creates a new instance of a <see cref="GeckoHistoryGotoIndexEventArgs"/> object.</summary>
		/// <param name="url"></param>
		/// <param name="index"></param>
		public GeckoHistoryGotoIndexEventArgs(Uri url, int index)
			: base(url)
		{
			Index = index;
		}
	}
	#endregion

	#region GeckoHistoryPurgeEventArgs
	/// <summary>Provides data for event.</summary>
	public class GeckoHistoryPurgeEventArgs
		: CancelEventArgs
	{
		/// <summary>
		/// Gets the number of entries to be purged from the history.
		/// </summary>
		public readonly int Count;

		/// <summary>Creates a new instance of a <see cref="GeckoHistoryPurgeEventArgs"/> object.</summary>
		/// <param name="count"></param>
		public GeckoHistoryPurgeEventArgs(int count)
		{
			Count = count;
		}

	}
	#endregion

	#region GeckoRequestProgressEventArgs
	/// <summary>Provides data for event.</summary>
	public class GeckoRequestProgressEventArgs
		: EventArgs
	{
		private nsIRequest _request;
		private GeckoResponse _reqWrapper;

		public readonly long CurrentProgress;
		public readonly long MaximumProgress;
		/// <summary>Creates a new instance of a <see cref="GeckoRequestProgressEventArgs"/> object.</summary>
		public GeckoRequestProgressEventArgs(long current, long max, nsIRequest req)
		{
			CurrentProgress = current;
			MaximumProgress = max;
			_request = req;
		}

		public GeckoResponse Request
		{
			get
			{
				return _reqWrapper ?? (_reqWrapper = new GeckoResponse(_request));
			}
		}
	}
	#endregion

	#region GeckoProgressEventArgs
	/// <summary>Provides data for  event.</summary>
	public class GeckoProgressEventArgs
		: EventArgs
	{
		public readonly long CurrentProgress;
		public readonly long MaximumProgress;
		/// <summary>Creates a new instance of a <see cref="GeckoProgressEventArgs"/> object.</summary>
		public GeckoProgressEventArgs(long current, long max)
		{
			CurrentProgress = current;
			MaximumProgress = max;
		}
	}
	#endregion

	#region GeckoNavigatedEventArgs
	/// <summary>Provides data for event.</summary>
	public class GeckoNavigatedEventArgs
		: EventArgs
	{
		// Wrapper is not often needed, so store only nsIRequest
		private nsIRequest _response;
		private GeckoResponse _wrapper;

		public readonly GeckoWindow DomWindow;
		public readonly Boolean DomWindowTopLevel;

		public readonly Uri Uri;

		public readonly Boolean IsSameDocument;
		public readonly Boolean IsErrorPage;

		/// <summary>Creates a new instance of a <see cref="GeckoNavigatedEventArgs"/> object.</summary>
		/// <param name="value"></param>
		/// <param name="response"></param>
		internal GeckoNavigatedEventArgs(Uri value, nsIRequest response, GeckoWindow domWind, bool _sameDocument, bool _errorPage)
		{
			Uri = value;
			_response = response;
			DomWindow = domWind;
			DomWindowTopLevel = ((domWind == null) ? true : DomWindow.DomWindow.Equals(DomWindow.Top.DomWindow));

			IsSameDocument = _sameDocument;
			IsErrorPage = _errorPage;
		}

		public GeckoResponse Response
		{
			get { return _wrapper ?? ( _wrapper = new GeckoResponse( _response ) ); }
		}
	}
	#endregion

	#region GeckoRedirectingEventArgs
	/// <summary>Provides data for event.</summary>
	public class GeckoRedirectingEventArgs
		: CancelEventArgs
	{
		public readonly Uri Uri;
		public readonly GeckoWindow DomWindow;
		public readonly Boolean DomWindowTopLevel;

		/// <summary>Creates a new instance of a <see cref="GeckoRedirectingEventArgs"/> object.</summary>
		/// <param name="value"></param>
		public GeckoRedirectingEventArgs(Uri value, GeckoWindow domWind)
		{
			Uri = value;
			DomWindow = domWind;
			DomWindowTopLevel = ((domWind == null) ? true : DomWindow.DomWindow.Equals(DomWindow.Top.DomWindow));
		}
	}
	#endregion

	#region GeckoCreateWindowEventArgs

	/// <summary>Provides data for event.</summary>
	public class GeckoCreateWindowEventArgs
		: EventArgs
	{
		private GeckoWebBrowser _webBrowser;

		public readonly GeckoWindowFlags Flags;
		
		/// <summary>Creates a new instance of a <see cref="GeckoCreateWindowEventArgs"/> object.</summary>
		/// <param name="flags"></param>
		public GeckoCreateWindowEventArgs(GeckoWindowFlags flags)
		{
			Flags = flags;
		}

		/// <summary>
		/// Gets or sets the <see cref="GeckoWebBrowser"/> used in the new window.
		/// </summary>
		public GeckoWebBrowser WebBrowser
		{
			get { return _webBrowser; }
			set { _webBrowser = value; }
		}
		
	}
	#endregion

	#region GeckoCreateWindow2EventArgs

	/// <summary>Provides data for event.</summary>
	public class GeckoCreateWindow2EventArgs
		: CancelEventArgs
	{
		private GeckoWebBrowser _webBrowser;

		public readonly String Uri;
		public readonly GeckoWindowFlags Flags;

		/// <summary>Creates a new instance of a <see cref="GeckoCreateWindowEventArgs"/> object.</summary>
		/// <param name="flags"></param>
		/// <param name="uri"></param>
		public GeckoCreateWindow2EventArgs(GeckoWindowFlags flags, String uri)
			:base(false)
		{
			Flags = flags;
			Uri = uri;
		}

		/// <summary>
		/// Gets or sets the <see cref="GeckoWebBrowser"/> used in the new window.
		/// </summary>
		public GeckoWebBrowser WebBrowser
		{
			get { return _webBrowser; }
			set { _webBrowser = value; }
		}
	}
	#endregion

	#region GeckoWindowSetBoundsEventArgs

	/// <summary>Provides data for event.</summary>
	public class GeckoWindowSetBoundsEventArgs
		: EventArgs
	{
		public readonly Rectangle Bounds;
		public readonly BoundsSpecified BoundsSpecified;

		/// <summary>Creates a new instance of a <see cref="GeckoWindowSetBoundsEventArgs"/> object.</summary>
		/// <param name="bounds"></param>
		/// <param name="specified"></param>
		public GeckoWindowSetBoundsEventArgs(Rectangle bounds, BoundsSpecified specified)
		{
			Bounds = bounds;
			BoundsSpecified = specified;
		}
	}
	#endregion

	#region GeckoContextMenuEventArgs
	/// <summary>Provides data for event.</summary>
	public class GeckoContextMenuEventArgs
		: EventArgs
	{
		/// <summary>
		/// Gets the location where the context menu will be displayed.
		/// </summary>
		public readonly Point Location;
		public readonly Uri BackgroundImageSrc;
		public readonly Uri ImageSrc;
		public readonly string AssociatedLink;

		private GeckoNode _targetNode;

		/// <summary>Creates a new instance of a <see cref="GeckoContextMenuEventArgs"/> object.</summary>
		public GeckoContextMenuEventArgs(Point location, ContextMenu contextMenu, string associatedLink, Uri backgroundImageSrc, Uri imageSrc, GeckoNode targetNode)
		{
			Location = location;
			_contextMenu = contextMenu;
			AssociatedLink = associatedLink;
			BackgroundImageSrc = backgroundImageSrc;
			ImageSrc = imageSrc;
			_targetNode = targetNode;
		}

		

		/// <summary>
		/// Gets or sets the context menu to be displayed.  Set this property to null to disable
		/// the context menu.
		/// </summary>
		public ContextMenu ContextMenu
		{
			get { return _contextMenu; }
			set { _contextMenu = value; }
		}
		ContextMenu _contextMenu;

		

		public GeckoNode TargetNode
		{
			get { return _targetNode; }
		}
		
	}
	#endregion

	#region GeckoObserveHttpModifyRequestEventArgs
	/// <summary>Provides data for event.</summary>
	public class GeckoObserveHttpModifyRequestEventArgs
		: CancelEventArgs {
		public readonly Uri Uri;
		public readonly Uri Referrer;
		public readonly string RequestMethod;
		public readonly Byte[] RequestBody;
		public readonly List<KeyValuePair<string, string>> RequestHeaders;
		public readonly HttpChannel Channel;
		public readonly bool? ReqBodyContainsHeaders;

		/// <summary>Creates a new instance of a <see cref="GeckoObserveHttpModifyRequestEventArgs"/> object.</summary>
		/// <param name="uri">Uri</param>
		/// <param name="refVal">Referrer</param>
		/// <param name="reqMethod">Request Method</param>
		/// <param name="reqBody">Request Body</param>
		/// <param name="reqHeaders">Request Headers</param>
		/// <param name="httpChan">Reference to Http Channel</param>
		/// <param name="bodyContainsHeaders">Does ReqBody contain the headers</param>
		public GeckoObserveHttpModifyRequestEventArgs(Uri uri, Uri refVal, String reqMethod, Byte[] reqBody, List<KeyValuePair<string, string>> reqHeaders, HttpChannel httpChan, bool? bodyContainsHeaders)
			: base(false) {
			Uri = uri;
			Referrer = refVal;
			RequestMethod = reqMethod;
			RequestBody = reqBody;
			RequestHeaders = reqHeaders;
			Channel = httpChan;
			ReqBodyContainsHeaders = bodyContainsHeaders;
		}
	}
	#endregion

	#endregion
}
