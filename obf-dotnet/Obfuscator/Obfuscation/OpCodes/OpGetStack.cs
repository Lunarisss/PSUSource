﻿using System;
using Obfuscator.Bytecode.IR;

namespace Obfuscator.Obfuscation.OpCodes
{
	public class OpGetStack : VOpCode
	{
		public override bool IsInstruction(Instruction Instruction)
		{
			return Instruction.OpCode == OpCode.OpGetStack;
		}

		public override string GetObfuscated(ObfuscationContext ObfuscationContext)
		{
			return "Stack[Instruction[OP_A]] = Stack;";
		}
	}
}
