/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2006-2011 Lytico
 *
 * http://www.limada.org
 * 
 */

using System;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace Limaki {
    public class OS {
        protected static Nullable<bool> _mono = null;
        public static bool Mono {
            get {
                if (_mono == null) {
                    _mono = Type.GetType("Mono.Runtime") != null; ;
                }
                return _mono.Value;
            }
        }

        protected static Nullable<bool> _unix = null;
        public static bool Unix {
            get {
                if (_unix == null) {
                    _unix = Environment.OSVersion.Platform.ToString().Contains("Unix");
                }
                return _unix.Value;
            }
        }

        protected static Nullable<bool> _isWin64BitOS = null;
        /// <summary>     
        /// The function determines whether the current operating system is a      
        /// 64-bit operating system.     
        /// </summary>     
        /// <returns>     
        /// The function returns true if the operating system is 64-bit-windows     
        /// otherwise, it returns false.     
        /// </returns>    
        public static bool IsWin64BitOS {
            get {
                if (_isWin64BitOS == null) {

                    _isWin64BitOS = IsWin64Process || IsWin32on64BitProcess;
                    
                }
                return _isWin64BitOS.Value;
            }
        }
        public static bool IsWin64Process { get { return IntPtr.Size == 8; } }
        /// <summary>
        /// Detect whether the current process is a 32-bit process                
        /// running on a 64-bit system.         
        /// </summary>
        public static bool IsWin32on64BitProcess { get {
            if(_isWin64BitProcess==null) {
                _isWin64BitProcess = Is64BitProc (Process.GetCurrentProcess ());
            }
            return _isWin64BitProcess.Value;
        } }

        protected static Nullable<bool> _isWin64BitProcess = null;
        /// <summary>  
        /// checks if the process is 64 bit  
        /// </summary>  
        /// <param name="os"></param>  
        /// <returns>  
        /// The function returns true if the process is 64-bit;        
        /// otherwise, it returns false.  
        /// </returns>    
        public static bool Is64BitProc(System.Diagnostics.Process p) {
            // 32-bit programs run on both 32-bit and 64-bit Windows           
            // Detect whether the current process is a 32-bit process                
            // running on a 64-bit system.               
            bool result;
            return ((DoesWin32MethodExist("kernel32.dll", "IsWow64Process") && IsWow64Process(p.Handle, out result)) && result);
        }

        /// <summary>     
        /// The function determins whether a method exists in the export      
        /// table of a certain module.     
        /// </summary>     
        /// <param name="moduleName">The name of the module</param>     
        /// <param name="methodName">The name of the method</param>     
        /// <returns>     
        /// The function returns true if the method specified by methodName      
        /// exists in the export table of the module specified by moduleName.     
        /// </returns>       
        static bool DoesWin32MethodExist(string moduleName, string methodName) {
            IntPtr moduleHandle = GetModuleHandle(moduleName);
            if (moduleHandle == IntPtr.Zero)
                return false;
            return (GetProcAddress(moduleHandle, methodName) != IntPtr.Zero);
        }

        [DllImport("kernel32.dll")]
        static extern IntPtr GetCurrentProcess();

        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        static extern IntPtr GetModuleHandle(string moduleName);

        [DllImport("kernel32", CharSet = CharSet.Auto, SetLastError = true)]
        static extern IntPtr GetProcAddress(IntPtr hModule, [MarshalAs(UnmanagedType.LPStr)]string procName);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool IsWow64Process(IntPtr hProcess, out bool wow64Process);
    }
}
