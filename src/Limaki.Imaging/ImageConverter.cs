using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BitMiracle.LibTiff.Classic;
using Hjg.Pngcs;
using Limaki.Contents;
using Limaki.Common;

namespace Limaki.ImageLibs {

    public class ImageConverter : ContentConverter<Stream> {

        public override IEnumerable<Tuple<long, long>> SupportedTypes {
            get { yield return Tuple.Create(ContentTypes.TIF, ContentTypes.PNG); }
        }

        public override Content<Stream> Use (Content<Stream> source, Content<Stream> sink) {
            if (ProveTypes(source, ContentTypes.TIF, sink, ContentTypes.PNG)) {
                if (sink.Data == null)
                    sink.Data = new MemoryStream();
                sink.Compression = CompressionType.neverCompress;

                var error = Tif2Png (source.Data, sink.Data);
                if (error != null) {
                    sink.ContentType = ContentTypes.Text;
                    sink.Data = string.Format ("{0}: {1}", source.Description ?? "", error).AsUnicodeStream ();
                }
                
            }
            return sink;

        }

        public string Tif2Png (Stream tifSource, Stream pngSink) {
            try {
                using (var tif = Tiff.Open (tifSource, "r")) {
                    string error = "";
                    var tifrgb = TiffRgbaImage.Create (tif, false, out error);

                    var f = tif.GetField (TiffTag.XRESOLUTION);
                    var dpiX = f == null ? 150 : f[0].ToInt ();
                    f = tif.GetField (TiffTag.YRESOLUTION);
                    var dpiY = f == null ? 150 : f[0].ToInt ();
                    var greyscale = tifrgb.BitsPerSample == 8;
                    var palette = tifrgb.Photometric == Photometric.PALETTE;
                    var pngWriter = new PngWriter (pngSink,
                        new ImageInfo (tifrgb.Width, tifrgb.Height, tifrgb.BitsPerSample, tifrgb.Alpha == ExtraSample.ASSOCALPHA, greyscale, palette)) {
                        ShouldCloseStream = false
                    };

                    pngWriter.GetMetadata ().SetDpi (dpiX, dpiY);
                    var buflen = tif.ScanlineSize ();
                    var buffer = new byte[buflen];
                    for (int row = 0; row < tifrgb.Height; row++) {
                        tif.ReadScanline (buffer, row);
                        pngWriter.SetUseUnPackedMode (true);
                        if (tifrgb.Photometric == Photometric.MINISWHITE)
                            for (int i = 0; i < buffer.Length; i++)
                                buffer[i] = (byte) ~buffer[i];
                        pngWriter.WriteRowByte (buffer, row);
                    }
                    pngWriter.End ();
                    pngSink.Position = 0;
                    return null;
                }
            } catch (Exception e) {
                return "Conversion not supported";
            }
        }
      

    }
}
