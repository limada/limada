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
// Generated by IDLImporter from file nsIDOMNSEvent.idl
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
	[Guid("2580b4a2-6d85-4ca6-9be2-98f3406ad296")]
	public interface nsIDOMNSEvent
	{
	}
	
	/// <summary>nsIDOMNSEventConsts </summary>
	public class nsIDOMNSEventConsts
	{
		
		// <summary>
        //This Source Code Form is subject to the terms of the Mozilla Public
        // License, v. 2.0. If a copy of the MPL was not distributed with this
        // file, You can obtain one at http://mozilla.org/MPL/2.0/. </summary>
		public const long MOUSEDOWN = 0x00000001;
		
		// 
		public const long MOUSEUP = 0x00000002;
		
		// 
		public const long MOUSEOVER = 0x00000004;
		
		// 
		public const long MOUSEOUT = 0x00000008;
		
		// 
		public const long MOUSEMOVE = 0x00000010;
		
		// 
		public const long MOUSEDRAG = 0x00000020;
		
		// 
		public const long CLICK = 0x00000040;
		
		// 
		public const long DBLCLICK = 0x00000080;
		
		// 
		public const long KEYDOWN = 0x00000100;
		
		// 
		public const long KEYUP = 0x00000200;
		
		// 
		public const long KEYPRESS = 0x00000400;
		
		// 
		public const long DRAGDROP = 0x00000800;
		
		// 
		public const long FOCUS = 0x00001000;
		
		// 
		public const long BLUR = 0x00002000;
		
		// 
		public const long SELECT = 0x00004000;
		
		// 
		public const long CHANGE = 0x00008000;
		
		// 
		public const long RESET = 0x00010000;
		
		// 
		public const long SUBMIT = 0x00020000;
		
		// 
		public const long SCROLL = 0x00040000;
		
		// 
		public const long LOAD = 0x00080000;
		
		// 
		public const long UNLOAD = 0x00100000;
		
		// 
		public const long XFER_DONE = 0x00200000;
		
		// 
		public const long ABORT = 0x00400000;
		
		// 
		public const long ERROR = 0x00800000;
		
		// 
		public const long LOCATE = 0x01000000;
		
		// 
		public const long MOVE = 0x02000000;
		
		// 
		public const long RESIZE = 0x04000000;
		
		// 
		public const long FORWARD = 0x08000000;
		
		// 
		public const long HELP = 0x10000000;
		
		// 
		public const long BACK = 0x20000000;
		
		// 
		public const long TEXT = 0x40000000;
		
		// 
		public const long ALT_MASK = 0x00000001;
		
		// 
		public const long CONTROL_MASK = 0x00000002;
		
		// 
		public const long SHIFT_MASK = 0x00000004;
		
		// 
		public const long META_MASK = 0x00000008;
	}
}
