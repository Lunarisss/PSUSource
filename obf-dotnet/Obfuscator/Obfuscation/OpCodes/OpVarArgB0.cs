using System;
using Obfuscator.Bytecode.IR;

namespace Obfuscator.Obfuscation.OpCodes
{
	public class OpVarArgB0 : VOpCode
	{
		public override bool IsInstruction(Instruction Instruction)
		{
			return Instruction.OpCode == OpCode.OpVarArg && Instruction.B == 0;
		}

		public override string GetObfuscated(ObfuscationContext ObfuscationContext)
		{
			return "local A = Instruction[OP_A]; Top = A + VarArgs - 1; for Index = 0, VarArgs do Stack[A + Index] = VarArg[Index]; end; for I = Top + 1, StackSize do Stack[I] = nil; end;";
		}
	}
}
