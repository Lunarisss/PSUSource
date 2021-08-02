using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Microsoft.CSharp.RuntimeBinder;
using Obfuscator.Bytecode.IR;
using Obfuscator.Extensions;
using Obfuscator.Obfuscation.OpCodes;

namespace Obfuscator.Obfuscation.Security
{
	public class TestSpam
	{
		public TestSpam(ObfuscationContext ObfuscationContext, Chunk Chunk, List<Instruction> Instructions, List<VOpCode> Virtuals, int Start, int End)
		{
			this.ObfuscationContext = ObfuscationContext;
			this.Chunk = Chunk;
			this.Instructions = Instructions;
			this.Virtuals = Virtuals;
			this.Start = Start;
			this.End = End;
		}

		public void DoInstructions()
		{
			List<BasicBlock> BasicBlocks = new BasicBlock().GenerateBasicBlocksFromInstructions(this.Chunk, this.Instructions);
			List<Instruction> Additions = new List<Instruction>();
			foreach (BasicBlock Block in BasicBlocks)
			{
				Instruction Last = Block.Instructions.Last<Instruction>();
				OpCode opCode = Last.OpCode;
				OpCode opCode2 = opCode;
				if (opCode2 - OpCode.OpEq <= 3)
				{
					Instruction PreviousTrue = Last;
					Instruction PreviousFalse = Last;
					Instruction T = Last.JumpTo;
					if (TestSpam.<>o__8.<>p__0 == null)
					{
						TestSpam.<>o__8.<>p__0 = CallSite<Func<CallSite, object, Instruction>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(Instruction), typeof(TestSpam)));
					}
					Instruction F = TestSpam.<>o__8.<>p__0.Target(TestSpam.<>o__8.<>p__0, Last.References[2]);
					for (int I = 0; I < this.Random.Next(5, 10); I++)
					{
						Instruction IsTrue = new Instruction(PreviousTrue);
						Instruction IsFalse = new Instruction(PreviousFalse);
						PreviousTrue.JumpTo = IsTrue;
						PreviousTrue.IsJump = true;
						Additions.Add(IsTrue);
						PreviousTrue = IsTrue;
						IsTrue.IsJump = true;
						IsTrue.JumpTo = T;
						IsTrue.References[2] = IsTrue;
						PreviousFalse.References[2] = IsFalse;
						Additions.Add(IsFalse);
						PreviousFalse = IsFalse;
						IsFalse.IsJump = true;
						IsFalse.JumpTo = IsFalse;
						IsFalse.References[2] = F;
					}
					Instruction JumpTrue = new Instruction(this.Chunk, OpCode.OpJump, new object[]
					{
						T
					});
					PreviousTrue.JumpTo = JumpTrue;
					PreviousTrue.IsJump = true;
					Additions.Add(JumpTrue);
					Instruction JumpFalse = new Instruction(this.Chunk, OpCode.OpJump, new object[]
					{
						F
					});
					PreviousFalse.References[2] = JumpFalse;
					Additions.Add(JumpFalse);
					for (int I2 = 0; I2 < this.Random.Next(5, 10); I2++)
					{
						Instruction NextJumpTrue = new Instruction(this.Chunk, OpCode.OpJump, new object[]
						{
							T
						});
						Instruction NextJumpFalse = new Instruction(this.Chunk, OpCode.OpJump, new object[]
						{
							F
						});
						JumpTrue.References[0] = NextJumpTrue;
						JumpFalse.References[0] = NextJumpFalse;
						JumpTrue = NextJumpTrue;
						JumpFalse = NextJumpFalse;
						Additions.Add(JumpTrue);
						Additions.Add(JumpFalse);
					}
				}
			}
			List<BasicBlock> AllowedBlocks = new List<BasicBlock>();
			foreach (BasicBlock Block2 in BasicBlocks)
			{
				OpCode opCode3 = Block2.Instructions.Last<Instruction>().OpCode;
				OpCode opCode4 = opCode3;
				if (opCode4 - OpCode.OpJump <= 5 || opCode4 - OpCode.OpReturn <= 3)
				{
					AllowedBlocks.Add(Block2);
				}
			}
			Additions.Shuffle<Instruction>();
			foreach (Instruction Instruction in Additions)
			{
				AllowedBlocks.Random<BasicBlock>().Instructions.Add(Instruction);
			}
			List<Instruction> Before = this.Chunk.Instructions.Take(this.Start).ToList<Instruction>();
			List<Instruction> After = this.Chunk.Instructions.Skip(this.End).ToList<Instruction>();
			this.Chunk.Instructions.Clear();
			this.Chunk.Instructions.AddRange(Before);
			foreach (BasicBlock Block3 in BasicBlocks)
			{
				foreach (Instruction Instruction2 in Block3.Instructions)
				{
					this.Chunk.Instructions.Add(Instruction2);
				}
			}
			this.Chunk.Instructions.AddRange(After);
			this.Chunk.UpdateMappings();
			this.End = this.Chunk.InstructionMap[BasicBlocks.Last<BasicBlock>().Instructions.Last<Instruction>()];
			List<Instruction> IList = this.Chunk.Instructions.Skip(this.Start).Take(this.End - this.Start).ToList<Instruction>();
		}

		private Random Random = new Random();

		private Chunk Chunk;

		private List<Instruction> Instructions;

		private List<VOpCode> Virtuals;

		private ObfuscationContext ObfuscationContext;

		private int Start;

		private int End;
	}
}
