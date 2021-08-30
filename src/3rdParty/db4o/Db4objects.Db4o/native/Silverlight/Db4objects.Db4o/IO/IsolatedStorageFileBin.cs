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
using Db4objects.Db4o.Ext;
using Sharpen.Lang;
using Sharpen.Util;

namespace Db4objects.Db4o.IO
{
        class IsolatedStorageFileBin : IBin
		{
        	private readonly IsolatedStorageFileStream _fileStream;
        	private string _fullPath;

        	internal IsolatedStorageFileBin(BinConfiguration config, IsolatedStorageFile store)
			{
				Action cleanUp = Close;
				try
				{
					_fileStream = OpenFile(config, store);
					cleanUp = () => {};
				}
				catch (IsolatedStorageException e)
				{
					ThrowMappedException(e, config.Uri(), store);
				}
				finally
				{
					cleanUp();
				}				
            }

        	internal string Path
        	{
				get { return _fullPath; }
        	}

        	private static void ThrowMappedException(Exception e, string path, IsolatedStorageFile store)
        	{
        		if (store.FileExists(path))
        		{
        			throw new DatabaseFileLockedException(path, e);
        		}
        		throw new Db4oIOException(e);
        	}

        	private IsolatedStorageFileStream OpenFile(BinConfiguration config, IsolatedStorageFile store)
        	{
        		_fullPath = config.Uri();
        		string path = config.Uri();
        		IsolatedStorageFileStream stream = new IsolatedStorageFileStream(path, FileModeFor(store, path), FileAccessFor(config), FileShareFor(config), store);
        		Fill(stream, config.InitialLength(), 0);

				return stream;
        	}

        	private static FileMode FileModeFor(IsolatedStorageFile store, string path)
        	{
        		return store.FileExists(path) ? FileMode.Open : FileMode.CreateNew;
        	}

        	private static void Fill(Stream stream, long length, byte value)
        	{
        		if (length > 0)
        		{
        			byte[] bytes = new byte[length];
        			Arrays.Fill(bytes, value);
					stream.Write(bytes, 0, bytes.Length);
        		}
        	}

        	private static FileShare FileShareFor(BinConfiguration config)
        	{
        		return config.LockFile() ? FileShare.None : FileShare.ReadWrite;
        	}

        	private static FileAccess FileAccessFor(BinConfiguration config)
        	{
        		return config.ReadOnly() ? FileAccess.Read : FileAccess.ReadWrite;
        	}

        	#region IBin Members

            public long Length()
            {
                return _fileStream.Length;
            }

            public int Read(long position, byte[] bytes, int bytesToRead)
            {
                try
                {
                    Seek(position);
                    return _fileStream.Read(bytes, 0, bytesToRead);
                }
                catch (IOException e)
                {
                    throw new Db4oIOException(e);
                }
            }

            public void Write(long position, byte[] bytes, int bytesToWrite)
            {
                try
                {
                    Seek(position);
                    _fileStream.Write(bytes, 0, bytesToWrite);
                }
                catch (NotSupportedException e)
                {
                    throw new Db4oIOException(e);
                }
            }

            public void Sync()
            {
                _fileStream.Flush();                
            }

            public void Sync(IRunnable runnable)
            {
                Sync();
                runnable.Run();
                Sync();
            }

            public int SyncRead(long position, byte[] bytes, int bytesToRead)
            {
                return Read(position, bytes, bytesToRead);
            }

            public void Close()
            {
				if (_fileStream != null)
				{
					_fileStream.Close();
					RaiseOnCloseEvent();
				}
            }

	       	#endregion

            private void Seek(long position)
            {
                if (DTrace.enabled)
                {
                    DTrace.RegularSeek.Log(position);
                }
                _fileStream.Seek(position, SeekOrigin.Begin);
            }

			private void RaiseOnCloseEvent()
			{
				Action<object, EventArgs> onClose = OnClose;
				if (onClose != null)
				{
					onClose(this, EventArgs.Empty);
				}
			}

			internal event Action<object, EventArgs> OnClose;
		}
}

#endif
