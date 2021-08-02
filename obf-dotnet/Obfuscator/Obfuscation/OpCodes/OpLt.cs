using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Microsoft.CSharp.RuntimeBinder;
using Obfuscator.Bytecode.IR;

namespace Obfuscator.Obfuscation.OpCodes
{
	public class OpLt : VOpCode
	{
		public override bool IsInstruction(Instruction Instruction)
		{
			return Instruction.OpCode == OpCode.OpLt && Instruction.A == 0 && !Instruction.IsConstantB && !Instruction.IsConstantC;
		}

		public override string GetObfuscated(ObfuscationContext ObfuscationContext)
		{
			return "if (Stack[Instruction[OP_A]] >= Stack[Instruction[OP_C]]) then InstructionPoint = Instruction[OP_B]; end;";
		}

		public override void Mutate(Instruction Instruction)
		{
			Instruction.IsConstantA = false;
			Instruction.IsConstantB = false;
			Instruction.IsConstantC = false;
			Instruction.A = Instruction.B;
			if (OpLt.<>o__2.<>p__1 == null)
			{
				OpLt.<>o__2.<>p__1 = CallSite<Func<CallSite, object, int>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(int), typeof(OpLt)));
			}
			Func<CallSite, object, int> target = OpLt.<>o__2.<>p__1.Target;
			CallSite <>p__ = OpLt.<>o__2.<>p__1;
			if (OpLt.<>o__2.<>p__0 == null)
			{
				OpLt.<>o__2.<>p__0 = CallSite<Func<CallSite, Dictionary<Instruction, int>, object, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(OpLt), new CSharpArgumentInfo[]
				{
					CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null),
					CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
				}));
			}
			Instruction.B = target(<>p__, OpLt.<>o__2.<>p__0.Target(OpLt.<>o__2.<>p__0, Instruction.Chunk.InstructionMap, Instruction.References[2]));
			Instruction.InstructionType = InstructionType.AsBxC;
		}
	}
}
