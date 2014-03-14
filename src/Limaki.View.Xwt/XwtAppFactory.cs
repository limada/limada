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
using Limada.Usecases;
using Limaki.Common;
using Limaki.Common.IOC;
using Limaki.Usecases;
using Limaki.View.XwtBackend;
using Xwt;

namespace Limaki.View.XwtBackend {

    public class XwtAppFactory : AppFactory<Limada.Usecases.LimadaResourceLoader> {
        
        ToolkitType ToolkitType {
            get { return Xwt.ToolkitType.Wpf; }
        }

        public XwtAppFactory () : base() { }
       
        public void Run () {

            Application.Initialize(this.ToolkitType);
            this.Create(new XwtContextResourceLoader());

            Window w = null;
            Action onShow = null;
            if (true) {
                var com = CreateUseCase();
                w = com.MainWindowBackend as Window;
                if (com.OnShow != null)
                    onShow += com.OnShow;
            } else {
                w = new PrototypeWindow().Composed();
            }
            w.Show();
            if (onShow != null)
                onShow();

            MessageDialog.RootWindow = w;

            Application.Run();

            w.Dispose();

            Application.Dispose();
        }

        public override bool TakeType (System.Type type) {
            if(type.GetInterfaces().Any(t=>t==typeof(IToolkitAware))) {
                if (type.GetConstructors().Any(tc => tc.GetParameters().Length == 0)) {
                    var loader = Activator.CreateInstance(type) as IToolkitAware;
                    return loader.ToolkitType == this.ToolkitType;
                }
            }
            return base.TakeType(type);
        }

        public IXwtConceptUseCaseComposer CreateUseCase () {

            var backendComposer = Registry.Pooled<IXwtConceptUseCaseComposer>();
            backendComposer.MainWindowBackend = new MainWindowBackend();
            backendComposer.WindowSize = new Size(800, 600);
            
            //mainWindow.Icon = Limaki.View.Properties.GdiIconery.LimadaLogo;

            var factory = new UsecaseFactory<ConceptUsecase>();
            factory.Composer = new ConceptUsecaseComposer();
            factory.BackendComposer = backendComposer;

            var useCase = factory.Create();
            factory.Compose(useCase);

            CallPlugins(factory, useCase);

            useCase.Start();

            if (useCase.ApplicationQuitted) {
                Application.Exit();
            }
            return backendComposer;
        }

        public void CallPlugins (UsecaseFactory<ConceptUsecase> factory, ConceptUsecase useCase) {
            var factories = Registry.Pooled<UsecaseFactories<ConceptUsecase>>();
            foreach (var item in factories) {
                item.Composer = factory.Composer;
                item.BackendComposer = factory.BackendComposer;
                item.Compose(useCase);
            }
        }
    }


}