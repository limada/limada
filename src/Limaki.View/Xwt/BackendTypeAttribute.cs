using System;

namespace Xwt.Backends {
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface)]
    public class BackendTypeAttribute : Attribute {
        public BackendTypeAttribute (Type type) {
            Type = type;
        }

        public Type Type {
            get;
            private set;
        }
    }
}