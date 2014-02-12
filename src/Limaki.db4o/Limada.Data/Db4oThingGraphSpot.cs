using Limaki.Model.Content;
using Limaki.Contents.IO;
using Limaki.Contents;

namespace Limada.Data {

    public class Db4oThingGraphSpot : ContentDetector {
        public Db4oThingGraphSpot ()
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