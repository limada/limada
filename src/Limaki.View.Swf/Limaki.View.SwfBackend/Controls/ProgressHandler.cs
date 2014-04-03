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

using System;
using System.Windows.Forms;
using System.Drawing;
using Limaki.Common;

namespace Limaki.View.SwfBackend.Controls {

    public class ProgressHandler:IDisposable, IProgressHandler {
        public ProgressHandler(Form owner) {
            this.Owner = owner;
            Show();
        }
        public ProgressHandler():this(null) {}

        ProgressViewer _progressViewer = null;
        ProgressViewer ProgressViewer {
            get {
                if (_progressViewer == null) {
                    _progressViewer = new ProgressViewer();
                }
                return _progressViewer; ;
            }
        }
        Form _splash = null;
        bool isSplashOwner = false;
        Label label = null;
        Form Splash {
            get {
                if (_splash == null) {
                    _splash = new Form();
                    _splash.Owner = Owner;
                    label = new Label{Dock=DockStyle.Top,TextAlign = ContentAlignment.TopCenter};
                    label.Height = 24;

                    _splash.Size = ProgressViewer.Size;
                    _splash.Height += (label.Height+10);

                    if (Owner == null)
                        _splash.StartPosition = FormStartPosition.CenterScreen;
                    else {
                        _splash.StartPosition = FormStartPosition.Manual;
                        _splash.Location = new Point(Owner.Location.X + (Owner.Size.Width/2-_splash.Width/2),
                                                     Owner.Location.Y + Owner.Size.Height / 2 - _splash.Height / 2);
                        
                    }
                    _splash.TopMost = true;
                    _splash.FormBorderStyle = FormBorderStyle.None;
                    _splash.BackColor = SystemColors.Window;
                    //_splash.TransparencyKey = _splash.BackColor;
                    _splash.AllowTransparency = true;
                    _splash.Opacity = 1d;
                    ProgressViewer.Dock = DockStyle.Fill;
                    _splash.Controls.Add(ProgressViewer);
                    _splash.Controls.Add(label);
                   
                    isSplashOwner = true;
                }
                return _splash;
            }

        }
        public bool UseOpacity { get; set; }

        public virtual void Show(string message) {
            Splash.Show();
            Splash.BringToFront();
            label.Text = message;
        }
        protected virtual void Show() {
            Splash.Show();
        }
        public virtual void Close () {
            Dispose();
        }

        public void Write(string m, int progress, int count, params object[] param) {
            if(UseOpacity)
                Splash.Opacity = (double)progress  / count;
            ProgressViewer.Write(m, progress, count, param);
        }
        public void Write(string m, int progress, int count) {
            Write(m, progress, count,null);
        }

        private int progress = 0;
        public void Write(string m, int count) {
            Write(m, ++progress, count, null);
        }

        #region IDisposable Members

        public Action Disposing { get; set; }
        public void Dispose() {
            if (Disposing != null)
                Disposing();
            if(_splash!=null) {
                Splash.Parent = null;
                Splash.Close();
                _splash = null;
            }

            //if (_progressViewer != null) {
            //    _progressViewer.Dispose();
            //    _progressViewer = null;
            //}
        }

        #endregion

        public Form Owner { get; set; }

        
    }
}