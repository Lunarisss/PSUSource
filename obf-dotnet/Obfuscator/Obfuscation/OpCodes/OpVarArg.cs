using System;
using Obfuscator.Bytecode.IR;

namespace Obfuscator.Obfuscation.OpCodes
{
	public class OpVarArg : VOpCode
	{
		public override bool IsInstruction(Instruction Instruction)
		{
			return Instruction.OpCode == OpCode.OpVarArg && Instruction.B != 0;
		}

		public override string GetObfuscated(ObfuscationContext ObfuscationContext)
		{
			return "local A = Instruction[OP_A]; local B = Instruction[OP_B]; for Index = 1, B, 1 do Stack[A + Index - 1] = VarArg[Index - 1]; end;";
		}
	}
}
