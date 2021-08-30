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
using System;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Query;

namespace Db4objects.Db4o.Config
{
	[Obsolete("Since 8.0")]
	public class JavaSupport : IConfigurationItem
	{
		public void Prepare(IConfiguration config)
		{
			AddDb4OBasicAliases(config);
			AddExceptionAliases(config);
			AddQueryAliases(config);
			AddCollectionAliases(config);
			AddClientServerAliases(config);
			
			config.ObjectClass("java.lang.Class").Translate(new TType());
		}

		private static void AddDb4OBasicAliases(IConfiguration config)
		{
			config.AddAlias(new WildcardAlias("com.db4o.ext.*", "Db4objects.Db4o.Ext.*, Db4objects.Db4o"));
			config.AddAlias(new TypeAlias("com.db4o.StaticField", FullyQualifiedName(typeof(StaticField))));
			config.AddAlias(new TypeAlias("com.db4o.StaticClass", FullyQualifiedName(typeof(StaticClass))));
		}

		private static void AddCollectionAliases(IConfiguration config)
		{
			config.AddAlias(new TypeAlias("com.db4o.foundation.Collection4", FullyQualifiedName(typeof(Collection4))));
			config.AddAlias(new TypeAlias("com.db4o.foundation.List4", FullyQualifiedName(typeof(List4))));
		}

		private static void AddClientServerAliases(IConfiguration config)
		{
#if !SILVERLIGHT
			config.AddAlias(new TypeAlias("com.db4o.User", FullyQualifiedName(typeof(User))));
			config.AddAlias(new TypeAlias("com.db4o.cs.internal.messages.MUserMessage$UserMessagePayload", "Db4objects.Db4o.CS.Internal.Messages.MUserMessage+UserMessagePayload, Db4objects.Db4o.CS"));
			config.AddAlias(new TypeAlias("com.db4o.internal.TransportObjectContainer$KnownObjectIdentity", "Db4objects.Db4o.Internal.TransportObjectContainer+KnownObjectIdentity, Db4objects.Db4o"));
			config.AddAlias(new WildcardAlias("com.db4o.cs.internal.*", "Db4objects.Db4o.CS.Internal.*, Db4objects.Db4o.CS"));
#endif
		}

		private static void AddQueryAliases(IConfiguration config)
		{
			config.AddAlias(new TypeAlias("com.db4o.query.Evaluation", FullyQualifiedName(typeof(IEvaluation))));
			config.AddAlias(new TypeAlias("com.db4o.query.Candidate", FullyQualifiedName(typeof(ICandidate))));
			config.AddAlias(new WildcardAlias("com.db4o.internal.query.processor.*", "Db4objects.Db4o.Internal.Query.Processor.*, Db4objects.Db4o"));
		}

		private static void AddExceptionAliases(IConfiguration config)
		{
			config.AddAlias(new TypeAlias("com.db4o.foundation.ChainedRuntimeException", FullyQualifiedName(typeof(Exception))));
			config.AddAlias(new TypeAlias("java.lang.Throwable", FullyQualifiedName(typeof(Exception))));
			config.AddAlias(new TypeAlias("java.lang.RuntimeException", FullyQualifiedName(typeof(Exception))));
			config.AddAlias(new TypeAlias("java.lang.Exception", FullyQualifiedName(typeof(Exception))));
		}

		private static string FullyQualifiedName(Type type)
		{
			return ReflectPlatform.FullyQualifiedName(type);
		}

		public void Apply(IInternalObjectContainer container)
		{
		}
	}
}
