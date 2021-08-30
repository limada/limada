/* This file is part of the db4o object database http://www.db4o.com

Copyright (C) 2004 - 2010  Versant Corporation http://www.versant.com

db4o is free software; you can redistribute it and/or modify it under
the terms of version 3 of the GNU General Public License as published
by the Free Software Foundation.

db4o is distributed in the hope that it will be useful, but WITHOUT ANY
WARRANTY; without even the implied warranty of MERCHANTABILITY or
FITNESS FOR A PARTICULAR PURPOSE.  See the GNU General Public License
for more details.

You should have received a copy of the GNU General Public License along
with this program.  If not, see http://www.gnu.org/licenses/. */
using System.IO;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Config.Encoding;
using Db4objects.Db4o.Diagnostic;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Reflect;
using Db4objects.Db4o.Typehandlers;

namespace Db4objects.Db4o.Internal.Config
{
	public class CommonConfigurationImpl : ICommonConfiguration
	{
		private readonly Config4Impl _config;

		public CommonConfigurationImpl(Config4Impl config)
		{
			_config = config;
		}

		public virtual int ActivationDepth
		{
			get
			{
				return _config.ActivationDepth();
			}
			set
			{
				int depth = value;
				_config.ActivationDepth(depth);
			}
		}

		public virtual void Add(IConfigurationItem configurationItem)
		{
			_config.Add(configurationItem);
		}

		public virtual void AddAlias(IAlias alias)
		{
			_config.AddAlias(alias);
		}

		public virtual void RemoveAlias(IAlias alias)
		{
			_config.RemoveAlias(alias);
		}

		public virtual bool AllowVersionUpdates
		{
			set
			{
				bool flag = value;
				_config.AllowVersionUpdates(flag);
			}
		}

		public virtual bool AutomaticShutDown
		{
			set
			{
				bool flag = value;
				_config.AutomaticShutDown(flag);
			}
		}

		public virtual int BTreeNodeSize
		{
			set
			{
				int size = value;
				_config.BTreeNodeSize(size);
			}
		}

		public virtual bool Callbacks
		{
			set
			{
				bool flag = value;
				_config.Callbacks(flag);
			}
		}

		public virtual void CallbackMode(CallBackMode mode)
		{
			_config.CallbackMode(mode);
		}

		public virtual bool CallConstructors
		{
			set
			{
				bool flag = value;
				_config.CallConstructors(flag);
			}
		}

		public virtual bool DetectSchemaChanges
		{
			set
			{
				bool flag = value;
				_config.DetectSchemaChanges(flag);
			}
		}

		public virtual IDiagnosticConfiguration Diagnostic
		{
			get
			{
				return _config.Diagnostic();
			}
		}

		public virtual bool ExceptionsOnNotStorable
		{
			set
			{
				bool flag = value;
				_config.ExceptionsOnNotStorable(flag);
			}
		}

		public virtual bool InternStrings
		{
			set
			{
				bool flag = value;
				_config.InternStrings(flag);
			}
		}

		public virtual void MarkTransient(string attributeName)
		{
			_config.MarkTransient(attributeName);
		}

		public virtual int MessageLevel
		{
			set
			{
				int level = value;
				_config.MessageLevel(level);
			}
		}

		public virtual IObjectClass ObjectClass(object clazz)
		{
			return _config.ObjectClass(clazz);
		}

		public virtual bool OptimizeNativeQueries
		{
			get
			{
				return _config.OptimizeNativeQueries();
			}
			set
			{
				bool optimizeNQ = value;
				_config.OptimizeNativeQueries(optimizeNQ);
			}
		}

		public virtual IQueryConfiguration Queries
		{
			get
			{
				return _config.Queries();
			}
		}

		public virtual void ReflectWith(IReflector reflector)
		{
			_config.ReflectWith(reflector);
		}

		public virtual TextWriter OutStream
		{
			set
			{
				TextWriter outStream = value;
				_config.SetOut(outStream);
			}
		}

		public virtual IStringEncoding StringEncoding
		{
			set
			{
				IStringEncoding encoding = value;
				_config.StringEncoding(encoding);
			}
		}

		public virtual bool TestConstructors
		{
			set
			{
				bool flag = value;
				_config.TestConstructors(flag);
			}
		}

		public virtual int UpdateDepth
		{
			set
			{
				int depth = value;
				_config.UpdateDepth(depth);
			}
		}

		public virtual bool WeakReferences
		{
			set
			{
				bool flag = value;
				_config.WeakReferences(flag);
			}
		}

		public virtual int WeakReferenceCollectionInterval
		{
			set
			{
				int milliseconds = value;
				_config.WeakReferenceCollectionInterval(milliseconds);
			}
		}

		public virtual void RegisterTypeHandler(ITypeHandlerPredicate predicate, ITypeHandler4
			 typeHandler)
		{
			_config.RegisterTypeHandler(predicate, typeHandler);
		}

		public virtual IEnvironmentConfiguration Environment
		{
			get
			{
				return new _IEnvironmentConfiguration_139(this);
			}
		}

		private sealed class _IEnvironmentConfiguration_139 : IEnvironmentConfiguration
		{
			public _IEnvironmentConfiguration_139(CommonConfigurationImpl _enclosing)
			{
				this._enclosing = _enclosing;
			}

			public void Add(object service)
			{
				this._enclosing._config.EnvironmentContributions().Add(service);
			}

			private readonly CommonConfigurationImpl _enclosing;
		}

		public virtual void NameProvider(INameProvider provider)
		{
			_config.NameProvider(provider);
		}

		public virtual int MaxStackDepth
		{
			get
			{
				return _config.MaxStackDepth();
			}
			set
			{
				int maxStackDepth = value;
				_config.MaxStackDepth(maxStackDepth);
			}
		}
	}
}
