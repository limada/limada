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
// Generated by IDLImporter from file nsIPaymentUIGlue.idl
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
	[Guid("b9afa678-71a5-4975-bcdb-0c4098730eff")]
	public interface nsIPaymentUIGlueCallback
	{
		
		/// <summary>
        ///This Source Code Form is subject to the terms of the Mozilla Public
        /// License, v. 2.0. If a copy of the MPL was not distributed with this file,
        /// You can obtain one at http://mozilla.org/MPL/2.0/. </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void Onresult([MarshalAs(UnmanagedType.LPStruct)] nsAStringBase requestId, [MarshalAs(UnmanagedType.LPStruct)] nsAStringBase result);
	}
	
	/// <summary>nsIPaymentUIGlue </summary>
	[ComImport()]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("4dda9aa0-df88-4dcd-a583-199e516fa438")]
	public interface nsIPaymentUIGlue
	{
		
		/// <summary>
        /// for each JWT provided via navigator.mozPay call.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void ConfirmPaymentRequest([MarshalAs(UnmanagedType.LPStruct)] nsAStringBase requestId, Gecko.JsVal paymentRequestsInfo, [MarshalAs(UnmanagedType.Interface)] nsIPaymentUIGlueCallback successCb, [MarshalAs(UnmanagedType.Interface)] nsIPaymentUIGlueCallback errorCb);
		
		/// <summary>Member ShowPaymentFlow </summary>
		/// <param name='requestId'> </param>
		/// <param name='paymentFlowInfo'> </param>
		/// <param name='errorCb'> </param>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void ShowPaymentFlow([MarshalAs(UnmanagedType.LPStruct)] nsAStringBase requestId, [MarshalAs(UnmanagedType.Interface)] nsIPaymentFlowInfo paymentFlowInfo, [MarshalAs(UnmanagedType.Interface)] nsIPaymentUIGlueCallback errorCb);
		
		/// <summary>Member Cleanup </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void Cleanup();
	}
}
