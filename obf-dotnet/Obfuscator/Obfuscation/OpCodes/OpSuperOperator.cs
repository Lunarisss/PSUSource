using System;
using System.Collections.Generic;
using Obfuscator.Bytecode.IR;

namespace Obfuscator.Obfuscation.OpCodes
{
	public class OpSuperOperator : VOpCode
	{
		public override bool IsInstruction(Instruction Instruction)
		{
			return false;
		}

		public override string GetObfuscated(ObfuscationContext ObfuscationContext)
		{
			string Obfuscated = "";
			int InstructionPoint = 0;
			foreach (Instruction Instruction in this.Instructions)
			{
				VOpCode VirtualOpcode = this.Virtuals[InstructionPoint];
				Instruction.UpdateRegisters();
				bool flag = !Instruction.CustomInstructionData.Mutated;
				if (flag)
				{
					if (VirtualOpcode != null)
					{
						VirtualOpcode.Mutate(Instruction);
					}
				}
				Instruction.CustomInstructionData.Mutated = true;
				Obfuscated = Obfuscated + VirtualOpcode.GetObfuscated(ObfuscationContext) + " Instruction = Instruction[OP_E]; ";
				Instruction.WrittenVIndex = new int?(0);
				InstructionPoint++;
			}
			return Obfuscated + " Instruction = Instruction[OP_E]; ";
		}

		public List<Instruction> Instructions = new List<Instruction>();

		public List<VOpCode> Virtuals = new List<VOpCode>();

		public bool IsWritten = false;
	}
}
