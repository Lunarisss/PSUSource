using System;
using Obfuscator.Bytecode.IR;

namespace Obfuscator.Obfuscation.OpCodes
{
	public class OpCallB0C1 : VOpCode
	{
		public override bool IsInstruction(Instruction Instruction)
		{
			return Instruction.OpCode == OpCode.OpCall && Instruction.B == 0 && Instruction.C == 1;
		}

		public override string GetObfuscated(ObfuscationContext ObfuscationContext)
		{
			return "local A = Instruction[OP_A]; Stack[A](UnPack(Stack, A + 1, Top)); for I = A + 1, Top do Stack[I] = nil; end;";
		}

		public override void Mutate(Instruction Instruction)
		{
			Instruction.B = 0;
			Instruction.C = 0;
		}
	}
}
