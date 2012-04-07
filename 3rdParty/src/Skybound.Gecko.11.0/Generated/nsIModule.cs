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
// Generated by IDLImporter from file nsIModule.idl
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
    /// The nsIModule interface.
    /// </summary>
	[ComImport()]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("7392D032-5371-11d3-994E-00805FD26FEE")]
	public interface nsIModule
	{
		
		/// <summary>
        /// Object Instance Creation
        ///
        /// Obtains a Class Object from a nsIModule for a given CID and IID pair.
        /// This class object can either be query to a nsIFactory or a may be
        /// query to a nsIClassInfo.
        ///
        /// @param aCompMgr  : The global component manager
        /// @param aClass    : ClassID of object instance requested
        /// @param aIID      : IID of interface requested
        ///
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		System.IntPtr GetClassObject([MarshalAs(UnmanagedType.Interface)] nsIComponentManager aCompMgr, ref System.Guid aClass, ref System.Guid aIID);
		
		/// <summary>
        /// One time registration callback
        ///
        /// When the nsIModule is discovered, this method will be
        /// called so that any setup registration can be preformed.
        ///
        /// @param aCompMgr  : The global component manager
        /// @param aLocation : The location of the nsIModule on disk
        /// @param aLoaderStr: Opaque loader specific string
        /// @param aType     : Loader Type being used to load this module
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void RegisterSelf([MarshalAs(UnmanagedType.Interface)] nsIComponentManager aCompMgr, [MarshalAs(UnmanagedType.Interface)] nsIFile aLocation, [MarshalAs(UnmanagedType.LPStr)] string aLoaderStr, [MarshalAs(UnmanagedType.LPStr)] string aType);
		
		/// <summary>
        /// One time unregistration callback
        ///
        /// When the nsIModule is being unregistered, this method will be
        /// called so that any unregistration can be preformed
        ///
        /// @param aCompMgr   : The global component manager
        /// @param aLocation  : The location of the nsIModule on disk
        /// @param aLoaderStr : Opaque loader specific string
        ///
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void UnregisterSelf([MarshalAs(UnmanagedType.Interface)] nsIComponentManager aCompMgr, [MarshalAs(UnmanagedType.Interface)] nsIFile aLocation, [MarshalAs(UnmanagedType.LPStr)] string aLoaderStr);
		
		/// <summary>
        /// Module load management
        ///
        /// @param aCompMgr  : The global component manager
        ///
        /// @return indicates to the caller if the module can be unloaded.
        /// Returning PR_TRUE isn't a guarantee that the module will be
        /// unloaded. It constitues only willingness of the module to be
        /// unloaded.  It is very important to ensure that no outstanding
        /// references to the module's code/data exist before returning
        /// PR_TRUE.
        /// Returning PR_FALSE guaratees that the module won't be unloaded.
        /// </summary>
		[return: MarshalAs(UnmanagedType.U1)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		bool CanUnload([MarshalAs(UnmanagedType.Interface)] nsIComponentManager aCompMgr);
	}
}
