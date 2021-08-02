using System;
using Obfuscator.Bytecode.IR;

namespace Obfuscator.Obfuscation.OpCodes
{
	public class OpCallC2 : VOpCode
	{
		public override bool IsInstruction(Instruction Instruction)
		{
			return Instruction.OpCode == OpCode.OpCall && Instruction.B > 2 && Instruction.C == 2;
		}

		public override string GetObfuscated(ObfuscationContext ObfuscationContext)
		{
			return "local A = Instruction[OP_A]; Stack[A] = Stack[A](UnPack(Stack, A + 1, Instruction[OP_B])); for I = A + 1, StackSize do Stack[I] = nil; end;";
		}

		public override void Mutate(Instruction Instruction)
		{
			Instruction.B += Instruction.A - 1;
			Instruction.C = 0;
		}
	}
}
