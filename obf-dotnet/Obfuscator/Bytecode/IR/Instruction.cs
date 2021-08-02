using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Microsoft.CSharp.RuntimeBinder;
using Obfuscator.Obfuscation;

namespace Obfuscator.Bytecode.IR
{
	[Serializable]
	public class Instruction
	{
		public Instruction(Instruction Instruction)
		{
			this.OpCode = Instruction.OpCode;
			this.InstructionType = Instruction.InstructionType;
			this.ConstantType = Instruction.ConstantType;
			this.References = new List<object>(Instruction.References);
			this.BackReferences = new List<Instruction>(Instruction.BackReferences);
			this.Chunk = Instruction.Chunk;
			this.A = Instruction.A;
			this.B = Instruction.B;
			this.C = Instruction.C;
			this.Data = Instruction.Data;
			this.Line = Instruction.Line;
			this.IsJump = Instruction.IsJump;
			this.JumpTo = Instruction.JumpTo;
			this.IsConstantA = Instruction.IsConstantA;
			this.IsConstantB = Instruction.IsConstantB;
			this.IsConstantC = Instruction.IsConstantC;
		}

		public Instruction(Chunk Chunk, OpCode OpCode, params object[] References)
		{
			this.OpCode = OpCode;
			InstructionType Type;
			bool flag = Deserializer.InstructionMappings.TryGetValue(OpCode, out Type);
			if (flag)
			{
				this.InstructionType = Type;
			}
			else
			{
				this.InstructionType = InstructionType.ABC;
			}
			this.ConstantType = InstructionConstantType.NK;
			this.References = new List<object>
			{
				null,
				null,
				null,
				null,
				null
			};
			this.BackReferences = new List<Instruction>();
			this.Chunk = Chunk;
			this.A = 0;
			this.B = 0;
			this.C = 0;
			this.Data = 0;
			this.Line = 0;
			this.IsConstantA = false;
			this.IsConstantB = false;
			this.IsConstantC = false;
			for (int I = 0; I < References.Length; I++)
			{
				object Reference = References[I];
				this.References[I] = Reference;
				Instruction Instruction = Reference as Instruction;
				bool flag2 = Instruction != null;
				if (flag2)
				{
					Instruction.BackReferences.Add(this);
				}
			}
		}

		public void UpdateRegisters()
		{
			bool flag = this.InstructionType == InstructionType.Data;
			if (!flag)
			{
				this.PC = this.Chunk.InstructionMap[this];
				OpCode opCode = this.OpCode;
				OpCode opCode2 = opCode;
				switch (opCode2)
				{
				case OpCode.OpLoadK:
				case OpCode.OpGetGlobal:
				case OpCode.OpSetGlobal:
				{
					this.IsConstantB = true;
					Dictionary<Constant, int> constantMap = this.Chunk.ConstantMap;
					if (Instruction.<>o__30.<>p__0 == null)
					{
						Instruction.<>o__30.<>p__0 = CallSite<Func<CallSite, object, Constant>>.Create(Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof(Constant), typeof(Instruction)));
					}
					this.B = constantMap[Instruction.<>o__30.<>p__0.Target(Instruction.<>o__30.<>p__0, this.References[0])];
					goto IL_452;
				}
				case OpCode.OpLoadBool:
				case OpCode.OpLoadNil:
				case OpCode.OpGetUpValue:
				case OpCode.OpSetUpValue:
				case OpCode.OpNewTable:
				case OpCode.OpUnm:
				case OpCode.OpNot:
				case OpCode.OpLen:
				case OpCode.OpConcat:
				case OpCode.OpTest:
				case OpCode.OpTestSet:
				case OpCode.OpCall:
				case OpCode.OpTailCall:
				case OpCode.OpReturn:
				case OpCode.OpTForLoop:
				case OpCode.OpSetList:
				case OpCode.OpClose:
				case OpCode.OpVarArg:
				case OpCode.None:
					goto IL_452;
				case OpCode.OpGetTable:
				case OpCode.OpSetTable:
				case OpCode.OpSelf:
				case OpCode.OpAdd:
				case OpCode.OpSub:
				case OpCode.OpMul:
				case OpCode.OpDiv:
				case OpCode.OpMod:
				case OpCode.OpPow:
				case OpCode.OpEq:
				case OpCode.OpLt:
				case OpCode.OpLe:
				{
					Constant ConstantB = this.References[0] as Constant;
					bool flag2 = ConstantB != null;
					if (flag2)
					{
						this.IsConstantB = true;
						this.B = this.Chunk.ConstantMap[ConstantB];
					}
					else
					{
						this.IsConstantB = false;
					}
					Constant ConstantC = this.References[1] as Constant;
					bool flag3 = ConstantC != null;
					if (flag3)
					{
						this.IsConstantC = true;
						this.C = this.Chunk.ConstantMap[ConstantC];
					}
					else
					{
						this.IsConstantC = false;
					}
					goto IL_452;
				}
				case OpCode.OpJump:
				case OpCode.OpForLoop:
				case OpCode.OpForPrep:
					break;
				case OpCode.OpClosure:
				{
					Dictionary<Chunk, int> chunkMap = this.Chunk.ChunkMap;
					if (Instruction.<>o__30.<>p__2 == null)
					{
						Instruction.<>o__30.<>p__2 = CallSite<Func<CallSite, object, Chunk>>.Create(Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof(Chunk), typeof(Instruction)));
					}
					this.B = chunkMap[Instruction.<>o__30.<>p__2.Target(Instruction.<>o__30.<>p__2, this.References[0])];
					goto IL_452;
				}
				case OpCode.Custom:
				{
					Constant ConstantA = this.References[0] as Constant;
					bool flag4 = ConstantA != null;
					if (flag4)
					{
						this.IsConstantA = true;
						this.A = this.Chunk.ConstantMap[ConstantA];
					}
					else
					{
						this.IsConstantA = false;
					}
					Constant ConstantB2 = this.References[1] as Constant;
					bool flag5 = ConstantB2 != null;
					if (flag5)
					{
						this.IsConstantB = true;
						this.B = this.Chunk.ConstantMap[ConstantB2];
					}
					else
					{
						this.IsConstantB = false;
					}
					Constant ConstantC2 = this.References[2] as Constant;
					bool flag6 = ConstantC2 != null;
					if (flag6)
					{
						this.IsConstantC = true;
						this.C = this.Chunk.ConstantMap[ConstantC2];
					}
					else
					{
						this.IsConstantC = false;
					}
					Instruction InstructionA = this.References[0] as Instruction;
					bool flag7 = InstructionA != null;
					if (flag7)
					{
						this.A = this.Chunk.InstructionMap[InstructionA];
					}
					Instruction InstructionB = this.References[1] as Instruction;
					bool flag8 = InstructionB != null;
					if (flag8)
					{
						this.B = this.Chunk.InstructionMap[InstructionB];
					}
					Instruction InstructionC = this.References[2] as Instruction;
					bool flag9 = InstructionC != null;
					if (flag9)
					{
						this.C = this.Chunk.InstructionMap[InstructionC];
					}
					goto IL_452;
				}
				default:
					if (opCode2 != OpCode.OpLoadJump)
					{
						goto IL_452;
					}
					break;
				}
				Dictionary<Instruction, int> instructionMap = this.Chunk.InstructionMap;
				if (Instruction.<>o__30.<>p__1 == null)
				{
					Instruction.<>o__30.<>p__1 = CallSite<Func<CallSite, object, Instruction>>.Create(Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof(Instruction), typeof(Instruction)));
				}
				this.B = instructionMap[Instruction.<>o__30.<>p__1.Target(Instruction.<>o__30.<>p__1, this.References[0])];
				IL_452:;
			}
		}

		public void SetupReferences()
		{
			switch (this.OpCode)
			{
			case OpCode.OpLoadK:
			case OpCode.OpGetGlobal:
			case OpCode.OpSetGlobal:
			{
				Constant Reference = this.Chunk.Constants[this.B];
				this.References[0] = Reference;
				Reference.BackReferences.Add(this);
				break;
			}
			case OpCode.OpGetTable:
			case OpCode.OpSetTable:
			case OpCode.OpSelf:
			case OpCode.OpAdd:
			case OpCode.OpSub:
			case OpCode.OpMul:
			case OpCode.OpDiv:
			case OpCode.OpMod:
			case OpCode.OpPow:
			{
				bool flag = this.B > 255;
				if (flag)
				{
					this.IsConstantB = true;
					this.References[0] = this.Chunk.Constants[this.B -= 256];
					if (Instruction.<>o__31.<>p__5 == null)
					{
						Instruction.<>o__31.<>p__5 = CallSite<Action<CallSite, object, Instruction>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "Add", null, typeof(Instruction), new CSharpArgumentInfo[]
						{
							CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
							CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null)
						}));
					}
					Action<CallSite, object, Instruction> target = Instruction.<>o__31.<>p__5.Target;
					CallSite <>p__ = Instruction.<>o__31.<>p__5;
					if (Instruction.<>o__31.<>p__4 == null)
					{
						Instruction.<>o__31.<>p__4 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "BackReferences", typeof(Instruction), new CSharpArgumentInfo[]
						{
							CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
						}));
					}
					target(<>p__, Instruction.<>o__31.<>p__4.Target(Instruction.<>o__31.<>p__4, this.References[0]), this);
				}
				bool flag2 = this.C > 255;
				if (flag2)
				{
					this.IsConstantC = true;
					this.References[1] = this.Chunk.Constants[this.C -= 256];
					if (Instruction.<>o__31.<>p__7 == null)
					{
						Instruction.<>o__31.<>p__7 = CallSite<Action<CallSite, object, Instruction>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "Add", null, typeof(Instruction), new CSharpArgumentInfo[]
						{
							CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
							CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null)
						}));
					}
					Action<CallSite, object, Instruction> target2 = Instruction.<>o__31.<>p__7.Target;
					CallSite <>p__2 = Instruction.<>o__31.<>p__7;
					if (Instruction.<>o__31.<>p__6 == null)
					{
						Instruction.<>o__31.<>p__6 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "BackReferences", typeof(Instruction), new CSharpArgumentInfo[]
						{
							CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
						}));
					}
					target2(<>p__2, Instruction.<>o__31.<>p__6.Target(Instruction.<>o__31.<>p__6, this.References[1]), this);
				}
				break;
			}
			case OpCode.OpJump:
			case OpCode.OpForLoop:
			case OpCode.OpForPrep:
			{
				Instruction Reference2 = this.Chunk.Instructions[this.Chunk.InstructionMap[this] + this.B + 1];
				this.References[0] = Reference2;
				Reference2.BackReferences.Add(this);
				break;
			}
			case OpCode.OpEq:
			case OpCode.OpLt:
			case OpCode.OpLe:
			{
				bool flag3 = this.B > 255;
				if (flag3)
				{
					this.IsConstantB = true;
					this.References[0] = this.Chunk.Constants[this.B -= 256];
					if (Instruction.<>o__31.<>p__1 == null)
					{
						Instruction.<>o__31.<>p__1 = CallSite<Action<CallSite, object, Instruction>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "Add", null, typeof(Instruction), new CSharpArgumentInfo[]
						{
							CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
							CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null)
						}));
					}
					Action<CallSite, object, Instruction> target3 = Instruction.<>o__31.<>p__1.Target;
					CallSite <>p__3 = Instruction.<>o__31.<>p__1;
					if (Instruction.<>o__31.<>p__0 == null)
					{
						Instruction.<>o__31.<>p__0 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "BackReferences", typeof(Instruction), new CSharpArgumentInfo[]
						{
							CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
						}));
					}
					target3(<>p__3, Instruction.<>o__31.<>p__0.Target(Instruction.<>o__31.<>p__0, this.References[0]), this);
				}
				bool flag4 = this.C > 255;
				if (flag4)
				{
					this.IsConstantC = true;
					this.References[1] = this.Chunk.Constants[this.C -= 256];
					if (Instruction.<>o__31.<>p__3 == null)
					{
						Instruction.<>o__31.<>p__3 = CallSite<Action<CallSite, object, Instruction>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "Add", null, typeof(Instruction), new CSharpArgumentInfo[]
						{
							CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
							CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null)
						}));
					}
					Action<CallSite, object, Instruction> target4 = Instruction.<>o__31.<>p__3.Target;
					CallSite <>p__4 = Instruction.<>o__31.<>p__3;
					if (Instruction.<>o__31.<>p__2 == null)
					{
						Instruction.<>o__31.<>p__2 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "BackReferences", typeof(Instruction), new CSharpArgumentInfo[]
						{
							CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
						}));
					}
					target4(<>p__4, Instruction.<>o__31.<>p__2.Target(Instruction.<>o__31.<>p__2, this.References[1]), this);
				}
				Instruction Reference3 = this.Chunk.Instructions[this.Chunk.InstructionMap[this] + this.Chunk.Instructions[this.Chunk.InstructionMap[this] + 1].B + 1 + 1];
				this.References[2] = Reference3;
				Reference3.BackReferences.Add(this);
				break;
			}
			case OpCode.OpTest:
			case OpCode.OpTestSet:
			case OpCode.OpTForLoop:
			{
				Instruction Reference4 = this.Chunk.Instructions[this.Chunk.InstructionMap[this] + this.Chunk.Instructions[this.Chunk.InstructionMap[this] + 1].B + 1 + 1];
				this.References[2] = Reference4;
				Reference4.BackReferences.Add(this);
				break;
			}
			case OpCode.OpClosure:
				this.References[0] = this.Chunk.Chunks[this.B];
				break;
			}
		}

		public OpCode OpCode;

		public InstructionType InstructionType;

		public InstructionConstantType ConstantType;

		[Dynamic(new bool[]
		{
			false,
			true
		})]
		public List<dynamic> References;

		public List<Instruction> BackReferences;

		public Chunk Chunk;

		public int A;

		public int B;

		public int C;

		public int D;

		public int PC;

		public int Data;

		public int Line;

		public bool IsConstantA = false;

		public bool IsConstantB = false;

		public bool IsConstantC = false;

		public bool RequiresCustomData = false;

		[Dynamic]
		public dynamic CustomData;

		public BasicBlock Block;

		public bool IsJump;

		public Instruction JumpTo;

		public bool IgnoreInstruction = false;

		public int? WrittenVIndex = null;

		public int? WrittenA = null;

		public int? WrittenB = null;

		public int? WrittenC = null;

		public Func<Instruction, Instruction> RegisterHandler = null;

		public CustomInstructionData CustomInstructionData = new CustomInstructionData();
	}
}
