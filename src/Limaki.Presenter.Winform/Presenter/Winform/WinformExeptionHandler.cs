using System;
using System.Windows.Forms;
using Limaki.Common;

namespace Limaki.Presenter.Winform {
    public class WinformExeptionHandler:IExceptionHandler {

        public virtual void Catch(Exception e, MessageType messageType) {
            MessageBoxButtons buttons = MessageBoxButtons.RetryCancel;
            if (messageType == MessageType.OK)
                buttons = MessageBoxButtons.OK;
            if (MessageBox.Show(e.Message, "Error", buttons) == DialogResult.Cancel)
                throw e;
        }
        public virtual void Catch(Exception e) {
            if (MessageBox.Show(e.Message, "Error", MessageBoxButtons.RetryCancel) == DialogResult.Cancel)
                throw e;
        }

    }
}