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
// Generated by IDLImporter from file nsIIDBFactory.idl
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
	using System.Windows.Forms;
	
	
	/// <summary>
    /// Interface that defines the indexedDB property on a window.  See
    /// http://dvcs.w3.org/hg/IndexedDB/raw-file/tip/Overview.html#idl-def-IDBFactory
    /// for more information.
    /// </summary>
	[ComImport()]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("885abbb7-cf81-4945-b5f1-07fed07ada82")]
	public interface nsIIDBFactory
	{
		
		/// <summary>
        /// Interface that defines the indexedDB property on a window.  See
        /// http://dvcs.w3.org/hg/IndexedDB/raw-file/tip/Overview.html#idl-def-IDBFactory
        /// for more information.
        /// </summary>
		[return: MarshalAs(UnmanagedType.Interface)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		nsIIDBOpenDBRequest Open([MarshalAs(UnmanagedType.LPStruct)] nsAStringBase name, long version, int argc);
		
		[return: MarshalAs(UnmanagedType.Interface)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		nsIIDBOpenDBRequest DeleteDatabase([MarshalAs(UnmanagedType.LPStruct)] nsAStringBase name);
		
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		short Cmp(System.IntPtr first, System.IntPtr second, System.IntPtr jsContext);
	}
}
