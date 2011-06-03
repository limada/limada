using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Limaki.Common {
    public class Localizer {
        public string this[string item] {
            get { return item; }
        }
        public static string Get(string item) {
            return item;
        }
    }

}
