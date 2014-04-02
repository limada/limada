using System;

namespace Limaki.Iconerias {
    [AttributeUsage(AttributeTargets.Method)]
    public class IconAttribute:Attribute {
        public string Name { get; set; }
        public string Id { get; set; }
    }
}