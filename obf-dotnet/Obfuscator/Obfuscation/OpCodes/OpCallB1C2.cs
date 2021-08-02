using System;
using Obfuscator.Bytecode.IR;

namespace Obfuscator.Obfuscation.OpCodes
{
	public class OpCallB1C2 : VOpCode
	{
		public override bool IsInstruction(Instruction Instruction)
		{
			return Instruction.OpCode == OpCode.OpCall && Instruction.B == 1 && Instruction.C == 2;
		}

		public override string GetObfuscated(ObfuscationContext ObfuscationContext)
		{
			return "local A = Instruction[OP_A]; Stack[A] = Stack[A]();";
		}

		public override void Mutate(Instruction Instruction)
		{
			Instruction.B = 0;
			Instruction.C = 0;
		}
	}
}
