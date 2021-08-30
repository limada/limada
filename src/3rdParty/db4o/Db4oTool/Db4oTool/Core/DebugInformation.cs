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
using System.Collections.Generic;
using System.IO;
using Mono.Cecil.Cil;
using Mono.Collections.Generic;

namespace Db4oTool.Core
{
	public class DebugInformation
	{
		public static string InstructionInformationFor(Instruction instruction, Collection<Instruction> containingInstructions)
		{
			SequencePoint sp = FindSequencePointRelatedTo(instruction, containingInstructions);
			return FormatSourceInformation(sp, instruction);
		}

		private static SequencePoint FindSequencePointRelatedTo(Instruction instruction, IEnumerable<Instruction> containingInstructions)
		{
			SequencePoint sp = null;
			foreach (Instruction current in containingInstructions)
			{
				if (current.SequencePoint != null)
				{
					sp = current.SequencePoint;
				}

				if (current == instruction)
				{
					break;
				}
			}
			return sp;
		}

		private static string FormatSourceInformation(SequencePoint sp, Instruction instruction)
		{
			Func<SequencePoint, string> sequencePointFormater = delegate(SequencePoint s)
			                                                    	{
																		return string.Format("{0} ({1}, {2}) (IL instruction: {3} {4})", s.Document.Url, s.StartLine, s.StartColumn, instruction.OpCode, instruction.Operand);
																	};

			if (sp == null) return string.Format("{0} {1}", instruction.OpCode, instruction.Operand);

			if (!File.Exists(sp.Document.Url))
			{
				return sequencePointFormater(sp);
			}

			string lineContent = ReadSourceLineFor(sp);
			return lineContent != null
					? string.Format("{0} ({1}, {2}) : {3}", sp.Document.Url, sp.StartLine, sp.StartColumn, lineContent.Substring(sp.StartColumn - 1, sp.EndColumn - sp.StartColumn - 1))
					: sequencePointFormater(sp);
		}

		private static string ReadSourceLineFor(SequencePoint sp)
		{
			string lineContent = string.Empty;
			
			using (FileStream fileStream = new FileStream(sp.Document.Url, FileMode.Open))
			using (TextReader reader = new StreamReader(fileStream))
			{
				//For some reason, sometimes StartLine holds an invalid line number.
				//in this case we are going to handle as if the source file is not available.                
				for (int i = 0; i < sp.StartLine && lineContent != null; i++)
				{
					lineContent = reader.ReadLine();
				}

			}
			
			return lineContent;
		}
	}
}
