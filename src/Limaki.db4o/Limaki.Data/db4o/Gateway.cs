/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2006-2014 Lytico
 *
 * http://www.limada.org
 * 
 */

using Db4objects.Db4o;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.CS;
using Db4objects.Db4o.CS.Config;
using Limaki.Contents.IO;
using System;
using System.Diagnostics;
using System.IO;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

namespace Limaki.Data.db4o {

    public class Gateway : GatewayBase {

        # region session
        private ICommonConfigurationProvider _configuration = null;
        public ICommonConfiguration Configuration {
            get {
                if (_configuration == null) {
                    var accessMode = IoMode.None;
                    if (Iori != null)
                        accessMode = Iori.AccessMode;
                    if (accessMode.HasFlag(IoMode.Server)) {
                        _configuration = Db4oClientServer.NewServerConfiguration();
                    } else if (accessMode.HasFlag(IoMode.Client)) {
                        _configuration = Db4oClientServer.NewClientConfiguration();
                    } else {
                        _configuration = Db4oEmbedded.NewConfiguration();
                    } 

                    InitConfiguration(_configuration.Common);
                }
                return _configuration.Common;
            }
        }

        bool _isClosed = false;

        IObjectServer Server { get; set; }
        IObjectContainer _session = null;

        public virtual IObjectContainer Session {
            get {
                if (!_isClosed) {
                    if (_session == null) {
                        try {
                            var emb = _configuration as IEmbeddedConfiguration;
                            if (emb != null)
                                _session = CreateEmbeddedSession(emb);

                            var client = _configuration as IClientConfiguration;
                            if (client != null)
                                _session = CreateClientSession(client);

                            var server = _configuration as IServerConfiguration;
                            if (server != null) {
                                this.Server = OpenServer(server);
                                _session = CreateClientSession(this.Server);
                            }

                            if (_session == null)
                                throw new IOException();

                        } catch (Exception e) {
                            Exception ex = new Exception(
                                e.Message + "\nFile open failed:\t" +
                                Iori.Path + Iori.Name + Iori.Extension,
                                e);
                            throw ex;
                        }
                    }
                }
                return _session;
            }
        }

        public virtual IObjectContainer CreateEmbeddedSession(IEmbeddedConfiguration config) {
            var file = Iori.ToFileName(this.Iori);
            if (!System.IO.File.Exists(file)) {
                config.File.BlockSize = 16;
                Trace.TraceInformation("{0}: File not exists: {1}", this.GetType().FullName, file);
            }
            return Db4oEmbedded.OpenFile(config,file);
        }

        public virtual IObjectContainer CreateClientSession (IClientConfiguration config) {
            var clientCer = "limada.limo.client.cer";
            if (File.Exists (clientCer)) {
                config.AddConfigurationItem (new ClientSslSupport (CheckCertificate));
            }

            return Db4oClientServer.OpenClient (config, Iori.Server, Iori.Port, Iori.User, Iori.Password);
        }

        protected virtual bool CheckCertificate (object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) {
            return true;
        }

        public virtual IObjectContainer CreateClientSession (IObjectServer server) {
            return server.OpenClient();
        }
      
        public virtual IObjectServer OpenServer (IServerConfiguration config) {
            // remark: if port == 0, then server runs in embedded mode
            var file = Iori.ToFileName (this.Iori);
            var serverCer = "limada.limo.cer";
            if (File.Exists (serverCer)) {
                var certificate = new X509Certificate2 (serverCer,"");
                config.AddConfigurationItem (new ServerSslSupport (certificate));
            }
            
            var server = Db4oClientServer.OpenServer (config, file, this.Iori.Port);
            try {
                Trace.WriteLine (string.Format ("db4o server {0} running at: {1}", file, server.Ext ().Port ()));
                server.GrantAccess(this.Iori.User, this.Iori.Password);
                return server;
            } catch {
                server.Close();
                throw;
            }
        }

        # endregion session

        #region IGateway Member

        public override void Open(Iori iori) {
            _isClosed = false;
            this.Iori = iori;
        }

        public override void Close() {
            _isClosed = true;
            if (_session != null) {
                try {
                    Session.Close();
                    Session.Dispose();
                    _configuration = null;
                    _session = null;
                } catch (Db4objects.Db4o.Ext.Db4oException e) {
                    // TODO: a curios exception is thrown here:
                    // "This functionality is only available for indexed fields."
                    // it is: failing of UniqueFieldValueConstraint
                    // see also: Graph.Flush()
                    throw e;
                } finally {
                    _session = null;
                    _configuration = null;
                    if (Server != null) {
                        try {
                            Server.Close();
                        } catch { throw;
                        } finally { Server = null;
                        }
                    }
                }
            }
            this.Iori = null;
        }

        public virtual bool HasSession() {
            return _session != null;
        }
        public override bool IsOpen() {
            return Iori != null;
        }
        public override bool IsClosed() {
            return _isClosed;
        }
        public virtual void InitConfiguration(ICommonConfiguration configuration) {
            configuration.MarkTransient(typeof(Limaki.Common.TransientAttribute).FullName);
        }

        public override void Dispose () {
            
        }
        #endregion
    }
}
