using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Microsoft.CSharp.RuntimeBinder;
using Obfuscator.Bytecode.IR;

namespace Obfuscator.Obfuscation.OpCodes
{
	public class OpForPrep : VOpCode
	{
		public override bool IsInstruction(Instruction Instruction)
		{
			return Instruction.OpCode == OpCode.OpForPrep;
		}

		public override string GetObfuscated(ObfuscationContext ObfuscationContext)
		{
			return "local A = Instruction[OP_A]; Stack[A] = 0 + (Stack[A]); Stack[A + 1] = 0 + (Stack[A + 1]); Stack[A + 2] = 0 + (Stack[A + 2]); local Index = Stack[A]; local Step = Stack[A + 2]; if (Step > 0) then if (Index > Stack[A + 1]) then InstructionPoint = Instruction[OP_B]; else Stack[A + 3] = Index; end; elseif (Index < Stack[A + 1]) then InstructionPoint = Instruction[OP_B]; else Stack[A + 3] = Index; end;";
		}

		public override void Mutate(Instruction Instruction)
		{
			if (OpForPrep.<>o__2.<>p__1 == null)
			{
				OpForPrep.<>o__2.<>p__1 = CallSite<Func<CallSite, object, int>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(int), typeof(OpForPrep)));
			}
			Func<CallSite, object, int> target = OpForPrep.<>o__2.<>p__1.Target;
			CallSite <>p__ = OpForPrep.<>o__2.<>p__1;
			if (OpForPrep.<>o__2.<>p__0 == null)
			{
				OpForPrep.<>o__2.<>p__0 = CallSite<Func<CallSite, Dictionary<Instruction, int>, object, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(OpForPrep), new CSharpArgumentInfo[]
				{
					CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null),
					CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
				}));
			}
			Instruction.B = target(<>p__, OpForPrep.<>o__2.<>p__0.Target(OpForPrep.<>o__2.<>p__0, Instruction.Chunk.InstructionMap, Instruction.References[0]));
		}
	}
}
