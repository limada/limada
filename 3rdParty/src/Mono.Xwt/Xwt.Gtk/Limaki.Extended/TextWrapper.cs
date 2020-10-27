// 
// TextWrapper.cs
//  
// Author:
//       Lytico 
// 
// Copyright (c) 2014 Lytico (http://www.limada.org)
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
using Xwt.Drawing;
using System.Linq;
using Xwt;
using Xwt.GtkBackend;
using Xwt.CairoBackend;
using System.Collections.Generic;

namespace Xwt.GtkBackend
{
	public class TextWrapper
	{

		public double LineY { get; protected set; }

		public double LineWidth { get; protected set; }

		public double LineHeight { get; protected set; }

		public double Baseline { get; protected set; }

		public int LineStart { get; protected set; }

		public int CursorPos { get; protected set; }

		public bool HasLineFeed { get; protected set; }

		public Action<TextWrapper> SingleLine { get; set; }

		public Action<TextWrapper> MultiLine { get; set; }

		public double MaxHeight { get; protected set; }

		public double MaxWidth { get; protected set; }

		public Xwt.Size PreferedSize { get; set; }

		internal void Wrap (TextLayout layout, CairoContextBackend ctx)
		{

			var text = layout.Text;
			var be = (GtkTextLayoutBackendHandler.PangoBackend) Toolkit.GetBackend (layout);
			var pl = be.Layout;

			var fe = ctx.Context.FontExtents;
			Baseline = fe.Ascent / (fe.Ascent + fe.Descent);

			LineHeight = fe.Ascent + fe.Descent;
			LineY = LineHeight ; 

			var iLf = text.IndexOfAny (new char[]{ '\n', '\r' });
			HasLineFeed = iLf >= 0;

			var textWidth = layout.Width;

			if (!HasLineFeed && (layout.Height <= 0 || layout.Height <= LineHeight) && (layout.Width <= 0 || textWidth <= layout.Width)) {
				LineWidth = textWidth;
				SingleLine (this);
			} else {

				var ellipsize = pl.Ellipsize;
				pl.Ellipsize = Pango.EllipsizeMode.None;

				var lc = pl.LineCount;
     			var scale = Pango.Scale.PangoScale;
				var wrap = pl.Wrap;

				var layoutHeight = layout.Height;
				if (layoutHeight <= 0) {
					var plw = 0;
					var plh = 0;
					pl.GetSize (out plw, out plh);
					layoutHeight = plh / scale;
				}

				MaxHeight = PreferedSize.Height > 0 ? PreferedSize.Height : (layoutHeight <= 0 ? double.MaxValue : layoutHeight);
				MaxWidth = PreferedSize.Width > 0 ? PreferedSize.Width : (layout.Width <= 0 ? textWidth : layout.Width);

				CursorPos = 0;
				LineStart = 0;

				foreach (var ll in Analyselayout(pl)) {
					if (LineY > MaxHeight)
						break;

					LineWidth = ll.Width;
					LineStart = ll.Pos;
					CursorPos = ll.Pos + ll.Len;

					MultiLine (this);
					LineY += ll.Height;
								
				}
				pl.Ellipsize = ellipsize;
			}
		}

		public struct Line {
			public int Pos  {get;set;}
			public int Len  {get;set;}
			public double Width {get;set;}
			public double Height {get;set;}
		}

		IEnumerable<Line> Analyselayout (Pango.Layout pl)
		{
			var text = pl.Text;
			var iter = pl.Iter;
			var lli = 0;
			var ll = 0;
			var li = 0;
			var iRun = 0;
			var numGlyph = 0;
			Pango.LayoutLine line = iter.Line;
			Pango.LayoutRun run = iter.Run;
			var scale = Pango.Scale.PangoScale;

			Func<Line> render = () => {
				if (numGlyph == 0)
					return default(Line);
				ll = numGlyph;
				var newline = line != null && line.StartIndex != li;
				if (newline) {
					li = line.StartIndex;
				}

				if (iter.Line.StartIndex != line.StartIndex && iter.Line.IsParagraphStart)
					ll--;
				var llrect = new Pango.Rectangle();
				var rect = new Pango.Rectangle();
				line.GetExtents(ref rect, ref llrect);
				var lline = new Line{
					Pos = lli, Len = Math.Min (ll, text.Length - lli), Width = run.Glyphs.Width, Height = llrect.Height/scale
				};
				if (iter.Line.IsParagraphStart)
					ll++;
				lli += ll;
				return lline;
			};

			do {
				var newRun = iter.Run.Glyphs != null && iter.Run.Item.Offset != iRun;
				if (newRun) {
					iRun = iter.Run.Item.Offset;

					yield return render ();

					numGlyph = 0;
					line = null;

				}
				if (line == null) {
					line = iter.Line;
					run = iter.Run;
				}

				numGlyph++;
			} while(iter.NextChar ());
			yield return render ();

		}
	}

	public partial class GtkTextLayoutBackendHandler  {
		WrapMode wrapMode;
		public override void SetWrapMode (object backend, WrapMode value) 
		{
			var tl = (PangoBackend)backend;
			if (value == WrapMode.Word || value == WrapMode.None)
				tl.Layout.Wrap = Pango.WrapMode.Word;
			else if (value == WrapMode.Character)
				tl.Layout.Wrap = Pango.WrapMode.Char;
			else if (value == WrapMode.WordAndCharacter)
				tl.Layout.Wrap = Pango.WrapMode.WordChar;
			wrapMode = value;
		}

		public override object Create ()
		{
			var layout =  new PangoBackend {
				Layout = Pango.CairoHelper.CreateLayout (SharedContext)
			};
			SetTrimming (layout, TextTrimming.Word);
			SetWrapMode (layout, WrapMode.Word);
			return layout;

		}

		public override object Create (Context context) {
			return Create();
		}
	}
}