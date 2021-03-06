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
// Generated by IDLImporter from file nsIAccessibleEditableText.idl
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
    ///-*- Mode: C++; tab-width: 2; indent-tabs-mode: nil; c-basic-offset: 2 -*-
    ///
    /// This Source Code Form is subject to the terms of the Mozilla Public
    /// License, v. 2.0. If a copy of the MPL was not distributed with this
    /// file, You can obtain one at http://mozilla.org/MPL/2.0/. </summary>
	[ComImport()]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("93d0ba57-0d20-49d1-aede-8fde6699855d")]
	public interface nsIAccessibleEditableText
	{
		
		/// <summary>
        /// Replaces the text represented by this object by the given text.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void SetTextContents([MarshalAs(UnmanagedType.CustomMarshaler, MarshalType = "Gecko.CustomMarshalers.AStringMarshaler")] nsAStringBase text);
		
		/// <summary>
        /// Inserts text at the specified position.
        ///
        /// @param text - text that is inserted.
        /// @param position - index at which to insert the text.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void InsertText([MarshalAs(UnmanagedType.CustomMarshaler, MarshalType = "Gecko.CustomMarshalers.AStringMarshaler")] nsAStringBase text, int position);
		
		/// <summary>
        /// Copies the text range into the clipboard.
        ///
        /// @param startPos - start index of the text to moved into the clipboard.
        /// @param endPos - end index of the text to moved into the clipboard.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void CopyText(int startPos, int endPos);
		
		/// <summary>
        /// Deletes a range of text and copies it to the clipboard.
        ///
        /// @param startPos - start index of the text to be deleted.
        /// @param endOffset - end index of the text to be deleted.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void CutText(int startPos, int endPos);
		
		/// <summary>
        /// Deletes a range of text.
        ///
        /// @param startPos - start index of the text to be deleted.
        /// @param endPos - end index of the text to be deleted.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void DeleteText(int startPos, int endPos);
		
		/// <summary>
        /// Pastes text from the clipboard.
        ///
        /// @param position - index at which to insert the text from the system
        /// clipboard into the text represented by this object.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void PasteText(int position);
	}
}
