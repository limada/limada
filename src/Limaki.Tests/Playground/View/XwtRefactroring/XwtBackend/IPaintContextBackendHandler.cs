using Xwt.Backends;
using Xwt.Drawing;

namespace Limaki.Painting {
    public interface IPaintContextBackendHandler : IContextBackendHandler {

        void InitBackend (object backend);

        void DrawTextLayout (object backend, TextLayout layout, double x, double y, double height);
        void TextLayout (object backend, TextLayout layout, double x, double y, double height);

        void TranslatePath (object backend, double x, double y);

        
    }
}