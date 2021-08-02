using System;
using Obfuscator.Bytecode.IR;

namespace Obfuscator.Obfuscation.OpCodes
{
	public class OpSetGlobal : VOpCode
	{
		public override bool IsInstruction(Instruction Instruction)
		{
			return Instruction.OpCode == OpCode.OpSetGlobal;
		}

		public override string GetObfuscated(ObfuscationContext ObfuscationContext)
		{
			return "Environment[Constants[Instruction[OP_B]]] = Stack[Instruction[OP_A]];";
		}

		public override void Mutate(Instruction Instruction)
		{
			Instruction.ConstantType |= InstructionConstantType.RB;
		}
	}
}
