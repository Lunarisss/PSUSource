using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Microsoft.CSharp.RuntimeBinder;
using Obfuscator.Bytecode.IR;

namespace Obfuscator.Obfuscation.OpCodes
{
	public class OpLtB : VOpCode
	{
		public override bool IsInstruction(Instruction Instruction)
		{
			return Instruction.OpCode == OpCode.OpLt && Instruction.A == 0 && Instruction.IsConstantB && !Instruction.IsConstantC;
		}

		public override string GetObfuscated(ObfuscationContext ObfuscationContext)
		{
			return "if (Constants[Instruction[OP_A]] >= Stack[Instruction[OP_C]]) then InstructionPoint = Instruction[OP_B]; end;";
		}

		public override void Mutate(Instruction Instruction)
		{
			Instruction.IsConstantA = true;
			Instruction.IsConstantB = false;
			Instruction.IsConstantC = false;
			Instruction.A = Instruction.B;
			if (OpLtB.<>o__2.<>p__1 == null)
			{
				OpLtB.<>o__2.<>p__1 = CallSite<Func<CallSite, object, int>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(int), typeof(OpLtB)));
			}
			Func<CallSite, object, int> target = OpLtB.<>o__2.<>p__1.Target;
			CallSite <>p__ = OpLtB.<>o__2.<>p__1;
			if (OpLtB.<>o__2.<>p__0 == null)
			{
				OpLtB.<>o__2.<>p__0 = CallSite<Func<CallSite, Dictionary<Instruction, int>, object, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(OpLtB), new CSharpArgumentInfo[]
				{
					CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null),
					CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
				}));
			}
			Instruction.B = target(<>p__, OpLtB.<>o__2.<>p__0.Target(OpLtB.<>o__2.<>p__0, Instruction.Chunk.InstructionMap, Instruction.References[2]));
			Instruction.InstructionType = InstructionType.AsBxC;
			Instruction.ConstantType |= InstructionConstantType.RA;
		}
	}
}
