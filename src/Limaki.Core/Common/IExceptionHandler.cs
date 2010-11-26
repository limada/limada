using System;

namespace Limaki.Common {
    public enum MessageType {
        OK,
        RetryCancel
    }
    public interface IExceptionHandler {
        void Catch ( Exception e );
        void Catch(Exception e, MessageType messageType);
    }

    public class ThrowingExceptionHandler:IExceptionHandler {
        public virtual void Catch(Exception e) {
            throw e;
        }
        public virtual void Catch(Exception e, MessageType messageType) {
            throw e;
        }
    }
}