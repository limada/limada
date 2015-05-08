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

using Limaki.Common.Linqish;
using System;
using System.Linq.Expressions;
using System.Security.Permissions;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Threading;
using Xwt.Backends;

namespace Limaki.View.WpfBackend {

    public static class WpfExtensions {

        public static void DoEvents() {
            Application.Current.Dispatcher.Invoke (DispatcherPriority.Background,
                new Action (delegate { }));
        }

        [SecurityPermissionAttribute (SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
        public static void DoEvents2 () {
            var frame = new DispatcherFrame ();
            Dispatcher.CurrentDispatcher.BeginInvoke (DispatcherPriority.Background,
                new DispatcherOperationCallback (f => { ((DispatcherFrame)f).Continue = false; return null; }), frame);
            Dispatcher.PushFrame (frame);
        }


        public static DependencyProperty RegisterDependencyProperty<T,M> (Expression<Func<T,M>> member, PropertyMetadata typeMetadata) {
            var name = ExpressionUtils.MemberName (member);
            return DependencyProperty
                .Register (name, typeof(M), typeof(T), typeMetadata);
        }

        public static Binding Binding<T, M> (T source, Expression<Func<T, M>> member, BindingMode mode) {
            var name = ExpressionUtils.MemberName (member);
            return new Binding (name) { Source = source, Mode = mode, BindsDirectlyToSource = true };
        }

        public static BindingExpressionBase Bind<T, M> (this DependencyProperty dp, T source, Expression<Func<T, M>> member, DependencyObject target, BindingMode mode = BindingMode.Default) {
            return BindingOperations.SetBinding (target, dp, Binding (source, member, mode));
        }

        public static bool IsKeyModifyingPopupState (KeyEventArgs e) {
            return ((Keyboard.Modifiers.HasFlag(ModifierKeys.Alt) && ((e.SystemKey == Key.Down) || (e.SystemKey == Key.Up)))
                  || (e.Key == Key.Escape));
        }

 
    }
}