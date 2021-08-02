using System;
using Obfuscator.Bytecode.IR;

namespace Obfuscator.Obfuscation.OpCodes
{
	public class OpCustom : VOpCode
	{
		public override bool IsInstruction(Instruction Instruction)
		{
			return false;
		}

		public override string GetObfuscated(ObfuscationContext ObfuscationContext)
		{
			return this.Obfuscated;
		}

		public string Obfuscated = "";
	}
}
