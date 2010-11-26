using Limaki.Actions;
using Limaki.Drawing;
using Limaki.Drawing.UI;

namespace Limaki.Widgets {
    public abstract class WidgetLayerBase : Layer<Scene> {
        //public WidgetLayerBase(IZoomTarget zoomTarget, IScrollTarget scrollTarget)
        //    : base(zoomTarget, scrollTarget) {
        //    Priority = ActionPriorities.LayerPriority;
        //}

        public WidgetLayerBase(ICamera camera) : base(camera) { }

        public override Scene Data {
            get { return _data; }
            set {
                bool refresh = value != _data;
                if (refresh) {
                    DisposeData();
                    if (value != null) {
                        isDataOwner = _data != value;
                        _data = value;
                    }

                    DataChanged();
                }
            }
        }

        public override SizeI Size {
            get {
                if (Data != null) {
                    var result= Data.Shape.Size;
                    var distance = this.Layout.Distance;
                    result.Width += distance.Width;
                    result.Height += distance.Height;
                    var offset = this.Offset;
                    if (offset.X < 0) {
                        result.Width += distance.Width;
                    }
                    if (offset.Y < 0) {
                        result.Height += distance.Height;
                    }
                    return result;
                } else {
                    return SizeI.Empty;
                }
            }
            set {
                base.Size = value;
            }
        }

        public override PointI Offset {
            get {
                if (Data != null) {
                    var result = Data.Shape.Location;
                    if (result.X < 0) {
                        result.X -= this.Layout.Distance.Width;
                    }
                    if (result.Y < 0) {
                        result.Y -= this.Layout.Distance.Height;
                    }
                    return result;
                } else {
                    return PointI.Empty;
                }
            }
            set {
                base.Offset = value;
            }
        }

        public override void DataChanged() { }

        private ILayout<Scene, IWidget> _layout = null;
        public virtual ILayout<Scene, IWidget> Layout {
            get { return _layout; }
            set { _layout = value; }
        }

    }
}