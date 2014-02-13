using System;
using System.IO;

namespace Limaki.Contents.IO {

    public class StreamContentIoPool : ContentIoPool<Stream, Content<Stream>> { }

    public class StreamContentIoManager : IoUriManager<Stream, Content<Stream>, StreamContentIoPool> { }
}