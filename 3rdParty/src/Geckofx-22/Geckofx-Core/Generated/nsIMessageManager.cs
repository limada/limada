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
// Generated by IDLImporter from file nsIMessageManager.idl
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
    /// Message managers provide a way for chrome-privileged JS code to
    /// communicate with each other, even across process boundaries.
    ///
    /// Message managers are separated into "parent side" and "child side".
    /// These don't always correspond to process boundaries, but can.  For
    /// each child-side message manager, there is always exactly one
    /// corresponding parent-side message manager that it sends messages
    /// to.  However, for each parent-side message manager, there may be
    /// either one or many child-side managers it can message.
    ///
    /// Message managers that always have exactly one "other side" are of
    /// type nsIMessageSender.  Parent-side message managers that have many
    /// "other sides" are of type nsIMessageBroadcaster.
    ///
    /// Child-side message managers can send synchronous messages to their
    /// parent side, but not the other way around.
    ///
    /// There are two realms of message manager hierarchies.  One realm
    /// approximately corresponds to DOM elements, the other corresponds to
    /// process boundaries.
    ///
    /// Message managers corresponding to DOM elements
    /// ==============================================
    ///
    /// In this realm of message managers, there are
    /// - "frame message managers" which correspond to frame elements
    /// - "window message managers" which correspond to top-level chrome
    /// windows
    /// - the "global message manager", on the parent side.  See below.
    ///
    /// The DOM-realm message managers can communicate in the ways shown by
    /// the following diagram.  The parent side and child side can
    /// correspond to process boundaries, but don't always.
    ///
    /// Parent side                         Child side
    /// -------------                       ------------
    /// global MMg
    /// |
    /// +-->window MMw1
    /// |    |
    /// |    +-->frame MMp1_1<------------>frame MMc1_1
    /// |    |
    /// |    +-->frame MMp1_2<------------>frame MMc1_2
    /// |    ...
    /// |
    /// +-->window MMw2
    /// ...
    ///
    /// For example: a message sent from MMc1_1, from the child side, is
    /// sent only to MMp1_1 on the parent side.  However, note that all
    /// message managers in the hierarchy above MMp1_1, in this diagram
    /// MMw1 and MMg, will also notify their message listeners when the
    /// message arrives.
    /// For example: a message broadcast through the global MMg on the
    /// parent side would be broadcast to MMw1, which would transitively
    /// broadcast it to MMp1_1, MM1p_2".  The message would next be
    /// broadcast to MMw2, and so on down the hierarchy.
    ///
    /// ***** PERFORMANCE AND SECURITY WARNING *****
    /// Messages broadcast through the global MM and window MMs can result
    /// in messages being dispatched across many OS processes, and to many
    /// processes with different permissions.  Great care should be taken
    /// when broadcasting.
    ///
    /// Interfaces
    /// ----------
    ///
    /// The global MMg and window MMw's are message broadcasters implementing
    /// nsIMessageBroadcaster while the frame MMp's are simple message senders
    /// (nsIMessageSender). Their counterparts in the content processes are
    /// message senders implementing nsIContentFrameMessageManager.
    ///
    /// nsIMessageListenerManager
    /// /                           \
    /// nsIMessageSender                               nsIMessageBroadcaster
    /// |
    /// nsISyncMessageSender (content process/in-process only)
    /// |
    /// nsIContentFrameMessageManager (content process/in-process only)
    /// |
    /// nsIInProcessContentFrameMessageManager (in-process only)
    ///
    ///
    /// Message managers in the chrome process can also be QI'ed to nsIFrameScriptLoader.
    ///
    ///
    /// Message managers corresponding to process boundaries
    /// ====================================================
    ///
    /// The second realm of message managers is the "process message
    /// managers".  With one exception, these always correspond to process
    /// boundaries.  The picture looks like
    ///
    /// Parent process                      Child processes
    /// ----------------                    -----------------
    /// global PPMM
    /// |
    /// +<----> child PPMM
    /// |
    /// +-->parent PMM1<------------------>child process CMM1
    /// |
    /// +-->parent PMM2<------------------>child process PMM2
    /// ...
    ///
    /// For example: the parent-process PMM1 sends messages directly to
    /// only the child-process CMM1.
    ///
    /// For example: CMM1 sends messages directly to PMM1.  The global PPMM
    /// will also notify their message listeners when the message arrives.
    ///
    /// For example: messages sent through the global PPMM will be
    /// dispatched to the listeners of the same-process, "child PPMM".
    /// They will also be broadcast to PPM1, PPM2, etc.
    ///
    /// ***** PERFORMANCE AND SECURITY WARNING *****
    /// Messages broadcast through the global PPMM can result in messages
    /// being dispatched across many OS processes, and to many processes
    /// with different permissions.  Great care should be taken when
    /// broadcasting.
    ///
    /// Requests sent to parent-process message listeners should usually
    /// have replies scoped to the requesting CPMM.  The following pattern
    /// is common
    ///
    /// const ParentProcessListener = {
    /// receiveMessage: function(aMessage) {
    /// let childMM = aMessage.target.QueryInterface(Ci.nsIMessageSender);
    /// switch (aMessage.name) {
    /// case "Foo:Request":
    /// // service request
    /// childMM.sendAsyncMessage("Foo:Response", { data });
    /// }
    /// }
    /// };
    /// </summary>
	[ComImport()]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("2b44eb57-a9c6-4773-9a1e-fe0818739a4c")]
	public interface nsIMessageListener
	{
		
		/// <summary>
        /// This is for JS only.
        /// receiveMessage is called with one parameter, which has the following
        /// properties:
        /// {
        /// target:  %the target of the message. Either an element owning
        /// the message manager, or message manager itself if no
        /// element owns it%
        /// name:    %message name%,
        /// sync:    %true or false%.
        /// data:    %structured clone of the sent message data%,
        /// json:    %same as .data, deprecated%,
        /// objects: %array of handles or null, always null if sync is false%
        /// }
        /// @note objects property isn't implemented yet.
        ///
        /// Each listener is invoked with its own copy of the message
        /// parameter.
        ///
        /// When the listener is called, 'this' value is the target of the message.
        ///
        /// If the message is synchronous, the possible return value is
        /// returned as JSON (will be changed to use structured clones).
        /// When there are multiple listeners to sync messages, each
        /// listener's return value is sent back as an array.  |undefined|
        /// return values show up as undefined values in the array.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void ReceiveMessage();
	}
	
	/// <summary>nsIMessageListenerManager </summary>
	[ComImport()]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("9c37a142-3de3-4902-a1a4-133f37d5980a")]
	public interface nsIMessageListenerManager
	{
		
		/// <summary>
        /// Register |listener| to receive |messageName|.  All listener
        /// callbacks for a particular message are invoked when that message
        /// is received.
        ///
        /// The message manager holds a strong ref to |listener|.
        ///
        /// If the same listener registers twice for the same message, the
        /// second registration is ignored.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void AddMessageListener([MarshalAs(UnmanagedType.LPStruct)] nsAStringBase messageName, [MarshalAs(UnmanagedType.Interface)] nsIMessageListener listener);
		
		/// <summary>
        /// No longer invoke |listener| when |messageName| is received, after
        /// the first time removeMessageListener() is called.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void RemoveMessageListener([MarshalAs(UnmanagedType.LPStruct)] nsAStringBase messageName, [MarshalAs(UnmanagedType.Interface)] nsIMessageListener listener);
		
		/// <summary>Member MarkForCC </summary>
		/// <returns>A System.Boolean</returns>
		[return: MarshalAs(UnmanagedType.U1)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		bool MarkForCC();
	}
	
	/// <summary>
    /// Message "senders" have a single "other side" to which messages are
    /// sent.  For example, a child-process message manager will send
    /// messages that are only delivered to its one parent-process message
    /// manager.
    /// </summary>
	[ComImport()]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("7f23767d-0f39-40c1-a22d-d3ab8a481f9d")]
	public interface nsIMessageSender : nsIMessageListenerManager
	{
		
		/// <summary>
        /// Register |listener| to receive |messageName|.  All listener
        /// callbacks for a particular message are invoked when that message
        /// is received.
        ///
        /// The message manager holds a strong ref to |listener|.
        ///
        /// If the same listener registers twice for the same message, the
        /// second registration is ignored.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		new void AddMessageListener([MarshalAs(UnmanagedType.LPStruct)] nsAStringBase messageName, [MarshalAs(UnmanagedType.Interface)] nsIMessageListener listener);
		
		/// <summary>
        /// No longer invoke |listener| when |messageName| is received, after
        /// the first time removeMessageListener() is called.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		new void RemoveMessageListener([MarshalAs(UnmanagedType.LPStruct)] nsAStringBase messageName, [MarshalAs(UnmanagedType.Interface)] nsIMessageListener listener);
		
		[return: MarshalAs(UnmanagedType.U1)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		new bool MarkForCC();
		
		/// <summary>
        /// Send |messageName| and |obj| to the "other side" of this message
        /// manager.  This invokes listeners who registered for
        /// |messageName|.
        ///
        /// See nsIMessageListener::receiveMessage() for the format of the
        /// data delivered to listeners.
        /// @throws NS_ERROR_NOT_INITIALIZED if the sender is not initialized.  For
        /// example, we will throw NS_ERROR_NOT_INITIALIZED if we try to send
        /// a message to a cross-process frame but the other process has not
        /// yet been set up.
        /// @throws NS_ERROR_FAILURE when the message receiver cannot be found.  For
        /// example, we will throw NS_ERROR_FAILURE if we try to send a message
        /// to a cross-process frame whose process has crashed.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void SendAsyncMessage([MarshalAs(UnmanagedType.LPStruct)] nsAStringBase messageName, Gecko.JsVal obj, System.IntPtr jsContext, int argc);
	}
	
	/// <summary>
    /// Message "broadcasters" don't have a single "other side" that they
    /// send messages to, but rather a set of subordinate message managers.
    /// For example, broadcasting a message through a window message
    /// manager will broadcast the message to all frame message managers
    /// within its window.
    /// </summary>
	[ComImport()]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("d36346b9-5d3b-497d-9c28-ffbc3e4f6d0d")]
	public interface nsIMessageBroadcaster : nsIMessageListenerManager
	{
		
		/// <summary>
        /// Register |listener| to receive |messageName|.  All listener
        /// callbacks for a particular message are invoked when that message
        /// is received.
        ///
        /// The message manager holds a strong ref to |listener|.
        ///
        /// If the same listener registers twice for the same message, the
        /// second registration is ignored.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		new void AddMessageListener([MarshalAs(UnmanagedType.LPStruct)] nsAStringBase messageName, [MarshalAs(UnmanagedType.Interface)] nsIMessageListener listener);
		
		/// <summary>
        /// No longer invoke |listener| when |messageName| is received, after
        /// the first time removeMessageListener() is called.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		new void RemoveMessageListener([MarshalAs(UnmanagedType.LPStruct)] nsAStringBase messageName, [MarshalAs(UnmanagedType.Interface)] nsIMessageListener listener);
		
		[return: MarshalAs(UnmanagedType.U1)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		new bool MarkForCC();
		
		/// <summary>
        /// Like |sendAsyncMessage()|, but also broadcasts this message to
        /// all "child" message managers of this message manager.  See long
        /// comment above for details.
        ///
        /// WARNING: broadcasting messages can be very expensive and leak
        /// sensitive data.  Use with extreme caution.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void BroadcastAsyncMessage([MarshalAs(UnmanagedType.LPStruct)] nsAStringBase messageName, Gecko.JsVal obj, System.IntPtr jsContext, int argc);
		
		/// <summary>
        /// Number of subordinate message managers.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		uint GetChildCountAttribute();
		
		/// <summary>
        /// Return a single subordinate message manager.
        /// </summary>
		[return: MarshalAs(UnmanagedType.Interface)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		nsIMessageListenerManager GetChildAt(uint aIndex);
	}
	
	/// <summary>nsISyncMessageSender </summary>
	[ComImport()]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("83be5862-2996-4685-ae7d-ae25bd795d50")]
	public interface nsISyncMessageSender : nsIMessageSender
	{
		
		/// <summary>
        /// Register |listener| to receive |messageName|.  All listener
        /// callbacks for a particular message are invoked when that message
        /// is received.
        ///
        /// The message manager holds a strong ref to |listener|.
        ///
        /// If the same listener registers twice for the same message, the
        /// second registration is ignored.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		new void AddMessageListener([MarshalAs(UnmanagedType.LPStruct)] nsAStringBase messageName, [MarshalAs(UnmanagedType.Interface)] nsIMessageListener listener);
		
		/// <summary>
        /// No longer invoke |listener| when |messageName| is received, after
        /// the first time removeMessageListener() is called.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		new void RemoveMessageListener([MarshalAs(UnmanagedType.LPStruct)] nsAStringBase messageName, [MarshalAs(UnmanagedType.Interface)] nsIMessageListener listener);
		
		/// <summary>Member MarkForCC </summary>
		/// <returns>A System.Boolean</returns>
		[return: MarshalAs(UnmanagedType.U1)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		new bool MarkForCC();
		
		/// <summary>
        /// Send |messageName| and |obj| to the "other side" of this message
        /// manager.  This invokes listeners who registered for
        /// |messageName|.
        ///
        /// See nsIMessageListener::receiveMessage() for the format of the
        /// data delivered to listeners.
        /// @throws NS_ERROR_NOT_INITIALIZED if the sender is not initialized.  For
        /// example, we will throw NS_ERROR_NOT_INITIALIZED if we try to send
        /// a message to a cross-process frame but the other process has not
        /// yet been set up.
        /// @throws NS_ERROR_FAILURE when the message receiver cannot be found.  For
        /// example, we will throw NS_ERROR_FAILURE if we try to send a message
        /// to a cross-process frame whose process has crashed.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		new void SendAsyncMessage([MarshalAs(UnmanagedType.LPStruct)] nsAStringBase messageName, Gecko.JsVal obj, System.IntPtr jsContext, int argc);
		
		/// <summary>
        /// Like |sendAsyncMessage()|, except blocks the sender until all
        /// listeners of the message have been invoked.  Returns an array
        /// containing return values from each listener invoked.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		Gecko.JsVal SendSyncMessage([MarshalAs(UnmanagedType.LPStruct)] nsAStringBase messageName, Gecko.JsVal obj, System.IntPtr jsContext, int argc);
	}
	
	/// <summary>nsIContentFrameMessageManager </summary>
	[ComImport()]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("894ff2d4-39a3-4df8-9d76-8ee329975488")]
	public interface nsIContentFrameMessageManager : nsISyncMessageSender
	{
		
		/// <summary>
        /// Register |listener| to receive |messageName|.  All listener
        /// callbacks for a particular message are invoked when that message
        /// is received.
        ///
        /// The message manager holds a strong ref to |listener|.
        ///
        /// If the same listener registers twice for the same message, the
        /// second registration is ignored.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		new void AddMessageListener([MarshalAs(UnmanagedType.LPStruct)] nsAStringBase messageName, [MarshalAs(UnmanagedType.Interface)] nsIMessageListener listener);
		
		/// <summary>
        /// No longer invoke |listener| when |messageName| is received, after
        /// the first time removeMessageListener() is called.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		new void RemoveMessageListener([MarshalAs(UnmanagedType.LPStruct)] nsAStringBase messageName, [MarshalAs(UnmanagedType.Interface)] nsIMessageListener listener);
		
		/// <summary>Member MarkForCC </summary>
		/// <returns>A System.Boolean</returns>
		[return: MarshalAs(UnmanagedType.U1)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		new bool MarkForCC();
		
		/// <summary>
        /// Send |messageName| and |obj| to the "other side" of this message
        /// manager.  This invokes listeners who registered for
        /// |messageName|.
        ///
        /// See nsIMessageListener::receiveMessage() for the format of the
        /// data delivered to listeners.
        /// @throws NS_ERROR_NOT_INITIALIZED if the sender is not initialized.  For
        /// example, we will throw NS_ERROR_NOT_INITIALIZED if we try to send
        /// a message to a cross-process frame but the other process has not
        /// yet been set up.
        /// @throws NS_ERROR_FAILURE when the message receiver cannot be found.  For
        /// example, we will throw NS_ERROR_FAILURE if we try to send a message
        /// to a cross-process frame whose process has crashed.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		new void SendAsyncMessage([MarshalAs(UnmanagedType.LPStruct)] nsAStringBase messageName, Gecko.JsVal obj, System.IntPtr jsContext, int argc);
		
		/// <summary>
        /// Like |sendAsyncMessage()|, except blocks the sender until all
        /// listeners of the message have been invoked.  Returns an array
        /// containing return values from each listener invoked.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		new Gecko.JsVal SendSyncMessage([MarshalAs(UnmanagedType.LPStruct)] nsAStringBase messageName, Gecko.JsVal obj, System.IntPtr jsContext, int argc);
		
		/// <summary>
        /// The current top level window in the frame or null.
        /// </summary>
		[return: MarshalAs(UnmanagedType.Interface)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		nsIDOMWindow GetContentAttribute();
		
		/// <summary>
        /// The top level docshell or null.
        /// </summary>
		[return: MarshalAs(UnmanagedType.Interface)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		nsIDocShell GetDocShellAttribute();
		
		/// <summary>
        /// Print a string to stdout.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void Dump([MarshalAs(UnmanagedType.LPStruct)] nsAStringBase aStr);
		
		/// <summary>
        /// If leak detection is enabled, print a note to the leak log that this
        /// process will intentionally crash.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void PrivateNoteIntentionalCrash();
		
		/// <summary>
        /// Ascii base64 data to binary data and vice versa
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void Atob([MarshalAs(UnmanagedType.LPStruct)] nsAStringBase aAsciiString, [MarshalAs(UnmanagedType.LPStruct)] nsAStringBase retval);
		
		/// <summary>Member Btoa </summary>
		/// <param name='aBase64Data'> </param>
		/// <param name='retval'> </param>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void Btoa([MarshalAs(UnmanagedType.LPStruct)] nsAStringBase aBase64Data, [MarshalAs(UnmanagedType.LPStruct)] nsAStringBase retval);
	}
	
	/// <summary>nsIInProcessContentFrameMessageManager </summary>
	[ComImport()]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("a2325927-9c0c-437d-9215-749c79235031")]
	public interface nsIInProcessContentFrameMessageManager : nsIContentFrameMessageManager
	{
		
		/// <summary>
        /// Register |listener| to receive |messageName|.  All listener
        /// callbacks for a particular message are invoked when that message
        /// is received.
        ///
        /// The message manager holds a strong ref to |listener|.
        ///
        /// If the same listener registers twice for the same message, the
        /// second registration is ignored.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		new void AddMessageListener([MarshalAs(UnmanagedType.LPStruct)] nsAStringBase messageName, [MarshalAs(UnmanagedType.Interface)] nsIMessageListener listener);
		
		/// <summary>
        /// No longer invoke |listener| when |messageName| is received, after
        /// the first time removeMessageListener() is called.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		new void RemoveMessageListener([MarshalAs(UnmanagedType.LPStruct)] nsAStringBase messageName, [MarshalAs(UnmanagedType.Interface)] nsIMessageListener listener);
		
		/// <summary>Member MarkForCC </summary>
		/// <returns>A System.Boolean</returns>
		[return: MarshalAs(UnmanagedType.U1)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		new bool MarkForCC();
		
		/// <summary>
        /// Send |messageName| and |obj| to the "other side" of this message
        /// manager.  This invokes listeners who registered for
        /// |messageName|.
        ///
        /// See nsIMessageListener::receiveMessage() for the format of the
        /// data delivered to listeners.
        /// @throws NS_ERROR_NOT_INITIALIZED if the sender is not initialized.  For
        /// example, we will throw NS_ERROR_NOT_INITIALIZED if we try to send
        /// a message to a cross-process frame but the other process has not
        /// yet been set up.
        /// @throws NS_ERROR_FAILURE when the message receiver cannot be found.  For
        /// example, we will throw NS_ERROR_FAILURE if we try to send a message
        /// to a cross-process frame whose process has crashed.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		new void SendAsyncMessage([MarshalAs(UnmanagedType.LPStruct)] nsAStringBase messageName, Gecko.JsVal obj, System.IntPtr jsContext, int argc);
		
		/// <summary>
        /// Like |sendAsyncMessage()|, except blocks the sender until all
        /// listeners of the message have been invoked.  Returns an array
        /// containing return values from each listener invoked.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		new Gecko.JsVal SendSyncMessage([MarshalAs(UnmanagedType.LPStruct)] nsAStringBase messageName, Gecko.JsVal obj, System.IntPtr jsContext, int argc);
		
		/// <summary>
        /// The current top level window in the frame or null.
        /// </summary>
		[return: MarshalAs(UnmanagedType.Interface)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		new nsIDOMWindow GetContentAttribute();
		
		/// <summary>
        /// The top level docshell or null.
        /// </summary>
		[return: MarshalAs(UnmanagedType.Interface)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		new nsIDocShell GetDocShellAttribute();
		
		/// <summary>
        /// Print a string to stdout.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		new void Dump([MarshalAs(UnmanagedType.LPStruct)] nsAStringBase aStr);
		
		/// <summary>
        /// If leak detection is enabled, print a note to the leak log that this
        /// process will intentionally crash.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		new void PrivateNoteIntentionalCrash();
		
		/// <summary>
        /// Ascii base64 data to binary data and vice versa
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		new void Atob([MarshalAs(UnmanagedType.LPStruct)] nsAStringBase aAsciiString, [MarshalAs(UnmanagedType.LPStruct)] nsAStringBase retval);
		
		/// <summary>Member Btoa </summary>
		/// <param name='aBase64Data'> </param>
		/// <param name='retval'> </param>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		new void Btoa([MarshalAs(UnmanagedType.LPStruct)] nsAStringBase aBase64Data, [MarshalAs(UnmanagedType.LPStruct)] nsAStringBase retval);
		
		/// <summary>Member GetOwnerContent </summary>
		/// <returns>A System.IntPtr</returns>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		System.IntPtr GetOwnerContent();
	}
	
	/// <summary>nsIFrameScriptLoader </summary>
	[ComImport()]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("a54acd34-4141-46f5-b71b-e2ca32879b08")]
	public interface nsIFrameScriptLoader
	{
		
		/// <summary>
        /// Load a script in the (remote) frame. aURL must be the absolute URL.
        /// data: URLs are also supported. For example data:,dump("foo\n");
        /// If aAllowDelayedLoad is true, script will be loaded when the
        /// remote frame becomes available. Otherwise the script will be loaded
        /// only if the frame is already available.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void LoadFrameScript([MarshalAs(UnmanagedType.LPStruct)] nsAStringBase aURL, [MarshalAs(UnmanagedType.U1)] bool aAllowDelayedLoad);
		
		/// <summary>
        /// Removes aURL from the list of scripts which support delayed load.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void RemoveDelayedFrameScript([MarshalAs(UnmanagedType.LPStruct)] nsAStringBase aURL);
	}
	
	/// <summary>nsIProcessChecker </summary>
	[ComImport()]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("134ccbf0-5c08-11e2-bcfd-0800200c9a66")]
	public interface nsIProcessChecker
	{
		
		/// <summary>
        /// Return true iff the "remote" process has |aPermission|.  This is
        /// intended to be used by JS implementations of cross-process DOM
        /// APIs, like so
        ///
        /// recvFooRequest: function(message) {
        /// if (!message.target.assertPermission("foo")) {
        /// return false;
        /// }
        /// // service foo request
        ///
        /// This interface only returns meaningful data when our content is
        /// in a separate process.  If it shares the same OS process as us,
        /// then applying this permission check doesn't add any security,
        /// though it doesn't hurt anything either.
        ///
        /// Note: If the remote content process does *not* have |aPermission|,
        /// it will be killed as a precaution.
        /// </summary>
		[return: MarshalAs(UnmanagedType.U1)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		bool AssertPermission([MarshalAs(UnmanagedType.LPStruct)] nsAStringBase aPermission);
		
		/// <summary>
        /// Return true iff the "remote" process has |aManifestURL|.  This is
        /// intended to be used by JS implementations of cross-process DOM
        /// APIs, like so
        ///
        /// recvFooRequest: function(message) {
        /// if (!message.target.assertContainApp("foo")) {
        /// return false;
        /// }
        /// // service foo request
        ///
        /// This interface only returns meaningful data when our content is
        /// in a separate process.  If it shares the same OS process as us,
        /// then applying this manifest URL check doesn't add any security,
        /// though it doesn't hurt anything either.
        ///
        /// Note: If the remote content process does *not* contain |aManifestURL|,
        /// it will be killed as a precaution.
        /// </summary>
		[return: MarshalAs(UnmanagedType.U1)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		bool AssertContainApp([MarshalAs(UnmanagedType.LPStruct)] nsAStringBase aManifestURL);
		
		/// <summary>Member AssertAppHasPermission </summary>
		/// <param name='aPermission'> </param>
		/// <returns>A System.Boolean</returns>
		[return: MarshalAs(UnmanagedType.U1)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		bool AssertAppHasPermission([MarshalAs(UnmanagedType.LPStruct)] nsAStringBase aPermission);
	}
}
