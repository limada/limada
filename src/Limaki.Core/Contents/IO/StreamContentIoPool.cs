using System;
using System.Collections.Generic;
using System.IO;
using Limaki.Common;
using System.Linq;

namespace Limaki.Contents.IO {

    public class StreamContentIoPool : ContentIoPool<Stream, Content<Stream>> { 
        
        public IEnumerable<ContentInfo> ContentInfos { 
            get { return ContentInfoPool;}
        }

    }

    public class StreamContentIoManager : IoUriManager<Stream, Content<Stream>, StreamContentIoPool> { }

    public static class ContentIoExtensions {
        
        public static string MimeType (this long id) {
            return Registry.Pooled<StreamContentIoPool> ().ContentInfos.Where (p => p.ContentType == id).Select(p=>p.MimeType).FirstOrDefault()??$"{id:X16}";
        }

    }
}