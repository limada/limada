using Limaki.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Limaki.View.Headless.VidgetBackends {

    public class HeadlessExeptionHandlerBackend : IExceptionHandler {

        public void Catch (Exception e) {
            throw e;
        }

        public void Catch (Exception e, MessageType messageType) {
            throw e;
        }
    }
}
