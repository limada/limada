
namespace Limaki.Common {

    public interface IComposer<T> {
        void Factor(T subject);
        void Compose (T subject);
    }
}