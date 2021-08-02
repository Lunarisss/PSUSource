using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text;
using Microsoft.CSharp.RuntimeBinder;
using Obfuscator.Bytecode.IR;
using Obfuscator.Extensions;
using Obfuscator.Obfuscation;
using Obfuscator.Obfuscation.OpCodes;

namespace Obfuscator.Bytecode
{
	public class Serializer
	{
		public Serializer(ObfuscationContext ObfuscationContext, ObfuscationSettings ObfuscationSettings)
		{
			this.ObfuscationContext = ObfuscationContext;
			this.ObfuscationSettings = ObfuscationSettings;
		}

		public void SerializeLChunk(Chunk Chunk, List<byte> Bytes)
		{
			Serializer.<>c__DisplayClass6_0 CS$<>8__locals1 = new Serializer.<>c__DisplayClass6_0();
			CS$<>8__locals1.Bytes = Bytes;
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.Chunk = Chunk;
			CS$<>8__locals1.Chunk.Constants.Shuffle<Constant>();
			CS$<>8__locals1.Chunk.UpdateMappings();
			foreach (Instruction Instruction in CS$<>8__locals1.Chunk.Instructions)
			{
				Instruction.UpdateRegisters();
				bool flag = !Instruction.CustomInstructionData.Serialize || Instruction.InstructionType == InstructionType.Data;
				if (!flag)
				{
					CustomInstructionData CustomInstructionData = Instruction.CustomInstructionData;
					VOpCode VirtualOpcode = CustomInstructionData.OpCode;
					bool flag2 = !CustomInstructionData.Mutated;
					if (flag2)
					{
						if (VirtualOpcode != null)
						{
							VirtualOpcode.Mutate(Instruction);
						}
					}
					CustomInstructionData.Mutated = true;
				}
			}
			ChunkStep[] chunkSteps = this.ObfuscationContext.ChunkSteps;
			for (int i = 0; i < chunkSteps.Length; i++)
			{
				switch (chunkSteps[i])
				{
				case ChunkStep.ParameterCount:
					CS$<>8__locals1.<SerializeLChunk>g__WriteByte|0(CS$<>8__locals1.Chunk.ParameterCount);
					break;
				case ChunkStep.Instructions:
					CS$<>8__locals1.<SerializeLChunk>g__WriteInt32|2(CS$<>8__locals1.Chunk.Constants.Count);
					foreach (Constant Constant in CS$<>8__locals1.Chunk.Constants)
					{
						bool optimizeString = false;
						bool flag3 = Constant.Type == ConstantType.String;
						if (flag3)
						{
							if (Serializer.<>o__6.<>p__4 == null)
							{
								Serializer.<>o__6.<>p__4 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof(string), typeof(Serializer)));
							}
							string str = Serializer.<>o__6.<>p__4.Target(Serializer.<>o__6.<>p__4, Constant.Data);
							bool flag4 = str.Length >= 100 && str.IndexOf("https://") == -1 && str.IndexOf("http://") == -1;
							if (flag4)
							{
								optimizeString = true;
							}
						}
						bool flag5 = !optimizeString;
						if (flag5)
						{
							CS$<>8__locals1.<SerializeLChunk>g__WriteByte|0((byte)this.ObfuscationContext.ConstantMapping[(int)Constant.Type]);
						}
						else
						{
							CS$<>8__locals1.<SerializeLChunk>g__WriteByte|0((byte)this.ObfuscationContext.ConstantMapping[4]);
						}
						switch (Constant.Type)
						{
						case ConstantType.Boolean:
						{
							Serializer.<>c__DisplayClass6_0 CS$<>8__locals2 = CS$<>8__locals1;
							if (Serializer.<>o__6.<>p__9 == null)
							{
								Serializer.<>o__6.<>p__9 = CallSite<Func<CallSite, object, bool>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(bool), typeof(Serializer)));
							}
							CS$<>8__locals2.<SerializeLChunk>g__WriteBool|5(Serializer.<>o__6.<>p__9.Target(Serializer.<>o__6.<>p__9, Constant.Data));
							break;
						}
						case ConstantType.Number:
						{
							Serializer.<>c__DisplayClass6_0 CS$<>8__locals3 = CS$<>8__locals1;
							if (Serializer.<>o__6.<>p__10 == null)
							{
								Serializer.<>o__6.<>p__10 = CallSite<Func<CallSite, object, double>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(double), typeof(Serializer)));
							}
							CS$<>8__locals3.<SerializeLChunk>g__WriteNumber|4(Serializer.<>o__6.<>p__10.Target(Serializer.<>o__6.<>p__10, Constant.Data));
							break;
						}
						case ConstantType.String:
						{
							bool flag6 = optimizeString;
							if (flag6)
							{
								Serializer.<>c__DisplayClass6_0 CS$<>8__locals4 = CS$<>8__locals1;
								if (Serializer.<>o__6.<>p__11 == null)
								{
									Serializer.<>o__6.<>p__11 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(Serializer)));
								}
								CS$<>8__locals4.<SerializeLChunk>g__WriteStringRaw|7(Serializer.<>o__6.<>p__11.Target(Serializer.<>o__6.<>p__11, Constant.Data));
							}
							else
							{
								Serializer.<>c__DisplayClass6_0 CS$<>8__locals5 = CS$<>8__locals1;
								if (Serializer.<>o__6.<>p__12 == null)
								{
									Serializer.<>o__6.<>p__12 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(Serializer)));
								}
								CS$<>8__locals5.<SerializeLChunk>g__WriteString|6(Serializer.<>o__6.<>p__12.Target(Serializer.<>o__6.<>p__12, Constant.Data));
							}
							break;
						}
						case ConstantType.Int16:
						{
							Serializer.<>c__DisplayClass6_0 CS$<>8__locals6 = CS$<>8__locals1;
							if (Serializer.<>o__6.<>p__8 == null)
							{
								Serializer.<>o__6.<>p__8 = CallSite<Func<CallSite, object, short>>.Create(Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof(short), typeof(Serializer)));
							}
							Func<CallSite, object, short> target = Serializer.<>o__6.<>p__8.Target;
							CallSite <>p__ = Serializer.<>o__6.<>p__8;
							if (Serializer.<>o__6.<>p__7 == null)
							{
								Serializer.<>o__6.<>p__7 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Add, typeof(Serializer), new CSharpArgumentInfo[]
								{
									CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
									CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, null)
								}));
							}
							CS$<>8__locals6.<SerializeLChunk>g__WriteInt16|3(target(<>p__, Serializer.<>o__6.<>p__7.Target(Serializer.<>o__6.<>p__7, Constant.Data, 256)));
							break;
						}
						case ConstantType.Int32:
						{
							Serializer.<>c__DisplayClass6_0 CS$<>8__locals7 = CS$<>8__locals1;
							if (Serializer.<>o__6.<>p__6 == null)
							{
								Serializer.<>o__6.<>p__6 = CallSite<Func<CallSite, object, short>>.Create(Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof(short), typeof(Serializer)));
							}
							Func<CallSite, object, short> target2 = Serializer.<>o__6.<>p__6.Target;
							CallSite <>p__2 = Serializer.<>o__6.<>p__6;
							if (Serializer.<>o__6.<>p__5 == null)
							{
								Serializer.<>o__6.<>p__5 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Add, typeof(Serializer), new CSharpArgumentInfo[]
								{
									CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
									CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, null)
								}));
							}
							CS$<>8__locals7.<SerializeLChunk>g__WriteInt32|2((int)target2(<>p__2, Serializer.<>o__6.<>p__5.Target(Serializer.<>o__6.<>p__5, Constant.Data, 65536)));
							break;
						}
						}
					}
					CS$<>8__locals1.<SerializeLChunk>g__WriteInt32|2(CS$<>8__locals1.Chunk.Instructions.Count);
					foreach (Instruction Instruction2 in CS$<>8__locals1.Chunk.Instructions)
					{
						CS$<>8__locals1.<SerializeLChunk>g__SerializeInstruction|8(Instruction2);
					}
					break;
				case ChunkStep.Chunks:
					CS$<>8__locals1.<SerializeLChunk>g__WriteInt32|2(CS$<>8__locals1.Chunk.Chunks.Count);
					foreach (Chunk SubChunk in CS$<>8__locals1.Chunk.Chunks)
					{
						this.SerializeLChunk(SubChunk, CS$<>8__locals1.Bytes);
					}
					break;
				case ChunkStep.StackSize:
					CS$<>8__locals1.<SerializeLChunk>g__WriteInt16|3((short)CS$<>8__locals1.Chunk.StackSize);
					break;
				}
			}
		}

		public List<byte> Serialize(Chunk HeadChunk)
		{
			List<byte> Bytes = new List<byte>();
			this.SerializeLChunk(HeadChunk, Bytes);
			return Bytes;
		}

		private ObfuscationContext ObfuscationContext;

		private ObfuscationSettings ObfuscationSettings;

		private Random Random = new Random();

		private Encoding LuaEncoding = Encoding.GetEncoding(28591);

		public List<string> Types = new List<string>();
	}
}
