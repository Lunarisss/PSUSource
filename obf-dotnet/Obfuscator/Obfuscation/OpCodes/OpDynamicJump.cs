using System;
using Obfuscator.Bytecode.IR;

namespace Obfuscator.Obfuscation.OpCodes
{
	public class OpDynamicJump : VOpCode
	{
		public override bool IsInstruction(Instruction Instruction)
		{
			return Instruction.OpCode == OpCode.OpDynamicJump;
		}

		public override string GetObfuscated(ObfuscationContext ObfuscationContext)
		{
			return "InstructionPoint = Stack[Instruction[OP_A]];";
		}
	}
}
