using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using Microsoft.CSharp.RuntimeBinder;
using Obfuscator.Bytecode.IR;
using Obfuscator.Obfuscation.OpCodes;
using Obfuscator.Utility;

namespace Obfuscator.Obfuscation.Generation.Macros
{
	public class LuaMacros
	{
		public LuaMacros(Chunk HeadChunk, List<VOpCode> Virtuals)
		{
			this.HeadChunk = HeadChunk;
			this.Virtuals = Virtuals;
		}

		private void DoChunk(Chunk Chunk)
		{
			foreach (Chunk SubChunk in Chunk.Chunks)
			{
				this.DoChunk(SubChunk);
			}
			for (int InstructionPoint = 0; InstructionPoint < Chunk.Instructions.Count; InstructionPoint++)
			{
				Instruction Instruction = Chunk.Instructions[InstructionPoint];
				if (LuaMacros.<>o__4.<>p__3 == null)
				{
					LuaMacros.<>o__4.<>p__3 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof(LuaMacros), new CSharpArgumentInfo[]
					{
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
					}));
				}
				Func<CallSite, object, bool> target = LuaMacros.<>o__4.<>p__3.Target;
				CallSite <>p__ = LuaMacros.<>o__4.<>p__3;
				bool flag = Instruction.OpCode == OpCode.OpGetGlobal;
				object arg2;
				if (flag)
				{
					if (LuaMacros.<>o__4.<>p__2 == null)
					{
						LuaMacros.<>o__4.<>p__2 = CallSite<Func<CallSite, bool, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.BinaryOperationLogical, ExpressionType.And, typeof(LuaMacros), new CSharpArgumentInfo[]
						{
							CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null),
							CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
						}));
					}
					Func<CallSite, bool, object, object> target2 = LuaMacros.<>o__4.<>p__2.Target;
					CallSite <>p__2 = LuaMacros.<>o__4.<>p__2;
					bool arg = flag;
					if (LuaMacros.<>o__4.<>p__1 == null)
					{
						LuaMacros.<>o__4.<>p__1 = CallSite<Func<CallSite, List<string>, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "Contains", null, typeof(LuaMacros), new CSharpArgumentInfo[]
						{
							CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null),
							CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
						}));
					}
					Func<CallSite, List<string>, object, object> target3 = LuaMacros.<>o__4.<>p__1.Target;
					CallSite <>p__3 = LuaMacros.<>o__4.<>p__1;
					List<string> macros = LuaMacros.Macros;
					if (LuaMacros.<>o__4.<>p__0 == null)
					{
						LuaMacros.<>o__4.<>p__0 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Data", typeof(LuaMacros), new CSharpArgumentInfo[]
						{
							CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
						}));
					}
					arg2 = target2(<>p__2, arg, target3(<>p__3, macros, LuaMacros.<>o__4.<>p__0.Target(LuaMacros.<>o__4.<>p__0, Instruction.References[0])));
				}
				else
				{
					arg2 = flag;
				}
				bool flag2 = target(<>p__, arg2);
				if (flag2)
				{
					int A = Instruction.A;
					bool flag3 = Chunk.Instructions[InstructionPoint + 1].OpCode == OpCode.OpCall && Chunk.Instructions[InstructionPoint + 1].A == Instruction.A;
					if (flag3)
					{
						if (LuaMacros.<>o__4.<>p__4 == null)
						{
							LuaMacros.<>o__4.<>p__4 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Data", typeof(LuaMacros), new CSharpArgumentInfo[]
							{
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
							}));
						}
						object obj = LuaMacros.<>o__4.<>p__4.Target(LuaMacros.<>o__4.<>p__4, Instruction.References[0]);
						object obj2 = obj;
						string text = obj2 as string;
						if (text != null)
						{
							if (text == "PSU_GETSTACK")
							{
								Utility.VoidInstruction(Chunk.Instructions[InstructionPoint + 1]);
								Chunk.Instructions.RemoveAt(InstructionPoint + 1);
								Utility.VoidInstruction(Instruction);
								Instruction.OpCode = OpCode.OpGetStack;
								Instruction.A = A;
							}
						}
					}
				}
			}
			Chunk.UpdateMappings();
		}

		public void DoChunks()
		{
			this.DoChunk(this.HeadChunk);
		}

		private static List<string> Macros = new List<string>
		{
			"PSU_GETSTACK"
		};

		private Chunk HeadChunk;

		private List<VOpCode> Virtuals;
	}
}
