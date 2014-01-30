/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2014 Lytico
 *
 * http://www.limada.org
 * 
 */

using Xwt.Backends;
using Xwt.Drawing;

namespace Limaki.View.XwtBackend {

    public class XwtSystemColorsBackend : SystemColorsBackend {

        public override Color ScrollBar { get { return Colors.Gray; } }
        public override Color Background { get { return Colors.White; } }
        public override Color ActiveCaption { get { return Colors.Black; } }
        public override Color InactiveCaption { get { return Colors.Black; } }
        public override Color Menu { get { return Colors.Blue; } }
        public override Color Window { get { return Colors.White; } }
        public override Color WindowFrame { get { return Colors.Blue; } }
        public override Color MenuText { get { return Colors.Black; } }
        public override Color WindowText { get { return Colors.Black; } }
        public override Color CaptionText { get { return Colors.Black; } }
        public override Color ActiveBorder { get { return Colors.Blue; } }
        public override Color InactiveBorder { get { return Colors.Blue; } }
        public override Color ApplicationWorkspace { get { return Colors.White; } }
        public override Color Highlight { get { return Colors.Black; } }
        public override Color HighlightText { get { return Colors.Black; } }
        public override Color ButtonFace { get { return Colors.Gray; } }
        public override Color ButtonShadow { get { return Colors.Gray; } }
        public override Color GrayText { get { return Colors.Gray; } }
        public override Color ButtonText { get { return Colors.Black; } }
        public override Color InactiveCaptionText { get { return Colors.Black; } }
        public override Color ButtonHighlight { get { return Colors.Black; } }
        public override Color TooltipText { get { return Colors.Black; } }
        public override Color TooltipBackground { get { return Colors.Yellow; } }
        public override Color MenuHighlight { get { return Colors.Black; } }
        public override Color MenuBar { get { return Colors.Gray; } }

    }

}
