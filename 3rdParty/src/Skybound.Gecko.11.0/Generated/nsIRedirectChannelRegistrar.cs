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
// Generated by IDLImporter from file nsIRedirectChannelRegistrar.idl
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
    /// Used on the chrome process as a service to join channel implementation
    /// and parent IPC protocol side under a unique id.  Provides this way a generic
    /// communication while redirecting to various protocols.
    ///
    /// See also nsIChildChannel and nsIParentChannel.
    /// </summary>
	[ComImport()]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("efa36ea2-5b07-46fc-9534-a5acb8b77b72")]
	public interface nsIRedirectChannelRegistrar
	{
		
		/// <summary>
        /// Register the redirect target channel and obtain a unique ID for that
        /// channel.
        ///
        /// Primarily used in HttpChannelParentListener::AsyncOnChannelRedirect to get
        /// a channel id sent to the HttpChannelChild being redirected.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		uint RegisterChannel([MarshalAs(UnmanagedType.Interface)] nsIChannel channel);
		
		/// <summary>
        /// First, search for the channel registered under the id.  If found return
        /// it.  Then, register under the same id the parent side of IPC protocol
        /// to let it be later grabbed back by the originator of the redirect and
        /// notifications from the real channel could be forwarded to this parent
        /// channel.
        ///
        /// Primarily used in parent side of an IPC protocol implementation
        /// in reaction to nsIChildChannel.connectParent(id) called from the child
        /// process.
        /// </summary>
		[return: MarshalAs(UnmanagedType.Interface)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		nsIChannel LinkChannels(uint id, [MarshalAs(UnmanagedType.Interface)] nsIParentChannel channel);
		
		/// <summary>
        /// Returns back the channel previously registered under the ID with
        /// registerChannel method.
        ///
        /// Primarilly used in chrome IPC side of protocols when attaching a redirect
        /// target channel to an existing 'real' channel implementation.
        /// </summary>
		[return: MarshalAs(UnmanagedType.Interface)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		nsIChannel GetRegisteredChannel(uint id);
		
		/// <summary>
        /// Returns the stream listener that shall be attached to the redirect target
        /// channel, all notification from the redirect target channel will be
        /// forwarded to this stream listener.
        ///
        /// Primarilly used in HttpChannelParentListener::OnRedirectResult callback
        /// to grab the created parent side of the channel and forward notifications
        /// to it.
        /// </summary>
		[return: MarshalAs(UnmanagedType.Interface)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		nsIParentChannel GetParentChannel(uint id);
		
		/// <summary>
        /// To not force all channel implementations to support weak reference
        /// consumers of this service must ensure release of registered channels them
        /// self.  This releases both the real and parent channel registered under
        /// the id.
        ///
        /// Primarilly used in HttpChannelParentListener::OnRedirectResult callback.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void DeregisterChannels(uint id);
	}
}
