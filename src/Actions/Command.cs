/*
 * Limaki 
 * Version 0.064
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

    public class ActCommand<T, P> : Command<T,P> {
        public ActCommand(T target, P parameter, Act<T, P> act) : base(target, parameter) { this.Act = act; }

        private Act<T,P> _act = null;
        public Act<T,P> Act {
            get { return _act; }
            set { _act = value; }
        }

        public override void Execute() {
            if (Act != null) {
                Act (Target, Parameter);
            }
        }
        protected override void Dispose(bool disposing) {
            base.Dispose(disposing);
            Act = null;
        }
    }
}
