// 
// WPFEngine.cs
//  
// Author:
//       Carlos Alberto Cortez <calberto.cortez@gmail.com>
//       Luis Reis <luiscubal@gmail.com>
//       Thomas Ziegler <ziegler.thomas@web.de>
// 
// Copyright (c) 2011 Carlos Alberto Cortez
// Copyright (c) 2012 Luís Reis
// Copyright (c) 2012 Thomas Ziegler
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
using System.Windows;

using Xwt.Backends;
using Xwt.Drawing;
using Xwt.Engine;

namespace Xwt.WPFBackend
{
	public class WPFEngine : Xwt.Backends.EngineBackend
	{
		System.Windows.Application application;

        public static WidgetRegistry Registry {
            get;
            set;
        }

		public override void InitializeApplication ()
		{
			application = System.Windows.Application.Current;

			if (application == null)
				application = new System.Windows.Application ();
        }

        public override void InitializeRegistry(WidgetRegistry registry) {

            Registry = registry;
            registry.FromEngine = this;

			registry.RegisterBackend (typeof (Window), typeof (WindowBackend));
			registry.RegisterBackend (typeof (Dialog), typeof (DialogBackend));
			registry.RegisterBackend (typeof (Notebook), typeof (NotebookBackend));
			registry.RegisterBackend (typeof (Menu), typeof (MenuBackend));
			registry.RegisterBackend (typeof (MenuItem), typeof (MenuItemBackend));
			registry.RegisterBackend (typeof (CheckBoxMenuItem), typeof (CheckboxMenuItemBackend));
			registry.RegisterBackend (typeof (RadioButtonMenuItem), typeof (RadioButtonMenuItemBackend));
			registry.RegisterBackend (typeof (SeparatorMenuItem), typeof (SeparatorMenuItemBackend));
			registry.RegisterBackend (typeof (Table), typeof (BoxBackend));
			registry.RegisterBackend (typeof (Box), typeof (BoxBackend));
			registry.RegisterBackend (typeof (Label), typeof (LabelBackend));
			registry.RegisterBackend (typeof (TextEntry), typeof (TextEntryBackend));
			registry.RegisterBackend (typeof (Button), typeof (ButtonBackend));
			registry.RegisterBackend (typeof (ToggleButton), typeof (ToggleButtonBackend));
			registry.RegisterBackend (typeof (MenuButton), typeof (MenuButtonBackend));
			registry.RegisterBackend (typeof (CheckBox), typeof (CheckBoxBackend));
			registry.RegisterBackend (typeof (TreeView), typeof (TreeViewBackend));
			registry.RegisterBackend (typeof (TreeStore), typeof (TreeStoreBackend));
			registry.RegisterBackend (typeof (ImageView), typeof (ImageViewBackend));
			registry.RegisterBackend (typeof (Separator), typeof (SeparatorBackend));
			registry.RegisterBackend (typeof (Image), typeof (ImageHandler));
			registry.RegisterBackend (typeof (Font), typeof (FontBackendHandler));
			registry.RegisterBackend (typeof (Clipboard), typeof (ClipboardBackend));
			registry.RegisterBackend (typeof (ComboBox), typeof (ComboBoxBackend));
			registry.RegisterBackend (typeof (ComboBoxEntry), typeof (ComboBoxEntryBackend));
			registry.RegisterBackend (typeof (ScrollView), typeof (ScrollViewBackend));
			registry.RegisterBackend (typeof (Frame), typeof (FrameBackend));
			registry.RegisterBackend (typeof (Canvas), typeof (CanvasBackend));
			registry.RegisterBackend (typeof (Context), typeof (ContextBackendHandler));
			registry.RegisterBackend (typeof (Gradient), typeof (GradientBackendHandler));
			registry.RegisterBackend (typeof (TextLayout), typeof (TextLayoutBackendHandler));
			registry.RegisterBackend (typeof (Widget), typeof (CustomWidgetBackend));
			registry.RegisterBackend (typeof (Paned), typeof (PanedBackend));
			registry.RegisterBackend (typeof (ScrollAdjustment), typeof (ScrollAdjustmentBackend));
			registry.RegisterBackend (typeof (OpenFileDialog), typeof (OpenFileDialogBackend));
			registry.RegisterBackend (typeof (SelectFolderDialog), typeof (SelectFolderDialogBackend));
			registry.RegisterBackend (typeof (IAlertDialogBackend), typeof (AlertDialogBackend));
			registry.RegisterBackend (typeof (ImageBuilder), typeof (ImageBuilderBackendHandler));
			registry.RegisterBackend (typeof (ImagePattern), typeof (ImagePatternBackendHandler));
			registry.RegisterBackend (typeof (ListView), typeof (ListViewBackend));
			registry.RegisterBackend (typeof (ListStore), typeof (ListDataSource));
			registry.RegisterBackend (typeof (ListBox), typeof (ListBoxBackend));
			registry.RegisterBackend (typeof (ProgressBar), typeof (ProgressBarBackend));
		}

		public override void RunApplication ()
		{
			application.Run ();
		}

		public override void ExitApplication ()
		{
			application.Shutdown();
		}

		public override void InvokeAsync (Action action)
		{
			if (action == null)
				throw new ArgumentNullException ("action");

			application.Dispatcher.BeginInvoke (action, new object [0]);
		}

		public override object TimerInvoke (Func<bool> action, TimeSpan timeSpan)
		{
			if (action == null)
				throw new ArgumentNullException ("action");

			return Timeout.Add (action, timeSpan, application.Dispatcher);
		}

		public override void CancelTimerInvoke (object id)
		{
			if (id == null)
				throw new ArgumentNullException ("id");

			Timeout.CancelTimeout ((uint)id);
		}

		public override IWindowFrameBackend GetBackendForWindow (object nativeWindow)
		{
			return new WindowFrameBackend () {
				Window = (System.Windows.Window) nativeWindow
			};
		}

		public override object GetNativeWidget (Widget w)
		{
            var backend = (IWpfWidgetBackend) Registry.GetBackend (w); 
			return backend.Widget;
		}

		public override object GetNativeParentWindow (Widget w)
		{
			var backend = (IWpfWidgetBackend) Registry.GetBackend (w);

			FrameworkElement e = backend.Widget;
			while ((e = e.Parent as FrameworkElement) != null)
				if (e is System.Windows.Window)
					return e;

			return null;
		}
	}
}

