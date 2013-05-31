using Limaki.Model.Content;
using Limaki.Model.Content.IO;

namespace Limada.Data {
    public class Db4oThingGraphInfo : ContentInfoSink {
        public Db4oThingGraphInfo ()
            : base(
                new ContentInfo[] {
                                      new ContentInfo(
                                          "Limada Things (db4o)",
                                          Db4oThingGraphContentType,
                                          "limo",
                                          "application/limo",
                                          CompressionType.None
                                          )
                                  }
                ) { }

        public static long Db4oThingGraphContentType = unchecked((long) 0xf6399a943c1b2bf3);
    }
}