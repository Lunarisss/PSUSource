using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using Microsoft.CSharp.RuntimeBinder;
using Obfuscator.Bytecode.IR;
using Obfuscator.Obfuscation.OpCodes;
using Obfuscator.Utility;

namespace Obfuscator.Obfuscation.Security
{
	public class BytecodeSecurity
	{
		public BytecodeSecurity(Chunk HeadChunk, ObfuscationSettings ObfuscationSettings, ObfuscationContext ObfuscationContext, List<VOpCode> Virtuals)
		{
			this.Virtuals = Virtuals;
			this.HeadChunk = HeadChunk;
			this.ObfuscationSettings = ObfuscationSettings;
			this.ObfuscationContext = ObfuscationContext;
		}

		public void DoChunk(Chunk Chunk)
		{
			foreach (Chunk SubChunk in Chunk.Chunks)
			{
				this.DoChunk(SubChunk);
			}
			bool ByteCodeSecurity = false;
			Instruction Begin = null;
			for (int InstructionPoint = 0; InstructionPoint < Chunk.Instructions.Count; InstructionPoint++)
			{
				Instruction Instruction = Chunk.Instructions[InstructionPoint];
				if (BytecodeSecurity.<>o__7.<>p__3 == null)
				{
					BytecodeSecurity.<>o__7.<>p__3 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof(BytecodeSecurity), new CSharpArgumentInfo[]
					{
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
					}));
				}
				Func<CallSite, object, bool> target = BytecodeSecurity.<>o__7.<>p__3.Target;
				CallSite <>p__ = BytecodeSecurity.<>o__7.<>p__3;
				bool flag = Instruction.OpCode == OpCode.OpGetGlobal;
				object arg2;
				if (flag)
				{
					if (BytecodeSecurity.<>o__7.<>p__2 == null)
					{
						BytecodeSecurity.<>o__7.<>p__2 = CallSite<Func<CallSite, bool, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.BinaryOperationLogical, ExpressionType.And, typeof(BytecodeSecurity), new CSharpArgumentInfo[]
						{
							CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null),
							CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
						}));
					}
					Func<CallSite, bool, object, object> target2 = BytecodeSecurity.<>o__7.<>p__2.Target;
					CallSite <>p__2 = BytecodeSecurity.<>o__7.<>p__2;
					bool arg = flag;
					if (BytecodeSecurity.<>o__7.<>p__1 == null)
					{
						BytecodeSecurity.<>o__7.<>p__1 = CallSite<Func<CallSite, List<string>, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "Contains", null, typeof(BytecodeSecurity), new CSharpArgumentInfo[]
						{
							CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null),
							CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
						}));
					}
					Func<CallSite, List<string>, object, object> target3 = BytecodeSecurity.<>o__7.<>p__1.Target;
					CallSite <>p__3 = BytecodeSecurity.<>o__7.<>p__1;
					List<string> macros = BytecodeSecurity.Macros;
					if (BytecodeSecurity.<>o__7.<>p__0 == null)
					{
						BytecodeSecurity.<>o__7.<>p__0 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Data", typeof(BytecodeSecurity), new CSharpArgumentInfo[]
						{
							CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
						}));
					}
					arg2 = target2(<>p__2, arg, target3(<>p__3, macros, BytecodeSecurity.<>o__7.<>p__0.Target(BytecodeSecurity.<>o__7.<>p__0, Instruction.References[0])));
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
						if (BytecodeSecurity.<>o__7.<>p__5 == null)
						{
							BytecodeSecurity.<>o__7.<>p__5 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof(string), typeof(BytecodeSecurity)));
						}
						Func<CallSite, object, string> target4 = BytecodeSecurity.<>o__7.<>p__5.Target;
						CallSite <>p__4 = BytecodeSecurity.<>o__7.<>p__5;
						if (BytecodeSecurity.<>o__7.<>p__4 == null)
						{
							BytecodeSecurity.<>o__7.<>p__4 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Data", typeof(BytecodeSecurity), new CSharpArgumentInfo[]
							{
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
							}));
						}
						string Global = target4(<>p__4, BytecodeSecurity.<>o__7.<>p__4.Target(BytecodeSecurity.<>o__7.<>p__4, Instruction.References[0]));
						Utility.VoidInstruction(Chunk.Instructions[InstructionPoint + 1]);
						Utility.VoidInstruction(Instruction);
						string text = Global;
						string a = text;
						if (!(a == "PSU_MAX_SECURITY_START"))
						{
							if (a == "PSU_MAX_SECURITY_END")
							{
								if (ByteCodeSecurity)
								{
									Chunk.UpdateMappings();
									bool flag4 = Begin == Instruction;
									if (flag4)
									{
										Begin = null;
										ByteCodeSecurity = false;
									}
									else
									{
										Instruction = Chunk.Instructions[InstructionPoint];
										int Start = Chunk.InstructionMap[Begin];
										int End = Chunk.InstructionMap[Instruction] + 1;
										List<Instruction> InstructionList = Chunk.Instructions.Skip(Start).Take(End - Start).ToList<Instruction>();
										new RegisterMutation(this.ObfuscationContext, Chunk, InstructionList, this.Virtuals, Start, End).DoInstructions();
										Start = Chunk.InstructionMap[Begin];
										End = Chunk.InstructionMap[Instruction] + 1;
										InstructionList = Chunk.Instructions.Skip(Start).Take(End - Start).ToList<Instruction>();
										new TestSpam(this.ObfuscationContext, Chunk, InstructionList, this.Virtuals, Start, End).DoInstructions();
										Start = Chunk.InstructionMap[Begin];
										End = Chunk.InstructionMap[Instruction] + 1;
										InstructionList = Chunk.Instructions.Skip(Start).Take(End - Start).ToList<Instruction>();
										new TestRemove(this.ObfuscationContext, Chunk, InstructionList, this.Virtuals, Start, End).DoInstructions();
										Begin = null;
										ByteCodeSecurity = false;
									}
								}
							}
						}
						else if (!ByteCodeSecurity)
						{
							Begin = Chunk.Instructions[InstructionPoint + 2];
							ByteCodeSecurity = true;
						}
					}
				}
			}
			Chunk.Instructions.Insert(0, new Instruction(Chunk, OpCode.None, Array.Empty<object>()));
			Chunk.UpdateMappings();
		}

		public void DoChunks()
		{
			this.DoChunk(this.HeadChunk);
		}

		private static List<string> Macros = new List<string>
		{
			"PSU_MAX_SECURITY_START",
			"PSU_MAX_SECURITY_END"
		};

		private Random Random = new Random();

		private Chunk HeadChunk;

		private ObfuscationSettings ObfuscationSettings;

		private ObfuscationContext ObfuscationContext;

		private List<VOpCode> Virtuals;
	}
}
