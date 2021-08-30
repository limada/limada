using System.Collections;
using System.Collections.Generic;

namespace Limaki.Common {
    
    public class KeyMaker {
        
        protected static IEqualityComparer comparer = EqualityComparer<object>.Default;

        public static int GetHashCode<T1, T2>(T1 item1, T2 item2) {
            int h = comparer.GetHashCode(item1);
            h = (h << 5) - h + comparer.GetHashCode(item2);
            return h;
        }

        public static int AddHashCode<T>(T item1, int h) {
            return h = (h << 5) - h + comparer.GetHashCode(item1);
        }

        public static int AddHashCode (int h, int h1) {
            return h = (h << 5) - h + h1;
        }

        public static int GetHashCode(params object[] args) {
            if (args == null || args.Length == 0)
                return 0;
            int h = comparer.GetHashCode(args[0]);
            for (int i = 1; i < args.Length; i++)
                h = (h << 5) - h + comparer.GetHashCode(args[i]);
            return h;
        }

        public static int GetHashCode<T1, T2, T3>(T1 item1, T2 item2, T3 item3) {
            int h = comparer.GetHashCode(item1);
            h = (h << 5) - h + comparer.GetHashCode(item2);
            h = (h << 5) - h + comparer.GetHashCode(item3);
            return h;
        }

        public static int GetHashCode<T1, T2, T3, T4>(T1 item1, T2 item2, T3 item3, T4 item4) {
            int h = comparer.GetHashCode(item1);
            h = (h << 5) - h + comparer.GetHashCode(item2);
            h = (h << 5) - h + comparer.GetHashCode(item3);
            h = (h << 5) - h + comparer.GetHashCode(item4);
            return h;
        }

        public static int GetHashCode<T1, T2, T3, T4, T5>(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5) {
            int h = comparer.GetHashCode(item1);
            h = (h << 5) - h + comparer.GetHashCode(item2);
            h = (h << 5) - h + comparer.GetHashCode(item3);
            h = (h << 5) - h + comparer.GetHashCode(item4);
            h = (h << 5) - h + comparer.GetHashCode(item5);
            return h;
        }

        public static int GetHashCode<T1, T2, T3, T4, T5, T6>(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6) {
            int h = comparer.GetHashCode(item1);
            h = (h << 5) - h + comparer.GetHashCode(item2);
            h = (h << 5) - h + comparer.GetHashCode(item3);
            h = (h << 5) - h + comparer.GetHashCode(item4);
            h = (h << 5) - h + comparer.GetHashCode(item5);
            h = (h << 5) - h + comparer.GetHashCode(item6);
            return h;
        }

        public static int GetHashCode<T1, T2, T3, T4, T5, T6, T7>(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7) {
            int h = comparer.GetHashCode(item1);
            h = (h << 5) - h + comparer.GetHashCode(item2);
            h = (h << 5) - h + comparer.GetHashCode(item3);
            h = (h << 5) - h + comparer.GetHashCode(item4);
            h = (h << 5) - h + comparer.GetHashCode(item5);
            h = (h << 5) - h + comparer.GetHashCode(item6);
            h = (h << 5) - h + comparer.GetHashCode(item7);
            return h;
        }
    }
}