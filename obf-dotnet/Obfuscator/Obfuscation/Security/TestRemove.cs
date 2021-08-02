using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Microsoft.CSharp.RuntimeBinder;
using Obfuscator.Bytecode.IR;
using Obfuscator.Obfuscation.OpCodes;
using Obfuscator.Utility;

namespace Obfuscator.Obfuscation.Security
{
	public class TestRemove
	{
		public TestRemove(ObfuscationContext ObfuscationContext, Chunk Chunk, List<Instruction> Instructions, List<VOpCode> Virtuals, int Start, int End)
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
			int TrueIndex;
			Constant True = Utility.GetOrAddConstant(this.Chunk, ConstantType.Boolean, true, out TrueIndex);
			int FalseIndex;
			Constant False = Utility.GetOrAddConstant(this.Chunk, ConstantType.Boolean, false, out FalseIndex);
			foreach (BasicBlock Block in BasicBlocks)
			{
				for (int InstructionPoint = 0; InstructionPoint < Block.Instructions.Count; InstructionPoint++)
				{
					Instruction Instruction = Block.Instructions[InstructionPoint];
					bool flag = Instruction.OpCode == OpCode.OpTest;
					if (flag)
					{
						bool flag2 = Instruction.C == 1;
						if (flag2)
						{
							if (TestRemove.<>o__8.<>p__0 == null)
							{
								TestRemove.<>o__8.<>p__0 = CallSite<Func<CallSite, object, Instruction>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(Instruction), typeof(TestRemove)));
							}
							Instruction L = TestRemove.<>o__8.<>p__0.Target(TestRemove.<>o__8.<>p__0, Instruction.References[2]);
							Instruction R = Instruction.JumpTo;
							L.BackReferences.Remove(Instruction);
							R.BackReferences.Remove(Instruction);
							Instruction.OpCode = OpCode.OpNot;
							Instruction.B = Instruction.A;
							Instruction.A = (int)(this.Chunk.StackSize + 1);
							Instruction.C = 0;
							Instruction.IsJump = false;
							Instruction.JumpTo = null;
							Block.Instructions.Insert(Block.Instructions.IndexOf(Instruction) + 1, new Instruction(this.Chunk, OpCode.OpNot, Array.Empty<object>())
							{
								A = (int)(this.Chunk.StackSize + 1),
								B = (int)(this.Chunk.StackSize + 1)
							});
							Block.Instructions.Insert(Block.Instructions.IndexOf(Instruction) + 2, new Instruction(this.Chunk, OpCode.OpNewTable, Array.Empty<object>())
							{
								A = (int)(this.Chunk.StackSize + 2)
							});
							Block.Instructions.Insert(Block.Instructions.IndexOf(Instruction) + 3, new Instruction(this.Chunk, OpCode.OpLoadJump, new object[]
							{
								L
							})
							{
								A = (int)(this.Chunk.StackSize + 3)
							});
							Block.Instructions.Insert(Block.Instructions.IndexOf(Instruction) + 4, new Instruction(this.Chunk, OpCode.OpLoadJump, new object[]
							{
								R
							})
							{
								A = (int)(this.Chunk.StackSize + 4)
							});
							Block.Instructions.Insert(Block.Instructions.IndexOf(Instruction) + 5, new Instruction(this.Chunk, OpCode.OpSetTable, new object[]
							{
								True
							})
							{
								A = (int)(this.Chunk.StackSize + 2),
								B = TrueIndex,
								C = (int)(this.Chunk.StackSize + 3),
								IsConstantB = true
							});
							Block.Instructions.Insert(Block.Instructions.IndexOf(Instruction) + 6, new Instruction(this.Chunk, OpCode.OpSetTable, new object[]
							{
								False
							})
							{
								A = (int)(this.Chunk.StackSize + 2),
								B = FalseIndex,
								C = (int)(this.Chunk.StackSize + 4),
								IsConstantB = true
							});
							Block.Instructions.Insert(Block.Instructions.IndexOf(Instruction) + 7, new Instruction(this.Chunk, OpCode.OpGetTable, Array.Empty<object>())
							{
								A = (int)(this.Chunk.StackSize + 1),
								B = (int)(this.Chunk.StackSize + 2),
								C = (int)(this.Chunk.StackSize + 1)
							});
							Block.Instructions.Insert(Block.Instructions.IndexOf(Instruction) + 8, new Instruction(this.Chunk, OpCode.OpDynamicJump, Array.Empty<object>())
							{
								A = (int)(this.Chunk.StackSize + 1)
							});
							InstructionPoint += 8;
						}
						else
						{
							if (TestRemove.<>o__8.<>p__1 == null)
							{
								TestRemove.<>o__8.<>p__1 = CallSite<Func<CallSite, object, Instruction>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(Instruction), typeof(TestRemove)));
							}
							Instruction L2 = TestRemove.<>o__8.<>p__1.Target(TestRemove.<>o__8.<>p__1, Instruction.References[2]);
							Instruction R2 = Instruction.JumpTo;
							L2.BackReferences.Remove(Instruction);
							R2.BackReferences.Remove(Instruction);
							Instruction.OpCode = OpCode.OpNot;
							Instruction.B = Instruction.A;
							Instruction.A = (int)(this.Chunk.StackSize + 1);
							Instruction.C = 0;
							Instruction.IsJump = false;
							Instruction.JumpTo = null;
							Block.Instructions.Insert(Block.Instructions.IndexOf(Instruction) + 1, new Instruction(this.Chunk, OpCode.OpNot, Array.Empty<object>())
							{
								A = (int)(this.Chunk.StackSize + 1),
								B = (int)(this.Chunk.StackSize + 1)
							});
							Block.Instructions.Insert(Block.Instructions.IndexOf(Instruction) + 2, new Instruction(this.Chunk, OpCode.OpNewTable, Array.Empty<object>())
							{
								A = (int)(this.Chunk.StackSize + 2)
							});
							Block.Instructions.Insert(Block.Instructions.IndexOf(Instruction) + 3, new Instruction(this.Chunk, OpCode.OpLoadJump, new object[]
							{
								L2
							})
							{
								A = (int)(this.Chunk.StackSize + 3)
							});
							Block.Instructions.Insert(Block.Instructions.IndexOf(Instruction) + 4, new Instruction(this.Chunk, OpCode.OpLoadJump, new object[]
							{
								R2
							})
							{
								A = (int)(this.Chunk.StackSize + 4)
							});
							Block.Instructions.Insert(Block.Instructions.IndexOf(Instruction) + 5, new Instruction(this.Chunk, OpCode.OpSetTable, new object[]
							{
								False
							})
							{
								A = (int)(this.Chunk.StackSize + 2),
								B = FalseIndex,
								C = (int)(this.Chunk.StackSize + 3),
								IsConstantB = true
							});
							Block.Instructions.Insert(Block.Instructions.IndexOf(Instruction) + 6, new Instruction(this.Chunk, OpCode.OpSetTable, new object[]
							{
								True
							})
							{
								A = (int)(this.Chunk.StackSize + 2),
								B = TrueIndex,
								C = (int)(this.Chunk.StackSize + 4),
								IsConstantB = true
							});
							Block.Instructions.Insert(Block.Instructions.IndexOf(Instruction) + 7, new Instruction(this.Chunk, OpCode.OpGetTable, Array.Empty<object>())
							{
								A = (int)(this.Chunk.StackSize + 1),
								B = (int)(this.Chunk.StackSize + 2),
								C = (int)(this.Chunk.StackSize + 1)
							});
							Block.Instructions.Insert(Block.Instructions.IndexOf(Instruction) + 8, new Instruction(this.Chunk, OpCode.OpDynamicJump, Array.Empty<object>())
							{
								A = (int)(this.Chunk.StackSize + 1)
							});
							InstructionPoint += 8;
						}
					}
				}
			}
			List<Instruction> Before = this.Chunk.Instructions.Take(this.Start).ToList<Instruction>();
			List<Instruction> After = this.Chunk.Instructions.Skip(this.End).ToList<Instruction>();
			this.Chunk.Instructions.Clear();
			this.Chunk.Instructions.AddRange(Before);
			foreach (BasicBlock Block2 in BasicBlocks)
			{
				foreach (Instruction Instruction2 in Block2.Instructions)
				{
					this.Chunk.Instructions.Add(Instruction2);
				}
			}
			this.Chunk.Instructions.AddRange(After);
			this.Chunk.UpdateMappings();
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
