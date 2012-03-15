using System;
using System.Windows.Forms;

namespace Limaki.SWF.Viewers.ToolStripViewers {
    public class ToolStripCommand {

        public Action<object> Action { get; set; }
        public System.Drawing.Image Image { get; set; }
        public string Text { get; set; }
        public string ToolTipText { get; set; }
        public System.Drawing.Size Size { get; set; }

        public virtual void DoAction(object sender, EventArgs e) {

            if (Action != null)
                Action(sender);

            var commandItem = sender as IToolStripCommandItem;
            if (commandItem != null && commandItem.ToggleOnClick != null)
                ToggleCommand(commandItem, commandItem.ToggleOnClick);
        }

        public virtual void Attach(object target) {
            var control = target as ToolStripItem;
            if (control != null) {
                control.Image = this.Image;
                control.Text = this.Text;
                control.ToolTipText = this.ToolTipText;
                control.Size = this.Size;
                control.Click += this.DoAction;
            }
        }

        public virtual void DeAttach(object target) {
            var control = target as ToolStripItem;
            if (control != null) {
                control.Image = null;
                control.Text = string.Empty;
                control.ToolTipText = string.Empty;
                control.Click -= this.DoAction;
            }
        }

        public virtual void ToggleCommand(IToolStripCommandItem control1, IToolStripCommandItem control2) {
            if (control1 == control2)
                return;

            var command1 = control1.Command;
            control1.Command = control2.Command;
            control2.Command = command1;

        }
    }
}