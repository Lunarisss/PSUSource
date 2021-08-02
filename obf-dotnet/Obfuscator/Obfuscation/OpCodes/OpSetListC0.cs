using System;
using Obfuscator.Bytecode.IR;

namespace Obfuscator.Obfuscation.OpCodes
{
	public class OpSetListC0 : VOpCode
	{
		public override bool IsInstruction(Instruction Instruction)
		{
			return Instruction.OpCode == OpCode.OpSetList && Instruction.C == 0;
		}

		public override string GetObfuscated(ObfuscationContext ObfuscationContext)
		{
			return "";
		}

		public override void Mutate(Instruction Instruction)
		{
		}
	}
}
