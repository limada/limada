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
using System.IO;
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Internal;
using Sharpen.IO;
using File=Sharpen.IO.File;

namespace Db4objects.Db4o.IO
{
    public class RandomAccessFileFactory
    {
        public static RandomAccessFile NewRandomAccessFile(String path, bool readOnly, bool lockFile)
        {
            RandomAccessFile raf = null;
            bool ok = false;
            try
            {
                raf = new RandomAccessFile(path, readOnly, lockFile);
                if (lockFile)
                {
                    Platform4.LockFile(path, raf);
                }
                ok = true;
                return raf;
            }
            catch (IOException x)
            {
                if (new File(path).Exists())
                {
                    throw new DatabaseFileLockedException(path, x);
                }
                throw new Db4oIOException(x);
            } 
            finally
            {
                if(!ok && raf != null)
                {
                    raf.Close();
                }
            }
        }
    }
}
