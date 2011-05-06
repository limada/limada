#region ***** BEGIN LICENSE BLOCK *****
/* Version: MPL 1.1/GPL 2.0/LGPL 2.1
 *
 * The contents of this file are subject to the Mozilla Public License Version
 * 1.1 (the "License"); you may not use this file except in compliance with
 * the License. You may obtain a copy of the License at
 * http://www.mozilla.org/MPL/
 *
 * Software distributed under the License is distributed on an "AS IS" basis,
 * WITHOUT WARRANTY OF ANY KIND, either express or implied. See the License
 * for the specific language governing rights and limitations under the
 * License.
 *
 * The Original Code is Skybound Software code.
 *
 * The Initial Developer of the Original Code is Skybound Software.
 * Portions created by the Initial Developer are Copyright (C) 2008
 * the Initial Developer. All Rights Reserved.
 *
 * Contributor(s):
 *
 * Alternatively, the contents of this file may be used under the terms of
 * either the GNU General Public License Version 2 or later (the "GPL"), or
 * the GNU Lesser General Public License Version 2.1 or later (the "LGPL"),
 * in which case the provisions of the GPL or the LGPL are applicable instead
 * of those above. If you wish to allow use of your version of this file only
 * under the terms of either the GPL or the LGPL, and not to allow others to
 * use your version of this file under the terms of the MPL, indicate your
 * decision by deleting the provisions above and replace them with the notice
 * and other provisions required by the GPL or the LGPL. If you do not delete
 * the provisions above, a recipient may use your version of this file under
 * the terms of any one of the MPL, the GPL or the LGPL.
 */
#endregion END LICENSE BLOCK

using System;
using System.Runtime.InteropServices;

namespace Skybound.Gecko
{
	/// <summary>
	/// Provides helper methods to get and set string attributes on XPCOM interfaces.
	/// </summary>
	public static class nsString
	{
		public delegate void StringAttributeAnsi(nsACString str);
		
		public static string Get(StringAttributeAnsi getter)
		{
			using (nsACString str = new nsACString())
			{
				getter(str);
				return str.ToString();
			}
		}
		
		public static void Set(StringAttributeAnsi setter, string value)
		{
			using (nsACString str = new nsACString())
			{
				if (!string.IsNullOrEmpty(value))
					str.SetData(value);
				
				setter(str);
			}
		}
		
		public delegate void StringAttributeUnicode(nsAString str);
		
		public static string Get(StringAttributeUnicode getter)
		{
			using (nsAString str = new nsAString())
			{
				getter(str);
				return str.ToString();
			}
		}
		
		public static void Set(StringAttributeUnicode setter, string value)
		{
			using (nsAString str = new nsAString())
			{
				if (!string.IsNullOrEmpty(value))
					str.SetData(value);
				
				setter(str);
			}
		}
	}
	
	[StructLayout(LayoutKind.Explicit, Size=16)]
	public class nsACString : IDisposable
	{
		[DllImport("xpcom", CharSet = CharSet.Ansi)]
		static extern int NS_CStringContainerInit(nsACString container);
		
		[DllImport("xpcom", CharSet = CharSet.Ansi)]
		static extern int NS_CStringSetData(nsACString str, string data, int length);
		
		[DllImport("xpcom", CharSet = CharSet.Ansi)]
		static extern int NS_CStringGetData(nsACString str, out IntPtr data, IntPtr nullTerm);
		
		[DllImport("xpcom", CharSet = CharSet.Ansi)]
		static extern int NS_CStringContainerFinish(nsACString container);
		
		public nsACString()
		{
			NS_CStringContainerInit(this);
		}
		
		public nsACString(string value) : this()
		{
			if (value != null)
			{
				NS_CStringSetData(this, value, value.Length);
			}
		}
		
		~nsACString()
		{
			Dispose();
		}
		
		public void Dispose()
		{
			NS_CStringContainerFinish(this);
			GC.SuppressFinalize(this);
		}
		
		public void SetData(string value)
		{
			NS_CStringSetData(this, value, (value == null) ? 0 : value.Length);
		}
		
		public override string ToString()
		{
			IntPtr data;
			int length = NS_CStringGetData(this, out data, IntPtr.Zero);
			
			if (length > 0)
			{
				return Marshal.PtrToStringAnsi(data, length);
			}
			return "";
		}
	}
	
	[StructLayout(LayoutKind.Explicit, Size=16)]
	public class nsAString : IDisposable
	{
		[DllImport("xpcom", CharSet = CharSet.Unicode)]
		static extern int NS_StringContainerInit(nsAString container);
		
		[DllImport("xpcom", CharSet = CharSet.Unicode)]
		static extern int NS_StringSetData(nsAString str, string data, int length);
		
		[DllImport("xpcom", CharSet = CharSet.Unicode)]
		static extern int NS_StringGetData(nsAString str, out IntPtr data, IntPtr nullTerm);
		
		[DllImport("xpcom", CharSet = CharSet.Unicode)]
		static extern int NS_StringContainerFinish(nsAString container);
		
		public nsAString()
		{
			NS_StringContainerInit(this);
		}
		
		public nsAString(string value) : this()
		{
			if (value != null)
			{
				NS_StringSetData(this, value, value.Length);
			}
		}
		
		~nsAString()
		{
			Dispose();
		}
		
		public void Dispose()
		{
			NS_StringContainerFinish(this);
			GC.SuppressFinalize(this);
		}
		
		public void SetData(string value)
		{
			NS_StringSetData(this, value, (value == null) ? 0 : value.Length);
		}
		
		public override string ToString()
		{
			IntPtr data;
			int length = NS_StringGetData(this, out data, IntPtr.Zero);
			
			if (length > 0)
			{
				return Marshal.PtrToStringAuto(data, length);
			}
			return "";
		}
	}
}
