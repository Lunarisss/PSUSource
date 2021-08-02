using System;
using Obfuscator.Bytecode.IR;

namespace Obfuscator.Obfuscation.OpCodes
{
	public class OpSetList : VOpCode
	{
		public override bool IsInstruction(Instruction Instruction)
		{
			return Instruction.OpCode == OpCode.OpSetList && Instruction.B != 0 && Instruction.C != 0;
		}

		public override string GetObfuscated(ObfuscationContext ObfuscationContext)
		{
			return "local A = Instruction[OP_A]; local B = Instruction[OP_B]; local Offset = 50 * (Instruction[OP_C] - 1); local T = Stack[A]; local Count = 0; for Index = A + 1, B do T[Offset + Count + 1] = Stack[A + (Index - A)]; Count = Count + 1; end;";
		}

		public override void Mutate(Instruction Instruction)
		{
			Instruction.B += Instruction.A;
		}
	}
}
