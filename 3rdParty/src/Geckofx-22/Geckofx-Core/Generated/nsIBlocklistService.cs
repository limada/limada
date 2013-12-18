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
// Generated by IDLImporter from file nsIBlocklistService.idl
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
    ///This Source Code Form is subject to the terms of the Mozilla Public
    /// License, v. 2.0. If a copy of the MPL was not distributed with this
    /// file, You can obtain one at http://mozilla.org/MPL/2.0/. </summary>
	[ComImport()]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("cbba15b8-316d-4ae6-8ed9-fe9cf8386730")]
	public interface nsIBlocklistService
	{
		
		/// <summary>
        /// Determine if an item is blocklisted
        /// @param   id
        /// The ID of the item.
        /// @param   version
        /// The item's version.
        /// @param   appVersion
        /// The version of the application we are checking in the blocklist.
        /// If this parameter is null, the version of the running application
        /// is used.
        /// @param   toolkitVersion
        /// The version of the toolkit we are checking in the blocklist.
        /// If this parameter is null, the version of the running toolkit
        /// is used.
        /// @returns true if the item is compatible with this version of the
        /// application or this version of the toolkit, false, otherwise.
        /// </summary>
		[return: MarshalAs(UnmanagedType.U1)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		bool IsAddonBlocklisted([MarshalAs(UnmanagedType.LPStruct)] nsAStringBase id, [MarshalAs(UnmanagedType.LPStruct)] nsAStringBase version, [MarshalAs(UnmanagedType.LPStruct)] nsAStringBase appVersion, [MarshalAs(UnmanagedType.LPStruct)] nsAStringBase toolkitVersion);
		
		/// <summary>
        /// Determine the blocklist state of an add-on
        /// @param   id
        /// The ID of the item.
        /// @param   version
        /// The item's version.
        /// @param   appVersion
        /// The version of the application we are checking in the blocklist.
        /// If this parameter is null, the version of the running application
        /// is used.
        /// @param   toolkitVersion
        /// The version of the toolkit we are checking in the blocklist.
        /// If this parameter is null, the version of the running toolkit
        /// is used.
        /// @returns The STATE constant.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		uint GetAddonBlocklistState([MarshalAs(UnmanagedType.LPStruct)] nsAStringBase id, [MarshalAs(UnmanagedType.LPStruct)] nsAStringBase version, [MarshalAs(UnmanagedType.LPStruct)] nsAStringBase appVersion, [MarshalAs(UnmanagedType.LPStruct)] nsAStringBase toolkitVersion);
		
		/// <summary>
        /// Determine the blocklist state of a plugin
        /// @param   plugin
        /// The plugin to get the state for
        /// @param   appVersion
        /// The version of the application we are checking in the blocklist.
        /// If this parameter is null, the version of the running application
        /// is used.
        /// @param   toolkitVersion
        /// The version of the toolkit we are checking in the blocklist.
        /// If this parameter is null, the version of the running toolkit
        /// is used.
        /// @returns The STATE constant.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		uint GetPluginBlocklistState([MarshalAs(UnmanagedType.Interface)] nsIPluginTag plugin, [MarshalAs(UnmanagedType.LPStruct)] nsAStringBase appVersion, [MarshalAs(UnmanagedType.LPStruct)] nsAStringBase toolkitVersion);
		
		/// <summary>
        /// Determine the blocklist web page of an add-on.
        /// @param   id
        /// The ID of the blocked add-on.
        /// @returns The URL of the description page.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void GetAddonBlocklistURL([MarshalAs(UnmanagedType.LPStruct)] nsAStringBase id, [MarshalAs(UnmanagedType.LPStruct)] nsAStringBase version, [MarshalAs(UnmanagedType.LPStruct)] nsAStringBase appVersion, [MarshalAs(UnmanagedType.LPStruct)] nsAStringBase toolkitVersion, [MarshalAs(UnmanagedType.LPStruct)] nsAStringBase retval);
		
		/// <summary>
        /// Determine the blocklist web page of a plugin.
        /// @param   plugin
        /// The blocked plugin that we are determining the web page for.
        /// @returns The URL of the description page.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void GetPluginBlocklistURL([MarshalAs(UnmanagedType.Interface)] nsIPluginTag plugin, [MarshalAs(UnmanagedType.LPStruct)] nsAStringBase retval);
	}
	
	/// <summary>nsIBlocklistServiceConsts </summary>
	public class nsIBlocklistServiceConsts
	{
		
		// <summary>
        // Indicates that the item does not appear in the blocklist.
        // </summary>
		public const ulong STATE_NOT_BLOCKED = 0;
		
		// <summary>
        // enough to warant forcibly blocking.
        // </summary>
		public const ulong STATE_SOFTBLOCKED = 1;
		
		// <summary>
        // Indicates that the item should be blocked and never used.
        // </summary>
		public const ulong STATE_BLOCKED = 2;
		
		// <summary>
        // update available.
        // </summary>
		public const ulong STATE_OUTDATED = 3;
		
		// <summary>
        // Indicates that the item is vulnerable and there is an update.
        // </summary>
		public const ulong STATE_VULNERABLE_UPDATE_AVAILABLE = 4;
		
		// <summary>
        // Indicates that the item is vulnerable and there is no update.
        // </summary>
		public const ulong STATE_VULNERABLE_NO_UPDATE = 5;
	}
	
	/// <summary>
    /// nsIBlocklistPrompt is used, if available, by the default implementation of
    /// nsIBlocklistService to display a confirmation UI to the user before blocking
    /// extensions/plugins.
    /// </summary>
	[ComImport()]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("36f97f40-b0c9-11df-94e2-0800200c9a66")]
	public interface nsIBlocklistPrompt
	{
		
		/// <summary>
        /// Prompt the user about newly blocked addons. The prompt is then resposible
        /// for soft-blocking any addons that need to be afterwards
        ///
        /// @param  aAddons
        /// An array of addons and plugins that are blocked. These are javascript
        /// objects with properties:
        /// name    - the plugin or extension name,
        /// version - the version of the extension or plugin,
        /// icon    - the plugin or extension icon,
        /// disable - can be used by the nsIBlocklistPrompt to allows users to decide
        /// whether a soft-blocked add-on should be disabled,
        /// blocked - true if the item is hard-blocked, false otherwise,
        /// item    - the nsIPluginTag or Addon object
        /// @param  aCount
        /// The number of addons
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void Prompt([MarshalAs(UnmanagedType.LPArray, SizeParamIndex=1)] nsIVariant[] aAddons, uint aCount);
	}
}
