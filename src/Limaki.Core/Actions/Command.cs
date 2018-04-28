/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2006-2011 Lytico
 *
 * http://www.limada.org
 */

using System;

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
        
        public T Subject { get; set; }

        public Command(T subject) { this.Subject = subject;}

        protected override void Dispose(bool disposing) {
            Subject = default( T );
        }
    }

    public class Command<T,P> :Command<T>, ICommand<T,P> {
        
        public P Parameter { get; set; }

        public Command(T subject, P parameter):base(subject) {
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
        
        public P2 Parameter2 { get; set; }

        public Command(T subject, P parameter, P2 parameter2): base(subject,parameter) {
            this.Parameter2 = parameter2;
        }
    }

    public class ActionCommand<T, P> : Command<T,P> {
        
        public ActionCommand(T subject, P parameter, Action<T, P> action) : base(subject, parameter) { this.Action = action; }

        public Action<T, P> Action { get; set; }

        public override void Execute() {
            Action?.Invoke (Subject, Parameter);
        }
        protected override void Dispose(bool disposing) {
            base.Dispose(disposing);
            Action = null;
        }
    }
}
