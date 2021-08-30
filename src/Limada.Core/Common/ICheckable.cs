using System;
namespace Limaki.Common {
    /// <summary>
    /// 
    /// </summary>
    public interface ICheckable {
        bool Check();
    }

    public class CheckFailedException : ArgumentException {
        public CheckFailedException(string message):base(message){}

        public CheckFailedException(Type source, Type needed) :
            base(source.Name + " needs a " + needed.Name) { }
    }
}