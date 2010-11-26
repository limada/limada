/*
 * Limaki 
 * Version 0.081
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2006-2008 Lytico
 *
 * http://limada.sourceforge.net
 */

using System;
using System.Collections.Generic;
using System.Text;
using Limaki.Common;

namespace Limaki.Actions {
    public class Command:ICommand {
        /// <summary>
        /// Command.Execute makes nothing; override it in derived classes
        /// </summary>
        public virtual void Execute() {}

        public virtual void Dispose() {
            Dispose (true);
        }
        //~Command() {
        //    Dispose (false);
        //}
        protected virtual void Dispose(bool disposing) { }
    }

    public class Command<T> : Command,ICommand<T> {
        private T _target = default(T);
        public T Target {
            get { return _target; }
            set { _target = value; }
        }
        public Command(T target) { this.Target = target;}
        protected override void Dispose(bool disposing) {
            Target = default( T );
        }
    }

    public class Command<T,P> :Command<T>, ICommand<T,P> {
        private P _parameter = default(P);
        public P Parameter {
            get { return _parameter; }
            set { _parameter = value; }
        }

        public Command(T target, P parameter):base(target) {
            this.Parameter = parameter;
        }

        protected override void Dispose(bool disposing) {
            base.Dispose(disposing);
            Parameter = default(P);
        }

        //~Command( ) {
        //}
	}

    public class Command<T, P, P2> : Command<T, P>, ICommand<T, P, P2> {
        private P2 _parameter2 = default(P2);
        public P2 Parameter2 {
            get { return _parameter2; }
            set { _parameter2 = value; }
        }

        public Command(T target, P parameter, P2 parameter2): base(target,parameter) {
            this.Parameter2 = parameter2;
        }
    }

    public class ActionCommand<T, P> : Command<T,P> {
        public ActionCommand(T target, P parameter, Action<T, P> action) : base(target, parameter) { this.Action = action; }

        private Action<T,P> _act = null;
        public Action<T,P> Action {
            get { return _act; }
            set { _act = value; }
        }

        public override void Execute() {
            if (Action != null) {
                Action (Target, Parameter);
            }
        }
        protected override void Dispose(bool disposing) {
            base.Dispose(disposing);
            Action = null;
        }
    }
}
