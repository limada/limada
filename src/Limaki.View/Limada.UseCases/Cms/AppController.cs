using Limaki.Data;
using Limaki.Common;
using System.Collections.Specialized;
using System;

namespace Limada.Usecases.Cms {

    public class AppController {

        public virtual string SiteName { get; set; }

        public BackendController Backend { get; set; }

        public virtual void OpenBackend () {

            if (Backend == null) {
                Backend = new BackendController { IoInfo = new IoInfo() };
                GetAppSettings(Backend.IoInfo);
                Backend.Open();
            }
        }

        public Func<NameValueCollection> AppSettingsGetter { get; set; }

        protected void GetAppSettings (IoInfo ioInfo) {

            var appSettings = AppSettingsGetter();//;

            for (int i = 0; i < appSettings.Count; i++) {
                string key = appSettings.GetKey(i);
                string data = appSettings.Get(i);

                if (key == "DataBaseFileName") {
                    IoInfo.FromFileName(ioInfo, data);
                }

                if (key == "DataBaseServer") {
                    ioInfo.Server = data;
                }
                if (key == "DataBaseName") {
                    ioInfo.Name = data;
                }
                if (key == "DataBasePath") {
                    ioInfo.Path = data;
                }
                if (key == "DataBaseUser") {
                    ioInfo.User = data;
                }
                if (key == "DataBasePassword") {
                    ioInfo.Password = data;
                }
                if (key == "DataBaseProvider") {
                    ioInfo.Provider = data;
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


