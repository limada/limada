using System;
using System.Runtime.InteropServices;
using Gdk;
using Gtk;
using Xwt.CairoBackend;

namespace Xwt.GtkBackend {
    public static class GtkSharpWorkarounds {
        
        public static Color ToGdkColor(this Gdk.RGBA color)=>new Color((byte)(color.Red * 255), (byte)(color.Green * 255), (byte)(color.Blue * 255));

        static string StyleContextSandbox(Gtk.StyleContext ctx) {
            var result = $"{string.Join (",", ctx.ListClasses ())}";
            Console.WriteLine(result);
            return result;
        }

        public static RGBA? LookupColor (this Gtk.StyleContext ctx, string colorName) {
            if (ctx.LookupColor (colorName, out var col)) {
                return col;
            }

            return null;
        }

        public static Gdk.Color ColorFor(this Gtk.StyleContext ctx, string postfix, Gtk.StateType state) {
            var prefix = string.Empty;
            // see: https://developer.gnome.org/gtk3/stable/gtk-migrating-GtkStyleContext-css.html
            // examples: (see: gtk.css) selected_bg_color insensitive_bg_color base_color theme_text_color insensitive_base_color theme_unfocused_fg_color theme_unfocused_text_color theme_unfocused_bg_color

            switch (state) {
                case StateType.Normal:
                    prefix = "theme_unfocused_";
                    break;
                case StateType.Active:
                    break;
                case StateType.Prelight:
                    break;
                case StateType.Selected:
                    prefix = "selected_";
                    break;
                case StateType.Insensitive:
                    prefix = "insensitive_";
                    break;
                case StateType.Inconsistent:
                    break;
                case StateType.Focused:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(state), state, null);
            }
            if (ctx.LookupColor($"{prefix}{postfix}_color", out var col)) {
                return col.ToGdkColor();
            }

            ctx.LookupColor("base_color", out col);
            return col.ToGdkColor();
        }
        
        public static Gdk.Color Background(this Gtk.Style it, Gtk.StateType state) {

            return it.Context.ColorFor("bg", state);
        }
        
        public static Gdk.Color Dark(this Gtk.Style it, Gtk.StateType state)
        {
            // TODO
            return it.Foreground(state);
            
        }
        
        public static Gdk.Color Foreground(this Gtk.Style it, Gtk.StateType state)
        {
            return it.Context.ColorFor("fg", state);
          
        }
        
        public static Gdk.Color Base(this Gtk.Style it, Gtk.StateType state)
        {
            return it.Context.ColorFor("", state);
        }

        public static void ModifyBase(this Gtk.Widget it, Gtk.StateType state, Gdk.Color color) {
            // TODO
        }

    
    }
}