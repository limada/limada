using Xwt.Backends;

namespace Xwt.Swf.Xwt.SwfBackend {
    public abstract class Backend<F>: IBackend {
        protected F Frontend { get; set; }

        public virtual void InitializeBackend (object frontend, ApplicationContext context) {
            Frontend = (F)frontend;
            Context = context;
        }

        public virtual void EnableEvent (object eventId) {
        }

        public virtual void DisableEvent (object eventId) {
        }


        public ApplicationContext Context { get; private set; }
    }
}