using System.Globalization;
using Xwt.Backends;
using Xwt.Drawing;

namespace Xwt.Html5.Backend {

    public static class Html5Extensions {

        public static string ToHtml (this double value) {
            return value.ToString(CultureInfo.InvariantCulture);
        }
        
        public static string ToHtml (this FontStyle value) {
            if (FontStyle.Normal == value)
                return "";
            if (FontStyle.Italic == value)
                return "italic";
            return value.ToString ();
        }

        public static string ToStyle (this Color value) {
            return string.Format("\"rgba({0}, {1}, {2}, {3})\"",
                (byte)(value.Red * 255d), (byte)(value.Green * 255d), (byte)(value.Blue * 255d), value.Alpha.ToHtml ());
            
        }

        public static Font ToXwt (this FontData backend) {
            return CreateFrontend<Font>(backend);
        }

        public static T CreateFrontend<T> (object backend) {
            return ToolkitEngineBackend.GetToolkitBackend<Html5Engine>().CreateFrontend<T>(backend);
        }
    }
}