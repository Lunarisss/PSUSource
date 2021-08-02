using System;
using System.Collections.Generic;
using System.Linq;
using Obfuscator.Bytecode.IR;
using Obfuscator.Extensions;

namespace Obfuscator.Obfuscation.Security
{
	public class InstructionSwap
	{
		public InstructionSwap(ObfuscationContext ObfuscationContext, Chunk HeadChunk)
		{
			this.ObfuscationContext = ObfuscationContext;
			this.HeadChunk = HeadChunk;
		}

		public void DoChunk(Chunk Chunk)
		{
			Chunk.UpdateMappings();
			foreach (Chunk SubChunk in Chunk.Chunks)
			{
				this.DoChunk(SubChunk);
			}
			List<Instruction> Instructions = Chunk.Instructions.ToList<Instruction>();
			int InstructionPoint = 0;
			List<Instruction> IgnoredInstructions = new List<Instruction>();
			while (InstructionPoint < Instructions.Count)
			{
				Instruction Instruction = Instructions[InstructionPoint];
				InstructionPoint++;
				OpCode opCode = Instruction.OpCode;
				OpCode opCode2 = opCode;
				if (opCode2 - OpCode.OpJump > 5 && opCode2 - OpCode.OpForLoop > 2)
				{
					bool flag = Instruction.IsJump || Instruction.BackReferences.Count > 0 || InstructionPoint == 1 || InstructionPoint == Chunk.Instructions.Count;
					if (flag)
					{
						IgnoredInstructions.Add(Instruction);
					}
				}
				else
				{
					IgnoredInstructions.Add(Instruction);
				}
			}
			List<Instruction> Swapped = new List<Instruction>();
			foreach (Instruction Instruction2 in Instructions)
			{
				bool flag2 = !IgnoredInstructions.Contains(Instruction2);
				if (flag2)
				{
					Instruction PreviousInstruction = Chunk.Instructions[Chunk.InstructionMap[Instruction2] - 1];
					Instruction NextInstruction = Chunk.Instructions[Chunk.InstructionMap[Instruction2] + 1];
					bool flag3 = IgnoredInstructions.Contains(PreviousInstruction);
					if (!flag3)
					{
						bool flag4 = IgnoredInstructions.Contains(NextInstruction);
						if (!flag4)
						{
							PreviousInstruction.IsJump = true;
							PreviousInstruction.JumpTo = Instruction2;
							Instruction2.BackReferences.Add(PreviousInstruction);
							Instruction2.IsJump = true;
							Instruction2.JumpTo = NextInstruction;
							NextInstruction.BackReferences.Add(Instruction2);
							Swapped.Add(Instruction2);
							IgnoredInstructions.Add(Instruction2);
							IgnoredInstructions.Add(PreviousInstruction);
							IgnoredInstructions.Add(NextInstruction);
						}
					}
				}
			}
			Swapped.Shuffle<Instruction>();
			foreach (Instruction Instruction3 in Swapped)
			{
				Chunk.Instructions.Remove(Instruction3);
			}
			Chunk.Instructions.AddRange(Swapped);
			Chunk.UpdateMappings();
		}

		public void DoChunks()
		{
			this.DoChunk(this.HeadChunk);
		}

		private Random Random = new Random();

		public ObfuscationContext ObfuscationContext;

		public Chunk HeadChunk;
	}
}
