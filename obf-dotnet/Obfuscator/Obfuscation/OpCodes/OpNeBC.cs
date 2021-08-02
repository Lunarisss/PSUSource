﻿using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Microsoft.CSharp.RuntimeBinder;
using Obfuscator.Bytecode.IR;

namespace Obfuscator.Obfuscation.OpCodes
{
	public class OpNeBC : VOpCode
	{
		public override bool IsInstruction(Instruction Instruction)
		{
			return Instruction.OpCode == OpCode.OpEq && Instruction.A != 0 && Instruction.IsConstantB && Instruction.IsConstantC;
		}

		public override string GetObfuscated(ObfuscationContext ObfuscationContext)
		{
			return "if (Constants[Instruction[OP_A]] == Constants[Instruction[OP_C]]) then InstructionPoint = Instruction[OP_B]; end;";
		}

		public override void Mutate(Instruction Instruction)
		{
			Instruction.IsConstantA = true;
			Instruction.IsConstantB = false;
			Instruction.IsConstantC = true;
			Instruction.A = Instruction.B;
			if (OpNeBC.<>o__2.<>p__1 == null)
			{
				OpNeBC.<>o__2.<>p__1 = CallSite<Func<CallSite, object, int>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(int), typeof(OpNeBC)));
			}
			Func<CallSite, object, int> target = OpNeBC.<>o__2.<>p__1.Target;
			CallSite <>p__ = OpNeBC.<>o__2.<>p__1;
			if (OpNeBC.<>o__2.<>p__0 == null)
			{
				OpNeBC.<>o__2.<>p__0 = CallSite<Func<CallSite, Dictionary<Instruction, int>, object, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(OpNeBC), new CSharpArgumentInfo[]
				{
					CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null),
					CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
				}));
			}
			Instruction.B = target(<>p__, OpNeBC.<>o__2.<>p__0.Target(OpNeBC.<>o__2.<>p__0, Instruction.Chunk.InstructionMap, Instruction.References[2]));
			Instruction.InstructionType = InstructionType.AsBxC;
			Instruction.ConstantType |= (InstructionConstantType)40;
		}
	}
}