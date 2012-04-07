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
// Generated by IDLImporter from file rdfIDataSource.idl
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
    /// Interface used in RDF to describe data sources.
    ///
    /// @status PLASMA
    /// </summary>
	[ComImport()]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("ebce86bd-1568-4a34-a808-9ccf9cde8087")]
	public interface rdfIDataSource
	{
		
		/// <summary>
        /// Visit all the subject resources in the datasource. The order is
        /// intederminate and may change from one invocation to the next.
        /// The subjects will be in the aSubject argument in calls into
        /// aVisitor, aPredicate and aObject will be null.
        /// @note Implementations may throw NS_ERROR_NOT_IMPLEMENTED for
        /// this method, but in this case RDF serializations of this
        /// datasource will not be possible.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void VisitAllSubjects(rdfITripleVisitor aVisitor);
		
		/// <summary>
        /// Visit all the triples in the datasource. The order is
        /// intederminate and may change from one invocation to the next.
        /// @note Implementations may throw NS_ERROR_NOT_IMPLEMENTED for
        /// this method, but in this case RDF serializations of this
        /// datasource will not be possible.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void VisitAllTriples(rdfITripleVisitor aVisitor);
	}
}
