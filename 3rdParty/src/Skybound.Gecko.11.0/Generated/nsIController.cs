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
// Generated by IDLImporter from file nsIController.idl
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
	
	
	/// <summary>nsIController </summary>
	[ComImport()]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("D5B61B82-1DA4-11d3-BF87-00105A1B0627")]
	public interface nsIController
	{
		
		/// <summary>Member IsCommandEnabled </summary>
		/// <param name='command'> </param>
		/// <returns>A System.Boolean</returns>
		[return: MarshalAs(UnmanagedType.U1)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		bool IsCommandEnabled([MarshalAs(UnmanagedType.LPStr)] string command);
		
		/// <summary>Member SupportsCommand </summary>
		/// <param name='command'> </param>
		/// <returns>A System.Boolean</returns>
		[return: MarshalAs(UnmanagedType.U1)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		bool SupportsCommand([MarshalAs(UnmanagedType.LPStr)] string command);
		
		/// <summary>Member DoCommand </summary>
		/// <param name='command'> </param>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void DoCommand([MarshalAs(UnmanagedType.LPStr)] string command);
		
		/// <summary>Member OnEvent </summary>
		/// <param name='eventName'> </param>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void OnEvent([MarshalAs(UnmanagedType.LPStr)] string eventName);
	}
	
	/// <summary>
    ///Enhanced controller interface that allows for passing of parameters
    ///  to commands.
    /// </summary>
	[ComImport()]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("EBE55080-C8A9-11D5-A73C-DD620D6E04BC")]
	public interface nsICommandController
	{
		
		/// <summary>
        ///Enhanced controller interface that allows for passing of parameters
        ///  to commands.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void GetCommandStateWithParams([MarshalAs(UnmanagedType.LPStr)] string command, [MarshalAs(UnmanagedType.Interface)] nsICommandParams aCommandParams);
		
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void DoCommandWithParams([MarshalAs(UnmanagedType.LPStr)] string command, [MarshalAs(UnmanagedType.Interface)] nsICommandParams aCommandParams);
	}
	
	/// <summary>
    ///An API for registering commands in groups, to allow for
    ///  updating via nsIDOMWindow::UpdateCommands. </summary>
	[ComImport()]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("9F82C404-1C7B-11D5-A73C-ECA43CA836FC")]
	public interface nsIControllerCommandGroup
	{
		
		/// <summary>
        ///An API for registering commands in groups, to allow for
        ///  updating via nsIDOMWindow::UpdateCommands. </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void AddCommandToGroup([MarshalAs(UnmanagedType.LPStr)] string aCommand, [MarshalAs(UnmanagedType.LPStr)] string aGroup);
		
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void RemoveCommandFromGroup([MarshalAs(UnmanagedType.LPStr)] string aCommand, [MarshalAs(UnmanagedType.LPStr)] string aGroup);
		
		[return: MarshalAs(UnmanagedType.U1)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		bool IsCommandInGroup([MarshalAs(UnmanagedType.LPStr)] string aCommand, [MarshalAs(UnmanagedType.LPStr)] string aGroup);
		
		/// <summary>
        ///We should expose some methods that allow for enumeration.
        /// </summary>
		[return: MarshalAs(UnmanagedType.Interface)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		nsISimpleEnumerator GetGroupsEnumerator();
		
		[return: MarshalAs(UnmanagedType.Interface)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		nsISimpleEnumerator GetEnumeratorForGroup([MarshalAs(UnmanagedType.LPStr)] string aGroup);
	}
}
