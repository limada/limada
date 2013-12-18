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
// Generated by IDLImporter from file nsIRDFDelegateFactory.idl
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
    /// This interface should be implemented by an XPCOM factory that
    /// is registered to handle "@mozilla.org/rdf/delegate-factory/[key]/[scheme];1"
    /// ContractIDs.
    ///
    /// The factory will be invoked to create delegate objects from
    /// nsIRDFResource::GetDelegate().
    /// </summary>
	[ComImport()]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("A1B89470-A124-11d3-BE59-0020A6361667")]
	public interface nsIRDFDelegateFactory
	{
		
		/// <summary>
        /// Create a delegate for the specified RDF resource.
        ///
        /// The created delegate should forward AddRef() and Release()
        /// calls to the aOuter object.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		System.IntPtr CreateDelegate([MarshalAs(UnmanagedType.Interface)] nsIRDFResource aOuter, [MarshalAs(UnmanagedType.LPStr)] string aKey, ref System.Guid aIID);
	}
}
