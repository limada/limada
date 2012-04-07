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
// Generated by IDLImporter from file nsIDOMSVGAnimatedString.idl
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
	
	
	/// <summary>nsIDOMSVGAnimatedString </summary>
	[ComImport()]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("36f18f12-61a9-4529-8fa9-30050bd6ac00")]
	public interface nsIDOMSVGAnimatedString
	{
		
		/// <summary>Member GetBaseValAttribute </summary>
		/// <param name='aBaseVal'> </param>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void GetBaseValAttribute([MarshalAs(UnmanagedType.LPStruct)] nsAStringBase aBaseVal);
		
		/// <summary>Member SetBaseValAttribute </summary>
		/// <param name='aBaseVal'> </param>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void SetBaseValAttribute([MarshalAs(UnmanagedType.LPStruct)] nsAStringBase aBaseVal);
		
		/// <summary>
        /// raises DOMException on setting
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void GetAnimValAttribute([MarshalAs(UnmanagedType.LPStruct)] nsAStringBase aAnimVal);
	}
}
