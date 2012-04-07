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
// Generated by IDLImporter from file nsILoginManagerStorage.idl
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
    /// NOTE: This interface is intended to be implemented by modules
    /// providing storage mechanisms for the login manager.
    /// Other code should use the login manager's interfaces
    /// (nsILoginManager), and should not call storage modules
    /// directly.
    /// </summary>
	[ComImport()]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("32a4f9f1-60a8-4971-b54e-71ad661483ae")]
	public interface nsILoginManagerStorage
	{
		
		/// <summary>
        /// Initialize the component. Not invoked automatically.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void Init();
		
		/// <summary>
        /// Initialize the component, but override the default filename
        /// locations. This is primarily used to the unit tests and profile
        /// migration.
        ///
        /// @param aInputFile
        /// File to read for stored logins.
        /// @param aOutputFile
        /// If non-null, file to output logins to.
        ///
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void InitWithFile([MarshalAs(UnmanagedType.Interface)] nsIFile aInputFile, [MarshalAs(UnmanagedType.Interface)] nsIFile aOutputFile);
		
		/// <summary>
        /// Store a new login in the storage module.
        ///
        /// @param aLogin
        /// The login to be added.
        ///
        /// Default values for the login's nsILoginMetaInfo properties will be
        /// created. However, if the caller specifies non-default values, they will
        /// be used instead.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void AddLogin([MarshalAs(UnmanagedType.Interface)] nsILoginInfo aLogin);
		
		/// <summary>
        /// Remove a login from the storage module.
        ///
        /// @param aLogin
        /// The login to be removed.
        ///
        /// The specified login must exactly match a stored login. However, the
        /// values of any nsILoginMetaInfo properties are ignored.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void RemoveLogin([MarshalAs(UnmanagedType.Interface)] nsILoginInfo aLogin);
		
		/// <summary>
        /// Modify an existing login in the storage module.
        ///
        /// @param oldLogin
        /// The login to be modified.
        /// @param newLoginData
        /// The new login values (either a nsILoginInfo or nsIProperyBag)
        ///
        /// If newLoginData is a nsILoginInfo, all of the old login's nsILoginInfo
        /// properties are changed to the values from newLoginData (but the old
        /// login's nsILoginMetaInfo properties are unmodified).
        ///
        /// If newLoginData is a nsIPropertyBag, only the specified properties
        /// will be changed. The nsILoginMetaInfo properties of oldLogin can be
        /// changed in this manner.
        ///
        /// If the propertybag contains an item named "timesUsedIncrement", the
        /// login's timesUsed property will be incremented by the item's value.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void ModifyLogin([MarshalAs(UnmanagedType.Interface)] nsILoginInfo oldLogin, [MarshalAs(UnmanagedType.Interface)] nsISupports newLoginData);
		
		/// <summary>
        /// Remove all stored logins.
        ///
        /// The browser sanitization feature allows the user to clear any stored
        /// passwords. This interface allows that to be done without getting each
        /// login first (which might require knowing the master password).
        ///
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void RemoveAllLogins();
		
		/// <summary>
        /// Fetch all logins in the login manager. An array is always returned;
        /// if there are no logins the array is empty.
        ///
        /// @param count
        /// The number of elements in the array. JS callers can simply use
        /// the array's .length property and omit this param.
        /// @param logins
        /// An array of nsILoginInfo objects.
        ///
        /// NOTE: This can be called from JS as:
        /// var logins = pwmgr.getAllLogins();
        /// (|logins| is an array).
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void GetAllLogins(ref uint count, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex=0)] ref nsILoginInfo[] logins);
		
		/// <summary>
        /// Fetch all logins in the login manager. An array is always returned;
        /// if there are no logins the array is empty. This does not decrypt logins
        /// before returning the array
        ///
        /// @param count
        /// The number of elements in the array. JS callers can simply use
        /// the array's .length property and omit this param.
        /// @param logins
        /// An array of nsILoginInfo objects.
        ///
        /// NOTE: This can be called from JS as:
        /// var logins = pwmgr.getAllEncryptedLogins();
        /// (|logins| is an array).
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void GetAllEncryptedLogins(ref uint count, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex=0)] ref nsILoginInfo[] logins);
		
		/// <summary>
        /// Search for logins in the login manager. An array is always returned;
        /// if there are no logins the array is empty.
        ///
        /// @param count
        /// The number of elements in the array. JS callers can simply use
        /// the array's .length property, and supply an dummy object for
        /// this out param. For example: |searchLogins({}, matchData)|
        /// @param matchData
        /// The data used to search. This does not follow the same
        /// requirements as findLogins for those fields. Wildcard matches are
        /// simply not specified.
        /// @param logins
        /// An array of nsILoginInfo objects.
        ///
        /// NOTE: This can be called from JS as:
        /// var logins = pwmgr.searchLogins({}, matchData);
        /// (|logins| is an array).
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void SearchLogins(ref uint count, [MarshalAs(UnmanagedType.Interface)] nsIPropertyBag matchData, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex=0)] ref nsILoginInfo[] logins);
		
		/// <summary>
        /// Obtain a list of all hosts for which password saving is disabled.
        ///
        /// @param count
        /// The number of elements in the array. JS callers can simply use
        /// the array's .length property and omit this param.
        /// @param hostnames
        /// An array of hostname strings, in origin URL format without a
        /// pathname. For example: "https://www.site.com".
        ///
        /// NOTE: This can be called from JS as:
        /// var logins = pwmgr.getAllDisabledHosts();
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void GetAllDisabledHosts(ref uint count, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex=0)] ref System.IntPtr[] hostnames);
		
		/// <summary>
        /// Check to see if saving logins has been disabled for a host.
        ///
        /// @param aHost
        /// The hostname to check. This argument should be in the origin
        /// URL format, without a pathname. For example: "http://foo.com".
        /// </summary>
		[return: MarshalAs(UnmanagedType.U1)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		bool GetLoginSavingEnabled([MarshalAs(UnmanagedType.LPStruct)] nsAStringBase aHost);
		
		/// <summary>
        /// Disable (or enable) storing logins for the specified host. When
        /// disabled, the login manager will not prompt to store logins for
        /// that host. Existing logins are not affected.
        ///
        /// @param aHost
        /// The hostname to set. This argument should be in the origin
        /// URL format, without a pathname. For example: "http://foo.com".
        /// @param isEnabled
        /// Specify if saving logins should be enabled (true) or
        /// disabled (false)
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void SetLoginSavingEnabled([MarshalAs(UnmanagedType.LPStruct)] nsAStringBase aHost, [MarshalAs(UnmanagedType.U1)] bool isEnabled);
		
		/// <summary>
        /// Search for logins matching the specified criteria. Called when looking
        /// for logins that might be applicable to a form or authentication request.
        ///
        /// @param count
        /// The number of elements in the array. JS callers can simply use
        /// the array's .length property, and supply an dummy object for
        /// this out param. For example: |findLogins({}, hostname, ...)|
        /// @param aHostname
        /// The hostname to restrict searches to, in URL format. For
        /// example: "http://www.site.com".
        /// @param aActionURL
        /// For form logins, this argument should be the URL to which the
        /// form will be submitted. For protocol logins, specify null.
        /// @param aHttpRealm
        /// For protocol logins, this argument should be the HTTP Realm
        /// for which the login applies. This is obtained from the
        /// WWW-Authenticate header. See RFC2617. For form logins,
        /// specify null.
        /// @param logins
        /// An array of nsILoginInfo objects.
        ///
        /// NOTE: This can be called from JS as:
        /// var logins = pwmgr.findLogins({}, hostname, ...);
        ///
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void FindLogins(ref uint count, [MarshalAs(UnmanagedType.LPStruct)] nsAStringBase aHostname, [MarshalAs(UnmanagedType.LPStruct)] nsAStringBase aActionURL, [MarshalAs(UnmanagedType.LPStruct)] nsAStringBase aHttpRealm, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex=0)] ref nsILoginInfo[] logins);
		
		/// <summary>
        /// Search for logins matching the specified criteria, as with
        /// findLogins(). This interface only returns the number of matching
        /// logins (and not the logins themselves), which allows a caller to
        /// check for logins without causing the user to be prompted for a master
        /// password to decrypt the logins.
        ///
        /// @param aHostname
        /// The hostname to restrict searches to. Specify an empty string
        /// to match all hosts. A null value will not match any logins, and
        /// will thus always return a count of 0.
        /// @param aActionURL
        /// The URL to which a form login will be submitted. To match any
        /// form login, specify an empty string. To not match any form
        /// login, specify null.
        /// @param aHttpRealm
        /// The HTTP Realm for which the login applies. To match logins for
        /// any realm, specify an empty string. To not match logins for any
        /// realm, specify null.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		uint CountLogins([MarshalAs(UnmanagedType.LPStruct)] nsAStringBase aHostname, [MarshalAs(UnmanagedType.LPStruct)] nsAStringBase aActionURL, [MarshalAs(UnmanagedType.LPStruct)] nsAStringBase aHttpRealm);
		
		/// <summary>
        /// True when a master password prompt is being shown.
        /// </summary>
		[return: MarshalAs(UnmanagedType.U1)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		bool GetUiBusyAttribute();
	}
}
