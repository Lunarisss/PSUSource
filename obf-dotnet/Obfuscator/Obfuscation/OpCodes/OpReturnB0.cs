using System;
using Obfuscator.Bytecode.IR;

namespace Obfuscator.Obfuscation.OpCodes
{
	public class OpReturnB0 : VOpCode
	{
		public override bool IsInstruction(Instruction Instruction)
		{
			return Instruction.OpCode == OpCode.OpReturn && Instruction.B == 0;
		}

		public override string GetObfuscated(ObfuscationContext ObfuscationContext)
		{
			return "local A = Instruction[OP_A]; do return UnPack(Stack, A, Top); end;";
		}

		public override void Mutate(Instruction Instruction)
		{
			Instruction.B = 0;
		}
	}
}
