namespace Limaki.View.Visualizers {
    public interface IComposer<T> {
        void Factor(T composit);
        void Compose (T composit);
    }
}