// 
// Image.cs
//  
// Author:
//       Lluis Sanchez <lluis@xamarin.com>
//       Lytico (http://www.limada.org)
// 
// Copyright (c) 2011 Xamarin Inc
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System;
using Xwt.Backends;
using Xwt.Engine;
using System.Reflection;
using System.IO;

namespace Xwt.Drawing
{
    public sealed class Image: XwtObject<Image,ImageBackendHandler>, IDisposable
	{
	
		internal Image (WidgetRegistry registry, object backend): base(registry, backend) 
        {
        }

		public Image (Image image): base (image.Registry,image.handler.Copy (image.Backend))
		{
		}
		
		public static Image FromResource (WidgetRegistry registry, Type type, string resource)
		{
		    return FromResource(registry, type.Assembly, resource);
		}

        public static Image FromResource (WidgetRegistry registry, Assembly asm, string resource)
		{
            var img = GetBackendHandler(registry).LoadFromResource(asm, resource);
			if (img == null)
				throw new InvalidOperationException ("Resource not found: " + resource);
            return new Image(registry, img);
		}

        public static Image FromFile (WidgetRegistry registry, string file)
		{
            return new Image(registry, GetBackendHandler(registry).LoadFromFile(file));
		}

        public static Image FromStream (WidgetRegistry registry, Stream stream)
		{
            return new Image(registry, GetBackendHandler(registry).LoadFromStream(stream));
		}

        public static Image FromIcon (WidgetRegistry registry, string id, IconSize size)
		{
            return new Image(registry, GetBackendHandler(registry).LoadFromIcon(id, size));
		}

        #region static factory methods calling MainRegistry

        public static Image FromResource (Type type, string resource) 
        {
            return Image.FromResource(WidgetRegistry.MainRegistry, type, resource);
        }

        public static Image FromResource (Assembly asm, string resource) 
        {
            return Image.FromResource(WidgetRegistry.MainRegistry, asm, resource);
        }

        public static Image FromFile (string file) 
        {
            return Image.FromFile(WidgetRegistry.MainRegistry, file);
        }

        public static Image FromStream (Stream stream) 
        {
            return Image.FromStream(WidgetRegistry.MainRegistry, stream);
        }

        public static Image FromIcon (string id, IconSize size) 
        {
            return Image.FromIcon(WidgetRegistry.MainRegistry, id, size);
        }

        #endregion

        public Size Size {
			get { return handler.GetSize (Backend); }
		}
		
		public void SetPixel (int x, int y, Color color)
		{
			handler.SetPixel (Backend, x, y, color);
		}
		
		public Color GetPixel (int x, int y)
		{
			return handler.GetPixel (Backend, x, y);
		}
		
		public Image Scale (double scale)
		{
			double w = Size.Width * scale;
			double h = Size.Height * scale;
			return new Image (Registry, handler.Resize (Backend, w, h));
		}
		
		public Image Scale (double scaleX, double scaleY)
		{
			double w = Size.Width * scaleX;
			double h = Size.Height * scaleY;
            return new Image(Registry, handler.Resize(Backend, w, h));
		}
		
		public Image Resize (double width, double height)
		{
            return new Image(Registry, handler.Resize(Backend, width, height));
		}
		
		public Image ResizeToFitBox (double width, double height)
		{
			double r = Math.Min (width / Size.Width, height / Size.Height);
            return new Image(Registry, handler.Resize(Backend, Size.Width * r, Size.Height * r));
		}
		
		public Image ToGrayscale ()
		{
			throw new NotImplementedException ();
		}
		
		public Image ChangeOpacity (double opacity)
		{
            return new Image(Registry, handler.ChangeOpacity(Backend, opacity));
		}
		
		public void CopyArea (int srcX, int srcY, int width, int height, Image dest, int destX, int destY)
		{
			handler.CopyArea (Backend, srcX, srcY, width, height, dest.Backend, destX, destY);
		}
		
		public Image Crop (int srcX, int srcY, int width, int height)
		{
            return new Image(Registry, handler.Crop(Backend, srcX, srcY, width, height));
		}
		
		public void Dispose ()
		{
			handler.Dispose (Backend);
		}

        public void Save(Stream result, string p) {
            throw new NotImplementedException();
        }
    }
}

