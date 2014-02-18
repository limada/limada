using Limaki.View;
using Limaki.View.Rendering;
using Limaki.View.UI;
using Limaki.UnitTest;

namespace Limaki.Tests.View.Display {

    public enum Frame {
        Full,
        Half,
        Quarter
    }


    public class FrameTicker:Ticker, IRenderAction {
        public int FrameCount = 0;
        bool stopped = true;
        public override void Start () {
            FrameCount = 0;
            stopped = false;
            Enabled = true;
            base.Start();
        }
        public override int Stop () {
            stopped = true;
            Enabled = false;
            return base.Stop();
            
        }
        public override void Resume () {
            base.Resume();
            stopped = false;
        }

        public virtual void Instrument(IDisplay control) {
            control.EventControler.Add (this);
        }
        public virtual void Disinstrument(IDisplay control) {
            control.EventControler.Remove (this);
        }

        public virtual string FramePerSecond () {
            return ( (float)FrameCount / Elapsed  * 1000f ).ToString("#.#0");
        }

        #region IRenderAction Member

        public void OnPaint(IRenderEventArgs e) {
            if (!stopped)
                FrameCount++;
        }


        public bool Resolved { get { return true; } }
        public bool Exclusive { get { return false; } }
        public bool Enabled { get;set;}
        public int Priority { get; set; }
        public void Dispose() {}

        #endregion
    }
}