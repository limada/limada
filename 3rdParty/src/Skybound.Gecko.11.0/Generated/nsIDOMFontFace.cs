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
// Generated by IDLImporter from file nsIDOMFontFace.idl
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
	
	
	/// <summary>nsIDOMFontFace </summary>
	[ComImport()]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("9a3b1272-6585-4f41-b08f-fdc5da444cd0")]
	public interface nsIDOMFontFace
	{
		
		/// <summary>
        /// Note that the same physical font may have been found in multiple ways within a range.
        /// </summary>
		[return: MarshalAs(UnmanagedType.U1)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		bool GetFromFontGroupAttribute();
		
		/// <summary>Member GetFromLanguagePrefsAttribute </summary>
		/// <returns>A System.Boolean</returns>
		[return: MarshalAs(UnmanagedType.U1)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		bool GetFromLanguagePrefsAttribute();
		
		/// <summary>Member GetFromSystemFallbackAttribute </summary>
		/// <returns>A System.Boolean</returns>
		[return: MarshalAs(UnmanagedType.U1)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		bool GetFromSystemFallbackAttribute();
		
		/// <summary>
        /// available for all fonts
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void GetNameAttribute([MarshalAs(UnmanagedType.LPStruct)] nsAStringBase aName);
		
		/// <summary>
        /// full font name as obtained from the font resource
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void GetCSSFamilyNameAttribute([MarshalAs(UnmanagedType.LPStruct)] nsAStringBase aCSSFamilyName);
		
		/// <summary>
        /// meaningful only when the font is a user font defined using @font-face
        /// </summary>
		[return: MarshalAs(UnmanagedType.Interface)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		nsIDOMCSSFontFaceRule GetRuleAttribute();
		
		/// <summary>
        /// null if no associated @font-face rule
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		int GetSrcIndexAttribute();
		
		/// <summary>
        /// index in the rule's src list, -1 if no @font-face rule
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void GetURIAttribute([MarshalAs(UnmanagedType.LPStruct)] nsAStringBase aURI);
		
		/// <summary>
        /// null if not a downloaded font, i.e. local
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void GetLocalNameAttribute([MarshalAs(UnmanagedType.LPStruct)] nsAStringBase aLocalName);
		
		/// <summary>
        /// null if not a src:local(...) rule
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void GetFormatAttribute([MarshalAs(UnmanagedType.LPStruct)] nsAStringBase aFormat);
		
		/// <summary>
        /// as per http://www.w3.org/TR/css3-webfonts/#referencing
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void GetMetadataAttribute([MarshalAs(UnmanagedType.LPStruct)] nsAStringBase aMetadata);
	}
}
