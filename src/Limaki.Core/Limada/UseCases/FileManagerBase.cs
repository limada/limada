/*
 * Limada 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2006-2013 Lytico
 *
 * http://www.limada.org
 */

using System;
using System.IO;
using Limada.Data;
using Limada.Model;
using Limaki.Data;
using System.Collections.Generic;

namespace Limada.Usecases {

    public class FileManagerBase {

		protected IThingGraphProvider _thingGraphProvider = null;
	    public IThingGraphProvider ThingGraphProvider { 
            get { return _thingGraphProvider ?? (_thingGraphProvider = new MemoryThingGraphProvider()); } 
            set { _thingGraphProvider = value; } 
        }

	    public void Close() {
            if (_thingGraphProvider != null) {
                _thingGraphProvider.Close();
            }           
        }

	    protected DataProviderManager<IThingGraph> _thingGraphProviders = null;
        protected DataProviderManager<IThingGraph> ThingGraphProviderManager {
            get {
                return _thingGraphProviders ??
                       (_thingGraphProviders = new DataProviderManager<IThingGraph> {
                           DefaultExtension = "limo",
                           Progress = this.Progress
                       });
            }
        }

	    protected DataProviderManager<IEnumerable<IThing>> _thingsProviders = null;
        protected DataProviderManager<IEnumerable<IThing>> ThingsProviderManager {
            get {
                return _thingsProviders ??
                       (_thingsProviders = new DataProviderManager<IEnumerable<IThing>> { Progress = this.Progress });
            }
        }

		public virtual IThingGraphProvider GetThingGraphProvider(DataBaseInfo info) {
            return ThingGraphProviderManager.GetProvider(info) as IThingGraphProvider;
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

        public virtual IDataProvider<IEnumerable<IThing>> GetThingsProvider (DataBaseInfo info) {
            return ThingsProviderManager.GetProvider(info);
        }
	}
}



