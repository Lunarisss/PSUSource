using System;
using Obfuscator.Bytecode.IR;

namespace Obfuscator.Obfuscation.OpCodes
{
	public class OpReturnB2 : VOpCode
	{
		public override bool IsInstruction(Instruction Instruction)
		{
			return Instruction.OpCode == OpCode.OpReturn && Instruction.B == 2;
		}

		public override string GetObfuscated(ObfuscationContext ObfuscationContext)
		{
			return "do return (Stack[Instruction[OP_A]]); end;";
		}

		public override void Mutate(Instruction Instruction)
		{
			Instruction.B = 0;
		}
	}
}
