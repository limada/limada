using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using static Xwt.Interop.DllImportGtk;
using static Xwt.Interop.DllImportGdk;
using static Xwt.Interop.DllImportPango;
using static Xwt.Interop.DllImportGObj;

#if XWT_GTK3
using GtkTreeModel = Gtk.ITreeModel;
#else
using GtkTreeModel = Gtk.TreeModel;
#endif
using System.Diagnostics;

namespace Xwt.GtkBackend {

    public static partial class GtkWorkarounds {
		
#if !XWT_GTK3
		// Works around BXC #3801 - Managed Container subclasses are incorrectly resurrected, then leak.
		// It does this by registering an alternative callback for gtksharp_container_override_forall, which
		// ignores callbacks if the wrapper no longer exists. This means that the objects no longer enter a
		// finalized->release->dispose->re-wrap resurrection cycle.
		// We use a dynamic method to access internal/private GTK# API in a performant way without having to track
		// per-instance delegates.
		public static void FixContainerLeak (Gtk.Container c)
		{
			if (containerLeakFixed) {
				return;
			}

			FixContainerLeak (c.GetType ());
		}

		static void FixContainerLeak (Type t)
		{
			if (containerLeakFixed) {
				return;
			}

			if (fixedContainerTypes == null) {
				try {
					gtksharp_container_leak_fixed_marker ();
					containerLeakFixed = true;
					return;
				} catch (EntryPointNotFoundException) {
				}
				fixedContainerTypes = new HashSet<Type>();
				forallCallbacks = new Dictionary<IntPtr, ForallDelegate> ();
			}

			if (!fixedContainerTypes.Add (t)) {
				return;
			}

			//need to fix the callback for the type and all the managed supertypes
			var lookupGType = typeof (GLib.Object).GetMethod ("LookupGType", BindingFlags.Static | BindingFlags.NonPublic);
			do {
				var gt = (GLib.GType) lookupGType.Invoke (null, new[] { t });
				var cb = CreateForallCallback (gt.Val);
				forallCallbacks[gt.Val] = cb;
				gtksharp_container_override_forall (gt.Val, cb);
				t = t.BaseType;
			} while (fixedContainerTypes.Add (t) && t.Assembly != typeof (Gtk.Container).Assembly);
		}

		static ForallDelegate CreateForallCallback (IntPtr gtype)
		{
			var dm = new DynamicMethod (
				"ContainerForallCallback",
				typeof(void),
				new Type[] { typeof(IntPtr), typeof(bool), typeof(IntPtr), typeof(IntPtr) },
				typeof(GtkWorkarounds).Module,
				true);
			
			var invokerType = typeof(Gtk.Container.CallbackInvoker);
			
			//this was based on compiling a similar method and disassembling it
			ILGenerator il = dm.GetILGenerator ();
			var IL_002b = il.DefineLabel ();
			var IL_003f = il.DefineLabel ();
			var IL_0060 = il.DefineLabel ();
			var label_return = il.DefineLabel ();

			var loc_container = il.DeclareLocal (typeof(Gtk.Container));
			var loc_obj = il.DeclareLocal (typeof(object));
			var loc_invoker = il.DeclareLocal (invokerType);
			var loc_ex = il.DeclareLocal (typeof(Exception));

			//check that the type is an exact match
			// prevent stack overflow, because the callback on a more derived type will handle everything
			il.Emit (OpCodes.Ldarg_0);
			il.Emit (OpCodes.Call, typeof(GLib.ObjectManager).GetMethod ("gtksharp_get_type_id", BindingFlags.Static | BindingFlags.NonPublic));

			il.Emit (OpCodes.Ldc_I8, gtype.ToInt64 ());
			il.Emit (OpCodes.Newobj, typeof (IntPtr).GetConstructor (new Type[] { typeof (Int64) }));
			il.Emit (OpCodes.Call, typeof (IntPtr).GetMethod ("op_Equality", BindingFlags.Static | BindingFlags.Public));
			il.Emit (OpCodes.Brfalse, label_return);

			il.BeginExceptionBlock ();
			il.Emit (OpCodes.Ldnull);
			il.Emit (OpCodes.Stloc, loc_container);
			il.Emit (OpCodes.Ldsfld, typeof (GLib.Object).GetField ("Objects", BindingFlags.Static | BindingFlags.NonPublic));
			il.Emit (OpCodes.Ldarg_0);
			il.Emit (OpCodes.Box, typeof (IntPtr));
			il.Emit (OpCodes.Callvirt, typeof (System.Collections.Hashtable).GetProperty ("Item").GetGetMethod ());
			il.Emit (OpCodes.Stloc, loc_obj);
			il.Emit (OpCodes.Ldloc, loc_obj);
			il.Emit (OpCodes.Brfalse, IL_002b);

			var tref = typeof (GLib.Object).Assembly.GetType ("GLib.ToggleRef");
			il.Emit (OpCodes.Ldloc, loc_obj);
			il.Emit (OpCodes.Castclass, tref);
			il.Emit (OpCodes.Callvirt, tref.GetProperty ("Target").GetGetMethod ());
			il.Emit (OpCodes.Isinst, typeof (Gtk.Container));
			il.Emit (OpCodes.Stloc, loc_container);
			
			il.MarkLabel (IL_002b);
			il.Emit (OpCodes.Ldloc, loc_container);
			il.Emit (OpCodes.Brtrue, IL_003f);
			
			il.Emit (OpCodes.Ldarg_0);
			il.Emit (OpCodes.Ldarg_1);
			il.Emit (OpCodes.Ldarg_2);
			il.Emit (OpCodes.Ldarg_3);
			il.Emit (OpCodes.Call, typeof (Gtk.Container).GetMethod ("gtksharp_container_base_forall", BindingFlags.Static | BindingFlags.NonPublic));
			il.Emit (OpCodes.Br, IL_0060);
			
			il.MarkLabel (IL_003f);
			il.Emit (OpCodes.Ldloca_S, 2);
			il.Emit (OpCodes.Ldarg_2);
			il.Emit (OpCodes.Ldarg_3);
			il.Emit (OpCodes.Call, invokerType.GetConstructor (
				BindingFlags.Instance | BindingFlags.NonPublic, null, new Type[] { typeof (IntPtr), typeof (IntPtr) }, null));
			il.Emit (OpCodes.Ldloc, loc_container);
			il.Emit (OpCodes.Ldarg_1);
			il.Emit (OpCodes.Ldloc, loc_invoker);
			il.Emit (OpCodes.Box, invokerType);
			il.Emit (OpCodes.Ldftn, invokerType.GetMethod ("Invoke"));
			il.Emit (OpCodes.Newobj, typeof (Gtk.Callback).GetConstructor (
				BindingFlags.Instance | BindingFlags.Public, null, new Type[] { typeof (object), typeof (IntPtr) }, null));
			var forallMeth = typeof (Gtk.Container).GetMethod ("ForAll",
				BindingFlags.Instance | BindingFlags.NonPublic, null, new Type[] { typeof (bool), typeof (Gtk.Callback) }, null);
			il.Emit (OpCodes.Callvirt, forallMeth);
			
			il.MarkLabel (IL_0060);
			
			il.BeginCatchBlock (typeof (Exception));
			il.Emit (OpCodes.Stloc, loc_ex);
			il.Emit (OpCodes.Ldloc, loc_ex);
			il.Emit (OpCodes.Ldc_I4_0);
			il.Emit (OpCodes.Call, typeof (GLib.ExceptionManager).GetMethod ("RaiseUnhandledException"));
			il.Emit (OpCodes.Leave, label_return);
			il.EndExceptionBlock ();
			
			il.MarkLabel (label_return);
			il.Emit (OpCodes.Ret);
			
			return (ForallDelegate) dm.CreateDelegate (typeof (ForallDelegate));
		}
#endif
    }

}