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

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using Limaki.Iconerias;

namespace Limaki.View.WpfBackend {

    public class ToolStripDropDownButton : ToolStripButton {

        private IList<UIElement> _children = null;
        public IList<UIElement> Children { get { return _children ?? (_children = new List<UIElement> ()); } }

        public void AddItems (params UIElement[] children) {
            foreach (var child in children)
                Children.Add (child);
        }

        #region Composition

        protected override void Compose () {

            Style = ToolStripUtils.ToolbarItemStyle (this);

            ButtonPanel.Children.Add (ButtonImage);
            ButtonPanel.Children.Add (DropDownImage);

            this.Content = ButtonPanel;


        }

        StackPanel _buttonPanel = null;
        protected StackPanel ButtonPanel {
            get { return _buttonPanel ?? (_buttonPanel = new StackPanel { Orientation = Orientation.Horizontal, SnapsToDevicePixels = true }); }
        }

        Popup _childPopup = null;
        protected Popup ChildPopup {
            get {
                if (_childPopup == null) {
                    _childPopup = new Popup {
                        Child = ChildPanel,
                        Placement = System.Windows.Controls.Primitives.PlacementMode.Bottom,
                        PlacementTarget = ButtonPanel,
                        StaysOpen = false,
                        Focusable = true,
                    };

                    Popup.IsOpenProperty.Bind (this, o => o.PopupVisible, _childPopup, BindingMode.TwoWay);

                    _childPopup.LostFocus += (s, e) => {
                        Trace.WriteLine (string.Format ("LostFocus; PopupVisible {0}", this.PopupVisible));
                    };

                    _childPopup.LostMouseCapture += (s, e) => {
                        Trace.WriteLine (string.Format ("LostMouseCapture; PopupVisible {0}", this.PopupVisible));
                    };

                    _childPopup.Opened += (s, e) => {
                        Trace.WriteLine (string.Format ("Opened; PopupVisible {0}", this.PopupVisible));
                    };

                    _childPopup.Closed += (s, e) => {
                        Trace.WriteLine (string.Format ("Closed; PopupVisible {0}", this.PopupVisible));
                    };
                }
                return _childPopup;
            }
        }

        StackPanel _childPanel = null;
        protected StackPanel ChildPanel {
            get {
                return _childPanel
                       ?? (_childPanel = new StackPanel {
                           Orientation = Orientation.Vertical,
                           SnapsToDevicePixels = true,
                           Background = SystemColors.ControlBrush
                       });
            }
        }

        UIElement _dropDownImage = null;
        protected UIElement DropDownImage {
            get {
                if (_dropDownImage == null) {

                    var awesome = Iconery.Create<AwesomeIconeria> ();
                    _dropDownImage = ToolStripUtils.WpfImage (awesome.AsImage (awesome.FaCaretDown, 12));
                }

                return _dropDownImage;
            }
        }

        #endregion

        protected override void OnPreviewMouseDown (MouseButtonEventArgs e) {
            var pos = e.GetPosition (this);
            if (pos.X > this.ButtonImage.RenderSize.Width) {
                e.Handled = true;
                DropDownClick (this, e);
                return;
            }
            base.OnPreviewMouseDown (e);
        }

        protected void DropDownClick (object sender, MouseButtonEventArgs e) {
            Trace.WriteLine (string.Format ("{0} DropDownClick | {1}", sender, this.PopupVisible));

            if (ChildPopup.IsOpen)
                ClosePopup (true);
            else
                OpenPopup ();

            e.Handled = true;
        }

        protected void OpenPopup () {

            foreach (var child in this.Children.ToArray ()
                .Where (c => !ChildPanel.Children.Contains (c))) {
                child.PreviewMouseMove += ShowToolTip;
                child.MouseLeave += HideToolTip;
                child.PreviewMouseUp += (s, e) => {
                    this.PopupVisible = false;
                    child.RaiseEvent (new RoutedEventArgs (ButtonBase.ClickEvent));
                };
                ChildPanel.Children.Add (child);
            }
            //Mouse.AddPreviewMouseDownOutsideCapturedElementHandler (ChildPanel, OnMouseDownOutsideCapturedElement);
            //Mouse.Capture (ChildPanel);
            ChildPopup.IsOpen = true;

        }

        protected void ClosePopup (bool fromButton) {
            if (ChildPopup.IsOpen)
                ChildPopup.IsOpen = false;
            Mouse.Capture (null);
            
            if (fromButton)
                ButtonPanel.Focus ();
        }

        protected void OnMouseDownOutsideCapturedElement (object sender, MouseButtonEventArgs e) {
            ClosePopup (false);
        }

        public bool PopupVisible {
            get { return (bool)GetValue (PopupVisibleProperty); }
            set { SetValue (PopupVisibleProperty, value); }
        }

        public static DependencyProperty PopupVisibleProperty = WpfExtensions.RegisterDependencyProperty<ToolStripDropDownButton, bool> (
            o => o.PopupVisible, new UIPropertyMetadata (false, PopupVisibleChanged));

        protected static void PopupVisibleChanged (DependencyObject d, DependencyPropertyChangedEventArgs e) {
            Trace.WriteLine (string.Format ("{0} -> {1}", e.OldValue, e.NewValue));
        }

        #region ToolTip

        protected int toolTipStartTime = int.MaxValue;

        protected void ShowToolTip (object sender, MouseEventArgs e) {
            var child = sender as FrameworkElement;
            var toolTip = child.ToolTip as ToolTip;
            if (toolTip == null)
                return;
            var now = Environment.TickCount;
            if (toolTipStartTime == int.MaxValue) {
                toolTipStartTime = now;
                return;
            }
            var delay = ToolTipService.GetInitialShowDelay (this);
            var duration = ToolTipService.GetShowDuration (this);
            toolTip.IsOpen = now > toolTipStartTime + delay && now < toolTipStartTime + delay + duration;

        }

        protected void HideToolTip (object sender, MouseEventArgs e) {
            var child = sender as FrameworkElement;
            var toolTip = child.ToolTip as ToolTip;
            if (toolTip != null)
                toolTip.IsOpen = false;
            toolTipStartTime = int.MaxValue;
        }

        #endregion
    
    }
}