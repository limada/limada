using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using Limaki.Common;

namespace Limaki.View.WPF.Test {
    /// <summary>
    /// Interaktionslogik für "App.xaml"
    /// </summary>
    public partial class App : Application {
        protected override void OnStartup(StartupEventArgs e) {
           

            var loader = new WPFContextRecourceLoader();
            Registry.ConcreteContext = loader.CreateContext();
            loader.ApplyResources(Registry.ConcreteContext);

            base.OnStartup(e);
        }
    }
}
