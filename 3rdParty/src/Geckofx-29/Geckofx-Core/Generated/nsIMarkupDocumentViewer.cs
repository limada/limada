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
// Generated by IDLImporter from file nsIMarkupDocumentViewer.idl
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
    /// The nsIMarkupDocumentViewer
    /// This interface describes the properties of a content viewer
    /// for a markup document - HTML or XML
    /// </summary>
	[ComImport()]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("7aea9561-5346-401c-b40e-418688da2d0d")]
	public interface nsIMarkupDocumentViewer
	{
		
		/// <summary>
        ///Scrolls to a given DOM content node.
        ///	 </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void ScrollToNode([MarshalAs(UnmanagedType.Interface)] nsIDOMNode node);
		
		/// <summary>
        ///The amount by which to scale all text. Default is 1.0. </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		float GetTextZoomAttribute();
		
		/// <summary>
        ///The amount by which to scale all text. Default is 1.0. </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void SetTextZoomAttribute(float aTextZoom);
		
		/// <summary>
        ///The amount by which to scale all lengths. Default is 1.0. </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		float GetFullZoomAttribute();
		
		/// <summary>
        ///The amount by which to scale all lengths. Default is 1.0. </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void SetFullZoomAttribute(float aFullZoom);
		
		/// <summary>
        ///Disable entire author style level (including HTML presentation hints) </summary>
		[return: MarshalAs(UnmanagedType.U1)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		bool GetAuthorStyleDisabledAttribute();
		
		/// <summary>
        ///Disable entire author style level (including HTML presentation hints) </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void SetAuthorStyleDisabledAttribute([MarshalAs(UnmanagedType.U1)] bool aAuthorStyleDisabled);
		
		/// <summary>
        /// XXX comm-central only: bug 829543. Not the Character Encoding menu in
        /// browser!
        ///	 </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void GetForceCharacterSetAttribute([MarshalAs(UnmanagedType.LPStruct)] nsACStringBase aForceCharacterSet);
		
		/// <summary>
        /// XXX comm-central only: bug 829543. Not the Character Encoding menu in
        /// browser!
        ///	 </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void SetForceCharacterSetAttribute([MarshalAs(UnmanagedType.LPStruct)] nsACStringBase aForceCharacterSet);
		
		/// <summary>
        /// XXX comm-central only: bug 829543.
        ///	 </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void GetHintCharacterSetAttribute([MarshalAs(UnmanagedType.LPStruct)] nsACStringBase aHintCharacterSet);
		
		/// <summary>
        /// XXX comm-central only: bug 829543.
        ///	 </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void SetHintCharacterSetAttribute([MarshalAs(UnmanagedType.LPStruct)] nsACStringBase aHintCharacterSet);
		
		/// <summary>
        /// XXX comm-central only: bug 829543.
        ///	 </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		int GetHintCharacterSetSourceAttribute();
		
		/// <summary>
        /// XXX comm-central only: bug 829543.
        ///	 </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void SetHintCharacterSetSourceAttribute(int aHintCharacterSetSource);
		
		/// <summary>
        /// Requests the size of the content to the container.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void GetContentSize(ref int width, ref int height);
		
		/// <summary>
        ///The minimum font size </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		int GetMinFontSizeAttribute();
		
		/// <summary>
        ///The minimum font size </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void SetMinFontSizeAttribute(int aMinFontSize);
		
		/// <summary>
        /// Append |this| and all of its descendants to the given array,
        /// in depth-first pre-order traversal.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void AppendSubtree(System.IntPtr array);
		
		/// <summary>
        /// Set the maximum line width for the document.
        /// NOTE: This will generate a reflow!
        ///
        /// @param maxLineWidth The maximum width of any line boxes on the page,
        /// in CSS pixels.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void ChangeMaxLineBoxWidth(int maxLineBoxWidth);
		
		/// <summary>
        /// Instruct the refresh driver to discontinue painting until further
        /// notice.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void PausePainting();
		
		/// <summary>
        /// Instruct the refresh driver to resume painting after a previous call to
        /// pausePainting().
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void ResumePainting();
		
		/// <summary>
        /// Render the document as if being viewed on a device with the specified
        /// media type. This will cause a reflow.
        ///
        /// @param mediaType The media type to be emulated
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void EmulateMedium([MarshalAs(UnmanagedType.CustomMarshaler, MarshalType = "Gecko.CustomMarshalers.AStringMarshaler")] nsAStringBase aMediaType);
		
		/// <summary>
        /// Restore the viewer's natural media type
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void StopEmulatingMedium();
	}
}
