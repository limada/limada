namespace Limaki.Drawing.WPF.Shapes {
    public interface IWPFShape:IShape {
        System.Windows.Shapes.Shape Shape { get; }
    }
}