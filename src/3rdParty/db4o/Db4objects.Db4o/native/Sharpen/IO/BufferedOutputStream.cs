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

namespace Sharpen.IO
{
	public class BufferedOutputStream : IOutputStream
	{
		private IOutputStream _stream;

		public BufferedOutputStream(IOutputStream stream)
		{
			_stream = stream;
		}

		public BufferedOutputStream(IOutputStream stream, int bufferSize)
		{
			_stream = stream;
		}

		public void Write(int i)
		{
			_stream.Write(i);
		}

		public void Write(byte[] bytes)
		{
			_stream.Write(bytes);
		}

		public void Write(byte[] bytes, int offset, int length)
		{
			_stream.Write(bytes, offset, length);
		}

		public void Flush()
		{
			_stream.Flush();
		}

		public void Close()
		{
			_stream.Close();
		}
	}
}
