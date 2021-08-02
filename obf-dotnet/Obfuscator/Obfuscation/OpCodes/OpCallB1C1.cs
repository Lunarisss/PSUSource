using System;
using Obfuscator.Bytecode.IR;

namespace Obfuscator.Obfuscation.OpCodes
{
	public class OpCallB1C1 : VOpCode
	{
		public override bool IsInstruction(Instruction Instruction)
		{
			return Instruction.OpCode == OpCode.OpCall && Instruction.B == 1 && Instruction.C == 1;
		}

		public override string GetObfuscated(ObfuscationContext ObfuscationContext)
		{
			return "Stack[Instruction[OP_A]]();";
		}

		public override void Mutate(Instruction Instruction)
		{
			Instruction.B = 0;
			Instruction.C = 0;
		}
	}
}
