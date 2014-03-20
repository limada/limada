using Limaki.Drawing;

namespace Limaki.View {
    //TODO: find a better name
    //TODO: refactor Layout to IShaper and AlignOptions and some algos to perform layout (=alligner)
    /// <summary>
    /// source of Shapes and factory of Shapes
    /// create a shape for  TItem 
    /// or calls TItems shape property
    /// </summary>
    /// <typeparam name="TItem"></typeparam>
    public interface IShaper<TItem> {
        /// <summary>
        /// Gives back a propriate shape for this item
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        IShape CreateShape (TItem item);

        IShape GetShape (TItem item);

        /// <summary>
        /// Sets position and others things on target
        /// </summary>
        void Justify (TItem target);

        /// <summary>
        /// Sets position and others things on shape
        /// </summary>
        void Justify (TItem target, IShape shape);
    }
}