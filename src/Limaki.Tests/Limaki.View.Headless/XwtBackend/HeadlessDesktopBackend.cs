using System.Collections.Generic;
using Xwt.Backends;

namespace Xwt.Headless.Backend {

    public class HeadlessDesktopBackend : DesktopBackend {

        public HeadlessDesktopBackend () {

        }

        static bool cannotCallGetDpiForMonitor;
        public override double GetScaleFactor (object backend) {
            return 1;
        }

        #region implemented abstract members of DesktopBackend

        public override Point GetMouseLocation () {
            return Point.Zero;
        }

        public override IEnumerable<object> GetScreens () {
            return new object[0];
        }

        public override bool IsPrimaryScreen (object backend) {
            return true;
        }

        public override Rectangle GetScreenBounds (object backend) {
            return new Rectangle (0, 0, 800, 600);
        }

        public override Rectangle GetScreenVisibleBounds (object backend) {
            return GetScreenBounds (backend);
        }

        public override string GetScreenDeviceName (object backend) {
            return "HeadlessScreenDUmmy";
        }

        #endregion

    }
}