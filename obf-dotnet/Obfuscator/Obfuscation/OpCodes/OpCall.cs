using System;
using Obfuscator.Bytecode.IR;

namespace Obfuscator.Obfuscation.OpCodes
{
	public class OpCall : VOpCode
	{
		public override bool IsInstruction(Instruction Instruction)
		{
			return Instruction.OpCode == OpCode.OpCall && Instruction.B > 2 && Instruction.C > 2;
		}

		public override string GetObfuscated(ObfuscationContext ObfuscationContext)
		{
			return "local A = Instruction[OP_A]; local Results = ({ Stack[A](UnPack(Stack, A + 1, Instruction[OP_B])) }); local Limit = Instruction[OP_C]; local K = 0; for I = A, Limit, 1 do K = K + 1; Stack[I] = Results[K]; end; for I = Limit + 1, StackSize do Stack[I] = nil; end;";
		}

		public override void Mutate(Instruction Instruction)
		{
			Instruction.B += Instruction.A - 1;
			Instruction.C += Instruction.A - 2;
		}
	}
}
