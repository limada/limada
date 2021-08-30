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
using Db4objects.Db4o;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Handlers;

namespace Db4objects.Db4o.Internal
{
	internal sealed class MessageOutput
	{
		internal readonly TextWriter stream;

		internal MessageOutput(ObjectContainerBase a_stream, string msg)
		{
			stream = a_stream.ConfigImpl.OutStream();
			Print(msg, true);
		}

		internal MessageOutput(string a_StringParam, int a_intParam, TextWriter a_stream, 
			bool header)
		{
			stream = a_stream;
			Print(Db4objects.Db4o.Internal.Messages.Get(a_intParam, a_StringParam), header);
		}

		internal MessageOutput(string a_StringParam, int a_intParam, TextWriter a_stream)
			 : this(a_StringParam, a_intParam, a_stream, true)
		{
		}

		private void Print(string msg, bool header)
		{
			if (stream != null)
			{
				if (header)
				{
					stream.WriteLine("[" + Db4oFactory.Version() + "   " + DateHandlerBase.Now() + "] "
						);
				}
				stream.WriteLine(" " + msg);
			}
		}
	}
}
