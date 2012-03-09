using System;
using Limaki.Actions;
using Limaki.Drawing;
using Xwt;

namespace Limaki.View.Layout {

    public interface IDirtyCommand { }

    public class MoveCommand<TItem> : Command<TItem, Func<TItem, IShape>, Point>,IDirtyCommand {
        public MoveCommand(TItem target, Func<TItem, IShape> shape, Point param) : base(target, shape, param) { }
        public override void Execute() {
            var shape = this.Parameter (this.Subject);
            shape.Location = this.Parameter2;
        }
    }

    public class MoveByCommand<TItem> : Command<TItem, Func<TItem, IShape>, Size>, IDirtyCommand {
        public MoveByCommand (TItem target, Func<TItem, IShape> shape, Size param) : base (target, shape, param) { }
        public override void Execute () {
            var shape = this.Parameter (Subject);
            shape.Location -= this.Parameter2;
        }
    }

    public class ResizeCommand<TItem> : Command<TItem, Func<TItem, IShape>, Rectangle>, IDirtyCommand {
        public ResizeCommand (TItem target, Func<TItem, IShape> shape, Rectangle param) : base (target, shape, param) { }

        public override void Execute () {
            var shape = this.Parameter (Subject);
            shape.Location = this.Parameter2.Location;
            shape.Size = this.Parameter2.Size;
        }

    }
}