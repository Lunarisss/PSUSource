using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text;
using Microsoft.CSharp.RuntimeBinder;
using Obfuscator.Bytecode.IR;
using Obfuscator.Extensions;
using Obfuscator.Utility;

namespace Obfuscator.Bytecode
{
	public class Deserializer
	{
		public Deserializer(byte[] Input)
		{
			this.MemoryStream = new MemoryStream(Input);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public byte[] Read(int Size, bool FactorEndianness = true)
		{
			byte[] Bytes = new byte[Size];
			this.MemoryStream.Read(Bytes, 0, Size);
			bool flag = FactorEndianness && this.Endian == BitConverter.IsLittleEndian;
			if (flag)
			{
				Bytes = Bytes.Reverse<byte>().ToArray<byte>();
			}
			return Bytes;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public long ReadSizeT()
		{
			return (this.SizeSizeT == 4) ? ((long)this.ReadInt32(true)) : this.ReadInt64();
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public long ReadInt64()
		{
			return BitConverter.ToInt64(this.Read(8, true), 0);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int ReadInt32(bool FactorEndianness = true)
		{
			return BitConverter.ToInt32(this.Read(4, FactorEndianness), 0);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public byte ReadByte()
		{
			return this.Read(1, true)[0];
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public string ReadString()
		{
			int Count = (int)this.ReadSizeT();
			bool flag = Count == 0;
			string result;
			if (flag)
			{
				result = "";
			}
			else
			{
				byte[] Value = this.Read(Count, false);
				result = Deserializer.LuaEncoding.GetString(Value, 0, Count - 1);
			}
			return result;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public double ReadDouble()
		{
			return BitConverter.ToDouble(this.Read((int)this.SizeNumber, true), 0);
		}

		public Instruction DecodeInstruction(Chunk Chunk)
		{
			int Code = this.ReadInt32(true);
			Instruction Instruction = new Instruction(Chunk, (OpCode)(Code & 63), Array.Empty<object>());
			Instruction.Data = Code;
			bool expectingSetListData = this.ExpectingSetListData;
			Instruction result;
			if (expectingSetListData)
			{
				this.ExpectingSetListData = false;
				Instruction.InstructionType = InstructionType.Data;
				result = Instruction;
			}
			else
			{
				Instruction.A = (Code >> 6 & 255);
				switch (Instruction.InstructionType)
				{
				case InstructionType.ABC:
					Instruction.B = (Code >> 23 & 511);
					Instruction.C = (Code >> 14 & 511);
					break;
				case InstructionType.ABx:
					Instruction.B = (Code >> 14 & 262143);
					Instruction.C = 0;
					break;
				case InstructionType.AsBx:
					Instruction.B = (Code >> 14 & 262143) - 131071;
					Instruction.C = 0;
					break;
				}
				bool flag = Instruction.OpCode == OpCode.OpSetList && Instruction.C == 0;
				if (flag)
				{
					this.ExpectingSetListData = true;
				}
				result = Instruction;
			}
			return result;
		}

		public void DecodeInstructions(Chunk Chunk)
		{
			List<Instruction> Instructions = new List<Instruction>();
			Dictionary<Instruction, int> InstructionMap = new Dictionary<Instruction, int>();
			int InstructionCount = this.ReadInt32(true);
			for (int I = 0; I < InstructionCount; I++)
			{
				Instruction Instruction = this.DecodeInstruction(Chunk);
				Instructions.Add(Instruction);
				InstructionMap.Add(Instruction, I);
			}
			Chunk.Instructions = Instructions;
			Chunk.InstructionMap = InstructionMap;
		}

		public Constant DecodeConstant()
		{
			Constant Constant = new Constant();
			switch (this.ReadByte())
			{
			case 0:
				Constant.Type = ConstantType.Nil;
				Constant.Data = null;
				break;
			case 1:
				Constant.Type = ConstantType.Boolean;
				Constant.Data = (this.ReadByte() > 0);
				break;
			case 3:
				Constant.Type = ConstantType.Number;
				Constant.Data = this.ReadDouble();
				break;
			case 4:
				Constant.Type = ConstantType.String;
				Constant.Data = this.ReadString();
				break;
			}
			return Constant;
		}

		public void DecodeConstants(Chunk Chunk)
		{
			List<Constant> Constants = new List<Constant>();
			Dictionary<Constant, int> ConstantMap = new Dictionary<Constant, int>();
			int ConstantCount = this.ReadInt32(true);
			for (int I = 0; I < ConstantCount; I++)
			{
				Constant Constant = this.DecodeConstant();
				Constants.Add(Constant);
				ConstantMap.Add(Constant, I);
			}
			Chunk.Constants = Constants;
			Chunk.ConstantMap = ConstantMap;
		}

		public Chunk DecodeChunk()
		{
			Chunk Chunk = new Chunk
			{
				Name = this.ReadString(),
				Line = this.ReadInt32(true),
				LastLine = this.ReadInt32(true),
				UpValueCount = this.ReadByte(),
				ParameterCount = this.ReadByte(),
				VarArgFlag = this.ReadByte(),
				StackSize = this.ReadByte(),
				UpValues = new List<string>()
			};
			this.DecodeInstructions(Chunk);
			this.DecodeConstants(Chunk);
			this.DecodeChunks(Chunk);
			int Count = this.ReadInt32(true);
			for (int I = 0; I < Count; I++)
			{
				Chunk.Instructions[I].Line = this.ReadInt32(true);
			}
			Count = this.ReadInt32(true);
			for (int I2 = 0; I2 < Count; I2++)
			{
				this.ReadString();
				this.ReadInt32(true);
				this.ReadInt32(true);
			}
			Count = this.ReadInt32(true);
			for (int I3 = 0; I3 < Count; I3++)
			{
				Chunk.UpValues.Add(this.ReadString());
			}
			foreach (Instruction Instruction in Chunk.Instructions)
			{
				Instruction.SetupReferences();
			}
			return Chunk;
		}

		public void DecodeChunks(Chunk Chunk)
		{
			List<Chunk> Chunks = new List<Chunk>();
			Dictionary<Chunk, int> ChunkMap = new Dictionary<Chunk, int>();
			int ChunkCount = this.ReadInt32(true);
			for (int I = 0; I < ChunkCount; I++)
			{
				Chunk SubChunk = this.DecodeChunk();
				Chunks.Add(SubChunk);
				ChunkMap.Add(SubChunk, I);
			}
			Chunk.Chunks = Chunks;
			Chunk.ChunkMap = ChunkMap;
		}

		public Chunk DecodeFile()
		{
			int Header = this.ReadInt32(true);
			bool flag = Header != 457995617 && Header != 1635077147;
			if (flag)
			{
				throw new Exception("Obfuscation Error: Invalid LuaC File.");
			}
			bool flag2 = this.ReadByte() != 81;
			if (flag2)
			{
				throw new Exception("Obfuscation Error: Only Lua 5.1 is Supported.");
			}
			this.ReadByte();
			this.Endian = (this.ReadByte() == 0);
			this.ReadByte();
			this.SizeSizeT = this.ReadByte();
			this.ReadByte();
			this.SizeNumber = this.ReadByte();
			this.ReadByte();
			Chunk HeadChunk = this.DecodeChunk();
			Deserializer.<DecodeFile>g__RemoveJumps|22_0(HeadChunk);
			Deserializer.<DecodeFile>g__EditClosure|22_1(HeadChunk);
			Deserializer.<DecodeFile>g__UpdateChunk|22_2(HeadChunk);
			Deserializer.<DecodeFile>g__FixJumps|22_3(HeadChunk);
			Deserializer.<DecodeFile>g__UpdateChunk|22_2(HeadChunk);
			return HeadChunk;
		}

		[CompilerGenerated]
		internal static void <DecodeFile>g__RemoveJumps|22_0(Chunk Chunk)
		{
			for (int InstructionPoint = 0; InstructionPoint < Chunk.Instructions.Count; InstructionPoint++)
			{
				Instruction Instruction = Chunk.Instructions[InstructionPoint];
				OpCode opCode = Instruction.OpCode;
				OpCode opCode2 = opCode;
				if (opCode2 - OpCode.OpEq <= 4 || opCode2 == OpCode.OpTForLoop)
				{
					Utility.VoidInstruction(Chunk.Instructions[InstructionPoint + 1]);
					Chunk.Instructions.RemoveAt(InstructionPoint + 1);
				}
			}
			foreach (Chunk SubChunk in Chunk.Chunks)
			{
				Deserializer.<DecodeFile>g__RemoveJumps|22_0(SubChunk);
			}
			Chunk.UpdateMappings();
		}

		[CompilerGenerated]
		internal static void <DecodeFile>g__EditClosure|22_1(Chunk Chunk)
		{
			for (int InstructionPoint = 0; InstructionPoint < Chunk.Instructions.Count; InstructionPoint++)
			{
				Instruction Instruction = Chunk.Instructions[InstructionPoint];
				if (Deserializer.<>o__22.<>p__3 == null)
				{
					Deserializer.<>o__22.<>p__3 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof(Deserializer), new CSharpArgumentInfo[]
					{
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
					}));
				}
				Func<CallSite, object, bool> target = Deserializer.<>o__22.<>p__3.Target;
				CallSite <>p__ = Deserializer.<>o__22.<>p__3;
				bool flag = Instruction.OpCode == OpCode.OpClosure;
				object arg2;
				if (flag)
				{
					if (Deserializer.<>o__22.<>p__2 == null)
					{
						Deserializer.<>o__22.<>p__2 = CallSite<Func<CallSite, bool, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.BinaryOperationLogical, ExpressionType.And, typeof(Deserializer), new CSharpArgumentInfo[]
						{
							CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null),
							CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
						}));
					}
					Func<CallSite, bool, object, object> target2 = Deserializer.<>o__22.<>p__2.Target;
					CallSite <>p__2 = Deserializer.<>o__22.<>p__2;
					bool arg = flag;
					if (Deserializer.<>o__22.<>p__1 == null)
					{
						Deserializer.<>o__22.<>p__1 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.GreaterThan, typeof(Deserializer), new CSharpArgumentInfo[]
						{
							CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
							CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, null)
						}));
					}
					Func<CallSite, object, int, object> target3 = Deserializer.<>o__22.<>p__1.Target;
					CallSite <>p__3 = Deserializer.<>o__22.<>p__1;
					if (Deserializer.<>o__22.<>p__0 == null)
					{
						Deserializer.<>o__22.<>p__0 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "UpValueCount", typeof(Deserializer), new CSharpArgumentInfo[]
						{
							CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
						}));
					}
					arg2 = target2(<>p__2, arg, target3(<>p__3, Deserializer.<>o__22.<>p__0.Target(Deserializer.<>o__22.<>p__0, Instruction.References[0]), 0));
				}
				else
				{
					arg2 = flag;
				}
				bool flag2 = target(<>p__, arg2);
				if (flag2)
				{
					Instruction.CustomData = new List<int[]>();
					int I = 1;
					for (;;)
					{
						if (Deserializer.<>o__22.<>p__6 == null)
						{
							Deserializer.<>o__22.<>p__6 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof(Deserializer), new CSharpArgumentInfo[]
							{
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
							}));
						}
						Func<CallSite, object, bool> target4 = Deserializer.<>o__22.<>p__6.Target;
						CallSite <>p__4 = Deserializer.<>o__22.<>p__6;
						if (Deserializer.<>o__22.<>p__5 == null)
						{
							Deserializer.<>o__22.<>p__5 = CallSite<Func<CallSite, int, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.LessThanOrEqual, typeof(Deserializer), new CSharpArgumentInfo[]
							{
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null),
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
							}));
						}
						Func<CallSite, int, object, object> target5 = Deserializer.<>o__22.<>p__5.Target;
						CallSite <>p__5 = Deserializer.<>o__22.<>p__5;
						int arg3 = I;
						if (Deserializer.<>o__22.<>p__4 == null)
						{
							Deserializer.<>o__22.<>p__4 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "UpValueCount", typeof(Deserializer), new CSharpArgumentInfo[]
							{
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
							}));
						}
						if (!target4(<>p__4, target5(<>p__5, arg3, Deserializer.<>o__22.<>p__4.Target(Deserializer.<>o__22.<>p__4, Instruction.References[0]))))
						{
							break;
						}
						Instruction UpValue = Chunk.Instructions[InstructionPoint + I];
						if (Deserializer.<>o__22.<>p__7 == null)
						{
							Deserializer.<>o__22.<>p__7 = CallSite<Action<CallSite, object, int[]>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "Add", null, typeof(Deserializer), new CSharpArgumentInfo[]
							{
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null)
							}));
						}
						Deserializer.<>o__22.<>p__7.Target(Deserializer.<>o__22.<>p__7, Instruction.CustomData, new int[]
						{
							(UpValue.OpCode == OpCode.OpMove) ? 0 : 1,
							UpValue.B
						});
						I++;
					}
					int I2 = 1;
					for (;;)
					{
						if (Deserializer.<>o__22.<>p__10 == null)
						{
							Deserializer.<>o__22.<>p__10 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof(Deserializer), new CSharpArgumentInfo[]
							{
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
							}));
						}
						Func<CallSite, object, bool> target6 = Deserializer.<>o__22.<>p__10.Target;
						CallSite <>p__6 = Deserializer.<>o__22.<>p__10;
						if (Deserializer.<>o__22.<>p__9 == null)
						{
							Deserializer.<>o__22.<>p__9 = CallSite<Func<CallSite, int, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.LessThanOrEqual, typeof(Deserializer), new CSharpArgumentInfo[]
							{
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null),
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
							}));
						}
						Func<CallSite, int, object, object> target7 = Deserializer.<>o__22.<>p__9.Target;
						CallSite <>p__7 = Deserializer.<>o__22.<>p__9;
						int arg4 = I2;
						if (Deserializer.<>o__22.<>p__8 == null)
						{
							Deserializer.<>o__22.<>p__8 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "UpValueCount", typeof(Deserializer), new CSharpArgumentInfo[]
							{
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
							}));
						}
						if (!target6(<>p__6, target7(<>p__7, arg4, Deserializer.<>o__22.<>p__8.Target(Deserializer.<>o__22.<>p__8, Instruction.References[0]))))
						{
							break;
						}
						Chunk.Instructions.RemoveAt(InstructionPoint + 1);
						I2++;
					}
				}
			}
			foreach (Chunk SubChunk in Chunk.Chunks)
			{
				Deserializer.<DecodeFile>g__EditClosure|22_1(SubChunk);
			}
			Chunk.UpdateMappings();
		}

		[CompilerGenerated]
		internal static void <DecodeFile>g__UpdateChunk|22_2(Chunk Chunk)
		{
			foreach (Chunk SubChunk in Chunk.Chunks)
			{
				Deserializer.<DecodeFile>g__UpdateChunk|22_2(SubChunk);
			}
			Chunk.Constants.Shuffle<Constant>();
			Chunk.UpdateMappings();
			foreach (Instruction Instruction in Chunk.Instructions)
			{
				Instruction.UpdateRegisters();
			}
		}

		[CompilerGenerated]
		internal static void <DecodeFile>g__FixJumps|22_3(Chunk Chunk)
		{
			for (int InstructionPoint = 0; InstructionPoint < Chunk.Instructions.Count; InstructionPoint++)
			{
				Instruction Instruction = Chunk.Instructions[InstructionPoint];
				OpCode opCode = Instruction.OpCode;
				OpCode opCode2 = opCode;
				if (opCode2 != OpCode.OpLoadBool)
				{
					if (opCode2 - OpCode.OpEq <= 4 || opCode2 - OpCode.OpForLoop <= 2)
					{
						Instruction.IsJump = true;
						Instruction.JumpTo = Chunk.Instructions[InstructionPoint + 1];
						Instruction.JumpTo.BackReferences.Add(Instruction);
					}
				}
				else if (Instruction.C == 1)
				{
					Instruction.IsJump = true;
					Instruction.JumpTo = Chunk.Instructions[InstructionPoint + 2];
					Instruction.JumpTo.BackReferences.Add(Instruction);
				}
			}
			foreach (Chunk SubChunk in Chunk.Chunks)
			{
				Deserializer.<DecodeFile>g__FixJumps|22_3(SubChunk);
			}
			Chunk.UpdateMappings();
		}

		private MemoryStream MemoryStream;

		private bool Endian;

		private byte SizeNumber;

		private byte SizeSizeT;

		private bool ExpectingSetListData;

		private const bool BYTECODE_OPTIMIZATIONS = false;

		private static Encoding LuaEncoding = Encoding.GetEncoding(28591);

		public static Dictionary<OpCode, InstructionType> InstructionMappings = new Dictionary<OpCode, InstructionType>
		{
			{
				OpCode.OpMove,
				InstructionType.ABC
			},
			{
				OpCode.OpLoadK,
				InstructionType.ABx
			},
			{
				OpCode.OpLoadBool,
				InstructionType.ABC
			},
			{
				OpCode.OpLoadNil,
				InstructionType.ABC
			},
			{
				OpCode.OpGetUpValue,
				InstructionType.ABC
			},
			{
				OpCode.OpGetGlobal,
				InstructionType.ABx
			},
			{
				OpCode.OpGetTable,
				InstructionType.ABC
			},
			{
				OpCode.OpSetGlobal,
				InstructionType.ABx
			},
			{
				OpCode.OpSetUpValue,
				InstructionType.ABC
			},
			{
				OpCode.OpSetTable,
				InstructionType.ABC
			},
			{
				OpCode.OpNewTable,
				InstructionType.ABC
			},
			{
				OpCode.OpSelf,
				InstructionType.ABC
			},
			{
				OpCode.OpAdd,
				InstructionType.ABC
			},
			{
				OpCode.OpSub,
				InstructionType.ABC
			},
			{
				OpCode.OpMul,
				InstructionType.ABC
			},
			{
				OpCode.OpDiv,
				InstructionType.ABC
			},
			{
				OpCode.OpMod,
				InstructionType.ABC
			},
			{
				OpCode.OpPow,
				InstructionType.ABC
			},
			{
				OpCode.OpUnm,
				InstructionType.ABC
			},
			{
				OpCode.OpNot,
				InstructionType.ABC
			},
			{
				OpCode.OpLen,
				InstructionType.ABC
			},
			{
				OpCode.OpConcat,
				InstructionType.ABC
			},
			{
				OpCode.OpJump,
				InstructionType.AsBx
			},
			{
				OpCode.OpEq,
				InstructionType.ABC
			},
			{
				OpCode.OpLt,
				InstructionType.ABC
			},
			{
				OpCode.OpLe,
				InstructionType.ABC
			},
			{
				OpCode.OpTest,
				InstructionType.ABC
			},
			{
				OpCode.OpTestSet,
				InstructionType.ABC
			},
			{
				OpCode.OpCall,
				InstructionType.ABC
			},
			{
				OpCode.OpTailCall,
				InstructionType.ABC
			},
			{
				OpCode.OpReturn,
				InstructionType.ABC
			},
			{
				OpCode.OpForLoop,
				InstructionType.AsBx
			},
			{
				OpCode.OpForPrep,
				InstructionType.AsBx
			},
			{
				OpCode.OpTForLoop,
				InstructionType.ABC
			},
			{
				OpCode.OpSetList,
				InstructionType.ABC
			},
			{
				OpCode.OpClose,
				InstructionType.ABC
			},
			{
				OpCode.OpClosure,
				InstructionType.ABx
			},
			{
				OpCode.OpVarArg,
				InstructionType.ABC
			}
		};
	}
}
