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

        public Form MainForm() {
            var result = new VindowBackend ();
            
            CreateUseCase (result);
            
            return result.ToSwf() as Form;
        }

        public void CreateUseCase(IVindowBackend vindowBackend) {
            var mainform = vindowBackend.ToSwf() as Form;

            mainform.Icon = Limaki.View.Properties.GdiIconery.LimadaIcon;
            mainform.ClientSize = new System.Drawing.Size(800, 600);

            var backendComposer = new SwfConceptUseCaseComposer();
            backendComposer.MainWindowBackend = vindowBackend;

            var factory = new UsecaseFactory<ConceptUsecase>();
            factory.Composer = new ConceptUsecaseComposer();
            factory.BackendComposer = backendComposer;
            
            var useCase = factory.Create();
            factory.Compose(useCase);

            backendComposer.Compose2 (useCase);

            CallPlugins(factory, useCase);
            
            useCase.Start();

            if (useCase.ApplicationQuitted) {
                Application.Exit();
                Environment.Exit(0);
            }
        }

        public override void Run () {

            Application.EnableVisualStyles ();
            Application.SetCompatibleTextRenderingDefault (false);

            Application.Run (MainForm ());
        }
    }
    
}