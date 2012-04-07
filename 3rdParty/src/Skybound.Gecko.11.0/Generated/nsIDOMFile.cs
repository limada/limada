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
// Generated by IDLImporter from file nsIDOMFile.idl
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
	
	
	/// <summary>nsIDOMBlob </summary>
	[ComImport()]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("f62c6887-e3bc-495a-802c-287e12e969a0")]
	public interface nsIDOMBlob
	{
		
		/// <summary>Member GetSizeAttribute </summary>
		/// <returns>A System.UInt64</returns>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		ulong GetSizeAttribute();
		
		/// <summary>Member GetTypeAttribute </summary>
		/// <param name='aType'> </param>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void GetTypeAttribute([MarshalAs(UnmanagedType.LPStruct)] nsAStringBase aType);
		
		/// <summary>Member GetInternalStreamAttribute </summary>
		/// <returns>A nsIInputStream</returns>
		[return: MarshalAs(UnmanagedType.Interface)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		nsIInputStream GetInternalStreamAttribute();
		
		/// <summary>
        /// moz-filedata: protocol handler
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void GetInternalUrl([MarshalAs(UnmanagedType.Interface)] nsIPrincipal principal, [MarshalAs(UnmanagedType.LPStruct)] nsAStringBase retval);
		
		/// <summary>Member MozSlice </summary>
		/// <param name='start'> </param>
		/// <param name='end'> </param>
		/// <param name='contentType'> </param>
		/// <param name='argc'> </param>
		/// <returns>A nsIDOMBlob</returns>
		[return: MarshalAs(UnmanagedType.Interface)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		nsIDOMBlob MozSlice(long start, long end, [MarshalAs(UnmanagedType.LPStruct)] nsAStringBase contentType, int argc);
		
		/// <summary>
        /// Intended only for testing. It can be called on any thread.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		int GetFileId();
		
		/// <summary>
        /// the blob is initialized from a database. It can be called on any thread.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void AddFileInfo(System.IntPtr aFileInfo);
		
		/// <summary>
        /// shared or needs to be copied. It can be called on any thread.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		System.IntPtr GetFileInfo(System.IntPtr aFileManager);
	}
	
	/// <summary>nsIDOMFile </summary>
	[ComImport()]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("b096ef67-7b77-47f8-8e70-5d8ee36416bf")]
	public interface nsIDOMFile : nsIDOMBlob
	{
		
		/// <summary>Member GetSizeAttribute </summary>
		/// <returns>A System.UInt64</returns>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		new ulong GetSizeAttribute();
		
		/// <summary>Member GetTypeAttribute </summary>
		/// <param name='aType'> </param>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		new void GetTypeAttribute([MarshalAs(UnmanagedType.LPStruct)] nsAStringBase aType);
		
		/// <summary>Member GetInternalStreamAttribute </summary>
		/// <returns>A nsIInputStream</returns>
		[return: MarshalAs(UnmanagedType.Interface)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		new nsIInputStream GetInternalStreamAttribute();
		
		/// <summary>
        /// moz-filedata: protocol handler
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		new void GetInternalUrl([MarshalAs(UnmanagedType.Interface)] nsIPrincipal principal, [MarshalAs(UnmanagedType.LPStruct)] nsAStringBase retval);
		
		/// <summary>Member MozSlice </summary>
		/// <param name='start'> </param>
		/// <param name='end'> </param>
		/// <param name='contentType'> </param>
		/// <param name='argc'> </param>
		/// <returns>A nsIDOMBlob</returns>
		[return: MarshalAs(UnmanagedType.Interface)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		new nsIDOMBlob MozSlice(long start, long end, [MarshalAs(UnmanagedType.LPStruct)] nsAStringBase contentType, int argc);
		
		/// <summary>
        /// Intended only for testing. It can be called on any thread.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		new int GetFileId();
		
		/// <summary>
        /// the blob is initialized from a database. It can be called on any thread.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		new void AddFileInfo(System.IntPtr aFileInfo);
		
		/// <summary>
        /// shared or needs to be copied. It can be called on any thread.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		new System.IntPtr GetFileInfo(System.IntPtr aFileManager);
		
		/// <summary>Member GetNameAttribute </summary>
		/// <param name='aName'> </param>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void GetNameAttribute([MarshalAs(UnmanagedType.LPStruct)] nsAStringBase aName);
		
		/// <summary>Member GetMozFullPathAttribute </summary>
		/// <param name='aMozFullPath'> </param>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void GetMozFullPathAttribute([MarshalAs(UnmanagedType.LPStruct)] nsAStringBase aMozFullPath);
		
		/// <summary>
        /// This performs no security checks!
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void GetMozFullPathInternalAttribute([MarshalAs(UnmanagedType.LPStruct)] nsAStringBase aMozFullPathInternal);
	}
	
	/// <summary>nsIDOMMozBlobBuilder </summary>
	[ComImport()]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("006d2cde-ec18-41d4-acc3-43682dd418e2")]
	public interface nsIDOMMozBlobBuilder
	{
		
		/// <summary>Member GetBlob </summary>
		/// <param name='contentType'> </param>
		/// <returns>A nsIDOMBlob</returns>
		[return: MarshalAs(UnmanagedType.Interface)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		nsIDOMBlob GetBlob([MarshalAs(UnmanagedType.LPStruct)] nsAStringBase contentType);
		
		/// <summary>Member GetFile </summary>
		/// <param name='name'> </param>
		/// <param name='contentType'> </param>
		/// <returns>A nsIDOMFile</returns>
		[return: MarshalAs(UnmanagedType.Interface)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		nsIDOMFile GetFile([MarshalAs(UnmanagedType.LPStruct)] nsAStringBase name, [MarshalAs(UnmanagedType.LPStruct)] nsAStringBase contentType);
		
		/// <summary>Member Append </summary>
		/// <param name='data'> </param>
		/// <param name='jsContext'> </param>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void Append(System.IntPtr data, System.IntPtr jsContext);
	}
}
