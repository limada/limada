using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace Limaki.Common {
    public class Reflector {
        public static bool IsStorable (Type type){
            return !(type.IsPrimitive || type == typeof(string));
        }

        public static bool Implements(Type clazz, Type interfaze) {
            if (clazz.IsClass) {
                bool result = (interfaze.IsAssignableFrom(clazz));
                if (! result && interfaze.IsInterface) {
                    foreach (Type t in clazz.GetInterfaces()) {
                        if (t == interfaze) {
                            result = true;
                            break;
                        }
                    }
                }
                return result;
            } else {
                return false;
            }
        }
    }
}
