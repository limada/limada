using System;
using Xwt.GtkBackend;

namespace Limaki.View.GtkBackend {

    /// <summary>
    /// try to resolve clipboard problem:
    /// ClipboardTargetsReceivedFunc should have: (Gtk.Clipboard clipboard, Gdk.Atom[] atoms, int n_atoms)
    /// that is: Atom is an array
    /// </summary>
    public class GtkClipboardBackend1 : GtkClipboardBackend { 

        public override bool IsTypeAvailable (Xwt.TransferDataType type) {
            RequestTargets ();
            return base.IsTypeAvailable (type);
        }

        public virtual void RequestTargets () {
            clipboard.RequestTargets (ClipboardTargetsReceivedFunc);
        }

        public void ClipboardTargetsReceivedFunc (Gtk.Clipboard clipboard, Gdk.Atom atoms, int n_atoms) {
            if (atoms == null)
                return;
            // error in GtkSharp: atoms should be Gdk.Atom[]
            // var ats = (Gdk.Atom[])(object)atoms;
            //var ats = (object)GLib.Opaque.GetOpaque (atoms.Handle, typeof (Gdk.Atom  ), false);
            // var currentStruct = default(Gdk.Atom);
            // System.Runtime.InteropServices.Marshal.PtrToStructure (atoms.Handle, currentStruct);
            //GLib.Marshaller.PtrArrayToArray (atoms.Handle, true, true, typeof (Gdk.Atom)); // SIGSEV
            //foreach (var at in ats) {
            //    Console.WriteLine (at);
            //}
            // var s = GLib.Marshaller.NullTermPtrToStringArray (atoms.Handle, false);/// SIGSEV
            var at = new Gdk.Atom (atoms.Handle);
        }
    }
}
