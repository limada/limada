/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2010-2014 Lytico
 *
 * http://www.limada.org
 * 
 */

using System;
using System.Windows.Forms;
using Limada.Usecases;
using Limada.UseCases;
using Limaki.Usecases;
using Limaki.View;
using Limaki.View.SwfBackend;
using Limaki.View.SwfBackend.VidgetBackends;
using Limaki.View.Vidgets;
using Limaki.View.XwtBackend;

namespace Limada.Usecases {

    public class SwfAppFactory : UsecaseAppFactory<LimadaResourceLoader, ConceptUsecase> {

        public override Xwt.ToolkitType XwtToolkitType {
            get { return Xwt.ToolkitType.Other; }
        }

        public SwfAppFactory(): base(new SwfContextResourceLoader()) {}


        public ConceptUsecase CreateUseCase( ) {
            
            var vindow = new Vindow ();

            var mainform = vindow.Backend.ToSwf() as Form;
            Xwt.SwfBackend.SwfEngine.SwfApplicationContext.MainForm = mainform;

            Limaki.Iconerias.Iconery.Compose ();
            mainform.Icon = Limaki.View.Properties.GdiIconery.LimadaIcon;
            mainform.ClientSize = new System.Drawing.Size(800, 600);
              
            var backendComposer = new SwfConceptUseCaseComposer { 
                MainWindow = vindow
            };

            var factory = new UsecaseFactory<ConceptUsecase>();
            factory.Composer = new ConceptUsecaseComposer();
            factory.BackendComposer = backendComposer;
            
            var useCase = factory.Create();
            useCase.MainWindow = vindow;

            factory.Compose(useCase);

            backendComposer.FinalizeCompose?.Invoke ();

            CallPlugins(factory, useCase);
            
            useCase.Start();

            if (useCase.ApplicationQuitted) {
                Application.Exit();
                Environment.Exit(0);
            }

            return useCase;
        }

        public override void Run () {

            Application.EnableVisualStyles ();
            Application.SetCompatibleTextRenderingDefault (false);

            var context = new ApplicationContext ();
            Xwt.SwfBackend.SwfEngine.SwfApplicationContext = context;

            CreateUseCase ();

            Application.Run (context);
        }
    }
    
}