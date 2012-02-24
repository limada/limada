using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using Limaki.Common.Text.RTF;

namespace Limaki.Winform.Controls.TextEditor {
    /// <summary>

    /// </summary>
    public class TextBoxEditorControler {

        RichTextBox editor = null;
        public TextBoxEditorControler (RichTextBox editor) {
            this.editor = editor;
            this.editor.SelectionChanged += (sender, args) => {
                if (this.SelectionChanged != null) {
                    this.SelectionChanged (editor);
                }
            };
        }


        public IEnumerable<string> FontsUsable() {
            RichTextBox tryer = new RichTextBox ();
            tryer.Text = "123";
            tryer.SelectionStart = 0;
            tryer.SelectionLength = 3;

            foreach(var family in FontFamily.Families) {
                bool usable = true;
                try {
                    using (var font = new Font(family, 10)) {
                        tryer.SelectionFont = font;
                        if (tryer.SelectionFont == null) {
                            usable = false;
                        } else if (tryer.SelectionFont.FontFamily.Name != family.Name) {
                            usable = false;
                        }
                    }

                    if (!family.IsStyleAvailable((System.Drawing.FontStyle)0xF)) {
                        usable = false;
                    }

                } catch {
                    usable = false;
                }
                if (usable)
                    yield return family.Name;

            }
        }

        public Action<RichTextBox> SelectionChanged { get; set; }

        public bool IsSelectionChanged = false;

        
        /// <summary>
        /// TODO: add superscript, subscript,strikethroug
        /// </summary>
        /// <param name="style"></param>
        public void SetFontStyle(System.Drawing.FontStyle style) {
            SetEditorSelectedRTF((source) => {
                return new RTFHelper().SetAttributes(source,Convert(style));
            });
        }

        public static Limaki.Common.Text.RTF.FontStyle Convert(System.Drawing.FontStyle native) {
            var result = Limaki.Common.Text.RTF.FontStyle.Normal;
            if (native == null)
                return result;
            if ((native & System.Drawing.FontStyle.Italic) != 0) {
                result |= Limaki.Common.Text.RTF.FontStyle.Italic;
            }
            if ((native & System.Drawing.FontStyle.Underline) != 0) {
                result |= Limaki.Common.Text.RTF.FontStyle.Underline;
            }
            if ((native & System.Drawing.FontStyle.Bold) != 0) {
                result |= Limaki.Common.Text.RTF.FontStyle.Bold;
            }
            return result;
        }

        public void SetEditorSelectedRTF(Func<Stream, Stream> func) {
            try {
                if (!IsSelectionChanged) {
                    string selection = editor.SelectedRtf;

                    var pos = editor.SelectionStart;
                    var len = editor.SelectionLength;
                    if (!string.IsNullOrEmpty(selection)) {
                        var sb = new StringBuilder(selection);
                        
                        // every operation, if select all, adds additional (empty) paragraphs
                        // so we remove the last par:
                        if (selection.EndsWith("\\par\r\n}\r\n")) {
                            sb.Remove (sb.Length - 9, 9);
                            sb.Append ("}");
                        }

                        var source = new MemoryStream(Encoding.ASCII.GetBytes(sb.ToString()));
                        var target = func (source);
                        if (source != target) {
                            target.Position = 0;
                            var reader = new StreamReader(target);
                            editor.SelectedRtf = reader.ReadToEnd();
                            reader.Close();
                        }
                        source.Dispose();
                    }
                    editor.SelectionStart = pos;
                    editor.SelectionLength = len;
                    editor.Focus();
                }
            } catch (Exception ex) {
                MessageBox.Show(ex.Message.ToString(), "Error");
            }
        }


        // remark: if different fontfamilies,
        // selectionRTF has size*2 fs-controls
        // like: fs32 = size 16; fs 28 = size 14
        // looks like: 
        // {\rtf1\ansi\ansicpg1252\deff0\deflang1031
        // {\fonttbl
        // {\f0\fnil\fcharset0 Microsoft Sans Serif;}
        // {\f1\fnil\fcharset0 DejaVu Sans;}}
        // \uc1\pard\f0\fs28 text with size 14 \b\f1\fs32 text with size 16 \b0\f0\fs28 text with size 14}


        public void SetFontSize(float size) {
            SetEditorSelectedRTF((source)=> {
                return new RTFHelper().SetToSameSize(source, (int)(size * 2));
            });
        }

        public  void SetFontFamiliy(string newFontFamiliy) {
            SetEditorSelectedRTF((source) => {
                return new RTFHelper().SetToSameFont(source, newFontFamiliy);
            });
        }
    }
}