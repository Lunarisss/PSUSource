﻿using System;
using Obfuscator.Bytecode.IR;

namespace Obfuscator.Obfuscation.OpCodes
{
	public class OpLoadBoolC : VOpCode
	{
		public override bool IsInstruction(Instruction Instruction)
		{
			return Instruction.OpCode == OpCode.OpLoadBool && Instruction.C != 0;
		}

		public override string GetObfuscated(ObfuscationContext ObfuscationContext)
		{
			return "Stack[Instruction[OP_A]] = (Instruction[OP_B] ~= 0);";
		}

		public override void Mutate(Instruction Instruction)
		{
			Instruction.C = 0;
		}
	}
}
