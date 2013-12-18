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
// Generated by IDLImporter from file nsIDOMCSSConditionRule.idl
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
    /// Interface in the CSS OM for at-rules that conditionally apply their
    /// child rules.
    /// </summary>
	[ComImport()]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("942754f2-2c0e-461b-9c10-c0e929504fe1")]
	public interface nsIDOMCSSConditionRule : nsIDOMCSSGroupingRule
	{
		
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		new ushort GetTypeAttribute();
		
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		new void GetCssTextAttribute([MarshalAs(UnmanagedType.LPStruct)] nsAStringBase aCssText);
		
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		new void SetCssTextAttribute([MarshalAs(UnmanagedType.LPStruct)] nsAStringBase aCssText);
		
		/// <summary>
        /// raises(DOMException) on setting
        /// </summary>
		[return: MarshalAs(UnmanagedType.Interface)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		new nsIDOMCSSStyleSheet GetParentStyleSheetAttribute();
		
		[return: MarshalAs(UnmanagedType.Interface)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		new nsIDOMCSSRule GetParentRuleAttribute();
		
		/// <summary>
        /// Interface for at-rules that have child rules in the CSS OM.
        /// </summary>
		[return: MarshalAs(UnmanagedType.Interface)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		new nsIDOMCSSRuleList GetCssRulesAttribute();
		
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		new uint InsertRule([MarshalAs(UnmanagedType.LPStruct)] nsAStringBase rule, uint index);
		
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		new void DeleteRule(uint index);
		
		/// <summary>
        /// Interface in the CSS OM for at-rules that conditionally apply their
        /// child rules.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void GetConditionTextAttribute([MarshalAs(UnmanagedType.LPStruct)] nsAStringBase aConditionText);
		
		/// <summary>
        /// Interface in the CSS OM for at-rules that conditionally apply their
        /// child rules.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void SetConditionTextAttribute([MarshalAs(UnmanagedType.LPStruct)] nsAStringBase aConditionText);
	}
}
