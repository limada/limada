/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2014 Lytico
 *
 * http://www.limada.org
 * 
 */

using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Limaki.Iconerias;
using Limaki.View.Properties;

namespace Limaki.View.WpfBackends {

    public class ToolStripDropDownButton: ToolStripButton {
        
        protected override void Compose () {

            // has no effect:
            Mouse.AddPreviewMouseDownOutsideCapturedElementHandler (this, (s, e) => ClosePopup (false));

            Style = (System.Windows.Style)FindResource (ToolBar.ButtonStyleKey);

            ButtonPanel.Children.Add (WpfImage);
            ButtonPanel.Children.Add (DropDownImage);

            this.Content = ButtonPanel;

        }

        StackPanel _buttonPanel = null;
        protected StackPanel ButtonPanel {
            get { return _buttonPanel ?? (_buttonPanel = new StackPanel { Orientation = Orientation.Horizontal, SnapsToDevicePixels = true }); }
        }

        System.Windows.Controls.Primitives.Popup _childPopup = null;

        protected System.Windows.Controls.Primitives.Popup ChildPopup {
            get {
                if (_childPopup == null) {
                    _childPopup = new System.Windows.Controls.Primitives.Popup {
                                                     Child = ChildPanel,
                                                     Placement = System.Windows.Controls.Primitives.PlacementMode.Bottom,
                                                     PlacementTarget = ButtonPanel,
                                                     StaysOpen = false,
                                                     Focusable = true,
                                                 };
                    //_childPopup.LostFocus += (s, e) => ClosePopup (false);
                }
                return _childPopup;
            }
        }

        StackPanel _childPanel = null;
        protected StackPanel ChildPanel {
            get {
                return _childPanel ?? (_childPanel =
                                       new StackPanel {
                                                          Orientation = Orientation.Vertical,
                                                          SnapsToDevicePixels = true,
                                                          Background = this.Background,
                                                      });
            }
        }

        UIElement _dropDownImage = null;
        protected UIElement DropDownImage {
            get {
                if (_dropDownImage == null) {
                    var awesome = Iconery.Create<AwesomeIconeria>();
                    _dropDownImage = ToolStripUtils.WpfImage (awesome.AsImage (awesome.IconChevronDown, 12));
                    _dropDownImage.MouseLeftButtonDown += DropDownClick;

                }
                
                return _dropDownImage;
            }
        }

        protected void DropDownClick (object sender, System.Windows.Input.MouseButtonEventArgs e) {
            Trace.WriteLine ("DropDownImage clicked");

            if (ChildPopup.IsOpen)
                ClosePopup (true);
            else
                ViewChilds ();
            e.Handled = true;
        }

        protected void ClosePopup (bool fromButton) {
            if (ChildPopup.IsOpen)
                ChildPopup.IsOpen = false;
            ReleaseMouseCapture ();
            ChildPopup.ReleaseMouseCapture();

            if (fromButton)
                ButtonPanel.Focus ();
        }

        private ICollection<UIElement> _childs = null;
        protected ICollection<UIElement> Children { get { return _childs ?? (_childs = new List<UIElement>()); } }

        public ToolStripDropDownButton AddChilds (params System.Windows.UIElement[] childs) {
            foreach (var child in childs)
                Children.Add (child);
            return this;
        }

        protected void ViewChilds() {
            ChildPanel.Children.Clear();
            foreach (var elem in this.Children)
                ChildPanel.Children.Add (elem);

            ChildPopup.IsOpen = true;
            ChildPanel.Focus ();
           

        }
    }
}