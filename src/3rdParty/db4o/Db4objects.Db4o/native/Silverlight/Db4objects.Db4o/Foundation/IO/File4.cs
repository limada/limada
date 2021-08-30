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

namespace Db4objects.Db4o.Foundation.IO
{
	public class File4
	{
		
		public static void Delete(string file)
		{
			IsolatedStorageFile storageFile = IsolatedStorageFile.GetUserStoreForApplication();
			if (storageFile.FileExists(file))
			{
				storageFile.DeleteFile(file);
			}
		}

		public static void Copy(string from, string to)
		{
			throw new NotImplementedException();
		}

		public static bool Exists(string file)
		{
			IsolatedStorageFile storageFile = IsolatedStorageFile.GetUserStoreForApplication();
			return storageFile.FileExists(file);
		}

		public static long Size(string filePath)
		{
			using (IsolatedStorageFile storageFile = IsolatedStorageFile.GetUserStoreForApplication())
			{
				using (IsolatedStorageFileStream fileStream = storageFile.OpenFile(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
				{
					return fileStream.Length;
				}
			}
		}
	}
 }
#endif
