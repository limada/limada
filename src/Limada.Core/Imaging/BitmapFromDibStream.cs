using System;
using System.IO;

namespace Limaki.Drawing.Imaging {
    /// <summary>
    /// found at: http://www.codeproject.com/KB/GDI-plus/DIBtoBitmap.aspx?fid=355741&df=90&mpp=25&noise=3&sort=Position&view=Quick&select=2609843#xx2609843xx
    /// </summary>
    public class BitmapFromDibStream:Stream {
        Stream dib = null;
        byte[] header = null;
        public BitmapFromDibStream(Stream dib) {
            this.dib = dib;
            makeHeader();
        }

        private void makeHeader() {
            var reader = new BinaryReader(dib);

            int headerSize = reader.ReadInt32();
            int pixelSize = (int) dib.Length - headerSize;
            int fileSize = 14 + headerSize + pixelSize;

            var bmp = new MemoryStream(14);
            var writer = new BinaryWriter(bmp);

            /* Get the palette size
                   * The Palette size is stored as an int32 at offset 32
                   * Actually stored as number of colours, so multiply by 4
                   */
            dib.Position = 32;
            int paletteSize = 4 * reader.ReadInt32();

            // Get the palette size from the bbp if none was specified
            if ( paletteSize == 0 ) {
                /* Get the bits per pixel
                     * The bits per pixel is store as an int16 at offset 14
                     */
                dib.Position = 14;
                int bpp = reader.ReadInt16();

                // Only set the palette size if the bpp < 16
                if ( bpp < 16 )
                    paletteSize = 4 * ( 2 << ( bpp - 1 ) );
            }

            // 1. Write Bitmap File Header:			 
            writer.Write((byte) 'B');
            writer.Write((byte) 'M');
            writer.Write(fileSize);
            writer.Write((int) 0);
            writer.Write(14 + headerSize+paletteSize);
            header = bmp.GetBuffer();
            writer.Close();
            dib.Position = 0;
        }

        public override bool CanRead {
            get { return true; }
        }

        public override bool CanSeek {
            get { return false; }
        }

        public override bool CanWrite {
            get { return false; }
        }

        public override void Flush () {
                
        }

        public override long Length {
            get { return 14 + dib.Length; }
        }

        private long _position = 0;
        public override long Position {
            get { return _position; }
            set { 
                _position = value;
                if ( _position > 14 )
                    dib.Position = _position - 14;
            }
        }

        public override int Read ( byte[] buffer, int offset, int count ) {
            //attention: offset is for buffer only!
            int dibCount = count;
            int dibOffset = offset - 14;
            int result = 0;
            if (_position<14){
                int headerCount = Math.Min(count + (int)_position, 14);
                Array.Copy(header, (int)_position, buffer, offset, headerCount);
                dibCount -= headerCount;
                _position += headerCount;
                result = headerCount;
            }
            if ( _position >= 14 ) {
                result += dib.Read(buffer, offset + result, dibCount);
                _position = 14 + dib.Position;
            }
            return (int)result;
        }

        public override long Seek ( long offset, SeekOrigin origin ) {
            throw new Exception("The method or operation is not implemented.");
        }

        public override void SetLength ( long value ) {
            throw new Exception("The method or operation is not implemented.");
        }

        public override void Write ( byte[] buffer, int offset, int count ) {
            throw new Exception("The method or operation is not implemented.");
        }
    }
}