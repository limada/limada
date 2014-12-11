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

namespace Limaki.View.XwtBackend {

    public class XwtAppFactory : UsercaseAppFactory<LimadaResourceLoader, ConceptUsecase> {

        ToolkitType? _toolkitType = null;
        public override ToolkitType ToolkitType {
            get {
                if (_toolkitType.HasValue)
                    return _toolkitType.Value;
                return Xwt.ToolkitType.Wpf;
            }
            protected set { _toolkitType = value; }
        }

        public XwtAppFactory () : base () { }

        public override void Run () {

            var tkts = ConfigurationManager.AppSettings["ToolkitType"];
            if (!string.IsNullOrEmpty (tkts)) {
                var tk = this.ToolkitType;
                if (Enum.TryParse<Xwt.ToolkitType> (tkts, out tk))
                    this.ToolkitType = tk;
            }

            Application.Initialize (this.ToolkitType);

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