using System;

namespace Limaki.Common {
    public interface IProgressHandler {
        void Write (string m, int progress, int count, params object[] param);
        void Show (string m);
        void Close ();
    }
}