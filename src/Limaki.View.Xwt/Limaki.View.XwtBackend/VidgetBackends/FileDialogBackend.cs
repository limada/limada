﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Limaki.Viewers;
using Limaki.Viewers.Vidgets;
using Xwt;

namespace Limaki.View.XwtBackend {

    public abstract class FileDialogBackend : IFileDialogBackend {

        protected FileDialogMemento _dialog = null;

        public virtual void Compose (FileDialogMemento dialog) { this._dialog = dialog; }

        protected virtual bool Run<T> (IVidget parent) where T : FileDialog, new() {
            if (_dialog == null)
                throw new ArgumentException();
            var fileDialog = new T();
            FileDialogSetValue(_dialog, fileDialog);
            WindowFrame owner = null;
            if (parent != null) {
               
            }
            var result = fileDialog.Run(owner);
            fileDialog.Dispose();
            if (result) {
                FileDialogSetValue(fileDialog, _dialog);
            }
            return result;
        }

        private void FileDialogSetValue (FileDialogMemento source, FileDialog target) {

            target.Title = source.Title;
            target.InitialFileName = IoUtils.NiceFileName(source.FileName);
            target.CurrentFolder = source.InitialDirectory;
            var filters = source.Filter.Split('|');
            for (int i = 0; i < filters.Length; i += 2) {
                var exts = filters[i + 1].Split(';');
                target.Filters.Add(new FileDialogFilter(filters[i], exts));
            }

            target.ActiveFilter = target.Filters[source.FilterIndex];
            var saveDialog = target as SaveFileDialog;

        }

        private void FileDialogSetValue (FileDialog source, FileDialogMemento target) {
            target.Title = source.Title;
            target.FileName = source.FileName;
            target.FileNames = source.FileNames;
            target.InitialDirectory = source.CurrentFolder;
            target.Filter = GetFilters(source.Filters);
            target.FilterIndex = source.Filters.IndexOf(source.ActiveFilter);
        }

        private string GetFilters (IEnumerable<FileDialogFilter> filters) {
            var builder = new StringBuilder();
            foreach (var filter in filters) {
                if (builder.Length > 0)
                    builder.Append("|");

                builder.Append(filter.Name);
                builder.Append("|");

                int i = 0;
                foreach (var pattern in filter.Patterns) {
                    if (i++ > 0)
                        builder.Append(";");

                    builder.Append(pattern);
                }
            }

            return builder.ToString();
        }
        public abstract bool Run (IVidget parent);

        public virtual void Cleanup () { }

        public IFileDialogVidget Frontend { get; protected set; }

        public void InitializeBackend (IVidget frontend, VidgetApplicationContext context) {
            this.Frontend = frontend as IFileDialogVidget;
        }

        public Size Size {
            get { throw new ArgumentException(); }
        }

        public void Update () { }

        public void Invalidate () { }

        public void Invalidate (Rectangle rect) { }
        public void Dispose () { }
    }

    public class OpenFileDialogBackend : FileDialogBackend, IOpenfileDialogBackend {

        public override bool Run (IVidget parent) { return base.Run<OpenFileDialog>(parent); }

    }

    public class SaveFileDialogBackend : FileDialogBackend, ISavefileDialogBackend {

        public override bool Run (IVidget parent) { return base.Run<SaveFileDialog>(parent); }

    }
}