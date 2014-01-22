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
// Generated by IDLImporter from file nsIMobileConnectionProvider.idl
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
    /// License, v. 2.0. If a copy of the MPL was not distributed with this file,
    /// You can obtain one at http://mozilla.org/MPL/2.0/. </summary>
	[ComImport()]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("2cb8e811-7eaf-4cb9-8aa8-581e7a245edc")]
	public interface nsIMobileConnectionListener
	{
		
		/// <summary>
        ///This Source Code Form is subject to the terms of the Mozilla Public
        /// License, v. 2.0. If a copy of the MPL was not distributed with this file,
        /// You can obtain one at http://mozilla.org/MPL/2.0/. </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void NotifyVoiceChanged();
		
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void NotifyDataChanged();
		
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void NotifyCardStateChanged();
		
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void NotifyIccInfoChanged();
		
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void NotifyUssdReceived([MarshalAs(UnmanagedType.LPStruct)] nsAStringBase message, [MarshalAs(UnmanagedType.U1)] bool sessionEnded);
		
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void NotifyDataError([MarshalAs(UnmanagedType.LPStruct)] nsAStringBase message);
		
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void NotifyIccCardLockError([MarshalAs(UnmanagedType.LPStruct)] nsAStringBase lockType, uint retryCount);
		
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void NotifyCFStateChange([MarshalAs(UnmanagedType.U1)] bool success, ushort action, ushort reason, [MarshalAs(UnmanagedType.LPStruct)] nsAStringBase number, ushort timeSeconds, ushort serviceClass);
	}
	
	/// <summary>
    /// XPCOM component (in the content process) that provides the mobile
    /// network information.
    /// </summary>
	[ComImport()]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("152da558-c3c0-45ad-9ac5-1adaf7a83c0d")]
	public interface nsIMobileConnectionProvider
	{
		
		/// <summary>
        /// Called when a content process registers receiving unsolicited messages from
        /// RadioInterfaceLayer in the chrome process. Only a content process that has
        /// the 'mobileconnection' permission is allowed to register.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void RegisterMobileConnectionMsg([MarshalAs(UnmanagedType.Interface)] nsIMobileConnectionListener listener);
		
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void UnregisterMobileConnectionMsg([MarshalAs(UnmanagedType.Interface)] nsIMobileConnectionListener listener);
		
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void GetCardStateAttribute([MarshalAs(UnmanagedType.LPStruct)] nsAStringBase aCardState);
		
		[return: MarshalAs(UnmanagedType.Interface)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		nsIDOMMozMobileICCInfo GetIccInfoAttribute();
		
		[return: MarshalAs(UnmanagedType.Interface)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		nsIDOMMozMobileConnectionInfo GetVoiceConnectionInfoAttribute();
		
		[return: MarshalAs(UnmanagedType.Interface)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		nsIDOMMozMobileConnectionInfo GetDataConnectionInfoAttribute();
		
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void GetNetworkSelectionModeAttribute([MarshalAs(UnmanagedType.LPStruct)] nsAStringBase aNetworkSelectionMode);
		
		[return: MarshalAs(UnmanagedType.Interface)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		nsIDOMDOMRequest GetNetworks([MarshalAs(UnmanagedType.Interface)] nsIDOMWindow window);
		
		[return: MarshalAs(UnmanagedType.Interface)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		nsIDOMDOMRequest SelectNetwork([MarshalAs(UnmanagedType.Interface)] nsIDOMWindow window, [MarshalAs(UnmanagedType.Interface)] nsIDOMMozMobileNetworkInfo network);
		
		[return: MarshalAs(UnmanagedType.Interface)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		nsIDOMDOMRequest SelectNetworkAutomatically([MarshalAs(UnmanagedType.Interface)] nsIDOMWindow window);
		
		[return: MarshalAs(UnmanagedType.Interface)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		nsIDOMDOMRequest GetCardLock([MarshalAs(UnmanagedType.Interface)] nsIDOMWindow window, [MarshalAs(UnmanagedType.LPStruct)] nsAStringBase lockType);
		
		[return: MarshalAs(UnmanagedType.Interface)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		nsIDOMDOMRequest UnlockCardLock([MarshalAs(UnmanagedType.Interface)] nsIDOMWindow window, Gecko.JsVal info);
		
		[return: MarshalAs(UnmanagedType.Interface)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		nsIDOMDOMRequest SetCardLock([MarshalAs(UnmanagedType.Interface)] nsIDOMWindow window, Gecko.JsVal info);
		
		[return: MarshalAs(UnmanagedType.Interface)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		nsIDOMDOMRequest SendMMI([MarshalAs(UnmanagedType.Interface)] nsIDOMWindow window, [MarshalAs(UnmanagedType.LPStruct)] nsAStringBase mmi);
		
		[return: MarshalAs(UnmanagedType.Interface)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		nsIDOMDOMRequest CancelMMI([MarshalAs(UnmanagedType.Interface)] nsIDOMWindow window);
		
		[return: MarshalAs(UnmanagedType.Interface)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		nsIDOMDOMRequest GetCallForwardingOption([MarshalAs(UnmanagedType.Interface)] nsIDOMWindow window, ushort reason);
		
		[return: MarshalAs(UnmanagedType.Interface)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		nsIDOMDOMRequest SetCallForwardingOption([MarshalAs(UnmanagedType.Interface)] nsIDOMWindow window, [MarshalAs(UnmanagedType.Interface)] nsIDOMMozMobileCFInfo CFInfo);
	}
}