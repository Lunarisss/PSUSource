using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Microsoft.CSharp.RuntimeBinder;
using Obfuscator.Bytecode.IR;
using Obfuscator.Obfuscation.OpCodes;

namespace Obfuscator.Obfuscation.Generation
{
	public class SuperOperators
	{
		private void ProcessInstructions(Chunk Chunk, Dictionary<Instruction, bool> IgnoredInstructions, List<VOpCode> Virtuals, int InstructionPoint = 0)
		{
			bool flag = Virtuals.Count > 128;
			if (!flag)
			{
				OpSuperOperator Virtual = new OpSuperOperator();
				Virtuals.Add(Virtual);
				while (InstructionPoint < Chunk.Instructions.Count - 1)
				{
					Instruction Instruction = Chunk.Instructions[InstructionPoint];
					bool flag2 = IgnoredInstructions.ContainsKey(Instruction);
					if (flag2)
					{
						bool flag3 = Virtual.Instructions.Count < 5;
						if (flag3)
						{
							Virtuals.Remove(Virtual);
						}
						while (InstructionPoint + 1 < Chunk.Instructions.Count)
						{
							InstructionPoint++;
							bool flag4 = !IgnoredInstructions.ContainsKey(Chunk.Instructions[InstructionPoint]);
							if (flag4)
							{
								break;
							}
						}
						bool flag5 = InstructionPoint + 2 < Chunk.Instructions.Count;
						if (flag5)
						{
							this.ProcessInstructions(Chunk, IgnoredInstructions, Virtuals, InstructionPoint + 1);
						}
						break;
					}
					Virtual.Instructions.Add(Instruction);
					Virtual.Virtuals.Add(Instruction.CustomInstructionData.OpCode);
					InstructionPoint++;
					bool flag6 = InstructionPoint >= Chunk.Instructions.Count - 1;
					if (flag6)
					{
						break;
					}
					bool flag7 = Virtual.Instructions.Count >= 50;
					if (flag7)
					{
						this.ProcessInstructions(Chunk, IgnoredInstructions, Virtuals, InstructionPoint);
						break;
					}
				}
			}
		}

		private void OptimizeInstructions(Chunk Chunk, List<VOpCode> Virtuals)
		{
			int InstructionPoint = 0;
			Dictionary<Instruction, bool> IgnoredInstructions = new Dictionary<Instruction, bool>();
			while (InstructionPoint < Chunk.Instructions.Count - 1)
			{
				Instruction Instruction = Chunk.Instructions[InstructionPoint];
				OpCode opCode = Instruction.OpCode;
				OpCode opCode2 = opCode;
				if (opCode2 != OpCode.OpLoadBool)
				{
					switch (opCode2)
					{
					case OpCode.OpJump:
					case OpCode.OpForLoop:
					case OpCode.OpForPrep:
						IgnoredInstructions[Instruction] = true;
						if (SuperOperators.<>o__1.<>p__0 == null)
						{
							SuperOperators.<>o__1.<>p__0 = CallSite<Func<CallSite, Dictionary<Instruction, bool>, object, bool, object>>.Create(Binder.SetIndex(CSharpBinderFlags.None, typeof(SuperOperators), new CSharpArgumentInfo[]
							{
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null),
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, null)
							}));
						}
						SuperOperators.<>o__1.<>p__0.Target(SuperOperators.<>o__1.<>p__0, IgnoredInstructions, Instruction.References[0], true);
						break;
					case OpCode.OpEq:
					case OpCode.OpLt:
					case OpCode.OpLe:
					case OpCode.OpTest:
					case OpCode.OpTestSet:
					case OpCode.OpTForLoop:
					case OpCode.OpSetList:
					case OpCode.OpClosure:
						goto IL_A0;
					case OpCode.Custom:
						IgnoredInstructions[Instruction] = true;
						break;
					case OpCode.OpDynamicJump:
						IgnoredInstructions[Instruction] = true;
						break;
					}
				}
				else if (Instruction.C != 0)
				{
					goto IL_A0;
				}
				IL_13D:
				bool flag = Instruction.BackReferences.Count > 0;
				if (flag)
				{
					IgnoredInstructions[Instruction] = true;
				}
				bool ignoreInstruction = Instruction.IgnoreInstruction;
				if (ignoreInstruction)
				{
					IgnoredInstructions[Instruction] = true;
				}
				InstructionPoint++;
				continue;
				IL_A0:
				IgnoredInstructions[Instruction] = true;
				goto IL_13D;
			}
			this.ProcessInstructions(Chunk, IgnoredInstructions, Virtuals, 0);
		}

		public void DoChunk(Chunk Chunk, List<VOpCode> Virtuals)
		{
			foreach (Chunk SubChunk in Chunk.Chunks)
			{
				this.DoChunk(SubChunk, Virtuals);
			}
			this.OptimizeInstructions(Chunk, Virtuals);
		}
	}
}
