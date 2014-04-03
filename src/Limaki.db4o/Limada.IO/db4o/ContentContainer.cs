/*
 * Limada
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2006-2011 Lytico
 *
 * http://www.limada.org
 * 
 */

using Db4objects.Db4o;
using Db4objects.Db4o.Query;
using Limada.Model;
using Limaki.Contents;
using Limaki.Data;
using Limaki.Data.db4o;
using System;
using Id = System.Int64;

namespace Limada.IO.db4o {

    public class ContentContainer : IContentContainer<Id> {

        Gateway _gateway = null;

        public ContentContainer(IGateway gateway) {
            this._gateway = gateway as Gateway;
            if (this._gateway == null) {
                throw new ArgumentException("Wrong Gateway");
            }
            Configure();
        }

        private void Configure() {}

        public IObjectContainer Session {
            get {
                if (!_gateway.HasSession()) {
                    ConfigureSession(_gateway.Session);
                }
                return _gateway.Session;
            }
        }

        private void ConfigureSession(IObjectContainer session) {}

		// this is the type which is used as a base-class in queries
		// as long as there is only IRealData<byte[]> without hierarchie, this is enough
		Type realDataBaseType = typeof(RealData);

        public virtual bool Contains(IIdContent<Id> item) {
            bool result = false;
            try {
                result = Session.Ext().IsStored(item);
            } catch (Exception e) {
                throw e;
            } finally { }
            return result;
        }

        public virtual IIdContent<Id> UniqueData(IIdContent<Id> data) {
            IIdContent<Id> result = data;
            if (data != null && !Session.Ext().IsStored(data)) {
                IIdContent<Id> stored = this.GetById(data.Id);
                if (stored != null) {
                    long id = Session.Ext().GetID(stored);
                    Session.Ext().Bind(data, id);
                }
            }
            return result;
        }

        public virtual void Add(IIdContent<Id> item) {
            try {
                Session.Store(this.UniqueData(item));
            } catch (Exception e) {
                throw e;
            } finally { }
        }

        public virtual bool Contains(Id id) {
            bool result = false;
            try {
                IQuery query = Session.Query();
                // Attention! Here we use RealData and NOT IRealData as of query-index-usage
                query.Constrain(realDataBaseType);
                query.Descend("_id").Constrain(id);
                IObjectSet set = query.Execute();
                result = set.HasNext();
            } catch (Exception e) {
                throw e;
            } finally { }
            return result;
        }

        public virtual IIdContent<Id> GetById(Id id) {
            IIdContent<Id> result = null;
            try {
                IQuery query = Session.Query();
                // Attention! Here we use RealData and NOT IRealData as of query-index-usage
                query.Constrain(realDataBaseType);
                query.Descend("_id").Constrain(id);
                var set = query.Execute();
                if (set.HasNext())
                    result = set.Next() as IIdContent<Id>;
            } catch (Exception e) {
                throw e;
            } finally { }
            return result;
        }

        public virtual bool Remove(Id id) {
            bool result = false;
            try {
                bool contained = Contains(id);
                if (contained) {
                    // TODO: this is stupid: 
                    // get the expensive object only to delete it!
                    IIdContent<Id> item = GetById(id);
                    Session.Delete(item);
                }
            } catch (Exception e) {
                throw e;
            } finally { }
            return result;

        }

        public virtual bool Remove(IIdContent<Id> item) {
            bool result = false;
            try {
                bool contained = Contains(item);
                if (contained) {
                    Session.Delete(item);
                    result = true;
                }
            } catch (Exception e) {
                throw e;
            } finally { }
            return result;
        }
    }
}