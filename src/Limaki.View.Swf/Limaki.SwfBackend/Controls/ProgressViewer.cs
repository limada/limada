/*
 Limaki 
 
 Author: Lytico
 Copyright (C) 2009-2012 Lytico
 
 http://www.limada.org

 Permission is hereby granted, free of charge, to any person obtaining a copy
 of this software and associated documentation files (the "Software"), to deal
 in the Software without restriction, including without limitation the rights
 to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 copies of the Software, and to permit persons to whom the Software is
 furnished to do so, subject to the following conditions:
 
 The above copyright notice and this permission notice shall be included in
 all copies or substantial portions of the Software.
 
 THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 THE SOFTWARE.
  
 */

using System.Windows.Forms;

namespace Limaki.SwfBackend.Controls {
    public partial class ProgressViewer : UserControl {
        public ProgressViewer () {
            InitializeComponent();
        }

        public void Write (string m, int progress, int count) {
            Write(m, progress, count, null);
        }

        public void Write (string m, int progress, int count, params object[] param) {
            if (param != null)
                this.label1.Text = string.Format(m, param);
            else
                this.label1.Text = m;
            var c = (float) progress;
            this.progressBar1.Value = (int) ((c / count) * 100f);
            Application.DoEvents();
        }


        public void Write (string message) {
            this.label1.Text = message;
            Application.DoEvents();
        }
    }


}
