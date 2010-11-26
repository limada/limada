namespace Limaki.Widgets {
    public interface IWidgetFactory {
        IWidget CreateWidget ( object data );
        IEdgeWidget CreateEdgeWidget ( object data );
        IEdgeWidget CreateEdgeWidget ( object data, IWidget root, IWidget leaf );
        IWidget CreateWidget<T> ( T data );
        IEdgeWidget CreateEdgeWidget<T> ( T data );
    }
}