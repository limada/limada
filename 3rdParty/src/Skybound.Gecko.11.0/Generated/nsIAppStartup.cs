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
// Generated by IDLImporter from file nsIAppStartup.idl
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
	
	
	/// <summary>nsIAppStartup </summary>
	[ComImport()]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("dd3e7b3c-0974-4a38-b4d3-ce2213452432")]
	public interface nsIAppStartup
	{
		
		/// <summary>
        /// Create the hidden window.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void CreateHiddenWindow();
		
		/// <summary>
        /// Destroys the hidden window. This will have no effect if the hidden window
        /// has not yet been created.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void DestroyHiddenWindow();
		
		/// <summary>
        /// Runs an application event loop: normally the main event pump which
        /// defines the lifetime of the application. If there are no windows open
        /// and no outstanding calls to enterLastWindowClosingSurvivalArea this
        /// method will exit immediately.
        ///
        /// @returnCode NS_SUCCESS_RESTART_APP
        /// This return code indicates that the application should be
        /// restarted because quit was called with the eRestart flag.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void Run();
		
		/// <summary>
        /// There are situations where all application windows will be
        /// closed but we don't want to take this as a signal to quit the
        /// app. Bracket the code where the last window could close with
        /// these.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void EnterLastWindowClosingSurvivalArea();
		
		/// <summary>Member ExitLastWindowClosingSurvivalArea </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void ExitLastWindowClosingSurvivalArea();
		
		/// <summary>
        /// Exit the event loop, and shut down the app.
        ///
        /// @param aMode
        /// This parameter modifies how the app is shutdown, and it is
        /// constructed from the constants defined above.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void Quit(uint aMode);
		
		/// <summary>
        /// True if the application is in the process of shutting down.
        /// </summary>
		[return: MarshalAs(UnmanagedType.U1)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		bool GetShuttingDownAttribute();
		
		/// <summary>
        /// Returns an object with main, process, firstPaint, sessionRestored properties.
        /// Properties may not be available depending on platform or application
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		System.IntPtr GetStartupInfo(System.IntPtr jsContext);
		
		/// <summary>
        /// True if startup was interrupted by an interactive prompt.
        /// </summary>
		[return: MarshalAs(UnmanagedType.U1)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		bool GetInterruptedAttribute();
		
		/// <summary>
        /// True if startup was interrupted by an interactive prompt.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void SetInterruptedAttribute([MarshalAs(UnmanagedType.U1)] bool aInterrupted);
	}
}