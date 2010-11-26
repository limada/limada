
using System;
using System.IO;
using Limada.Data;
using Limada.Model;
using Limaki.Common;
using Limaki.Data;

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
		
		protected virtual IThingGraphProvider GetProvider(DataBaseInfo info) {
            var providers = Registry.Pool.TryGetCreate<DataProviders<IThingGraph>>();
            return providers.Find(info) as IThingGraphProvider;
        }
		
		public virtual bool OpenFile(DataBaseInfo fileName) {
            IThingGraphProvider provider = GetProvider(fileName);
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
	}
}



