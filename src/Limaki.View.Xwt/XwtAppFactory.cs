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
using Limada.UseCases;
using Limaki.Usecases;
using Limaki.View.XwtBackend;
using Xwt;
using System.Diagnostics;

namespace Limaki.View.XwtBackend {

    public class XwtAppFactory : UsecaseAppFactory<LimadaResourceLoader, ConceptUsecase> {

        ToolkitType? _toolkitType = null;
        public override ToolkitType XwtToolkitType {
            get {
                if (_toolkitType.HasValue)
                    return _toolkitType.Value;
				return OS.Unix ? Xwt.ToolkitType.Gtk : Xwt.ToolkitType.Wpf;
            }
            protected set { _toolkitType = value; }
        }

        public XwtAppFactory () : base () { }

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

            Window window = null;
            Action onShow = null;
            if (true) {
                var composer = CreateUseCase ();
                window = composer.MainWindowBackend as Window;
                if (composer.OnShow != null)
                    onShow += composer.OnShow;
            } else {
                window = new PrototypeWindow ().Composed ();
            }

            window.Show ();
            if (onShow != null)
                onShow ();

            MessageDialog.RootWindow = window;
            Application.UnhandledException += (s, e) =>
                Registry.Pooled<IExceptionHandler> ().Catch (e.ErrorException, MessageType.OK);

            Application.Run ();

            window.Dispose ();

            Application.Dispose ();
        }

        protected virtual IXwtConceptUseCaseComposer CreateUseCase () {
			Limaki.Iconerias.Iconery.Compose ();
            var backendComposer = Registry.Create<IXwtConceptUseCaseComposer> ();
            backendComposer.MainWindowBackend = new MainWindowBackend {
                Icon = Limaki.Iconerias.Iconery.LimadaLogo
            };
            backendComposer.WindowSize = new Size (800, 600);

            var factory = new UsecaseFactory<ConceptUsecase> ();
            factory.Composer = new ConceptUsecaseComposer ();
            factory.BackendComposer = backendComposer;

            var useCase = factory.Create ();
            factory.Compose (useCase);

            CallPlugins (factory, useCase);

            useCase.Start ();

            if (useCase.ApplicationQuitted) {
                Application.Exit ();
            }
            return backendComposer;
        }


    }


}