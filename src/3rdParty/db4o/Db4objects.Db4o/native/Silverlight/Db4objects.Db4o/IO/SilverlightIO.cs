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
#if SILVERLIGHT
using System;
using System.IO;
using System.IO.IsolatedStorage;

namespace Db4objects.Db4o.IO
{
	public static class SilverlightIO
	{
		public static bool Exists(string path)
		{
			return ExistsIn(IsolatedStorageFile.GetUserStoreForApplication(), path);
		}

		private static bool ExistsIn(IsolatedStorageFile storage, string path)
		{
			return storage.FileExists(path);
		}

		public static bool Delete(string path)
		{
			IsolatedStorageFile storage = IsolatedStorageFile.GetUserStoreForApplication();
			if (ExistsIn(storage, path))
			{
				storage.DeleteFile(path);
				return !ExistsIn(storage, path);
			}
			return false;
		}

		public static long Length(string path)
		{
			IsolatedStorageFile storage = IsolatedStorageFile.GetUserStoreForApplication();
			using (IsolatedStorageFileStream fileStream = storage.OpenFile(path, FileMode.Open, FileAccess.Read, FileShare.Read))
			{
				return fileStream.Length;
			}
		}
	}
}
#endif