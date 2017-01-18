using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Xwt.GtkBackend;
using System.Linq;

namespace Limaki.View.GtkBackend {

    /// <summary>
    /// workaround for ClipboardTargetsReceivedFunc:
    /// ClipboardTargetsReceivedFunc should have: (Gtk.Clipboard clipboard, Gdk.Atom[] atoms, int n_atoms)
    /// that is: Atom is an array
    /// </summary>
    public class GtkClipboardBackend1 : GtkClipboardBackend {

        public override bool IsTypeAvailable (Xwt.TransferDataType type) {
            ClipboardTargetsReceivedFuncWrapper.RequestTargets (clipboard, ClipboardTargetsReceivedFunc);
            clipboard.RequestTargets (ClipboardTargetsReceivedFuncWorkaround);
            return base.IsTypeAvailable (type);
        }

        public void ClipboardTargetsReceivedFunc (Gtk.Clipboard clipboard, Gdk.Atom [] atoms, int n_atoms) {
            Trace.WriteLine ($"{nameof (clipboard)}-{nameof (IsTypeAvailable)}:");
            foreach (var f in atoms)
                Trace.WriteLine ($"\t{f.Name}");
            if (atoms.Any (f => f == "text/x-moz-url-priv")) {
                var data = this.GetData (Xwt.TransferDataType.FromId ("text/x-moz-url-priv"));
                Trace.WriteLine ($"\t{data}");
            }
        }

        public void ClipboardTargetsReceivedFuncWorkaround (Gtk.Clipboard clipboard, Gdk.Atom atoms, int n_atoms) {
            
            if (atoms == null)
                return;

            // solution from:
            var hint = nameof (Gtk.TextBuffer.SerializeFormats);
            // 

            var list_ptr = atoms.Handle;
            var result = new Gdk.Atom [n_atoms];
            for (var i = 0; i < n_atoms; i++) {
                var format = Marshal.ReadIntPtr (list_ptr, i * IntPtr.Size);
                result [i] = format == IntPtr.Zero ? null : (Gdk.Atom)GLib.Opaque.GetOpaque (format, typeof (Gdk.Atom), false);
            }

        }
    }
}

namespace Limaki.View.GtkBackend {
    
    public delegate void ClipboardTargetsReceivedFunc (Gtk.Clipboard clipboard, Gdk.Atom [] atoms, int n_atoms);

    public class ClipboardTargetsReceivedFuncWrapper {

        [UnmanagedFunctionPointer (CallingConvention.Cdecl)]
        internal delegate void ClipboardTargetsReceivedFuncNative (IntPtr clipboard, IntPtr atoms, int n_atoms, IntPtr data);

        // https://developer.gnome.org/gtk3/2.90/gtk3-Clipboards.html#GtkClipboardTargetsReceivedFunc
        [DllImport (GtkInterop.LIBGTK, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void gtk_clipboard_request_targets (IntPtr raw, ClipboardTargetsReceivedFuncNative cb, IntPtr user_data);

        internal void NativeCallback (IntPtr clipboard, IntPtr atoms, int n_atoms, IntPtr data) {
            try {
                
                var result = new Gdk.Atom [n_atoms];
                for (var i = 0; i < n_atoms; i++) {
                    var format = Marshal.ReadIntPtr (atoms, i * IntPtr.Size);
                    result [i] = format == IntPtr.Zero ? null : (Gdk.Atom)GLib.Opaque.GetOpaque (format, typeof (Gdk.Atom), false);
                }

                managed (GLib.Object.GetObject (clipboard) as Gtk.Clipboard, result, n_atoms);

                if (release_on_call)
                    gch.Free ();
            } catch (Exception e) {
                GLib.ExceptionManager.RaiseUnhandledException (e, false);
            }
        }

        bool release_on_call = false;
        GCHandle gch;

        internal void PersistUntilCalled () {
            release_on_call = true;
            gch = GCHandle.Alloc (this);
        }

        internal ClipboardTargetsReceivedFuncNative NativeDelegate;
        ClipboardTargetsReceivedFunc managed;

        internal ClipboardTargetsReceivedFuncWrapper (ClipboardTargetsReceivedFunc managed) {
            this.managed = managed;
            if (managed != null)
                NativeDelegate = new ClipboardTargetsReceivedFuncNative (NativeCallback);
        }

        internal static ClipboardTargetsReceivedFunc GetManagedDelegate (ClipboardTargetsReceivedFuncNative native) {
            if (native == null)
                return null;
            var wrapper = (ClipboardTargetsReceivedFuncWrapper)native.Target;
            if (wrapper == null)
                return null;
            return wrapper.managed;
        }

        public static void RequestTargets (Gtk.Clipboard clipboard, ClipboardTargetsReceivedFunc cb) {
            var cb_wrapper = new ClipboardTargetsReceivedFuncWrapper (cb);
            cb_wrapper.PersistUntilCalled ();
            gtk_clipboard_request_targets (clipboard.Handle, cb_wrapper.NativeDelegate, IntPtr.Zero);
        }
    }

}