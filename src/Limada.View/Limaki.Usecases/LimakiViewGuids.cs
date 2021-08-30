using System;

namespace Limaki.Usecases {

    public class LimakiViewGuids {

        public static readonly Guid ToolkitGuid = new Guid ("312266D8-76FA-4C3B-B4C9-39F7B04EEE3F");

        public static readonly Guid GtkToolkitGuid = new Guid ("36FB195F-4AAA-4353-8A06-E792360EE63C");
        public static readonly Guid Gtk3ToolkitGuid = new Guid ("a5a091dc-7b48-4176-a462-bdf50dd34f06");
        public static readonly Guid WpfToolkitGuid = new Guid ("594AACED-2959-4192-AE9C-0A7ED9D45793");
        public static readonly Guid MacOsToolkitGuid = new Guid ("598a2a08-5fcb-4cc0-a503-9cd76a541bc4");

        public static readonly Guid BlazorViewGuid = new Guid ("b3f8e787-cce6-47db-9562-847758b95282");

        public static Guid PlatformDefault () {
            var fw = System.Runtime.InteropServices.RuntimeInformation.FrameworkDescription;
            var isCore = fw.Contains ("Core");

            if (Environment.OSVersion.Platform == PlatformID.MacOSX) {
                return MacOsToolkitGuid;
            }

            if (Environment.OSVersion.Platform == PlatformID.Win32NT) {
                return WpfToolkitGuid;
            }

            if (Environment.OSVersion.Platform == PlatformID.Unix) {
                return isCore ? Gtk3ToolkitGuid : GtkToolkitGuid;
            }

            return default;
        }

    }

}