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
using Limaki.Common;
using Limaki.Common.IOC;
using Limaki.Usecases;
using Limaki.Usecases.Concept;
using Limaki.View.XwtContext;
using Xwt;

namespace Limaki.View.XwtBackend {

    public class XwtAppFactory : AppFactory<Limada.Usecases.AppResourceLoader> {
        
        ToolkitType ToolkitType {
            get { return Xwt.ToolkitType.Gtk; }
        }

        public XwtAppFactory () : base() { }
       
        public void Run () {

            Application.Initialize(this.ToolkitType);
            this.Create(new XwtContextRecourceLoader());


            var w = MainWindow();// new PrototypeWindow ();// ();
            //w.Compose();
            w.Show();

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

        public Window MainWindow () {

            var result = new MainWindow();

            CreateUseCase(result);

            return result;
        }

        public void CreateUseCase (Window mainWindow) {

            var backendComposer = new XwtConceptUseCaseComposer {
                MainWindow = mainWindow, 
                WindowSize = new Size(800, 600)
            };
            

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
        }

        public void CallPlugins (UsecaseFactory<ConceptUsecase> factory, ConceptUsecase useCase) {
            var factories = Registry.Pool.TryGetCreate<UsecaseFactories<ConceptUsecase>>();
            foreach (var item in factories) {
                item.Composer = factory.Composer;
                item.BackendComposer = factory.BackendComposer;
                item.Compose(useCase);
            }
        }
    }


}