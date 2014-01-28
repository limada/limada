/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2006-2014 Lytico
 *
 * http://www.limada.org
 * 
 */

using System;
using System.ComponentModel;
using System.IO;

namespace Limaki.Viewers {
    public class FileDialogMemento {
        [DefaultValue(true)]
        public bool OverwritePrompt { get; set; }

        [DefaultValue("")]
        public string DefaultExt { get; set; }

        [DefaultValue("")]
        public string FileName { get; set; }

        public string[] FileNames { get; set; }

        [DefaultValue("")]
        [Localizable(true)]
        public string Filter { get; set; }

        [DefaultValue(1)]
        public int FilterIndex { get; set; }

        [DefaultValue("")]
        public string InitialDirectory { get; set; }


        [DefaultValue("")]
        [Localizable(true)]
        public string Title { get; set; }


        public void SetFileName(string fileName) {
            this.FileName = Path.GetFileNameWithoutExtension(fileName);
            var path = Path.GetDirectoryName(fileName);
            try {
                var uri = new Uri(path);
                if (uri.IsUnc || uri.IsFile)
                    this.InitialDirectory = path;
            } catch {}
        }

        public void ResetFileName () {
            SetFileName(this.FileName);
        }

       
    }
}