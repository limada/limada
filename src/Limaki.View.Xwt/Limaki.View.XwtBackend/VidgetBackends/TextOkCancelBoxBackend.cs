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
using Limaki.Iconerias;
using Limaki.View.Vidgets;
using Xwt;

namespace Limaki.View.XwtBackend {

    public class TextOkCancelBoxBackend : VidgetBackend<HBox>, ITextOkCancelBoxBackend, IVidgetBackend {

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

        protected override void Compose () {
            
            base.Compose ();
            
            var margin = new WidgetSpacing ();

            Widget.Margin = margin;

            TextEntry = new TextEntry { ShowFrame = true, Margin = margin };
            TitleLabel = new Label {
                WidthRequest = 80,
                TextColor = SystemColors.GrayText,
                TextAlignment = Alignment.Start,
                Font = TextEntry.Font.WithSize (TextEntry.Font.Size * .80),
                Margin = margin
            };


            var buttonOk = new Button {
                Image = Iconery.OK,
                Style = ButtonStyle.Flat,
                Margin = margin,
            };
            var buttonCancel = new Button {
                Image = Iconery.Cancel,
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

            Widget.PackStart (TitleLabel);
            Widget.PackEnd (buttonCancel);
            Widget.PackEnd (buttonOk);

            Widget.PackStart (TextEntry, true);
        }

        protected void OnFinish (DialogResult result) {
            if (Finish != null) {
                Finish (result);
            }
        }

        #endregion

        #region IVidgetBackend-Implementation

        public new TextOkCancelBox Frontend { get; set; }

        public override void InitializeBackend (IVidget frontend, VidgetApplicationContext context) {
            base.InitializeBackend (frontend, context);
            this.Frontend = (TextOkCancelBox) frontend;
        }

        public override void SetFocus () {
            TextEntry.SetFocus ();
        }

        #endregion

        
    }

}
