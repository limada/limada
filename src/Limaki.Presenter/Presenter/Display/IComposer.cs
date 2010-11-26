namespace Limaki.Presenter.Display {
    public interface IComposer<T> {
        void Factor(T display);
        void Compose ( T display );
    }
}