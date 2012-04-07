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
// Generated by IDLImporter from file nsIContentViewerFile.idl
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
    /// The nsIDocShellFile
    /// </summary>
	[ComImport()]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("1b373597-6132-49f7-940e-6ff62fca5d35")]
	public interface nsIContentViewerFile
	{
		
		/// <summary>
        /// The nsIDocShellFile
        /// </summary>
		[return: MarshalAs(UnmanagedType.U1)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		bool GetPrintableAttribute();
		
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void Print([MarshalAs(UnmanagedType.U1)] bool aSilent, System.IntPtr aDebugFile, [MarshalAs(UnmanagedType.Interface)] nsIPrintSettings aPrintSettings);
		
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void PrintWithParent([MarshalAs(UnmanagedType.Interface)] nsIDOMWindow aParentWin, [MarshalAs(UnmanagedType.Interface)] nsIPrintSettings aThePrintSettings, [MarshalAs(UnmanagedType.Interface)] nsIWebProgressListener aWPListener);
	}
}
