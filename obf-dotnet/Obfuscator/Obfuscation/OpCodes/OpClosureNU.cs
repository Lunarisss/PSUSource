using System;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using Microsoft.CSharp.RuntimeBinder;
using Obfuscator.Bytecode.IR;

namespace Obfuscator.Obfuscation.OpCodes
{
	public class OpClosureNU : VOpCode
	{
		public override bool IsInstruction(Instruction Instruction)
		{
			if (OpClosureNU.<>o__0.<>p__3 == null)
			{
				OpClosureNU.<>o__0.<>p__3 = CallSite<Func<CallSite, object, bool>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(bool), typeof(OpClosureNU)));
			}
			Func<CallSite, object, bool> target = OpClosureNU.<>o__0.<>p__3.Target;
			CallSite <>p__ = OpClosureNU.<>o__0.<>p__3;
			bool flag = Instruction.OpCode == OpCode.OpClosure;
			object arg2;
			if (flag)
			{
				if (OpClosureNU.<>o__0.<>p__2 == null)
				{
					OpClosureNU.<>o__0.<>p__2 = CallSite<Func<CallSite, bool, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.BinaryOperationLogical, ExpressionType.And, typeof(OpClosureNU), new CSharpArgumentInfo[]
					{
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null),
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
					}));
				}
				Func<CallSite, bool, object, object> target2 = OpClosureNU.<>o__0.<>p__2.Target;
				CallSite <>p__2 = OpClosureNU.<>o__0.<>p__2;
				bool arg = flag;
				if (OpClosureNU.<>o__0.<>p__1 == null)
				{
					OpClosureNU.<>o__0.<>p__1 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Equal, typeof(OpClosureNU), new CSharpArgumentInfo[]
					{
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, null)
					}));
				}
				Func<CallSite, object, int, object> target3 = OpClosureNU.<>o__0.<>p__1.Target;
				CallSite <>p__3 = OpClosureNU.<>o__0.<>p__1;
				if (OpClosureNU.<>o__0.<>p__0 == null)
				{
					OpClosureNU.<>o__0.<>p__0 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "UpValueCount", typeof(OpClosureNU), new CSharpArgumentInfo[]
					{
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
					}));
				}
				arg2 = target2(<>p__2, arg, target3(<>p__3, OpClosureNU.<>o__0.<>p__0.Target(OpClosureNU.<>o__0.<>p__0, Instruction.References[0]), 0));
			}
			else
			{
				arg2 = flag;
			}
			return target(<>p__, arg2);
		}

		public override string GetObfuscated(ObfuscationContext ObfuscationContext)
		{
			return "Stack[Instruction[OP_A]] = Wrap(Functions[Instruction[OP_B]], (nil), Environment);";
		}
	}
}
