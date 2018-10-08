// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//
// Copyright (c) 2007 Novell, Inc. (http://www.novell.com)
//
// Authors:
//	Jackson Harper (jackson@ximian.com)
//


using System.IO;

namespace Limaki.Common.Text.RTF.Parser {
	
    internal class Image {
        public float Width { get; set; }
        public float Height { get; set; }
    }

	public class Picture {
		private Image image;
		private MemoryStream data;
		private float width = -1;
		private float height = -1;

		private readonly static float dpix;

		static Picture ()
		{
            //dpix = TextRenderer.GetDpi ().Width;
		}

		public Picture ()
		{
			
		}

		public Minor ImageType { get; set; }

		public MemoryStream Data => data ?? (data = new MemoryStream ());

		public float Width {
			get {
				var w = width;
				if (w != -1) return w;
				if (image == null)
					image = ToImage ();
				w = image.Width;
				return w;
				
			}
		}

        private Image ToImage() {
            throw new System.NotImplementedException();
        }

		public float Height {
			get {
				var h = height;
				if (h != -1) return h;
				if (image == null)
					image = ToImage ();
				h = image.Height;
				return h;
			}
		}


		public void SetWidthFromTwips (int twips)
		{
			width = (int) (((float) twips / 1440.0F) * dpix + 0.5F);
		}

		public void SetHeightFromTwips (int twips)
		{
			height = (int) (((float) twips / 1440.0F) * dpix + 0.5F);
		}

		//
		// Makes sure that we got enough information to actually use the image
		//
		public bool IsValid ()
		{
			if  (data == null)
				return false;
			return ImageType == Minor.PngBlip || ImageType == Minor.WinMetafile;
		}


	}

}

