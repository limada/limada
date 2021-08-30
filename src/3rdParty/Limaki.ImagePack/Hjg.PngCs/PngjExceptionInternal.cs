using System;

namespace Hjg.Pngcs {
    /// <summary>
    /// Exception for internal problems
    /// </summary>
    [Serializable]
    public class PngjExceptionInternal : Exception {
        private const long serialVersionUID = 1L;

        public PngjExceptionInternal() {
        }

        public PngjExceptionInternal(String message, Exception cause)
            : base(message, cause) {
        }

        public PngjExceptionInternal(String message)
            : base(message) {
        }

        public PngjExceptionInternal(Exception cause)
            : base(cause.Message, cause) {
        }
    }
}
