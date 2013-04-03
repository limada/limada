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
                Backend = new BackendController { DataBaseInfo = new DataBaseInfo() };
                GetAppSettings(Backend.DataBaseInfo);
                Backend.Open();
            }
        }

        public Func<NameValueCollection> AppSettingsGetter { get; set; }

        protected void GetAppSettings (DataBaseInfo dataBaseInfo) {

            var appSettings = AppSettingsGetter();//;

            for (int i = 0; i < appSettings.Count; i++) {
                string key = appSettings.GetKey(i);
                string data = appSettings.Get(i);

                if (key == "DataBaseFileName") {
                    DataBaseInfo.FromFileName(dataBaseInfo, data);
                }

                if (key == "DataBaseServer") {
                    dataBaseInfo.Server = data;
                }
                if (key == "DataBaseName") {
                    dataBaseInfo.Name = data;
                }
                if (key == "DataBasePath") {
                    dataBaseInfo.Path = data;
                }
                if (key == "DataBaseUser") {
                    dataBaseInfo.User = data;
                }
                if (key == "DataBasePassword") {
                    dataBaseInfo.Password = data;
                }
                if (key == "DataBaseProvider") {
                    dataBaseInfo.Provider = data;
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


