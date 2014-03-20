using Limaki.Data;
using System.Collections.Specialized;
using System;

namespace Limada.UseCases.Cms {

    public class AppController {

        public virtual string SiteName { get; set; }

        public BackendController Backend { get; set; }

        public virtual void OpenBackend () {

            if (Backend == null) {
                Backend = new BackendController { Iori = new Iori() };
                GetAppSettings(Backend.Iori);
                Backend.Open();
            }
        }

        public Func<NameValueCollection> AppSettingsGetter { get; set; }

        protected void GetAppSettings (Iori iori) {

            var appSettings = AppSettingsGetter();//;

            for (int i = 0; i < appSettings.Count; i++) {
                string key = appSettings.GetKey(i);
                string data = appSettings.Get(i);

                if (key == "DataBaseFileName") {
                    Iori.FromFileName(iori, data);
                }

                if (key == "DataBaseServer") {
                    iori.Server = data;
                }
                if (key == "DataBaseName") {
                    iori.Name = data;
                }
                if (key == "DataBasePath") {
                    iori.Path = data;
                }
                if (key == "DataBaseUser") {
                    iori.User = data;
                }
                if (key == "DataBasePassword") {
                    iori.Password = data;
                }
                if (key == "DataBaseProvider") {
                    iori.Provider = data;
                }

                if (key == "SiteName") {
                    SiteName = data;
                }
            }

        }

        public void Close () {
            Backend.Close();
            Backend = null;
        }

    }
}


