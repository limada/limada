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
// Generated by IDLImporter from file nsIXPCSecurityManager.idl
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
	
	
	/// <summary>nsIXPCSecurityManager </summary>
	[ComImport()]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("31431440-f1ce-11d2-985a-006008962422")]
	public interface nsIXPCSecurityManager
	{
		
		/// <summary>
        /// For each of these hooks returning NS_OK means 'let the action continue'.
        /// Returning an error code means 'veto the action'. XPConnect will return
        /// JS_FALSE to the js engine if the action is vetoed. The implementor of this
        /// interface is responsible for setting a JS exception into the JSContext
        /// if that is appropriate.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void CanCreateWrapper(System.IntPtr aJSContext, ref System.Guid aIID, [MarshalAs(UnmanagedType.Interface)] nsISupports aObj, [MarshalAs(UnmanagedType.Interface)] nsIClassInfo aClassInfo, ref System.IntPtr aPolicy);
		
		/// <summary>Member CanCreateInstance </summary>
		/// <param name='aJSContext'> </param>
		/// <param name='aCID'> </param>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void CanCreateInstance(System.IntPtr aJSContext, ref System.Guid aCID);
		
		/// <summary>Member CanGetService </summary>
		/// <param name='aJSContext'> </param>
		/// <param name='aCID'> </param>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void CanGetService(System.IntPtr aJSContext, ref System.Guid aCID);
		
		/// <summary>
        /// Used for aAction below
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void CanAccess(uint aAction, System.IntPtr aCallContext, System.IntPtr aJSContext, System.IntPtr aJSObject, [MarshalAs(UnmanagedType.Interface)] nsISupports aObj, [MarshalAs(UnmanagedType.Interface)] nsIClassInfo aClassInfo, System.IntPtr aName, ref System.IntPtr aPolicy);
	}
}
