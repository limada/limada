using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Xwt.GtkBackend
{

	public partial class GtkClipboardBackend
	{

	
		public override IEnumerable<TransferDataType> GetTypesAvailable ()
		{
			Gdk.Atom [] result = null;
			ClipboardTargetsReceivedFuncWrapper.RequestTargets (clipboard, (clipboard, atoms, n_atoms) => {
				result = atoms;
			});
			clipboard.WaitIsTargetAvailable (null);
			foreach (var a in result) {
				yield return TransferDataType.FromId (a.Name);
			}

		}

	}

	public delegate void ClipboardTargetsReceivedFunc (Gtk.Clipboard clipboard, Gdk.Atom [] atoms, int n_atoms);

	public class ClipboardTargetsReceivedFuncWrapper
	{

		[UnmanagedFunctionPointer (CallingConvention.Cdecl)]
		internal delegate void ClipboardTargetsReceivedFuncNative (IntPtr clipboard, IntPtr atoms, int n_atoms, IntPtr data);

#if XWT_GTKSHARP3
		[UnmanagedFunctionPointer (CallingConvention.Cdecl)]
		delegate void d_gtk_clipboard_request_targets(IntPtr raw, ClipboardTargetsReceivedFuncNative cb, IntPtr user_data);
		static d_gtk_clipboard_request_targets gtk_clipboard_request_targets = FuncLoader.LoadFunction<d_gtk_clipboard_request_targets>(FuncLoader.GetProcAddress(GLibrary.Load(Library.Gtk), "gtk_clipboard_request_targets"));

#else
		// https://developer.gnome.org/gtk3/2.90/gtk3-Clipboards.html#GtkClipboardTargetsReceivedFunc
		[DllImport (GtkInterop.LIBGTK, CallingConvention = CallingConvention.Cdecl)]
		internal static extern void gtk_clipboard_request_targets (IntPtr raw, ClipboardTargetsReceivedFuncNative cb, IntPtr user_data);
#endif
		internal void NativeCallback (IntPtr clipboard, IntPtr atoms, int n_atoms, IntPtr data)
		{
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

		internal void PersistUntilCalled ()
		{
			release_on_call = true;
			gch = GCHandle.Alloc (this);
		}

		internal ClipboardTargetsReceivedFuncNative NativeDelegate;
		ClipboardTargetsReceivedFunc managed;

		internal ClipboardTargetsReceivedFuncWrapper (ClipboardTargetsReceivedFunc managed)
		{
			this.managed = managed;
			if (managed != null)
				NativeDelegate = new ClipboardTargetsReceivedFuncNative (NativeCallback);
		}

		internal static ClipboardTargetsReceivedFunc GetManagedDelegate (ClipboardTargetsReceivedFuncNative native)
		{
			if (native == null)
				return null;
			var wrapper = (ClipboardTargetsReceivedFuncWrapper)native.Target;
			if (wrapper == null)
				return null;
			return wrapper.managed;
		}

		public static void RequestTargets (Gtk.Clipboard clipboard, ClipboardTargetsReceivedFunc cb)
		{
			var cb_wrapper = new ClipboardTargetsReceivedFuncWrapper (cb);
			cb_wrapper.PersistUntilCalled ();
			gtk_clipboard_request_targets (clipboard.Handle, cb_wrapper.NativeDelegate, IntPtr.Zero);
		}
	}
}
