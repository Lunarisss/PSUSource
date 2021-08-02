using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Microsoft.CSharp.RuntimeBinder;
using Obfuscator.Bytecode.IR;

namespace Obfuscator.Obfuscation.OpCodes
{
	public class OpTForLoop : VOpCode
	{
		public override bool IsInstruction(Instruction Instruction)
		{
			return Instruction.OpCode == OpCode.OpTForLoop;
		}

		public override string GetObfuscated(ObfuscationContext ObfuscationContext)
		{
			return "local A = Instruction[OP_A]; local C = Instruction[OP_C]; local D = A + 2; local Result = ({ Stack[A](Stack[A + 1], Stack[D]); }); for Index = 1, C do Stack[D + Index] = Result[Index]; end; local R = Result[1]; if (R) then Stack[D] = R; InstructionPoint = Instruction[OP_B]; end;";
		}

		public override void Mutate(Instruction Instruction)
		{
			if (OpTForLoop.<>o__2.<>p__1 == null)
			{
				OpTForLoop.<>o__2.<>p__1 = CallSite<Func<CallSite, object, int>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(int), typeof(OpTForLoop)));
			}
			Func<CallSite, object, int> target = OpTForLoop.<>o__2.<>p__1.Target;
			CallSite <>p__ = OpTForLoop.<>o__2.<>p__1;
			if (OpTForLoop.<>o__2.<>p__0 == null)
			{
				OpTForLoop.<>o__2.<>p__0 = CallSite<Func<CallSite, Dictionary<Instruction, int>, object, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(OpTForLoop), new CSharpArgumentInfo[]
				{
					CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null),
					CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
				}));
			}
			Instruction.B = target(<>p__, OpTForLoop.<>o__2.<>p__0.Target(OpTForLoop.<>o__2.<>p__0, Instruction.Chunk.InstructionMap, Instruction.References[2]));
			Instruction.InstructionType = InstructionType.AsBxC;
		}
	}
}
