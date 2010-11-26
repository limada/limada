using Limaki.Common;
using Limaki.Drawing;

namespace Limaki.Winform.Displays {
    public class DisplayContextProcessor<TData> : ContextProcessor<DisplayBase<TData>>
        where TData : class {
        public override void ApplyProperties(IApplicationContext context, DisplayBase<TData> target) {
            
            StyleSheets styleSheets = context.Pool.TryGetCreate<StyleSheets>();
            IStyleSheet styleSheet = null;
            styleSheets.TryGetValue (styleSheets.StyleSheetNames[1], out styleSheet);
            target.DisplayKit.StyleSheet = styleSheet;

        }
    }
}