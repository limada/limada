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
// Generated by IDLImporter from file nsIWebBrowserPersist.idl
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
    /// Interface for persisting DOM documents and URIs to local or remote storage.
    /// </summary>
	[ComImport()]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("dd4e0a6a-210f-419a-ad85-40e8543b9465")]
	public interface nsIWebBrowserPersist : nsICancelable
	{
		
		/// <summary>
        /// Call this method to request that this object abort whatever operation it
        /// may be performing.
        ///
        /// @param aReason
        /// Pass a failure code to indicate the reason why this operation is
        /// being canceled.  It is an error to pass a success code.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		new void Cancel(int aReason);
		
		/// <summary>
        /// Flags governing how data is fetched and saved from the network.
        /// It is best to set this value explicitly unless you are prepared
        /// to accept the default values.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		uint GetPersistFlagsAttribute();
		
		/// <summary>
        /// Flags governing how data is fetched and saved from the network.
        /// It is best to set this value explicitly unless you are prepared
        /// to accept the default values.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void SetPersistFlagsAttribute(uint aPersistFlags);
		
		/// <summary>
        /// Current state of the persister object.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		uint GetCurrentStateAttribute();
		
		/// <summary>
        /// Value indicating the success or failure of the persist
        /// operation.
        ///
        /// @return NS_OK Operation was successful or is still ongoing.
        /// @return NS_BINDING_ABORTED Operation cancelled.
        /// @return NS_ERROR_FAILURE Non-specific failure.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		uint GetResultAttribute();
		
		/// <summary>
        /// Callback listener for progress notifications. The object that the
        /// embbedder supplies may also implement nsIInterfaceRequestor and be
        /// prepared to return nsIAuthPrompt or other interfaces that may be required
        /// to download data.
        ///
        /// @see nsIAuthPrompt
        /// @see nsIInterfaceRequestor
        /// </summary>
		[return: MarshalAs(UnmanagedType.Interface)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		nsIWebProgressListener GetProgressListenerAttribute();
		
		/// <summary>
        /// Callback listener for progress notifications. The object that the
        /// embbedder supplies may also implement nsIInterfaceRequestor and be
        /// prepared to return nsIAuthPrompt or other interfaces that may be required
        /// to download data.
        ///
        /// @see nsIAuthPrompt
        /// @see nsIInterfaceRequestor
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void SetProgressListenerAttribute([MarshalAs(UnmanagedType.Interface)] nsIWebProgressListener aProgressListener);
		
		/// <summary>
        /// Save the specified URI to file.
        ///
        /// @param aURI       URI to save to file. Some implementations of this interface
        /// may also support <CODE>nsnull</CODE> to imply the currently
        /// loaded URI.
        /// @param aCacheKey  An object representing the URI in the cache or
        /// <CODE>nsnull</CODE>.  This can be a necko cache key,
        /// an nsIWebPageDescriptor, or the currentDescriptor of an
        /// nsIWebPageDescriptor.
        /// @param aReferrer  The referrer URI to pass with an HTTP request or
        /// <CODE>nsnull</CODE>.
        /// @param aPostData  Post data to pass with an HTTP request or
        /// <CODE>nsnull</CODE>.
        /// @param aExtraHeaders Additional headers to supply with an HTTP request
        /// or <CODE>nsnull</CODE>.
        /// @param aFile      Target file. This may be a nsILocalFile object or an
        /// nsIURI object with a file scheme or a scheme that
        /// supports uploading (e.g. ftp).
        ///
        /// @see nsILocalFile
        /// @see nsIURI
        /// @see nsIInputStream
        ///
        /// @return NS_OK Operation has been started.
        /// @return NS_ERROR_INVALID_ARG One or more arguments was invalid.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void SaveURI([MarshalAs(UnmanagedType.Interface)] nsIURI aURI, [MarshalAs(UnmanagedType.Interface)] nsISupports aCacheKey, [MarshalAs(UnmanagedType.Interface)] nsIURI aReferrer, [MarshalAs(UnmanagedType.Interface)] nsIInputStream aPostData, [MarshalAs(UnmanagedType.LPStr)] string aExtraHeaders, [MarshalAs(UnmanagedType.Interface)] nsISupports aFile);
		
		/// <summary>
        /// Save a channel to a file. It must not be opened yet.
        /// @see saveURI
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void SaveChannel([MarshalAs(UnmanagedType.Interface)] nsIChannel aChannel, [MarshalAs(UnmanagedType.Interface)] nsISupports aFile);
		
		/// <summary>
        /// Save the specified DOM document to file and optionally all linked files
        /// (e.g. images, CSS, JS & subframes). Do not call this method until the
        /// document has finished loading!
        ///
        /// @param aDocument          Document to save to file. Some implementations of
        /// this interface may also support <CODE>nsnull</CODE>
        /// to imply the currently loaded document.
        /// @param aFile              Target local file. This may be a nsILocalFile object or an
        /// nsIURI object with a file scheme or a scheme that
        /// supports uploading (e.g. ftp).
        /// @param aDataPath          Path to directory where URIs linked to the document
        /// are saved or nsnull if no linked URIs should be saved.
        /// This may be a nsILocalFile object or an nsIURI object
        /// with a file scheme.
        /// @param aOutputContentType The desired MIME type format to save the
        /// document and all subdocuments into or nsnull to use
        /// the default behaviour.
        /// @param aEncodingFlags     Flags to pass to the encoder.
        /// @param aWrapColumn        For text documents, indicates the desired width to
        /// wrap text at. Parameter is ignored if wrapping is not
        /// specified by the encoding flags.
        ///
        /// @see nsILocalFile
        /// @see nsIURI
        ///
        /// @return NS_OK Operation has been started.
        /// @return NS_ERROR_INVALID_ARG One or more arguments was invalid.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void SaveDocument([MarshalAs(UnmanagedType.Interface)] nsIDOMDocument aDocument, [MarshalAs(UnmanagedType.Interface)] nsISupports aFile, [MarshalAs(UnmanagedType.Interface)] nsISupports aDataPath, [MarshalAs(UnmanagedType.LPStr)] string aOutputContentType, uint aEncodingFlags, uint aWrapColumn);
		
		/// <summary>
        /// Cancels the current operation. The caller is responsible for cleaning up
        /// partially written files or directories. This has the same effect as calling
        /// cancel with an argument of NS_BINDING_ABORTED.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void CancelSave();
	}
}