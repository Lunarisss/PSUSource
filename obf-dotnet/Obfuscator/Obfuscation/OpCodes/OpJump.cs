using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Microsoft.CSharp.RuntimeBinder;
using Obfuscator.Bytecode.IR;

namespace Obfuscator.Obfuscation.OpCodes
{
	public class OpJump : VOpCode
	{
		public override bool IsInstruction(Instruction Instruction)
		{
			return Instruction.OpCode == OpCode.OpJump;
		}

		public override string GetObfuscated(ObfuscationContext ObfuscationContext)
		{
			return "InstructionPoint = Instruction[OP_B];";
		}

		public override void Mutate(Instruction Instruction)
		{
			if (OpJump.<>o__2.<>p__1 == null)
			{
				OpJump.<>o__2.<>p__1 = CallSite<Func<CallSite, object, int>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(int), typeof(OpJump)));
			}
			Func<CallSite, object, int> target = OpJump.<>o__2.<>p__1.Target;
			CallSite <>p__ = OpJump.<>o__2.<>p__1;
			if (OpJump.<>o__2.<>p__0 == null)
			{
				OpJump.<>o__2.<>p__0 = CallSite<Func<CallSite, Dictionary<Instruction, int>, object, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(OpJump), new CSharpArgumentInfo[]
				{
					CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null),
					CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
				}));
			}
			Instruction.B = target(<>p__, OpJump.<>o__2.<>p__0.Target(OpJump.<>o__2.<>p__0, Instruction.Chunk.InstructionMap, Instruction.References[0]));
		}
	}
}
