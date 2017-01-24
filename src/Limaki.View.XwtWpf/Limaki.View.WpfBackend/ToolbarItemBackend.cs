using System;
using System.Windows;

namespace Limaki.View.WpfBackend {

    public abstract class ToolbarItemBackend<T> : VidgetBackend<T> where T:FrameworkElement {

        public void SetToolTip (string value) {
            Control.ToolTip = value;
        }

        protected System.Action<object> _action;
        public void SetAction (Action<object> value) {
            _action = value;
        }

        public bool IsEnabled {
            get { return Control.IsEnabled; }
            set { Control.IsEnabled = value; }
        }


    }
}