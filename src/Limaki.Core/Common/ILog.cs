/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2017 Lytico
 *
 * http://www.limada.org
 * 
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Limaki.Common {

    public interface ILog {
        void Error (string msg, Exception ex);
        void Error (string msg);
        void Debug (string msg);
        void Info (string msg);
        void Raw (string msg);
    }

    public class DelegateLog : ILog {

        public delegate void MsgHandler (string msg);
        public delegate void ExceptionMsgHandler (string msg, Exception ex);

        public event MsgHandler OnDebug;
        public event MsgHandler OnError;
        public event ExceptionMsgHandler OnExceptionError;
        public event MsgHandler OnInfo;
        public event MsgHandler OnRaw;

        public bool LogDebug { get; set; }

        public virtual void Debug (string msg) { if (LogDebug) OnDebug.Invoke (msg); }

        public virtual void Error (string msg, Exception ex) => OnExceptionError?.Invoke (msg, ex);

        public virtual void Error (string msg) => OnError?.Invoke (msg);

        public virtual void Info (string msg) => OnInfo?.Invoke (msg);

        public virtual void Raw (string msg) => OnRaw?.Invoke (msg);

        public virtual void Register (ILog log) {
            OnInfo += msg => log.Info (msg);
            OnDebug += log.Debug;
            OnError += log.Error;
            OnExceptionError += log.Error;
            OnRaw += log.Raw;
        }

        public virtual void UnRegister (ILog log) {
            OnInfo -= msg => log.Info (msg);
            OnDebug -= log.Debug;
            OnError -= log.Error;
            OnExceptionError -= log.Error;
            OnRaw -= log.Raw;
        }
    }

    public class TypedLog {

        public Type Type { get; set; }

        protected string ErrorMsg (string msg, Exception ex) => $"Error<{Type.Name}> {msg} Exception {ex.Message} | {DateTime.Now}";
        protected string ErrorMsg (string msg) => $"Error<{Type.Name}> {msg} | {DateTime.Now}";
        protected string DebugMsg (string msg) => $"Debug<{Type.Name}> {msg} | {DateTime.Now}";
        protected string InfoMsg (string msg) => $"Info<{Type.Name}> {msg} | {DateTime.Now}";

    }

    public class TraceLog : TypedLog, ILog {

        public void Error (string msg, Exception ex) {
            Trace.WriteLine (ErrorMsg (msg, ex));
        }
        public void Error (string msg) {
            Trace.WriteLine (ErrorMsg (msg));
        }

        public void Debug (string msg) {
#if DEBUG
            Trace.WriteLine (DebugMsg (msg));
#endif
        }

        public void Info (string msg) {
            Trace.WriteLine (InfoMsg (msg));
        }

        public void Raw (string msg) {
            Trace.WriteLine (msg);
        }
    }

    public class ConsoleLog : TypedLog, ILog {

        public void Error (string msg, Exception ex) {
            Console.WriteLine (ErrorMsg (msg, ex));
        }
        public void Error (string msg) {
            Console.WriteLine (ErrorMsg (msg));
        }

        public void Debug (string msg) {
#if DEBUG
            Console.WriteLine (DebugMsg (msg));
#endif
        }

        public void Info (string msg) {
            Console.WriteLine (InfoMsg (msg));
        }

        public void Raw (string msg) {
            Console.WriteLine (msg);
        }
    }

    public class Logger {

        protected DelegateLog _listener = new DelegateLog ();
        IDictionary<Type, ILog> _logs = new Dictionary<Type, ILog> ();

        public DelegateLog Listener => _listener;

        public bool Trace { get; set; }
        #if TRACE
        = true;
        #endif

        public bool Console { get; set; }
#if !TRACE
        = true;
#endif

        public bool Debug { get; set; }
#if DEBUG
        = true;
#endif
        public ILog Log (Type type) {

            if (!_logs.TryGetValue (type, out ILog result)) {
                var llog = new DelegateLog ();
                llog.LogDebug = Debug;

                if (Trace) {
                    var log = new TraceLog { Type = type };
                    llog.Register (log);
                }
                if (Console) {
                    var log = new ConsoleLog { Type = type };
                    llog.Register (log);
                }

                lock (_logs) {
                    _logs[type] = llog;
                }
                llog.Register (_listener);
                result = llog;
            }
            return result;
        }
    }
}
