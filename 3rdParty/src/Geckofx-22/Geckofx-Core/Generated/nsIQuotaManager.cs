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
// Generated by IDLImporter from file nsIQuotaManager.idl
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
	[Guid("8d74e6f8-81c3-4045-9bb7-70bdb7b11b25")]
	public interface nsIQuotaManager
	{
		
		/// <summary>
        /// Schedules an asynchronous callback that will return the total amount of
        /// disk space being used by storages for the given origin.
        ///
        /// @param aURI
        /// The URI whose usage is being queried.
        /// @param aCallback
        /// The callback that will be called when the usage is available.
        /// </summary>
		[return: MarshalAs(UnmanagedType.Interface)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		nsIQuotaRequest GetUsageForURI([MarshalAs(UnmanagedType.Interface)] nsIURI aURI, [MarshalAs(UnmanagedType.Interface)] nsIUsageCallback aCallback, uint aAppId, [MarshalAs(UnmanagedType.U1)] bool aInMozBrowserOnly, int argc);
		
		/// <summary>
        /// Removes all storages stored for the given URI. The files may not be
        /// deleted immediately depending on prohibitive concurrent operations.
        ///
        /// @param aURI
        /// The URI whose storages are to be cleared.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void ClearStoragesForURI([MarshalAs(UnmanagedType.Interface)] nsIURI aURI, uint aAppId, [MarshalAs(UnmanagedType.U1)] bool aInMozBrowserOnly, int argc);
	}
}
