using System;
using Obfuscator.Bytecode.IR;

namespace Obfuscator.Obfuscation.OpCodes
{
	public class OpSetUpValue : VOpCode
	{
		public override bool IsInstruction(Instruction Instruction)
		{
			return Instruction.OpCode == OpCode.OpSetUpValue;
		}

		public override string GetObfuscated(ObfuscationContext ObfuscationContext)
		{
			return "UpValues[Instruction[OP_B]] = Stack[Instruction[OP_A]];";
		}
	}
}
