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
// Generated by IDLImporter from file nsILayoutRegressionTester.idl
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
	
	
	/// <summary>nsILayoutRegressionTester </summary>
	[ComImport()]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("B249B2C0-EE11-11DA-8AD9-0800200C9A66")]
	public interface nsILayoutRegressionTester
	{
		
		/// <summary>
        /// an error occurred
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		int DumpFrameModel([MarshalAs(UnmanagedType.Interface)] nsIDOMWindow aWindowToDump, [MarshalAs(UnmanagedType.Interface)] nsILocalFile aFile, uint aFlagsMask);
		
		/// <summary>
        /// Compares the contents of frame model files
        /// @param aBaseFile           the baseline file, opened with read permissions
        /// @param aVerFile            file containing the results to verify, opened with read permissions
        /// @param aFlags              flags specifying output verbosity
        /// @param aResult             result of the comparison: zero if the files are same, non-zero if different
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		int CompareFrameModels([MarshalAs(UnmanagedType.Interface)] nsILocalFile aBaseFile, [MarshalAs(UnmanagedType.Interface)] nsILocalFile aVerFile, uint aFlags);
	}
}
