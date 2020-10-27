//
// PopoverBackend.cs
//
// Author:
//       Alan McGovern <alan@xamarin.com>
//
// Copyright (c) 2012 Xamarin, Inc.
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
using System.Windows.Media;
using System.Windows;
using System.Windows.Input;

namespace Xwt.WPFBackend
{
	public class PopoverBackend : Backend, IPopoverBackend
	{
		public Xwt.Popover.Position ActualPosition {
			get; set;
		}

		System.Windows.Controls.Border Border {
			get; set;
		}

		public Xwt.Drawing.Color BackgroundColor {
			get {
				return Border.Background.ToXwtColor ();
			}
			set {
				Border.Background = new SolidColorBrush (value.ToWpfColor ());
			}
		}

		IPopoverEventSink EventSink {
			get; set;
		}

		/// <summary>
		/// Control, if any, that should get the initial keyboard focus when the popover is shown.
		/// The control should be inside the popover, but it doesn't necessarily have to be an Xwt
		/// managed widget.
		/// </summary>
		public UIElement InitialFocus { get; set; }

		/// <summary>
		/// If set to true, then the arrow keys can't be used to move focus between controls.
		/// Regardless of this setting, tab still works to change focus and the arrow keys still
		/// work inside of controls that use them.
		/// </summary>
		public bool DisableArrowKeyNavigation { get; set; }

		new Popover Frontend {
			get { return (Popover)base.frontend; }
		}

		public System.Windows.Controls.Primitives.Popup NativeWidget {
			get; set;
		}

		/// <summary>
		/// Search up the visual tree, finding the PopupRoot for the popup.
		/// </summary>
		/// <returns>PopupRoot or null if not found for some reason</returns>
		public FrameworkElement GetPopupRoot ()
		{
			FrameworkElement element = Border;

			do {
				element = (FrameworkElement) VisualTreeHelper.GetParent (element);
				if (element == null)
					return null;

				if (element.GetType ().Name == "PopupRoot")
					return element;
			} while (true);
		}

		void SetupBorderMargin ()
		{
			if (Frontend.Padding != 0)
				return;

			Border.Padding = new Thickness (1, 1, 1, 1);
			Border.BorderThickness = new Thickness (1);
			Border.Margin = new Thickness (1);
		}

		public PopoverBackend ()
		{
			Border = new System.Windows.Controls.Border {
					Padding = new Thickness (15, 10, 15, 15),
					BorderThickness = new Thickness (1),
					Margin = new Thickness (10),
					Effect = new System.Windows.Media.Effects.DropShadowEffect () {
					Color = Colors.Black,
					Direction = 270,
					BlurRadius = 15,
					Opacity = .15,
					ShadowDepth = 1,
				}
			};
			Border.SetResourceReference (System.Windows.Controls.Border.BorderBrushProperty,
			                             SystemColors.ActiveBorderBrushKey);
			BackgroundColor = Xwt.Drawing.Color.FromBytes (230, 230, 230, 230);

			NativeWidget = new System.Windows.Controls.Primitives.Popup {
				AllowsTransparency = true,
				Child = Border,
				Placement = System.Windows.Controls.Primitives.PlacementMode.Custom,
				StaysOpen = false,
			};
			NativeWidget.Opened += NativeWidget_Opened;
			NativeWidget.Closed += NativeWidget_Closed;
			NativeWidget.PreviewKeyDown += NativeWidget_PreviewKeyDown;
		}

		public void Initialize (IPopoverEventSink sink)
		{
			EventSink = sink;
		}

		public void Show (Xwt.Popover.Position orientation, Xwt.Widget reference, Xwt.Rectangle positionRect, Widget child)
		{
			ActualPosition = orientation;
			SetupBorderMargin ();
			Border.Child = (System.Windows.FrameworkElement)Context.Toolkit.GetNativeWidget (child);
			NativeWidget.CustomPopupPlacementCallback = (popupSize, targetSize, offset) => {
				System.Windows.Point location;
				if (ActualPosition == Popover.Position.Top)
					location = new System.Windows.Point (positionRect.Left - popupSize.Width / 2,
					                                     positionRect.Height > 0 ? positionRect.Bottom : targetSize.Height);
				else
					location = new System.Windows.Point (positionRect.Left - popupSize.Width / 2,
					                                     positionRect.Top - popupSize.Height);

				return new[] {
					new System.Windows.Controls.Primitives.CustomPopupPlacement (location, System.Windows.Controls.Primitives.PopupPrimaryAxis.Horizontal)
				};
			};
			NativeWidget.PlacementTarget = (System.Windows.FrameworkElement)Context.Toolkit.GetNativeWidget (reference);
			NativeWidget.IsOpen = true;

			// Popups are special in that the automation properties need to be set on the PopupRoot, which only exists when the popup is shown
			// See https://social.msdn.microsoft.com/Forums/vstudio/en-US/d4ba12c8-7a87-478e-b064-5620f929a0cf/how-to-set-automationid-and-name-for-popup?forum=wpf
			var accessibleBackend = (AccessibleBackend)Toolkit.GetBackend (Frontend.Accessible);
			if (accessibleBackend != null) {
				FrameworkElement popupRoot = GetPopupRoot ();
				if (popupRoot != null)
					accessibleBackend.InitAutomationProperties (popupRoot);
			}
		}

		void NativeWidget_Opened (object sender, EventArgs e)
		{
			if (DisableArrowKeyNavigation) {
				FrameworkElement popupRoot = GetPopupRoot ();
				if (popupRoot != null)
					KeyboardNavigation.SetDirectionalNavigation (popupRoot, KeyboardNavigationMode.Once);
			}

			if (InitialFocus != null)
				InitialFocus.Focus ();
		}

		void NativeWidget_Closed (object sender, EventArgs e)
		{
			Border.Child = null;
			EventSink.OnClosed ();
		}

		void NativeWidget_PreviewKeyDown (object sender, System.Windows.Input.KeyEventArgs e)
		{
			// Close the popup when Escape is hit
			if (e.Key == System.Windows.Input.Key.Escape) {
				NativeWidget.IsOpen = false;
				e.Handled = true;
			}
		}

		public void Hide ()
		{
			NativeWidget.IsOpen = false;
			Border.Child = null;
		}

		public void Dispose ()
		{
			if (NativeWidget != null) {
				NativeWidget.Opened -= NativeWidget_Opened;
				NativeWidget.Closed -= NativeWidget_Closed;
			}
		}
	}
}