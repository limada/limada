using System.Collections.Generic;
using System.IO;
using System.Text;
using Limaki.Common.Text.RTF.Parser;

namespace Limaki.Common.Text.RTF {
    public class RTFFilter {
        public Stream Remove(Stream stream, IEnumerable<Pair<int, int>> removeList) {
            // refacoring: if the list is with order highest position first, then
            // it would be possible to use the same stream


            Stream newStream = new MemoryStream();
            stream.Position = 0;
            int count = 0;
            byte[] buffer = null;
            foreach (var s in removeList) {
                count = s.One - 1 - (int)stream.Position;
                buffer = new byte[count];
                count = stream.Read(buffer, 0, count);
                newStream.Write(buffer, 0, count);
                stream.Position = s.Two;
            }

            count = (int)(stream.Length - stream.Position);
            buffer = new byte[count];
            count = stream.Read(buffer, 0, count);
            newStream.Write(buffer, 0, count);

            newStream.Position = 0;
            stream.Position = 0;
            return newStream;
        }

        public Stream Replace(Stream stream, IEnumerable<Record<int, int, byte[]>> replaceList) {
            Stream newStream = new MemoryStream();
            stream.Position = 0;
            int count = 0;
            byte[] buffer = null;
            foreach (var s in replaceList) {
                count = s.One - 1 - (int)stream.Position;
                if (count > 0) {
                    buffer = new byte[count];
                    count = stream.Read(buffer, 0, count);
                    newStream.Write(buffer, 0, count);
                }
                if (s.Three != null && s.Three.Length > 0)
                    newStream.Write(s.Three, 0, s.Three.Length);

                stream.Position = s.One - 1 + s.Two;
            }

            count = (int)(stream.Length - stream.Position);
            buffer = new byte[count];
            count = stream.Read(buffer, 0, count);
            newStream.Write(buffer, 0, count);

            newStream.Position = 0;
            return newStream;
        }

        //static IDictionary<string, Parser.KeyStruct> _keyTable = null;
        //public static IDictionary<string,Parser.KeyStruct> KeyTable {
        //    get {
        //        if(_keyTable ==null) {
        //            KeyStruct[] keys = KeysInit.Init ();
        //            _keyTable = new Dictionary<string, Parser.KeyStruct>  (keys.Length);
        //            foreach(var s in keys){

        //            }
        //        }
        //        return _keyTable;
        //    }
        //}

        public string GetDoccom(Stream stream) {
            stream.Position = 0;
            Parser.RTF rtf = new Parser.RTF(stream);

            bool infoStarted = false;
            bool doccomStarted = false;

            int headerGroupCount = 0;
            rtf.ClassCallback[TokenClass.Group] = (r) => {
                if (r.Major == Major.BeginGroup) {
                    headerGroupCount++;
                } else if (r.Major == Major.EndGroup) {
                    headerGroupCount--;
                    if (infoStarted) {
                        infoStarted = headerGroupCount > 1;
                    }
                    doccomStarted = false;
                }
            };

            rtf.DestinationCallback = new DestinationCallback();
            rtf.DestinationCallback[Minor.Info] = (r) => {
                infoStarted = true;
            };

            rtf.DestinationCallback[Minor.IDoccomm] = (r) => {
                doccomStarted = true;
            };

            StringBuilder info = new StringBuilder();
            rtf.ClassCallback[TokenClass.Text] = (r) => {
                if (doccomStarted) {
                    info.Append(r.EncodedText);
                }
            };

            while (rtf.GetToken() != TokenClass.EOF && headerGroupCount >= 0) {
                rtf.RouteToken();
            }
            stream.Position = 0;
            return info.ToString ();

        }

        public Stream SetDoccom(Stream stream, string infoText) {
            stream.Position = 0;
            Parser.RTF rtf = new Parser.RTF(stream);

            int infoPos = -1;
            
            int infoEndPos = -1;
            int firstHeaderGroupPos = -1;
            
            int headerGroupCount = 0;

            int doccomPos = -1;
            int doccomEndPos = -1;


            rtf.ClassCallback[TokenClass.Group] = (r) => {
                if (r.Major == Major.BeginGroup) {
                    headerGroupCount++;
                    if (headerGroupCount == 2) {
                        firstHeaderGroupPos = r.CharPos;
                    }
                } else if (r.Major == Major.EndGroup) {
                    headerGroupCount--;
                    if (infoPos > 0 && headerGroupCount == 1) {
                        infoEndPos = r.CharPos;
                    }
                    if (doccomPos >0) {
                        doccomEndPos = r.CharPos;
                    }
                }
            };

            rtf.DestinationCallback = new DestinationCallback();
            rtf.DestinationCallback[Minor.Info] = (r) => {
                infoPos = r.CharPos;
            };

            rtf.DestinationCallback[Minor.IDoccomm] = (r) => {
                doccomPos = r.CharPos;
            };

            StringBuilder info = new StringBuilder();
            rtf.ClassCallback[TokenClass.Text] = (r) => {
                if (doccomPos > 0 && doccomEndPos == -1) {
                    info.Append(r.EncodedText);
                }
            };

            while (rtf.GetToken() != TokenClass.EOF && headerGroupCount >= 0) {
                rtf.RouteToken();
            }
            stream.Position = 0;
            
            var replaceList = new List<Record<int, int, byte[]>>();

            if (infoPos > 0) {
                if (doccomPos > 0) {
                    replaceList.Add(new Record<int, int, byte[]>(
                                         doccomPos+1, info.Length, Encoding.ASCII.GetBytes(infoText)));
                } else {
                    replaceList.Add (new Record<int, int, byte[]> (
                                         infoPos, 0, Encoding.ASCII.GetBytes ("{\\doccomm "+infoText+"}")));
                }
            } else {
                replaceList.Add(new Record<int, int, byte[]>(
                                     firstHeaderGroupPos, 0, 
                                     Encoding.ASCII.GetBytes("{\\info{\\doccomm "+infoText+"}}")));
            }
            return Replace(stream, replaceList);
        }
    }
}