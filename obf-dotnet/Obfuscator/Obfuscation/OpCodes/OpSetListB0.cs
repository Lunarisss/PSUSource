using System;
using Obfuscator.Bytecode.IR;

namespace Obfuscator.Obfuscation.OpCodes
{
	public class OpSetListB0 : VOpCode
	{
		public override bool IsInstruction(Instruction Instruction)
		{
			return Instruction.OpCode == OpCode.OpSetList && Instruction.B == 0 && Instruction.C != 0;
		}

		public override string GetObfuscated(ObfuscationContext ObfuscationContext)
		{
			return "local A = Instruction[OP_A]; local Table = Stack[A]; local Count, Offset = 0, 50 * (Instruction[OP_C] - 1); for Index = A + 1, Top, 1 do Table[Offset + Count + 1] = Stack[Index]; Count = Count + 1; end;";
		}
	}
}
