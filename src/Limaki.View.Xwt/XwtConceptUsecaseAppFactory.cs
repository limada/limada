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
using System.Linq;
using System.Configuration;
using Limada.Usecases;
using Limaki.Common;
using Limaki.Common.IOC;
using Limada.Usecases;
using Limaki.Usecases;
using Limaki.View.XwtBackend;
using Xwt;
using System.Diagnostics;
using Limaki.View.Vidgets;

namespace Limaki.View.XwtBackend {
    
    public class XwtConceptUsecaseAppFactory : UsecaseAppFactory<LimadaResourceLoader, ConceptUsecase> {

        ToolkitType? _toolkitType = null;
        public override ToolkitType XwtToolkitType {
            get {
                if (_toolkitType.HasValue)
                    return _toolkitType.Value;
				return OS.Unix ? Xwt.ToolkitType.Gtk : Xwt.ToolkitType.Wpf;
            }
            protected set { _toolkitType = value; }
        }

        public XwtConceptUsecaseAppFactory () : base () { }

        public override bool TakeToolkit (IToolkitAware loader) {
            return base.TakeToolkit (loader) || loader.ToolkitType == XwtContextResourceLoader.ToolkitGuid;
        }

        public override Guid ToolkitType {
            get {
                if (XwtToolkitType == Xwt.ToolkitType.Wpf)
                    return XwtContextResourceLoader.WpfToolkitGuid;
                if (XwtToolkitType == Xwt.ToolkitType.Gtk)
                    return XwtContextResourceLoader.GtkToolkitGuid;
                return base.ToolkitType;
            }
            protected set {
                base.ToolkitType = value;
            }
        }

        public override void Run () {

            var tkts = ConfigurationManager.AppSettings["ToolkitType"];
            if (!string.IsNullOrEmpty (tkts)) {
                var tk = this.XwtToolkitType;
                if (Enum.TryParse<Xwt.ToolkitType> (tkts, out tk))
                    this.XwtToolkitType = tk;
            }

            Application.Initialize (this.XwtToolkitType);

            this.Create (new XwtContextResourceLoader ());

            About.ToolKitType = XwtToolkitType.ToString ();

            Window window = null;
            Action onShow = null;
           
            var composer = CreateUseCase ();
            window = composer.MainWindow.Backend as Window;
            onShow += composer?.OnShow;


            window.Show ();
            onShow?.Invoke ();

            MessageDialog.RootWindow = window;
            Application.UnhandledException += (s, e) =>
                Registry.Pooled<IExceptionHandler> ().Catch (e.ErrorException, MessageType.OK);

            Application.Run ();

            window.Dispose ();

            Application.Dispose ();
        }


        protected virtual IXwtBackendConceptUseCaseComposer CreateUseCase () {

            var vindow = new Vindow (new MainWindowBackend());

            var mainWindowBackend = vindow.Backend as MainWindowBackend;
			Iconerias.Iconery.Compose ();
            mainWindowBackend.Icon = Iconerias.Iconery.LimadaLogo;
            mainWindowBackend.Size = new Size (800, 600);
            mainWindowBackend.Padding = 2;

            var backendComposer = Registry.Create<IXwtBackendConceptUseCaseComposer> ();
            backendComposer.MainWindow = vindow;

            var factory = new UsecaseFactory<ConceptUsecase> ();
            factory.Composer = new ConceptUsecaseComposer ();
            factory.BackendComposer = backendComposer;

            var useCase = factory.Create ();
            useCase.MainWindow = vindow;

            factory.Compose (useCase);

            backendComposer.FinalizeCompose?.Invoke ();

            CallPlugins (factory, useCase);

            useCase.Start ();

            if (useCase.ApplicationQuitted) {
                Application.Exit ();
            }
            return backendComposer;
        }


    }


}