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
using System.Text;
using Db4objects.Db4o.Internal.Encoding;

namespace Db4objects.Db4o.Internal
{
	internal class LegacyDb4oAssemblyNameMapper
	{
		static LegacyDb4oAssemblyNameMapper()
        {   
            LatinStringIO stringIO = new UnicodeStringIO();
            oldAssemblies = new byte[oldAssemblyNames.Length][];
            for (int i = 0; i < oldAssemblyNames.Length; i++)
            {
                oldAssemblies[i] = stringIO.Write(oldAssemblyNames[i]);
            }
        }

		internal byte[] MappedNameFor(byte[] nameBytes)
		{
			for (int i = 0; i < oldAssemblyNames.Length; i++)
			{
				byte[] assemblyName = oldAssemblies[i];

				int j = assemblyName.Length - 1;
				for (int k = nameBytes.Length - 1; k >= 0; k--)
				{
					if (nameBytes[k] != assemblyName[j])
					{
						break;
					}
					j--;
					if (j < 0)
					{
						return UpdateInternalClassName(nameBytes, i);
					}
				}
			}
			return nameBytes;
		}

		private static byte[] UpdateInternalClassName(byte[] bytes, int candidateMatchingAssemblyIndex)
		{
			UnicodeStringIO io = new UnicodeStringIO();
			string typeFQN = io.Read(bytes);

			string[] assemblyNameParts = typeFQN.Split(',');
			if (assemblyNameParts[1].Trim() != oldAssemblyNames[candidateMatchingAssemblyIndex])
			{
				return bytes;
			}

			string typeName = assemblyNameParts[0];
			return io.Write(FullyQualifiedNameFor(typeName).ToString());
		}

		private static StringBuilder FullyQualifiedNameFor(string typeName)
		{
			StringBuilder typeNameBuffer = new StringBuilder(typeName);
			ApplyNameSpaceRenamings(typeNameBuffer);
			typeNameBuffer.Append(", ");
			typeNameBuffer.Append(GetCurrentAssemblyName());
			return typeNameBuffer;
		}

		private static void ApplyNameSpaceRenamings(StringBuilder typeNameBuffer)
		{
			foreach (string[] renaming in NamespaceRenamings)
			{
				typeNameBuffer.Replace(renaming[0], renaming[1]);
			}
		}

		private static string GetCurrentAssemblyName()
		{
			return typeof(Platform4).Assembly.GetName().Name;
		}
		
		private static readonly string[] oldAssemblyNames = new string[] { "db4o-4.0-net1", "db4o-4.0-compact1" };
		private static readonly byte[][] oldAssemblies;

		private static readonly string[][] NamespaceRenamings = new string[][]
                {
                    new string[] { "com.db4o.ext", "Db4objects.Db4o.Ext"},
                    new string[] { "com.db4o.inside", "Db4objects.Db4o.Internal"},
                    new string[] { "com.db4o", "Db4objects.Db4o"}, 
                };

	}
}
