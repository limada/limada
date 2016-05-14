using System;
using System.Linq;
using Xwt.Backends;

using Xwt.Drawing;
using Xwt.GtkBackend;
using System.Collections.Generic;
using System.Diagnostics;

namespace Limaki.View.GtkBackend {
    public class GtkPrototyper {
        // TODO: mix with CairoContextBackendHandler.DrawTextLayout
        void Analyselayout (Pango.Layout pl, double x, double y) {
            var text = pl.Text;
            var iter = pl.Iter;
            var lli = 0;
            var ll = 0;
            var li = 0;
            var iRun = 0;
            var numGlyph = 0;
            Pango.LayoutLine line = null;
            Pango.LayoutRun run;

            Action render = () => {
                if (numGlyph == 0)
                    return;
                ll = numGlyph;
                var newline = line != null && line.StartIndex != li;
                if (newline) {
                    li = line.StartIndex;
                }

                if (iter.Line.StartIndex != line.StartIndex && iter.Line.IsParagraphStart)
                    ll--;

                var s = text.Substring (lli, Math.Min (ll, text.Length - lli));
                Trace.WriteLine (s);
                if (iter.Line.IsParagraphStart)
                    ll++;
                lli += ll;
            };

            do {
                var newRun = iter.Run.Glyphs != null && iter.Run.Item.Offset != iRun;
                if (newRun) {
                    iRun = iter.Run.Item.Offset;

                    render ();

                    numGlyph = 0;
                    line = null;

                }
                if (line == null) {
                    line = iter.Line;
                    run = iter.Run;
                }

                numGlyph++;
            } while (iter.NextChar ());
            render ();

        }

        Pango.Layout clone (Pango.Layout pl) {
            var al = new Pango.Layout (pl.Context) {
                FontDescription = pl.FontDescription,
                Width = pl.Width,
                Ellipsize = pl.Ellipsize,
                Wrap = pl.Wrap,
                Alignment = pl.Alignment,
                Justify = pl.Justify,
                SingleParagraphMode = pl.SingleParagraphMode,
                Spacing = pl.Spacing,
            };
            al.SetText (pl.Text);
            return al;
        }

        protected void prototype (Pango.Layout pl, double x, double y) {

            var text = pl.Text;

            foreach (var l in pl.Lines) {
                Trace.WriteLine (text.Substring (l.StartIndex, Math.Min (l.Length, text.Length - l.StartIndex)));
            }

            Trace.WriteLine ("----------");
            // see also: https://github.com/GNOME/tomboy/blob/master/Tomboy/Addins/PrintNotes/PrintNotesNoteAddin.cs
            // https://sourcecodebrowser.com/pango1.0/1.14.8/pango-renderer_8c.html#aaceb17b572fe371dc78068a27551ccbc

            // try with run
            var iter = pl.Iter;
            var lastline = default (Pango.LayoutLine);
            var lli = 0;
            var ll = 0;
            var li = 0;
            //text = text.Replace ("\n", "");//.Replace("\r","");
            do {

                if (iter.Run.Glyphs != null) {
                    ll = iter.Run.Glyphs.NumGlyphs;
                    var newline = iter.Line != null && iter.Line.StartIndex != li;
                    if (newline) {
                        li = iter.Line.StartIndex;
                    }

                    if (newline && iter.Line.IsParagraphStart) {
                        lli++;
                    }
                    var s = text.Substring (lli, Math.Min (ll, text.Length - lli));
                    Trace.WriteLine (s);

                    lli += ll;

                }
            } while (iter.NextRun ());

            Trace.WriteLine ("----------");
            lastline = default (Pango.LayoutLine);
            lli = 0;
            ll = 0;
            li = 0;
            iter = pl.Iter;
            var iRun = 0;
            var numGlyph = 0;
            Pango.LayoutLine line = null;
            Pango.LayoutRun run; ;
            //text = text.Replace ("\n", "");//.Replace("\r","");
            Action render = () => {
                if (numGlyph == 0)
                    return;
                ll = numGlyph;
                var newline = line != null && line.StartIndex != li;
                if (newline) {
                    li = line.StartIndex;
                }

                if (iter.Line.StartIndex != line.StartIndex && iter.Line.IsParagraphStart)
                    ll--;

                var s = text.Substring (lli, Math.Min (ll, text.Length - lli));
                Trace.WriteLine (s);
                if (iter.Line.IsParagraphStart)
                    ll++;
                lli += ll;
            };

            do {
                var newRun = iter.Run.Glyphs != null && iter.Run.Item.Offset != iRun;
                if (newRun) {
                    iRun = iter.Run.Item.Offset;

                    render ();


                    numGlyph = 0;
                    line = null;

                }
                if (line == null) {
                    line = iter.Line;
                    run = iter.Run;
                }

                numGlyph++;
            } while (iter.NextChar ());
            render ();

            iter = pl.Iter;

            var logical_rect = new Pango.Rectangle ();
            var lineX = 0;

            Action<string, Func<bool>> traceiter = (s, f) => {
                lastline = null;
                Trace.Write (s + "\t");
                iter = pl.Iter;
                do {
                    var newLine = lastline == null || lastline.StartIndex != iter.Line.StartIndex;
                    Trace.Write (string.Format ("{1}{0}", iter.Index, (newLine ? "|" : " ")));
                    if (newLine)
                        lastline = iter.Line;
                } while (f ());
                Trace.WriteLine ("");
            };

            traceiter ("line", () => iter.NextLine ());
            traceiter ("run", () => iter.NextRun ());
            traceiter ("cluster", () => iter.NextCluster ());
            traceiter ("char", () => iter.NextChar ());

        }
    }
}

