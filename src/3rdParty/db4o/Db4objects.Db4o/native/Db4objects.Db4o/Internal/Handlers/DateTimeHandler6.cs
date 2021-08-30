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
using Db4objects.Db4o.Marshall;

namespace Db4objects.Db4o.Internal.Handlers
{
    class DateTimeHandler6 : DateTimeHandler
    {
        protected override object ReadKind(IReadContext context, DateTime dateTime)
        {
            return dateTime;
        }

        protected override void WriteKind(IWriteContext context, DateTime dateTime)
        {
            // do nothing
        }

        protected override DateTime ReadKind (DateTime dateTime, byte[] bytes, int offset)
        {
            return dateTime;
        }

        protected override void WriteKind(DateTime dateTime, byte[] bytes, int offset)
        {
            // do nothing
        }
		//public override int LinkLength()
		//{
		//    return base.LinkLength() - Const4.LongLength;
		//}
    }
}
