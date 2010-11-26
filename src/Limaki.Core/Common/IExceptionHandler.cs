using System;

namespace Limaki.Common {
    public interface IExceptionHandler {
        void Catch ( Exception e );
    }

    public class ThrowingExceptionHandler:IExceptionHandler {
        public virtual void Catch(Exception e) {
            throw e;
        }
    }
}