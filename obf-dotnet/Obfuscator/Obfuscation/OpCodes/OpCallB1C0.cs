using System;
using Obfuscator.Bytecode.IR;

namespace Obfuscator.Obfuscation.OpCodes
{
	public class OpCallB1C0 : VOpCode
	{
		public override bool IsInstruction(Instruction Instruction)
		{
			return Instruction.OpCode == OpCode.OpCall && Instruction.B == 1 && Instruction.C == 0;
		}

		public override string GetObfuscated(ObfuscationContext ObfuscationContext)
		{
			return "local A = Instruction[OP_A]; local Results, Limit = _R(Stack[A]()); Top = Limit + A - 1; local K = 0; for I = A, Top do K = K + 1; Stack[I] = Results[K]; end; for I = Top + 1, StackSize do Stack[I] = nil; end;";
		}

		public override void Mutate(Instruction Instruction)
		{
			Instruction.B = 0;
			Instruction.C = 0;
		}
	}
}
