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
// Generated by IDLImporter from file nsIVolume.idl
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
    /// License, v. 2.0. If a copy of the MPL was not distributed with this file,
    /// You can obtain one at http://mozilla.org/MPL/2.0/. </summary>
	[ComImport()]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("8c163fe4-5577-11e3-b3d0-10bf48d707fb")]
	public interface nsIVolume
	{
		
		/// <summary>
        /// But some phones support multiple volumes.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void GetNameAttribute([MarshalAs(UnmanagedType.CustomMarshaler, MarshalType = "Gecko.CustomMarshalers.AStringMarshaler")] nsAStringBase aName);
		
		/// <summary>
        /// and is only valid when state == STATE_MOUNTED.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void GetMountPointAttribute([MarshalAs(UnmanagedType.CustomMarshaler, MarshalType = "Gecko.CustomMarshalers.AStringMarshaler")] nsAStringBase aMountPoint);
		
		/// <summary>
        /// from above.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		int GetStateAttribute();
		
		/// <summary>
        /// be different from the last time it transitioned to the mounted state.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		int GetMountGenerationAttribute();
		
		/// <summary>
        /// the wakelock every time the volume becomes mounted.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void GetMountLockNameAttribute([MarshalAs(UnmanagedType.CustomMarshaler, MarshalType = "Gecko.CustomMarshalers.AStringMarshaler")] nsAStringBase aMountLockName);
		
		/// <summary>
        /// Determines if a mountlock is currently being held against this volume.
        /// </summary>
		[return: MarshalAs(UnmanagedType.U1)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		bool GetIsMountLockedAttribute();
		
		/// <summary>
        /// current state isn't STATE_NOMEDIA.
        /// </summary>
		[return: MarshalAs(UnmanagedType.U1)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		bool GetIsMediaPresentAttribute();
		
		/// <summary>
        /// transitioning from mounted to sharing and back again.
        /// </summary>
		[return: MarshalAs(UnmanagedType.U1)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		bool GetIsSharingAttribute();
		
		/// <summary>
        /// once the volume has been formatted and mounted again.
        /// </summary>
		[return: MarshalAs(UnmanagedType.U1)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		bool GetIsFormattingAttribute();
		
		[return: MarshalAs(UnmanagedType.Interface)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		nsIVolumeStat GetStats();
		
		/// <summary>
        /// Automounter will unmount it, format it and then mount it again.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void Format();
		
		/// <summary>
        /// Whether this is a fake volume.
        /// </summary>
		[return: MarshalAs(UnmanagedType.U1)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		bool GetIsFakeAttribute();
	}
	
	/// <summary>nsIVolumeConsts </summary>
	public class nsIVolumeConsts
	{
		
		// <summary>
        // These MUST match the states from android's system/vold/Volume.h header
        // </summary>
		public const long STATE_INIT = -1;
		
		// 
		public const long STATE_NOMEDIA = 0;
		
		// 
		public const long STATE_IDLE = 1;
		
		// 
		public const long STATE_PENDING = 2;
		
		// 
		public const long STATE_CHECKING = 3;
		
		// 
		public const long STATE_MOUNTED = 4;
		
		// 
		public const long STATE_UNMOUNTING = 5;
		
		// 
		public const long STATE_FORMATTING = 6;
		
		// 
		public const long STATE_SHARED = 7;
		
		// 
		public const long STATE_SHAREDMNT = 8;
	}
}
