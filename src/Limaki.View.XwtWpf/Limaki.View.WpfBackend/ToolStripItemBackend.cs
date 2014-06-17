using System;
using System.Windows;
using Limaki.View.Vidgets;

namespace Limaki.View.WpfBackend {

    public abstract class ToolStripItemBackend<T> : VidgetBackend<T> where T:FrameworkElement {

        public void SetToolTip (string value) {
            Control.ToolTip = value;
        }

        protected System.Action<object> _action;
        public void SetAction (Action<object> value) {
            _action = value;
        }




    }
}