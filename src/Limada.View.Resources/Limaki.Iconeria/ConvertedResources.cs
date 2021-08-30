using System;
using System.Diagnostics;
using System.IO;
using XD = Xwt.Drawing;

namespace Limaki.Iconerias {

    /// <summary>
    /// <remarks>
    /// hide this class; System.Drawing should not be visible outside
    /// </remarks>
    /// </summary>
    internal class ConvertedResources {

		internal static XD.Image LimadaLogo { get; set; }
		internal static XD.Image SubWinIcon { get; set; }

        [DebuggerStepThrough]
		static ConvertedResources(){
            LimadaLogo = AsImage (global::Limaki.View.Resources.Resource.LogoDrop32);
            SubWinIcon = AsImage (global::Limaki.View.Resources.Resource.SubWin);
		}

        internal static XD.Image AsImage (byte[] source) {
            Xwt.Drawing.Image result = null;
            using (var stream = new MemoryStream (source)) {
                try {
                    stream.Position = 0;
                    result = Xwt.Drawing.Image.FromStream (stream);
                } catch (Exception ex) {
                    Trace.TraceError ("ConvertedResources.AsImage failed: {0}",ex.Message);
                }
            }
            return result;
        }
    }
}