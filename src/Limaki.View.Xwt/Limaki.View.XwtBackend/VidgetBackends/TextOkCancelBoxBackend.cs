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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Limaki.Common;
using Limaki.Drawing;
using Limaki.Drawing.Styles;
using Limaki.View.Vidgets;
using Xwt;

namespace Limaki.View.XwtBackend {

    public class TextOkCancelBoxBackend : HBox, ITextOkCancelBoxBackend, IVidgetBackend {

        public TextOkCancelBoxBackend() { Compose (); }

        public string Title {
            set {
                TitleLabel.Text = value;
                TitleLabel.WidthRequest = Registry.Pooled<IDrawingUtils> ()
                  .GetTextDimension (value, new Style ("") { Font = TitleLabel.Font }).Width;
            }
        }

        public string Text { get { return TextEntry.Text; } set { TextEntry.Text = value; } }

        public Action<DialogResult> Finish { get; set; }

        #region Composition

        protected Label TitleLabel { get; set; }
        protected TextEntry TextEntry { get; set; }

        protected void Compose () {

            var margin = new WidgetSpacing ();

            this.Margin = margin;

            TextEntry = new TextEntry { ShowFrame = false, Margin = margin };
            TitleLabel = new Label {
                WidthRequest = 80,
                TextColor = SystemColors.GrayText,
                TextAlignment = Alignment.Start,
                Font = TextEntry.Font.WithSize (TextEntry.Font.Size * .80),
                Margin = margin
            };


            var buttonOk = new Button {
                Image = Limaki.View.Properties.Iconery.OK,
                Style = ButtonStyle.Flat,
                Margin = margin,
            };
            var buttonCancel = new Button {
                Image = Limaki.View.Properties.Iconery.Cancel,
                Style = ButtonStyle.Flat,
                Margin = margin,

            };

            TextEntry.KeyPressed += (s, e) => {
                if (e.Key == Key.NumPadEnter || e.Key == Key.Return) {
                    OnFinish (DialogResult.Ok);
                } else if (e.Key == Key.Escape) {
                    OnFinish (DialogResult.Cancel);
                }
            };

            buttonOk.Clicked += (s, e) => OnFinish (DialogResult.Ok);
            buttonCancel.Clicked += (s, e) => OnFinish (DialogResult.Cancel);

            this.PackStart (TitleLabel);
            this.PackEnd (buttonCancel);
            this.PackEnd (buttonOk);
           
            this.PackStart (TextEntry, true);
        }

        protected void OnFinish (DialogResult result) {
            if (Finish != null) {
                Finish (result);
            }
        }

        #endregion

        #region IVidgetBackend-Implementation

        public TextOkCancelBox Frontend { get; set; }

        public virtual void InitializeBackend (IVidget frontend, VidgetApplicationContext context) {
            this.Frontend = (TextOkCancelBox) frontend;
        }

        void IVidgetBackend.Update () { XwtBackendHelper.VidgetBackendUpdate (this); }

        void IVidgetBackend.Invalidate () { XwtBackendHelper.VidgetBackendInvalidate (this); }

        void IVidgetBackend.Invalidate (Rectangle rect) { XwtBackendHelper.VidgetBackendInvalidate (this, rect); }

        #endregion

        
    }

}
