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
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Fileheader;

namespace Db4objects.Db4o.Internal.Fileheader
{
	/// <exclude></exclude>
	public class FileHeader1 : NewFileHeaderBase
	{
		private static readonly int TransactionPointerOffset = AccessTimeOffset + Const4.
			LongLength;

		private static readonly int BlocksizeOffset = TransactionPointerOffset + (Const4.
			IntLength * 2);

		public static readonly int HeaderLength = TransactionPointerOffset + (Const4.IntLength
			 * 6);

		// The header format is:
		// (byte) 'd'
		// (byte) 'b'
		// (byte) '4'
		// (byte) 'o'
		// (byte) headerVersion
		// (int) headerLock
		// (long) openTime
		// (long) accessTime
		// (int) Transaction pointer 1
		// (int) Transaction pointer 2
		// (int) blockSize
		// (int) classCollectionID
		// (int) freespaceID
		// (int) variablePartID
		public override int Length()
		{
			return HeaderLength;
		}

		protected override void Read(LocalObjectContainer file, ByteArrayBuffer reader)
		{
			NewTimerFileLock(file);
			OldEncryptionOff(file);
			CheckThreadFileLock(file, reader);
			reader.Seek(TransactionPointerOffset);
			file.SystemData().TransactionPointer1(reader.ReadInt());
			file.SystemData().TransactionPointer2(reader.ReadInt());
			reader.Seek(BlocksizeOffset);
			file.BlockSizeReadFromFile(reader.ReadInt());
			SystemData systemData = file.SystemData();
			systemData.ClassCollectionID(reader.ReadInt());
			reader.ReadInt();
			// was freespace ID, can no longer be read
			_variablePart = CreateVariablePart(file);
			int variablePartId = reader.ReadInt();
			_variablePart.Read(variablePartId, 0);
		}

		public override void WriteFixedPart(LocalObjectContainer file, bool startFileLockingThread
			, bool shuttingDown, StatefulBuffer writer, int blockSize)
		{
			throw new InvalidOperationException();
		}

		public override void WriteTransactionPointer(Transaction systemTransaction, int transactionPointer
			)
		{
			throw new InvalidOperationException();
		}

		protected override NewFileHeaderBase CreateNew()
		{
			return new FileHeader1();
		}

		protected override byte Version()
		{
			return (byte)1;
		}

		public override FileHeaderVariablePart CreateVariablePart(LocalObjectContainer file
			)
		{
			return new FileHeaderVariablePart1(file);
		}
	}
}
