// --------------------------------------------------------------------------------------------
// Version: MPL 1.1/GPL 2.0/LGPL 2.1
// 
// The contents of this file are subject to the Mozilla Public License Version
// 1.1 (the "License"); you may not use this file except in compliance with
// the License. You may obtain a copy of the License at
// http://www.mozilla.org/MPL/
// 
// Software distributed under the License is distributed on an "AS IS" basis,
// WITHOUT WARRANTY OF ANY KIND, either express or implied. See the License
// for the specific language governing rights and limitations under the
// License.
// 
// <remarks>
// Generated by IDLImporter from file nsIAlertsService.idl
// 
// You should use these interfaces when you access the COM objects defined in the mentioned
// IDL/IDH file.
// </remarks>
// --------------------------------------------------------------------------------------------
namespace Gecko
{
	using System;
	using System.Runtime.InteropServices;
	using System.Runtime.InteropServices.ComTypes;
	using System.Runtime.CompilerServices;
	
	
	/// <summary>
    ///This Source Code Form is subject to the terms of the Mozilla Public
    /// License, v. 2.0. If a copy of the MPL was not distributed with this
    /// file, You can obtain one at http://mozilla.org/MPL/2.0/. </summary>
	[ComImport()]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("160e87e1-d57d-456b-b6ea-17826f6ea7a8")]
	public interface nsIAlertsService
	{
		
		/// <summary>
        /// Displays a sliding notification window.
        ///
        /// @param imageUrl       A URL identifying the image to put in the alert.
        /// @param title          The title for the alert.
        /// @param text           The contents of the alert.
        /// @param textClickable  If true, causes the alert text to look like a link
        /// and notifies the listener when user attempts to
        /// click the alert text.
        /// @param cookie         A blind cookie the alert will pass back to the
        /// consumer during the alert listener callbacks.
        /// @param alertListener  Used for callbacks. May be null if the caller
        /// doesn't care about callbacks.
        /// @param name           The name of the notification. This is currently
        /// only used on OS X with Growl and Android.
        /// On OS X with Growl, users can disable notifications
        /// with a given name. On Android the name is hashed
        /// and used as a notification ID. Notifications will
        /// replace previous notifications with the same name.
        /// @param dir            Bidi override for the title. Valid values are
        /// "auto", "ltr" or "rtl". Only available on supported
        /// platforms.
        /// @param lang           Language of title and text of the alert. Only available
        /// on supported platforms.
        /// @throws NS_ERROR_NOT_AVAILABLE If the notification cannot be displayed.
        ///
        /// The following arguments will be passed to the alertListener's observe()
        /// method:
        /// subject - null
        /// topic   - "alertfinished" when the alert goes away
        /// "alertclickcallback" when the text is clicked
        /// "alertshow" when the alert is shown
        /// data    - the value of the cookie parameter passed to showAlertNotification.
        ///
        /// @note Depending on current circumstances (if the user's in a fullscreen
        /// application, for instance), the alert might not be displayed at all.
        /// In that case, if an alert listener is passed in it will receive the
        /// "alertfinished" notification immediately.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void ShowAlertNotification([MarshalAs(UnmanagedType.LPStruct)] nsAStringBase imageUrl, [MarshalAs(UnmanagedType.LPStruct)] nsAStringBase title, [MarshalAs(UnmanagedType.LPStruct)] nsAStringBase text, [MarshalAs(UnmanagedType.U1)] bool textClickable, [MarshalAs(UnmanagedType.LPStruct)] nsAStringBase cookie, [MarshalAs(UnmanagedType.Interface)] nsIObserver alertListener, [MarshalAs(UnmanagedType.LPStruct)] nsAStringBase name, [MarshalAs(UnmanagedType.LPStruct)] nsAStringBase dir, [MarshalAs(UnmanagedType.LPStruct)] nsAStringBase lang);
		
		/// <summary>
        /// Close alerts created by the service.
        ///
        /// @param name           The name of the notification to close. If no name
        /// is provided then only a notification created with
        /// no name (if any) will be closed.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void CloseAlert([MarshalAs(UnmanagedType.LPStruct)] nsAStringBase name);
	}
	
	/// <summary>nsIAlertsProgressListener </summary>
	[ComImport()]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("df1bd4b0-3a8c-40e6-806a-203f38b0bd9f")]
	public interface nsIAlertsProgressListener
	{
		
		/// <summary>
        /// Called to notify the alert service that progress has occurred for the
        /// given notification previously displayed with showAlertNotification().
        ///
        /// @param name         The name of the notification displaying the
        /// progress. On Android the name is hashed and used
        /// as a notification ID.
        /// @param progress     Numeric value in the range 0 to progressMax
        /// indicating the current progress.
        /// @param progressMax  Numeric value indicating the maximum progress.
        /// @param text         The contents of the alert. If not provided,
        /// the percentage will be displayed.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void OnProgress([MarshalAs(UnmanagedType.LPStruct)] nsAStringBase name, long progress, long progressMax, [MarshalAs(UnmanagedType.LPStruct)] nsAStringBase text);
		
		/// <summary>
        /// Called to cancel and hide the given notification previously displayed
        /// with showAlertNotification().
        ///
        /// @param name         The name of the notification.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void OnCancel([MarshalAs(UnmanagedType.LPStruct)] nsAStringBase name);
	}
}
