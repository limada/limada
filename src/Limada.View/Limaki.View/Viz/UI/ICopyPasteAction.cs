using Limaki.Actions;

namespace Limaki.View.Viz.UI {

    public interface ICopyPasteAction:IAction {
        void Copy ();
        void Paste ();
        
    }
}