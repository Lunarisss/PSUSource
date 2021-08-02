using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Microsoft.CSharp.RuntimeBinder;
using Obfuscator.Bytecode.IR;

namespace Obfuscator.Obfuscation.OpCodes
{
	public class OpTestSetC : VOpCode
	{
		public override bool IsInstruction(Instruction Instruction)
		{
			return Instruction.OpCode == OpCode.OpTestSet && Instruction.C != 0;
		}

		public override string GetObfuscated(ObfuscationContext ObfuscationContext)
		{
			return "local B = Stack[Instruction[OP_C]]; if (B) then Stack[Instruction[OP_A]] = B; InstructionPoint = Instruction[OP_B]; end;";
		}

		public override void Mutate(Instruction Instruction)
		{
			Instruction.C = Instruction.B;
			if (OpTestSetC.<>o__2.<>p__1 == null)
			{
				OpTestSetC.<>o__2.<>p__1 = CallSite<Func<CallSite, object, int>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(int), typeof(OpTestSetC)));
			}
			Func<CallSite, object, int> target = OpTestSetC.<>o__2.<>p__1.Target;
			CallSite <>p__ = OpTestSetC.<>o__2.<>p__1;
			if (OpTestSetC.<>o__2.<>p__0 == null)
			{
				OpTestSetC.<>o__2.<>p__0 = CallSite<Func<CallSite, Dictionary<Instruction, int>, object, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(OpTestSetC), new CSharpArgumentInfo[]
				{
					CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null),
					CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
				}));
			}
			Instruction.B = target(<>p__, OpTestSetC.<>o__2.<>p__0.Target(OpTestSetC.<>o__2.<>p__0, Instruction.Chunk.InstructionMap, Instruction.References[2]));
			Instruction.InstructionType = InstructionType.AsBxC;
		}
	}
}
