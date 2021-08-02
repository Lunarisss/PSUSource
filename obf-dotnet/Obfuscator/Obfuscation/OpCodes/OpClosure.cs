using System;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using Microsoft.CSharp.RuntimeBinder;
using Obfuscator.Bytecode.IR;

namespace Obfuscator.Obfuscation.OpCodes
{
	public class OpClosure : VOpCode
	{
		public override bool IsInstruction(Instruction Instruction)
		{
			if (OpClosure.<>o__0.<>p__3 == null)
			{
				OpClosure.<>o__0.<>p__3 = CallSite<Func<CallSite, object, bool>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(bool), typeof(OpClosure)));
			}
			Func<CallSite, object, bool> target = OpClosure.<>o__0.<>p__3.Target;
			CallSite <>p__ = OpClosure.<>o__0.<>p__3;
			bool flag = Instruction.OpCode == OpCode.OpClosure;
			object arg2;
			if (flag)
			{
				if (OpClosure.<>o__0.<>p__2 == null)
				{
					OpClosure.<>o__0.<>p__2 = CallSite<Func<CallSite, bool, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.BinaryOperationLogical, ExpressionType.And, typeof(OpClosure), new CSharpArgumentInfo[]
					{
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null),
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
					}));
				}
				Func<CallSite, bool, object, object> target2 = OpClosure.<>o__0.<>p__2.Target;
				CallSite <>p__2 = OpClosure.<>o__0.<>p__2;
				bool arg = flag;
				if (OpClosure.<>o__0.<>p__1 == null)
				{
					OpClosure.<>o__0.<>p__1 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.GreaterThan, typeof(OpClosure), new CSharpArgumentInfo[]
					{
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, null)
					}));
				}
				Func<CallSite, object, int, object> target3 = OpClosure.<>o__0.<>p__1.Target;
				CallSite <>p__3 = OpClosure.<>o__0.<>p__1;
				if (OpClosure.<>o__0.<>p__0 == null)
				{
					OpClosure.<>o__0.<>p__0 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "UpValueCount", typeof(OpClosure), new CSharpArgumentInfo[]
					{
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
					}));
				}
				arg2 = target2(<>p__2, arg, target3(<>p__3, OpClosure.<>o__0.<>p__0.Target(OpClosure.<>o__0.<>p__0, Instruction.References[0]), 0));
			}
			else
			{
				arg2 = flag;
			}
			return target(<>p__, arg2);
		}

		public override string GetObfuscated(ObfuscationContext ObfuscationContext)
		{
			return "local Function = Functions[Instruction[OP_B]]; local fUpValues = Instruction[OP_D]; local Indexes = {}; local nUpValues = SetMetaTable({}, { __index = function(_, Key) local UpValue = Indexes[Key]; return (UpValue[1][UpValue[2]]); end, __newindex = function(_, Key, Value) local UpValue = Indexes[Key]; UpValue[1][UpValue[2]] = Value; end; }); for Index = 1, Instruction[OP_C], 1 do local UpValue = fUpValues[Index]; if (UpValue[0] == 0) then Indexes[Index - 1] = ({ Stack, UpValue[1] }); else Indexes[Index - 1] = ({ UpValues, UpValue[1] }); end; lUpValues[#lUpValues + 1] = Indexes; end; Stack[Instruction[OP_A]] = Wrap(Function, nUpValues, Environment);";
		}

		public override void Mutate(Instruction Instruction)
		{
			Instruction.InstructionType = InstructionType.Closure;
			Instruction.C = (int)Instruction.Chunk.Chunks[Instruction.B].UpValueCount;
		}
	}
}
