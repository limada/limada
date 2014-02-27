using Limaki.Actions;

namespace Limaki.View.UI {

    public interface ICopyPasteAction:IAction {
        void Copy ();
        void Paste ();
        
    }
}