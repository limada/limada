/*
 * Limada 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2012-2013 Lytico
 *
 * http://www.limada.org
 */

using System;
using Limaki.Common;
using Limaki.Data;

namespace Limada.Usecases {

    public class DataProviderManager<T> {

        public string DefaultExtension { get; set; }
        public Action<string, int, int> Progress { get; set; }

        public virtual IDataProvider<T> GetProvider (IoInfo info) {
            var result = Providers.Find(info);
            if (result != null)
                result.Progress = this.Progress;
            return result;
        }

        string _saveFilter = null;
        public string SaveFilter { get { return _saveFilter ?? (_saveFilter = GetFilter(false)); } }

        string _readFilter = null;
        public string ReadFilter { get { return _readFilter ?? (_readFilter = GetFilter(true)); } }

        private DataProviders<T> _providers = null;
        protected DataProviders<T> Providers { get { return _providers ?? (_providers=Registry.Pool.TryGetCreate<DataProviders<T>>()); } }

        public string GetFilter (bool readable) {
            string filter = "";
            string defaultFilter = null;
            foreach (var provider in Providers) {
                if (provider.Saveable != readable || provider.Readable==readable) {
                    var f = provider.Description + "|*" + provider.Extension + "|";
                    if (DefaultExtension!=null && provider.Extension == "." + DefaultExtension)
                        defaultFilter = f;
                    else
                        filter += f;

                }
            }
            if (defaultFilter != null) {
                filter = defaultFilter + filter;
            }

            return filter;

        }

        
    }
}