﻿using Limaki.Common;
using Limaki.Drawing;

namespace Limaki.Presenter.UI {
    /// <summary>
    /// Scrolls with cursor keys
    /// </summary>
    public class KeyScrollAction : KeyActionBase, ICheckable {
        public virtual Get<IViewport> Viewport { get; set; }
        public virtual Get<IDeviceCursor> CursorGetter { get; set; }

        public override void OnKeyDown(KeyActionEventArgs e) {
            ProcessKey(e);
        }

        protected virtual RectangleI ProcessKey (KeyActionEventArgs e) {
            base.OnKeyDown(e);
            var viewport = Viewport();
            var x = 0;
            var y = 0;
            if (e.Key == Key.Down && e.ModifierKeys == ModifierKeys.None) {
                y = 1;
            }
            if (e.Key == Key.Up && e.ModifierKeys == ModifierKeys.None) {
                y = -1;
            }
            if (e.Key == Key.Left && e.ModifierKeys == ModifierKeys.None) {
                x = -1;
            }
            if (e.Key == Key.Right && e.ModifierKeys == ModifierKeys.None) {
                x = 1;
            }
            if (x!=0 || y!=0) {
                var pos = viewport.ClipOrigin;
                var size = viewport.ClipSize;
                var bounds = new RectangleI(viewport.DataOrigin, viewport.DataSize);

                var result = RectangleI.FromLTRB(
                    x == -1 && pos.X == bounds.X ? -1 : 0,
                    y == -1 && pos.Y == bounds.Y ? -1 : 0,
                    x == 1 && pos.X + size.Width >= bounds.Width ? 1 : 0,
                    y == 1 && pos.Y + size.Height >= bounds.Height ? 1 : 0
                    );
                x = pos.X + x * (size.Width / 10);
                y = pos.Y + y * (size.Height / 10);

                if (x < bounds.Left)
                    x = bounds.Left;
                if (x > bounds.Right)
                    x = bounds.Right - size.Width;
                if (y < bounds.Top)
                    y = bounds.Top;
                if (y > bounds.Bottom)
                    y = bounds.Bottom - size.Height;

                pos = new PointI(x, y);
                viewport.ClipOrigin = pos;
                viewport.Update();
                return result;
            } else {
                return new RectangleI();
            }
        }

        #region ICheckable Member

        public virtual bool Check() {
            if (this.Viewport == null) {
                throw new CheckFailedException(this.GetType(), typeof(IViewport));
            }
            return true;
        }

        #endregion

    }
}