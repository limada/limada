using System;

namespace Limaki.Common {
    public class EventArgs<T>:EventArgs {
        public EventArgs(T arg) {
            this.Arg = arg;
        }

        private T _arg;
        public T Arg {
            get { return _arg; }
            set { _arg = value; }
        }
    }
}
