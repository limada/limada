using System.Runtime.Serialization;
namespace Limada.Model {
    [DataContract]
    public class StringThing : Thing<string> {
        protected StringThing():base() { }
        public StringThing(string data):base(data){}
    }
}