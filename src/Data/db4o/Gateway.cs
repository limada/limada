/*
 * Limaki 
 * Version 0.07
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2006-2008 Lytico
 *
 * http://limada.sourceforge.net
 * 
 */

using System;
using System.Collections.Generic;
using System.Text;
using Db4objects.Db4o;
using Db4objects.Db4o.Config;

namespace Limaki.Data.db4o {
    public class Gateway : GatewayBase {
        # region session

        private IConfiguration _configuration = null;
        public IConfiguration Configuration {
            get {
                if (_configuration == null) {
                    _configuration = Db4oFactory.NewConfiguration();
                    InitConfiguration(_configuration);
                }
                return _configuration;
            }
            set { _configuration = value; }
        }

        bool isGatewayDisposing = false;

        IObjectContainer _session = null;
        public virtual IObjectContainer Session {
            get {
                if (!isGatewayDisposing) {
                    if (_session == null) {
                        try {
                            _session = Db4oFactory.OpenFile(_configuration, DataBaseInfo.Path + DataBaseInfo.Name + "." + this.FileExtension);
                        } catch (Exception e) {
                            throw e;
                        }
                    }
                }
                return _session;
            }
        }



        # endregion session

        #region IGateway Member

        public override void Open(DataBaseInfo dataBaseInfo) {
            isGatewayDisposing = false;
            this.DataBaseInfo = dataBaseInfo;

        }

        public override void Close() {
            isGatewayDisposing = true;
            if (_session != null) {
                try {
                    Session.Close();
                    Session.Dispose();
                } catch (Db4objects.Db4o.Ext.Db4oException e) {
                    // TODO: a curios exception is thrown here:
                    // "This functionality is only available for indexed fields."
                    // it is: failing of UniqueFieldValueConstraint
                    // see also: Graph.Flush()
                    throw e;
                } finally {
                    _session = null;
                }
            }
            this.DataBaseInfo = null;
        }

        public virtual bool HasSession() {
            return _session != null;
        }
        public override bool IsOpen() {
            return DataBaseInfo != null;
        }

        public virtual void InitConfiguration(IConfiguration configuration) {
            configuration.MarkTransient(typeof(Limaki.Common.TransientAttribute).FullName);
        }

        public override string FileExtension {
            get { return "limo007"; }
        }

        #endregion
    }
}
