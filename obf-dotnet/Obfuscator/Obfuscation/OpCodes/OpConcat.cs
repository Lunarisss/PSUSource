using System;
using Obfuscator.Bytecode.IR;

namespace Obfuscator.Obfuscation.OpCodes
{
	public class OpConcat : VOpCode
	{
		public override bool IsInstruction(Instruction Instruction)
		{
			return Instruction.OpCode == OpCode.OpConcat;
		}

		public override string GetObfuscated(ObfuscationContext ObfuscationContext)
		{
			return "local B = Instruction[OP_B]; local Result = Stack[B]; for Index = B + 1, Instruction[OP_C] do Result = Result .. Stack[Index]; end; Stack[Instruction[OP_A]] = Result;";
		}
	}
}
