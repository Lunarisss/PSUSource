using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Microsoft.CSharp.RuntimeBinder;
using Obfuscator.Bytecode.IR;

namespace Obfuscator.Obfuscation.OpCodes
{
	public class OpLoadJump : VOpCode
	{
		public override bool IsInstruction(Instruction Instruction)
		{
			return Instruction.OpCode == OpCode.OpLoadJump;
		}

		public override string GetObfuscated(ObfuscationContext ObfuscationContext)
		{
			return "Stack[Instruction[OP_A]] = Instruction[OP_B];";
		}

		public override void Mutate(Instruction Instruction)
		{
			if (OpLoadJump.<>o__2.<>p__1 == null)
			{
				OpLoadJump.<>o__2.<>p__1 = CallSite<Func<CallSite, object, int>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(int), typeof(OpLoadJump)));
			}
			Func<CallSite, object, int> target = OpLoadJump.<>o__2.<>p__1.Target;
			CallSite <>p__ = OpLoadJump.<>o__2.<>p__1;
			if (OpLoadJump.<>o__2.<>p__0 == null)
			{
				OpLoadJump.<>o__2.<>p__0 = CallSite<Func<CallSite, Dictionary<Instruction, int>, object, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(OpLoadJump), new CSharpArgumentInfo[]
				{
					CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null),
					CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
				}));
			}
			Instruction.B = target(<>p__, OpLoadJump.<>o__2.<>p__0.Target(OpLoadJump.<>o__2.<>p__0, Instruction.Chunk.InstructionMap, Instruction.References[0]));
			Instruction.InstructionType = InstructionType.AsBx;
		}
	}
}
