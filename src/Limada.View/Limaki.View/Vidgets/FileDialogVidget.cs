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

namespace Limaki.View.Vidgets {

    public interface IFileDialogVidget : IVidget { }

    public class FileDialogVidget : Vidget, IFileDialogVidget {
        
        private bool running;

        protected FileDialogVidget (FileDialogMemento dialog) : base() { this._dialog = dialog; }

        protected new IFileDialogBackend Backend {
            get { return (IFileDialogBackend) base.BackendHost.Backend; }
        }

        /// <summary>
        /// Shows the dialog.
        /// </summary>
        public bool Run () { return Run(null); }

        /// <summary>
        /// Shows the dialog.
        /// </summary>
        public bool Run (IVidget parentVidget) {
            try {
                running = true;
                Backend.Compose(_dialog);

                return Backend.Run(parentVidget);
            } finally {
                running = false;
                Backend.Cleanup();
            }
        }

        public override void Dispose () { }

        protected FileDialogMemento _dialog { get; set; }
    }


    public interface IFileDialogBackend : IVidgetBackend {

        void Compose (FileDialogMemento dialog);

        bool Run (IVidget parent);

        void Cleanup ();

    }

    public interface IOpenfileDialogBackend : IFileDialogBackend { }
    public interface ISavefileDialogBackend : IFileDialogBackend { }

    [BackendType(typeof(IOpenfileDialogBackend))]
    public class OpenfileDialogVidget : FileDialogVidget {
        public OpenfileDialogVidget (FileDialogMemento dialog) : base(dialog) { }
    }

    [BackendType(typeof(ISavefileDialogBackend))]
    public class SavefileDialogVidget : FileDialogVidget {
        public SavefileDialogVidget (FileDialogMemento dialog) : base(dialog) { }
    }

}