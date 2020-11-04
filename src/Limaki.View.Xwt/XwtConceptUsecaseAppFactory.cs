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
using System.IO;
using System.Runtime.InteropServices;
using Limaki.View.Vidgets;

namespace Limaki.View.XwtBackend {
    
    public class XwtConceptUsecaseAppFactory : UsecaseAppFactory<LimadaResourceLoader, ConceptUsecase> {

        public XwtConceptUsecaseAppFactory () : base () { }

        public override bool TakeToolkit (IToolkitAware loader) {
            return base.TakeToolkit (loader) || loader.ToolkitType == LimakiViewGuids.ToolkitGuid;
        }

        Guid DefaultGtkToolkitType () {
            var fw = System.Runtime.InteropServices.RuntimeInformation.FrameworkDescription;
            var isCore = fw.Contains ("Core");
            return isCore ? LimakiViewGuids.Gtk3ToolkitGuid : LimakiViewGuids.GtkToolkitGuid;
        }

        string GetGtkWinDir () {
            string Probe (string basepath) {
                var binPath = Path.Combine (basepath, Path.Combine ("gtk3-win", "bin"));
                return Directory.Exists (binPath) ? binPath : default;
            }

            return Probe (AppDomain.CurrentDomain.BaseDirectory) ?? Probe (Environment.GetFolderPath (Environment.SpecialFolder.ProgramFiles)) ?? default;
        }
        
        public override void Run () {
            
            ToolkitType = DefaultGtkToolkitType();
            
            if (ToolkitType == LimakiViewGuids.Gtk3ToolkitGuid && RuntimeInformation.IsOSPlatform (OSPlatform.Windows)) {
                var systemPath = Environment.GetEnvironmentVariable ("Path", EnvironmentVariableTarget.Machine);
                var binPath = GetGtkWinDir ();
                Trace.WriteLine ($"\n{nameof(GetGtkWinDir)} on {binPath}");
                if (binPath != default) {
                    Environment.SetEnvironmentVariable ("Path", $"{binPath};{systemPath}");
                }
            }
            
            var tkts = ConfigurationManager.AppSettings["ToolkitType"];
            if (!string.IsNullOrEmpty (tkts)) {
                if (Enum.TryParse<Xwt.ToolkitType> (tkts, out var tk)) {
                    XwtToolkitType = tk;
                    ToolkitType = Converter.ToLmk (XwtToolkitType);
                }
            } else {
                XwtToolkitType = Converter.ToXwtToolkitType (ToolkitType);
            }

            Application.Initialize (this.XwtToolkitType);

            this.Configure (new XwtContextResourceLoader ());

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