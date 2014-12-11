using System.Runtime.Serialization;
namespace Limada.Model {

    public interface IStringThing : IThing<string> {}

    [DataContract]
    public class StringThing : Thing<string>, IStringThing {
        protected StringThing():base() { }
        public StringThing(string data):base(data){}
    }
}