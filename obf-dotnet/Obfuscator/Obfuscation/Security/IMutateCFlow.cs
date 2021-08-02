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
	public class IMutateCFlow
	{
		public IMutateCFlow(ObfuscationContext ObfuscationContext, Chunk Chunk, List<Instruction> Instructions, List<VOpCode> Virtuals, int Start, int End)
		{
			this.ObfuscationContext = ObfuscationContext;
			this.Chunk = Chunk;
			this.Instructions = Instructions;
			this.Virtuals = Virtuals;
			this.Start = Start;
			this.End = End;
		}

		public void DoInstructions()
		{
			IMutateCFlow.<>c__DisplayClass8_0 CS$<>8__locals1 = new IMutateCFlow.<>c__DisplayClass8_0();
			CS$<>8__locals1.<>4__this = this;
			OpCustom SetVMKey = new OpCustom();
			SetVMKey.Obfuscated = "VMKey = Instruction[OP_A];";
			this.Virtuals.Add(SetVMKey);
			CS$<>8__locals1.Virtual = new OpCustom();
			this.Virtuals.Add(CS$<>8__locals1.Virtual);
			List<BasicBlock> BasicBlocks = new BasicBlock().GenerateBasicBlocksFromInstructions(this.Chunk, this.Instructions);
			List<BasicBlock> ProcessedBlocks = new List<BasicBlock>();
			foreach (BasicBlock Block in BasicBlocks)
			{
				bool flag = Block.References.Count > 0;
				if (flag)
				{
					IMutateCFlow.<>c__DisplayClass8_1 CS$<>8__locals2 = new IMutateCFlow.<>c__DisplayClass8_1();
					CS$<>8__locals2.CS$<>8__locals1 = CS$<>8__locals1;
					bool Continue = true;
					foreach (BasicBlock Reference in Block.References)
					{
						bool flag2 = Reference.BackReferences.Count != 1;
						if (flag2)
						{
							Continue = false;
						}
					}
					bool flag3 = !Continue;
					if (!flag3)
					{
						CS$<>8__locals2.Key = this.Random.Next(0, 256);
						Block.Instructions.Insert(Block.Instructions.Count - 1, new Instruction(this.Chunk, OpCode.Custom, Array.Empty<object>())
						{
							A = CS$<>8__locals2.Key,
							CustomInstructionData = new CustomInstructionData
							{
								OpCode = SetVMKey
							}
						});
						foreach (BasicBlock Reference2 in Block.References)
						{
							List<Instruction> InstructionList = Reference2.Instructions.ToList<Instruction>();
							using (List<Instruction>.Enumerator enumerator4 = InstructionList.GetEnumerator())
							{
								while (enumerator4.MoveNext())
								{
									IMutateCFlow.<>c__DisplayClass8_2 CS$<>8__locals3 = new IMutateCFlow.<>c__DisplayClass8_2();
									CS$<>8__locals3.CS$<>8__locals2 = CS$<>8__locals2;
									CS$<>8__locals3.Instruction = enumerator4.Current;
									bool References = false;
									foreach (object iReference in CS$<>8__locals3.Instruction.References)
									{
										if (IMutateCFlow.<>o__8.<>p__1 == null)
										{
											IMutateCFlow.<>o__8.<>p__1 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof(IMutateCFlow), new CSharpArgumentInfo[]
											{
												CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
											}));
										}
										Func<CallSite, object, bool> target = IMutateCFlow.<>o__8.<>p__1.Target;
										CallSite <>p__ = IMutateCFlow.<>o__8.<>p__1;
										if (IMutateCFlow.<>o__8.<>p__0 == null)
										{
											IMutateCFlow.<>o__8.<>p__0 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.NotEqual, typeof(IMutateCFlow), new CSharpArgumentInfo[]
											{
												CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
												CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant, null)
											}));
										}
										bool flag4 = target(<>p__, IMutateCFlow.<>o__8.<>p__0.Target(IMutateCFlow.<>o__8.<>p__0, iReference, null));
										if (flag4)
										{
											References = true;
										}
									}
									bool flag5 = References;
									if (!flag5)
									{
										bool isConstantA = CS$<>8__locals3.Instruction.IsConstantA;
										if (!isConstantA)
										{
											bool isConstantB = CS$<>8__locals3.Instruction.IsConstantB;
											if (!isConstantB)
											{
												bool isConstantC = CS$<>8__locals3.Instruction.IsConstantC;
												if (!isConstantC)
												{
													bool ignoreInstruction = CS$<>8__locals3.Instruction.IgnoreInstruction;
													if (!ignoreInstruction)
													{
														bool flag6 = CS$<>8__locals3.Instruction == Reference2.Instructions.First<Instruction>();
														if (!flag6)
														{
															bool flag7 = CS$<>8__locals3.Instruction.RegisterHandler != null;
															if (!flag7)
															{
																CS$<>8__locals3.Instruction.IgnoreInstruction = true;
																Instruction rInstruction = new Instruction(this.Chunk, OpCode.Custom, Array.Empty<object>());
																rInstruction.IgnoreInstruction = true;
																rInstruction.CustomInstructionData.OpCode = CS$<>8__locals3.CS$<>8__locals2.CS$<>8__locals1.Virtual;
																rInstruction.References[1] = CS$<>8__locals3.Instruction;
																rInstruction.InstructionType = InstructionType.ABx;
																Reference2.Instructions.Insert(1, rInstruction);
																rInstruction.RegisterHandler = delegate(Instruction Self)
																{
																	rInstruction.B = CS$<>8__locals3.CS$<>8__locals2.CS$<>8__locals1.<>4__this.Chunk.InstructionMap[CS$<>8__locals3.Instruction];
																	VOpCode NoOpCode;
																	CS$<>8__locals3.CS$<>8__locals2.CS$<>8__locals1.<>4__this.ObfuscationContext.InstructionMapping.TryGetValue(OpCode.None, out NoOpCode);
																	CS$<>8__locals3.CS$<>8__locals2.CS$<>8__locals1.Virtual.Obfuscated = "local oInstruction = Instructions[Instruction[OP_B]]; oInstruction[OP_ENUM] = BitXOR(oInstruction[OP_ENUM], VMKey); Instruction[OP_ENUM] = (" + Utility.IntegerToString(NoOpCode.VIndex, 0) + ");";
																	rInstruction.InstructionType = InstructionType.ABx;
																	return rInstruction;
																};
																CS$<>8__locals3.Instruction.RegisterHandler = delegate(Instruction _)
																{
																	CS$<>8__locals3.Instruction.WrittenVIndex = new int?(CS$<>8__locals3.Instruction.CustomInstructionData.OpCode.VIndex ^ CS$<>8__locals3.CS$<>8__locals2.Key);
																	return CS$<>8__locals3.Instruction;
																};
															}
														}
													}
												}
											}
										}
									}
								}
							}
						}
					}
				}
			}
			List<Instruction> Before = this.Chunk.Instructions.Take(this.Start).ToList<Instruction>();
			List<Instruction> After = this.Chunk.Instructions.Skip(this.End).ToList<Instruction>();
			this.Chunk.Instructions.Clear();
			this.Chunk.Instructions.AddRange(Before);
			foreach (BasicBlock Block2 in BasicBlocks)
			{
				foreach (Instruction Instruction in Block2.Instructions)
				{
					this.Chunk.Instructions.Add(Instruction);
				}
			}
			this.Chunk.Instructions.AddRange(After);
			this.Chunk.UpdateMappings();
			this.End = this.Chunk.InstructionMap[BasicBlocks.Last<BasicBlock>().Instructions.Last<Instruction>()];
			List<Instruction> IList = this.Chunk.Instructions.Skip(this.Start).Take(this.End - this.Start).ToList<Instruction>();
		}

		private Random Random = new Random();

		private Chunk Chunk;

		private List<Instruction> Instructions;

		private List<VOpCode> Virtuals;

		private ObfuscationContext ObfuscationContext;

		private int Start;

		private int End;
	}
}
