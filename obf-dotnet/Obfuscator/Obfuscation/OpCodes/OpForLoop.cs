using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Microsoft.CSharp.RuntimeBinder;
using Obfuscator.Bytecode.IR;

namespace Obfuscator.Obfuscation.OpCodes
{
	public class OpForLoop : VOpCode
	{
		public override bool IsInstruction(Instruction Instruction)
		{
			return Instruction.OpCode == OpCode.OpForLoop;
		}

		public override string GetObfuscated(ObfuscationContext ObfuscationContext)
		{
			return "local A = Instruction[OP_A]; local Step = Stack[A + 2]; local Index = Stack[A] + Step; Stack[A] = Index; if (Step > 0) then if (Index <= Stack[A + 1]) then InstructionPoint = Instruction[OP_B]; Stack[A + 3] = Index; end; elseif (Index >= Stack[A+1]) then InstructionPoint = Instruction[OP_B]; Stack[A + 3] = Index; end;";
		}

		public override void Mutate(Instruction Instruction)
		{
			if (OpForLoop.<>o__2.<>p__1 == null)
			{
				OpForLoop.<>o__2.<>p__1 = CallSite<Func<CallSite, object, int>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(int), typeof(OpForLoop)));
			}
			Func<CallSite, object, int> target = OpForLoop.<>o__2.<>p__1.Target;
			CallSite <>p__ = OpForLoop.<>o__2.<>p__1;
			if (OpForLoop.<>o__2.<>p__0 == null)
			{
				OpForLoop.<>o__2.<>p__0 = CallSite<Func<CallSite, Dictionary<Instruction, int>, object, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(OpForLoop), new CSharpArgumentInfo[]
				{
					CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null),
					CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
				}));
			}
			Instruction.B = target(<>p__, OpForLoop.<>o__2.<>p__0.Target(OpForLoop.<>o__2.<>p__0, Instruction.Chunk.InstructionMap, Instruction.References[0]));
		}
	}
}
