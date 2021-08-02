using System;
using Obfuscator.Bytecode.IR;

namespace Obfuscator.Obfuscation.OpCodes
{
	public class OpTailCall : VOpCode
	{
		public override bool IsInstruction(Instruction Instruction)
		{
			return Instruction.OpCode == OpCode.OpTailCall && Instruction.B > 1;
		}

		public override string GetObfuscated(ObfuscationContext ObfuscationContext)
		{
			return "local A = Instruction[OP_A]; do return Stack[A](UnPack(Stack, A + 1, Instruction[OP_B])) end;";
		}

		public override void Mutate(Instruction Instruction)
		{
			Instruction.B += Instruction.A - 1;
		}
	}
}
