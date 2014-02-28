using System.IO;
using System.Runtime.InteropServices.ComTypes;
using System;
using System.Runtime.InteropServices;
using System.Reflection;
using System.Diagnostics;

namespace Limaki.View.Swf.DragDrop {
    public class DataObjectHelper {

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int RegisterClipboardFormat(string format);

        public static IDataObject InteropDataObject(System.Windows.Forms.IDataObject dataObject) {
            return dataObject as System.Runtime.InteropServices.ComTypes.IDataObject;
        }
       public static byte[] GetData(System.Windows.Forms.IDataObject dataObject, string format, int index) {
           return GetData(dataObject as IDataObject, format, index);
           
       }
        public static byte[] GetData(IDataObject comUnderlyingDataObject, string format, int index) {
            //create a FORMATETC struct to request the data with
            FORMATETC formatetc = new FORMATETC();
            formatetc.cfFormat = (short)System.Windows.Forms.DataFormats.GetFormat(format).Id;
            formatetc.dwAspect = DVASPECT.DVASPECT_CONTENT;
            formatetc.lindex = index;
            formatetc.ptd = new IntPtr(0);
            formatetc.tymed = TYMED.TYMED_ISTREAM | TYMED.TYMED_ISTORAGE | TYMED.TYMED_HGLOBAL;

            //create STGMEDIUM to output request results into
            STGMEDIUM medium = new STGMEDIUM();

            //using the Com IDataObject interface get the data using the defined FORMATETC
            comUnderlyingDataObject.GetData(ref formatetc, out medium);

            //retrieve the data depending on the returned store type
            switch (medium.tymed) {
                case TYMED.TYMED_ISTORAGE:
                    //to handle a IStorage it needs to be written into a second unmanaged
                    //memory mapped storage and then the data can be read from memory into
                    //a managed byte and returned as a MemoryStream

                    NativeMethods.IStorage iStorage = null;
                    NativeMethods.IStorage iStorage2 = null;
                    NativeMethods.ILockBytes iLockBytes = null;
                    System.Runtime.InteropServices.ComTypes.STATSTG iLockBytesStat;
                    try {
                        //marshal the returned pointer to a IStorage object
                        iStorage = (NativeMethods.IStorage)Marshal.GetObjectForIUnknown(medium.unionmember);
                        Marshal.Release(medium.unionmember);

                        //create a ILockBytes (unmanaged byte array) and then create a IStorage using the byte array as a backing store
                        iLockBytes = NativeMethods.CreateILockBytesOnHGlobal(IntPtr.Zero, true);
                        iStorage2 = NativeMethods.StgCreateDocfileOnILockBytes(iLockBytes, 0x00001012, 0);

                        //copy the returned IStorage into the new IStorage
                        iStorage.CopyTo(0, null, IntPtr.Zero, iStorage2);
                        iLockBytes.Flush();
                        iStorage2.Commit(0);

                        //get the STATSTG of the ILockBytes to determine how many bytes were written to it
                        iLockBytesStat = new System.Runtime.InteropServices.ComTypes.STATSTG();
                        iLockBytes.Stat(out iLockBytesStat, 1);
                        int iLockBytesSize = (int)iLockBytesStat.cbSize;

                        //read the data from the ILockBytes (unmanaged byte array) into a managed byte array
                        byte[] iLockBytesContent = new byte[iLockBytesSize];
                        iLockBytes.ReadAt(0, iLockBytesContent, iLockBytesContent.Length, null);

                        
                        return iLockBytesContent;
                    } finally {
                        //release all unmanaged objects
                        Marshal.ReleaseComObject(iStorage2);
                        Marshal.ReleaseComObject(iLockBytes);
                        Marshal.ReleaseComObject(iStorage);
                    }

                case TYMED.TYMED_ISTREAM:
                    //to handle a IStream it needs to be read into a managed byte 

                    IStream iStream = null;
                    System.Runtime.InteropServices.ComTypes.STATSTG iStreamStat;
                    try {
                        //marshal the returned pointer to a IStream object
                        iStream = (IStream)Marshal.GetObjectForIUnknown(medium.unionmember);
                        Marshal.Release(medium.unionmember);

                        //get the STATSTG of the IStream to determine how many bytes are in it
                        iStreamStat = new System.Runtime.InteropServices.ComTypes.STATSTG();
                        iStream.Stat(out iStreamStat, 0);
                        int iStreamSize = (int)iStreamStat.cbSize;

                        //read the data from the IStream into a managed byte array
                        byte[] iStreamContent = new byte[iStreamSize];
                        iStream.Read(iStreamContent, iStreamContent.Length, IntPtr.Zero);

                        return iStreamContent;
                    } finally {
                        //release all unmanaged objects
                        Marshal.ReleaseComObject(iStream);
                    }

                case TYMED.TYMED_HGLOBAL:
                    return GetTYMED_HGLOBAL(comUnderlyingDataObject,format);
                        
                    if (false) {
                        //to handle a HGlobal the exisitng "GetDataFromHGLOBLAL" method is invoked via
                        //reflection

                        // but "GetDataFromHGLOBLAL"-method has an error!!!
                        var innerDataField = comUnderlyingDataObject.GetType().GetField("innerData",
                                                                                        BindingFlags.NonPublic |
                                                                                        BindingFlags.Instance);
                        var oleUnderlyingDataObject =
                            (System.Windows.Forms.IDataObject) innerDataField.GetValue(comUnderlyingDataObject);
                        var getDataFromHGLOBLALMethod =
                            oleUnderlyingDataObject.GetType().GetMethod("GetDataFromHGLOBLAL",
                                                                        BindingFlags.NonPublic | BindingFlags.Instance);
                        var result =  getDataFromHGLOBLALMethod
                            .Invoke(oleUnderlyingDataObject,
                                    new object[] {System.Windows.Forms.DataFormats.GetFormat((short) formatetc.cfFormat).Name, medium.unionmember});
                        return null;
                    }
            }

            return null;
        }


        /// <summary>
        /// Extracts data of type Dataformat.Html from an IdataObject data container
        /// This method shouldn't throw any exception but writes relevant exception informations in the debug window
        /// 
        /// copied from: http://bakamachine.blogspot.com/2006/05/workarond-for-dataobject-html.html
        /// author: Giuliano Sauro
        /// 
        /// </summary>
        /// <param name="data">IdataObject data container</param>
        /// <returns>A byte[] array with the decoded string or null if the method fails</returns>
        public static byte[] GetTYMED_HGLOBAL(IDataObject dataObject, string format) {
            var formatetc = new FORMATETC();
            formatetc.cfFormat = (short)System.Windows.Forms.DataFormats.GetFormat(format).Id;
            formatetc.dwAspect = DVASPECT.DVASPECT_CONTENT;
            formatetc.lindex = -1;
            formatetc.tymed = TYMED.TYMED_HGLOBAL;

            var stgmedium = new STGMEDIUM();
            stgmedium.tymed = TYMED.TYMED_HGLOBAL;
            stgmedium.pUnkForRelease = null;
            int queryResult = 0;

            try { 
                queryResult = dataObject.QueryGetData(ref formatetc);
            } catch (Exception exp) {
                Debug.WriteLine("HtmlFromIDataObject.GetHtml -> QueryGetData(ref format) threw an exception: " + Environment.NewLine + exp.ToString());
                return null;
            }

            if (queryResult != 0) {
                Debug.WriteLine("HtmlFromIDataObject.GetHtml -> QueryGetData(ref format) returned a code != 0 code: "+ queryResult.ToString());
                return null;
            }

            try {
                dataObject.GetData(ref formatetc, out stgmedium);
            } catch (Exception exp) {
                Debug.WriteLine("HtmlFromIDataObject.GetHtml -> GetData(ref format, out stgmedium) threw this exception: "+ Environment.NewLine + exp.ToString());
                return null;
            }

            if (stgmedium.unionmember == IntPtr.Zero) {
                Debug.WriteLine("HtmlFromIDataObject.GetHtml -> stgmedium.unionmember returned an IntPtr pointing to zero");
                return null;
            }

            var pointer = stgmedium.unionmember;
            var handleRef = new HandleRef(null, pointer);
            byte[] rawArray = null;

            try {
                var ptr1 = NativeMethods.GlobalLock(handleRef);
                var length = NativeMethods.GlobalSize(handleRef);
                rawArray = new byte[length];

                Marshal.Copy(ptr1, rawArray, 0, length);

            } catch (Exception exp) {
                Debug.WriteLine("HtmlFromIDataObject.GetHtml -> Html Import threw an exception: " + Environment.NewLine + exp.ToString());
            } finally {
                NativeMethods.GlobalUnlock(handleRef);
            }

            return rawArray;
        }


      
    }

    class NativeMethods {
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
        public static extern IntPtr GlobalLock(HandleRef handle);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
        public static extern bool GlobalUnlock(HandleRef handle);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
        public static extern int GlobalSize(HandleRef handle);

        [DllImport("kernel32.dll")]
        public static extern IntPtr GlobalLock(IntPtr hMem);

        [DllImport("ole32.dll", PreserveSig = false)]
        public static extern ILockBytes CreateILockBytesOnHGlobal(IntPtr hGlobal, bool fDeleteOnRelease);

        [DllImport("OLE32.DLL", CharSet = CharSet.Auto, PreserveSig = false)]
        public static extern IntPtr GetHGlobalFromILockBytes(ILockBytes pLockBytes);

        [DllImport("OLE32.DLL", CharSet = CharSet.Unicode, PreserveSig = false)]
        public static extern IStorage StgCreateDocfileOnILockBytes(ILockBytes plkbyt, uint grfMode, uint reserved);

        [ComImport, InterfaceType(ComInterfaceType.InterfaceIsIUnknown), Guid("0000000B-0000-0000-C000-000000000046")]
        public interface IStorage {
            [return: MarshalAs(UnmanagedType.Interface)]
            IStream CreateStream([In, MarshalAs(UnmanagedType.BStr)] string pwcsName, [In, MarshalAs(UnmanagedType.U4)] int grfMode, [In, MarshalAs(UnmanagedType.U4)] int reserved1, [In, MarshalAs(UnmanagedType.U4)] int reserved2);
            [return: MarshalAs(UnmanagedType.Interface)]
            IStream OpenStream([In, MarshalAs(UnmanagedType.BStr)] string pwcsName, IntPtr reserved1, [In, MarshalAs(UnmanagedType.U4)] int grfMode, [In, MarshalAs(UnmanagedType.U4)] int reserved2);
            [return: MarshalAs(UnmanagedType.Interface)]
            IStorage CreateStorage([In, MarshalAs(UnmanagedType.BStr)] string pwcsName, [In, MarshalAs(UnmanagedType.U4)] int grfMode, [In, MarshalAs(UnmanagedType.U4)] int reserved1, [In, MarshalAs(UnmanagedType.U4)] int reserved2);
            [return: MarshalAs(UnmanagedType.Interface)]
            IStorage OpenStorage([In, MarshalAs(UnmanagedType.BStr)] string pwcsName, IntPtr pstgPriority, [In, MarshalAs(UnmanagedType.U4)] int grfMode, IntPtr snbExclude, [In, MarshalAs(UnmanagedType.U4)] int reserved);
            void CopyTo(int ciidExclude, [In, MarshalAs(UnmanagedType.LPArray)] Guid[] pIIDExclude, IntPtr snbExclude, [In, MarshalAs(UnmanagedType.Interface)] IStorage stgDest);
            void MoveElementTo([In, MarshalAs(UnmanagedType.BStr)] string pwcsName, [In, MarshalAs(UnmanagedType.Interface)] IStorage stgDest, [In, MarshalAs(UnmanagedType.BStr)] string pwcsNewName, [In, MarshalAs(UnmanagedType.U4)] int grfFlags);
            void Commit(int grfCommitFlags);
            void Revert();
            void EnumElements([In, MarshalAs(UnmanagedType.U4)] int reserved1, IntPtr reserved2, [In, MarshalAs(UnmanagedType.U4)] int reserved3, [MarshalAs(UnmanagedType.Interface)] out object ppVal);
            void DestroyElement([In, MarshalAs(UnmanagedType.BStr)] string pwcsName);
            void RenameElement([In, MarshalAs(UnmanagedType.BStr)] string pwcsOldName, [In, MarshalAs(UnmanagedType.BStr)] string pwcsNewName);
            void SetElementTimes([In, MarshalAs(UnmanagedType.BStr)] string pwcsName, [In] System.Runtime.InteropServices.ComTypes.FILETIME pctime, [In] System.Runtime.InteropServices.ComTypes.FILETIME patime, [In] System.Runtime.InteropServices.ComTypes.FILETIME pmtime);
            void SetClass([In] ref Guid clsid);
            void SetStateBits(int grfStateBits, int grfMask);
            void Stat([Out]out System.Runtime.InteropServices.ComTypes.STATSTG pStatStg, int grfStatFlag);
        }

        [ComImport, Guid("0000000A-0000-0000-C000-000000000046"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        public interface ILockBytes {
            void ReadAt([In, MarshalAs(UnmanagedType.U8)] long ulOffset, [Out, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] byte[] pv, [In, MarshalAs(UnmanagedType.U4)] int cb, [Out, MarshalAs(UnmanagedType.LPArray)] int[] pcbRead);
            void WriteAt([In, MarshalAs(UnmanagedType.U8)] long ulOffset, IntPtr pv, [In, MarshalAs(UnmanagedType.U4)] int cb, [Out, MarshalAs(UnmanagedType.LPArray)] int[] pcbWritten);
            void Flush();
            void SetSize([In, MarshalAs(UnmanagedType.U8)] long cb);
            void LockRegion([In, MarshalAs(UnmanagedType.U8)] long libOffset, [In, MarshalAs(UnmanagedType.U8)] long cb, [In, MarshalAs(UnmanagedType.U4)] int dwLockType);
            void UnlockRegion([In, MarshalAs(UnmanagedType.U8)] long libOffset, [In, MarshalAs(UnmanagedType.U8)] long cb, [In, MarshalAs(UnmanagedType.U4)] int dwLockType);
            void Stat([Out]out System.Runtime.InteropServices.ComTypes.STATSTG pstatstg, [In, MarshalAs(UnmanagedType.U4)] int grfStatFlag);
        }

        [StructLayout(LayoutKind.Sequential)]
        public sealed class POINTL {
            public int x;
            public int y;
        }

        [StructLayout(LayoutKind.Sequential)]
        public sealed class SIZEL {
            public int cx;
            public int cy;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public sealed class FILEGROUPDESCRIPTORA {
            public uint cItems;
            public FILEDESCRIPTORA[] fgd;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public sealed class FILEDESCRIPTORA {
            public uint dwFlags;
            public Guid clsid;
            public SIZEL sizel;
            public POINTL pointl;
            public uint dwFileAttributes;
            public System.Runtime.InteropServices.ComTypes.FILETIME ftCreationTime;
            public System.Runtime.InteropServices.ComTypes.FILETIME ftLastAccessTime;
            public System.Runtime.InteropServices.ComTypes.FILETIME ftLastWriteTime;
            public uint nFileSizeHigh;
            public uint nFileSizeLow;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
            public string cFileName;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public sealed class FILEGROUPDESCRIPTORW {
            public uint cItems;
            public FILEDESCRIPTORW[] fgd;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public sealed class FILEDESCRIPTORW {
            public uint dwFlags;
            public Guid clsid;
            public SIZEL sizel;
            public POINTL pointl;
            public uint dwFileAttributes;
            public System.Runtime.InteropServices.ComTypes.FILETIME ftCreationTime;
            public System.Runtime.InteropServices.ComTypes.FILETIME ftLastAccessTime;
            public System.Runtime.InteropServices.ComTypes.FILETIME ftLastWriteTime;
            public uint nFileSizeHigh;
            public uint nFileSizeLow;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
            public string cFileName;
        }
    }
}