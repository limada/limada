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
// Generated by IDLImporter from file nsISSLErrorListener.idl
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
    /// A mechanism to report a broken SSL connection. The recipient should NOT block.
    /// </summary>
	[ComImport()]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("99f8d972-bae4-414c-b39b-47587d3ded68")]
	public interface nsISSLErrorListener
	{
		
		/// <summary>
        /// @param socketInfo A network communication context that can be used to obtain more information
        /// about the active connection.
        /// @param error The code associated with the error.
        /// @param targetSite The Site name that was used to open the current connection.
        ///
        /// @return The consumer shall return true if it wants to suppress the error message
        /// related to the error (the connection will still get canceled).
        /// </summary>
		[return: MarshalAs(UnmanagedType.U1)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		bool NotifySSLError([MarshalAs(UnmanagedType.Interface)] nsIInterfaceRequestor socketInfo, int error, [MarshalAs(UnmanagedType.LPStruct)] nsAUTF8StringBase targetSite);
	}
}
