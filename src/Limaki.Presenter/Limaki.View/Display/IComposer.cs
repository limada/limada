namespace Limaki.View.Display {
    public interface IComposer<T> {
        void Factor(T display);
        void Compose ( T display );
    }
}