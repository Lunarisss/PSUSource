using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using Microsoft.CSharp.RuntimeBinder;

namespace Obfuscator.Bytecode.IR
{
	public class BasicBlock
	{
		public List<BasicBlock> GenerateBasicBlocks(Chunk Chunk)
		{
			Random Random = new Random();
			List<BasicBlock> BasicBlocks = new List<BasicBlock>();
			int InstructionPoint = 0;
			BasicBlock BasicBlock = null;
			Dictionary<int, BasicBlock> BlockMap = new Dictionary<int, BasicBlock>();
			while (InstructionPoint < Chunk.Instructions.Count)
			{
				Instruction Instruction = Chunk.Instructions[InstructionPoint];
				bool flag = Instruction.BackReferences.Count > 0;
				if (flag)
				{
					BasicBlock = null;
				}
				bool flag2 = BasicBlock == null;
				if (flag2)
				{
					BasicBlock = new BasicBlock();
					BasicBlocks.Add(BasicBlock);
				}
				BasicBlock.Instructions.Add(Instruction);
				BlockMap[InstructionPoint] = BasicBlock;
				OpCode opCode = Instruction.OpCode;
				OpCode opCode2 = opCode;
				if (opCode2 - OpCode.OpJump <= 5 || opCode2 - OpCode.OpReturn <= 3)
				{
					BasicBlock = null;
				}
				bool isJump = Instruction.IsJump;
				if (isJump)
				{
					BasicBlock = null;
				}
				InstructionPoint++;
			}
			BasicBlocks.First<BasicBlock>().BackReferences.Add(new BasicBlock());
			foreach (BasicBlock Block in BasicBlocks)
			{
				bool flag3 = Block.Instructions.Count == 0;
				if (!flag3)
				{
					Instruction Instruction2 = Block.Instructions.Last<Instruction>();
					switch (Instruction2.OpCode)
					{
					case OpCode.OpJump:
					{
						if (BasicBlock.<>o__3.<>p__5 == null)
						{
							BasicBlock.<>o__3.<>p__5 = CallSite<Action<CallSite, List<BasicBlock>, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "Add", null, typeof(BasicBlock), new CSharpArgumentInfo[]
							{
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null),
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
							}));
						}
						Action<CallSite, List<BasicBlock>, object> target = BasicBlock.<>o__3.<>p__5.Target;
						CallSite <>p__ = BasicBlock.<>o__3.<>p__5;
						List<BasicBlock> references = Block.References;
						if (BasicBlock.<>o__3.<>p__4 == null)
						{
							BasicBlock.<>o__3.<>p__4 = CallSite<Func<CallSite, Dictionary<int, BasicBlock>, object, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(BasicBlock), new CSharpArgumentInfo[]
							{
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null),
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
							}));
						}
						Func<CallSite, Dictionary<int, BasicBlock>, object, object> target2 = BasicBlock.<>o__3.<>p__4.Target;
						CallSite <>p__2 = BasicBlock.<>o__3.<>p__4;
						Dictionary<int, BasicBlock> arg = BlockMap;
						if (BasicBlock.<>o__3.<>p__3 == null)
						{
							BasicBlock.<>o__3.<>p__3 = CallSite<Func<CallSite, Dictionary<Instruction, int>, object, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(BasicBlock), new CSharpArgumentInfo[]
							{
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null),
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
							}));
						}
						target(<>p__, references, target2(<>p__2, arg, BasicBlock.<>o__3.<>p__3.Target(BasicBlock.<>o__3.<>p__3, Chunk.InstructionMap, Instruction2.References[0])));
						break;
					}
					case OpCode.OpEq:
					case OpCode.OpLt:
					case OpCode.OpLe:
					case OpCode.OpTest:
					case OpCode.OpTestSet:
					case OpCode.OpTForLoop:
					{
						Block.References.Add(BlockMap[Chunk.InstructionMap[Instruction2] + 1]);
						if (BasicBlock.<>o__3.<>p__8 == null)
						{
							BasicBlock.<>o__3.<>p__8 = CallSite<Action<CallSite, List<BasicBlock>, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "Add", null, typeof(BasicBlock), new CSharpArgumentInfo[]
							{
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null),
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
							}));
						}
						Action<CallSite, List<BasicBlock>, object> target3 = BasicBlock.<>o__3.<>p__8.Target;
						CallSite <>p__3 = BasicBlock.<>o__3.<>p__8;
						List<BasicBlock> references2 = Block.References;
						if (BasicBlock.<>o__3.<>p__7 == null)
						{
							BasicBlock.<>o__3.<>p__7 = CallSite<Func<CallSite, Dictionary<int, BasicBlock>, object, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(BasicBlock), new CSharpArgumentInfo[]
							{
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null),
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
							}));
						}
						Func<CallSite, Dictionary<int, BasicBlock>, object, object> target4 = BasicBlock.<>o__3.<>p__7.Target;
						CallSite <>p__4 = BasicBlock.<>o__3.<>p__7;
						Dictionary<int, BasicBlock> arg2 = BlockMap;
						if (BasicBlock.<>o__3.<>p__6 == null)
						{
							BasicBlock.<>o__3.<>p__6 = CallSite<Func<CallSite, Dictionary<Instruction, int>, object, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(BasicBlock), new CSharpArgumentInfo[]
							{
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null),
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
							}));
						}
						target3(<>p__3, references2, target4(<>p__4, arg2, BasicBlock.<>o__3.<>p__6.Target(BasicBlock.<>o__3.<>p__6, Chunk.InstructionMap, Instruction2.References[2])));
						break;
					}
					case OpCode.OpCall:
					case OpCode.OpTailCall:
						goto IL_513;
					case OpCode.OpReturn:
						break;
					case OpCode.OpForLoop:
					case OpCode.OpForPrep:
					{
						Block.References.Add(BlockMap[Chunk.InstructionMap[Instruction2] + 1]);
						if (BasicBlock.<>o__3.<>p__2 == null)
						{
							BasicBlock.<>o__3.<>p__2 = CallSite<Action<CallSite, List<BasicBlock>, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "Add", null, typeof(BasicBlock), new CSharpArgumentInfo[]
							{
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null),
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
							}));
						}
						Action<CallSite, List<BasicBlock>, object> target5 = BasicBlock.<>o__3.<>p__2.Target;
						CallSite <>p__5 = BasicBlock.<>o__3.<>p__2;
						List<BasicBlock> references3 = Block.References;
						if (BasicBlock.<>o__3.<>p__1 == null)
						{
							BasicBlock.<>o__3.<>p__1 = CallSite<Func<CallSite, Dictionary<int, BasicBlock>, object, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(BasicBlock), new CSharpArgumentInfo[]
							{
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null),
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
							}));
						}
						Func<CallSite, Dictionary<int, BasicBlock>, object, object> target6 = BasicBlock.<>o__3.<>p__1.Target;
						CallSite <>p__6 = BasicBlock.<>o__3.<>p__1;
						Dictionary<int, BasicBlock> arg3 = BlockMap;
						if (BasicBlock.<>o__3.<>p__0 == null)
						{
							BasicBlock.<>o__3.<>p__0 = CallSite<Func<CallSite, Dictionary<Instruction, int>, object, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(BasicBlock), new CSharpArgumentInfo[]
							{
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null),
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
							}));
						}
						target5(<>p__5, references3, target6(<>p__6, arg3, BasicBlock.<>o__3.<>p__0.Target(BasicBlock.<>o__3.<>p__0, Chunk.InstructionMap, Instruction2.References[0])));
						break;
					}
					default:
						goto IL_513;
					}
					IL_539:
					foreach (BasicBlock Reference in Block.References)
					{
						Reference.BackReferences.Add(Block);
					}
					continue;
					IL_513:
					Block.References.Add(BlockMap[Chunk.InstructionMap[Instruction2] + 1]);
					goto IL_539;
				}
			}
			return BasicBlocks;
		}

		public List<BasicBlock> GenerateBasicBlocksFromInstructions(Chunk Chunk, List<Instruction> Instructions)
		{
			Random Random = new Random();
			List<BasicBlock> BasicBlocks = new List<BasicBlock>();
			int InstructionPoint = 0;
			BasicBlock BasicBlock = null;
			Dictionary<int, BasicBlock> BlockMap = new Dictionary<int, BasicBlock>();
			while (InstructionPoint < Instructions.Count)
			{
				Instruction Instruction = Instructions[InstructionPoint];
				bool flag = Instruction.BackReferences.Count > 0;
				if (flag)
				{
					BasicBlock = null;
				}
				bool flag2 = BasicBlock == null;
				if (flag2)
				{
					BasicBlock = new BasicBlock();
					BasicBlocks.Add(BasicBlock);
				}
				BasicBlock.Instructions.Add(Instruction);
				BlockMap[InstructionPoint] = BasicBlock;
				OpCode opCode = Instruction.OpCode;
				OpCode opCode2 = opCode;
				if (opCode2 - OpCode.OpJump <= 5 || opCode2 - OpCode.OpReturn <= 3)
				{
					BasicBlock = null;
				}
				bool isJump = Instruction.IsJump;
				if (isJump)
				{
					BasicBlock = null;
				}
				InstructionPoint++;
			}
			foreach (BasicBlock Block in BasicBlocks)
			{
				bool flag3 = Block.Instructions.Count == 0;
				if (!flag3)
				{
					Instruction Instruction2 = Block.Instructions.Last<Instruction>();
					switch (Instruction2.OpCode)
					{
					case OpCode.OpJump:
					{
						if (BasicBlock.<>o__4.<>p__8 == null)
						{
							BasicBlock.<>o__4.<>p__8 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof(BasicBlock), new CSharpArgumentInfo[]
							{
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
							}));
						}
						Func<CallSite, object, bool> target = BasicBlock.<>o__4.<>p__8.Target;
						CallSite <>p__ = BasicBlock.<>o__4.<>p__8;
						if (BasicBlock.<>o__4.<>p__7 == null)
						{
							BasicBlock.<>o__4.<>p__7 = CallSite<Func<CallSite, Dictionary<int, BasicBlock>, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ContainsKey", null, typeof(BasicBlock), new CSharpArgumentInfo[]
							{
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null),
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
							}));
						}
						Func<CallSite, Dictionary<int, BasicBlock>, object, object> target2 = BasicBlock.<>o__4.<>p__7.Target;
						CallSite <>p__2 = BasicBlock.<>o__4.<>p__7;
						Dictionary<int, BasicBlock> arg = BlockMap;
						if (BasicBlock.<>o__4.<>p__6 == null)
						{
							BasicBlock.<>o__4.<>p__6 = CallSite<Func<CallSite, Dictionary<Instruction, int>, object, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(BasicBlock), new CSharpArgumentInfo[]
							{
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null),
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
							}));
						}
						bool flag4 = target(<>p__, target2(<>p__2, arg, BasicBlock.<>o__4.<>p__6.Target(BasicBlock.<>o__4.<>p__6, Chunk.InstructionMap, Instruction2.References[0])));
						if (flag4)
						{
							if (BasicBlock.<>o__4.<>p__11 == null)
							{
								BasicBlock.<>o__4.<>p__11 = CallSite<Action<CallSite, List<BasicBlock>, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "Add", null, typeof(BasicBlock), new CSharpArgumentInfo[]
								{
									CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null),
									CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
								}));
							}
							Action<CallSite, List<BasicBlock>, object> target3 = BasicBlock.<>o__4.<>p__11.Target;
							CallSite <>p__3 = BasicBlock.<>o__4.<>p__11;
							List<BasicBlock> references = Block.References;
							if (BasicBlock.<>o__4.<>p__10 == null)
							{
								BasicBlock.<>o__4.<>p__10 = CallSite<Func<CallSite, Dictionary<int, BasicBlock>, object, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(BasicBlock), new CSharpArgumentInfo[]
								{
									CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null),
									CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
								}));
							}
							Func<CallSite, Dictionary<int, BasicBlock>, object, object> target4 = BasicBlock.<>o__4.<>p__10.Target;
							CallSite <>p__4 = BasicBlock.<>o__4.<>p__10;
							Dictionary<int, BasicBlock> arg2 = BlockMap;
							if (BasicBlock.<>o__4.<>p__9 == null)
							{
								BasicBlock.<>o__4.<>p__9 = CallSite<Func<CallSite, Dictionary<Instruction, int>, object, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(BasicBlock), new CSharpArgumentInfo[]
								{
									CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null),
									CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
								}));
							}
							target3(<>p__3, references, target4(<>p__4, arg2, BasicBlock.<>o__4.<>p__9.Target(BasicBlock.<>o__4.<>p__9, Chunk.InstructionMap, Instruction2.References[0])));
						}
						break;
					}
					case OpCode.OpEq:
					case OpCode.OpLt:
					case OpCode.OpLe:
					case OpCode.OpTest:
					case OpCode.OpTestSet:
					case OpCode.OpTForLoop:
					{
						bool flag5 = BlockMap.ContainsKey(Chunk.InstructionMap[Instruction2] + 1);
						if (flag5)
						{
							Block.References.Add(BlockMap[Chunk.InstructionMap[Instruction2] + 1]);
						}
						if (BasicBlock.<>o__4.<>p__14 == null)
						{
							BasicBlock.<>o__4.<>p__14 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof(BasicBlock), new CSharpArgumentInfo[]
							{
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
							}));
						}
						Func<CallSite, object, bool> target5 = BasicBlock.<>o__4.<>p__14.Target;
						CallSite <>p__5 = BasicBlock.<>o__4.<>p__14;
						if (BasicBlock.<>o__4.<>p__13 == null)
						{
							BasicBlock.<>o__4.<>p__13 = CallSite<Func<CallSite, Dictionary<int, BasicBlock>, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ContainsKey", null, typeof(BasicBlock), new CSharpArgumentInfo[]
							{
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null),
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
							}));
						}
						Func<CallSite, Dictionary<int, BasicBlock>, object, object> target6 = BasicBlock.<>o__4.<>p__13.Target;
						CallSite <>p__6 = BasicBlock.<>o__4.<>p__13;
						Dictionary<int, BasicBlock> arg3 = BlockMap;
						if (BasicBlock.<>o__4.<>p__12 == null)
						{
							BasicBlock.<>o__4.<>p__12 = CallSite<Func<CallSite, Dictionary<Instruction, int>, object, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(BasicBlock), new CSharpArgumentInfo[]
							{
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null),
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
							}));
						}
						bool flag6 = target5(<>p__5, target6(<>p__6, arg3, BasicBlock.<>o__4.<>p__12.Target(BasicBlock.<>o__4.<>p__12, Chunk.InstructionMap, Instruction2.References[2])));
						if (flag6)
						{
							if (BasicBlock.<>o__4.<>p__17 == null)
							{
								BasicBlock.<>o__4.<>p__17 = CallSite<Action<CallSite, List<BasicBlock>, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "Add", null, typeof(BasicBlock), new CSharpArgumentInfo[]
								{
									CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null),
									CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
								}));
							}
							Action<CallSite, List<BasicBlock>, object> target7 = BasicBlock.<>o__4.<>p__17.Target;
							CallSite <>p__7 = BasicBlock.<>o__4.<>p__17;
							List<BasicBlock> references2 = Block.References;
							if (BasicBlock.<>o__4.<>p__16 == null)
							{
								BasicBlock.<>o__4.<>p__16 = CallSite<Func<CallSite, Dictionary<int, BasicBlock>, object, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(BasicBlock), new CSharpArgumentInfo[]
								{
									CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null),
									CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
								}));
							}
							Func<CallSite, Dictionary<int, BasicBlock>, object, object> target8 = BasicBlock.<>o__4.<>p__16.Target;
							CallSite <>p__8 = BasicBlock.<>o__4.<>p__16;
							Dictionary<int, BasicBlock> arg4 = BlockMap;
							if (BasicBlock.<>o__4.<>p__15 == null)
							{
								BasicBlock.<>o__4.<>p__15 = CallSite<Func<CallSite, Dictionary<Instruction, int>, object, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(BasicBlock), new CSharpArgumentInfo[]
								{
									CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null),
									CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
								}));
							}
							target7(<>p__7, references2, target8(<>p__8, arg4, BasicBlock.<>o__4.<>p__15.Target(BasicBlock.<>o__4.<>p__15, Chunk.InstructionMap, Instruction2.References[2])));
						}
						break;
					}
					case OpCode.OpCall:
					case OpCode.OpTailCall:
						goto IL_858;
					case OpCode.OpReturn:
						break;
					case OpCode.OpForLoop:
					case OpCode.OpForPrep:
					{
						bool flag7 = BlockMap.ContainsKey(Chunk.InstructionMap[Instruction2] + 1);
						if (flag7)
						{
							Block.References.Add(BlockMap[Chunk.InstructionMap[Instruction2] + 1]);
						}
						if (BasicBlock.<>o__4.<>p__2 == null)
						{
							BasicBlock.<>o__4.<>p__2 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof(BasicBlock), new CSharpArgumentInfo[]
							{
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
							}));
						}
						Func<CallSite, object, bool> target9 = BasicBlock.<>o__4.<>p__2.Target;
						CallSite <>p__9 = BasicBlock.<>o__4.<>p__2;
						if (BasicBlock.<>o__4.<>p__1 == null)
						{
							BasicBlock.<>o__4.<>p__1 = CallSite<Func<CallSite, Dictionary<int, BasicBlock>, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ContainsKey", null, typeof(BasicBlock), new CSharpArgumentInfo[]
							{
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null),
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
							}));
						}
						Func<CallSite, Dictionary<int, BasicBlock>, object, object> target10 = BasicBlock.<>o__4.<>p__1.Target;
						CallSite <>p__10 = BasicBlock.<>o__4.<>p__1;
						Dictionary<int, BasicBlock> arg5 = BlockMap;
						if (BasicBlock.<>o__4.<>p__0 == null)
						{
							BasicBlock.<>o__4.<>p__0 = CallSite<Func<CallSite, Dictionary<Instruction, int>, object, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(BasicBlock), new CSharpArgumentInfo[]
							{
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null),
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
							}));
						}
						bool flag8 = target9(<>p__9, target10(<>p__10, arg5, BasicBlock.<>o__4.<>p__0.Target(BasicBlock.<>o__4.<>p__0, Chunk.InstructionMap, Instruction2.References[0])));
						if (flag8)
						{
							if (BasicBlock.<>o__4.<>p__5 == null)
							{
								BasicBlock.<>o__4.<>p__5 = CallSite<Action<CallSite, List<BasicBlock>, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "Add", null, typeof(BasicBlock), new CSharpArgumentInfo[]
								{
									CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null),
									CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
								}));
							}
							Action<CallSite, List<BasicBlock>, object> target11 = BasicBlock.<>o__4.<>p__5.Target;
							CallSite <>p__11 = BasicBlock.<>o__4.<>p__5;
							List<BasicBlock> references3 = Block.References;
							if (BasicBlock.<>o__4.<>p__4 == null)
							{
								BasicBlock.<>o__4.<>p__4 = CallSite<Func<CallSite, Dictionary<int, BasicBlock>, object, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(BasicBlock), new CSharpArgumentInfo[]
								{
									CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null),
									CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
								}));
							}
							Func<CallSite, Dictionary<int, BasicBlock>, object, object> target12 = BasicBlock.<>o__4.<>p__4.Target;
							CallSite <>p__12 = BasicBlock.<>o__4.<>p__4;
							Dictionary<int, BasicBlock> arg6 = BlockMap;
							if (BasicBlock.<>o__4.<>p__3 == null)
							{
								BasicBlock.<>o__4.<>p__3 = CallSite<Func<CallSite, Dictionary<Instruction, int>, object, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(BasicBlock), new CSharpArgumentInfo[]
								{
									CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null),
									CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
								}));
							}
							target11(<>p__11, references3, target12(<>p__12, arg6, BasicBlock.<>o__4.<>p__3.Target(BasicBlock.<>o__4.<>p__3, Chunk.InstructionMap, Instruction2.References[0])));
						}
						break;
					}
					default:
						goto IL_858;
					}
					IL_89A:
					foreach (BasicBlock Reference in Block.References)
					{
						Reference.BackReferences.Add(Block);
					}
					continue;
					IL_858:
					bool flag9 = BlockMap.ContainsKey(Chunk.InstructionMap[Instruction2] + 1);
					if (flag9)
					{
						Block.References.Add(BlockMap[Chunk.InstructionMap[Instruction2] + 1]);
					}
					goto IL_89A;
				}
			}
			return BasicBlocks;
		}

		public List<Instruction> Instructions = new List<Instruction>();

		public List<BasicBlock> References = new List<BasicBlock>();

		public List<BasicBlock> BackReferences = new List<BasicBlock>();
	}
}
