
using System;
using System.IO;
using Limada.Data;
using Limada.Model;
using Limaki.Common;
using Limaki.Data;
using System.Collections.Generic;

namespace Limada.UseCases {
	public class FileManagerBase {
		protected IThingGraphProvider _thingGraphProvider = null;
        public IThingGraphProvider ThingGraphProvider {
            get {
                if (_thingGraphProvider == null) {
                    _thingGraphProvider = new MemoryThingGraphProvider();
                }
                return _thingGraphProvider;
            }
            set { _thingGraphProvider = value; }
        }
		
		public void Close() {
            if (_thingGraphProvider != null) {
                _thingGraphProvider.Close();
            }           
        }
		
		public virtual IThingGraphProvider GetThingGraphProvider(DataBaseInfo info) {
            var providers = Registry.Pool.TryGetCreate<DataProviders<IThingGraph>>();
            var result =  providers.Find(info) as IThingGraphProvider;
            if (result != null)
                result.Progress = this.Progress;
            return result;
        }

        public virtual IDataProvider<IEnumerable<IThing>> GetThingsProvider(DataBaseInfo info) {
            var providers = Registry.Pool.TryGetCreate<DataProviders<IEnumerable<IThing>>>();
            var result = providers.Find(info);
            if (result != null)
                result.Progress = this.Progress;
            return result;
        }

		public virtual bool OpenFile(DataBaseInfo fileName) {
            IThingGraphProvider provider = GetThingGraphProvider(fileName);
            bool result = false;
            if (provider != null) {
                this.ThingGraphProvider.Close();
                try {
					provider.Open(fileName);
	                this.ThingGraphProvider = provider;
	                result = true;
					
                } catch (Exception e){
					result = false;	
				}
            }
            return result;
        }

        public Action<string, int, int> Progress { get; set; }
        protected virtual void Message(string m,int i, int count) {
            if (Progress != null)
                Progress(m,i,count);
        }
	}
}



