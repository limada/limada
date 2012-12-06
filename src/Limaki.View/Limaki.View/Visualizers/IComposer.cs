namespace Limaki.View.Visualizers {
    public interface IComposer<T> {
        void Factor(T display);
        void Compose ( T display );
    }
}