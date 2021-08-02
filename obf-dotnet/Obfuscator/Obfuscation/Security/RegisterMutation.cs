using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using Microsoft.CSharp.RuntimeBinder;
using Obfuscator.Bytecode.IR;
using Obfuscator.Extensions;
using Obfuscator.Obfuscation.OpCodes;
using Obfuscator.Utility;

namespace Obfuscator.Obfuscation.Security
{
	public class RegisterMutation
	{
		public RegisterMutation(ObfuscationContext ObfuscationContext, Chunk Chunk, List<Instruction> Instructions, List<VOpCode> Virtuals, int Start, int End)
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
			RegisterMutation.<>c__DisplayClass8_0 CS$<>8__locals1 = new RegisterMutation.<>c__DisplayClass8_0();
			CS$<>8__locals1.<>4__this = this;
			List<BasicBlock> BasicBlocks = new BasicBlock().GenerateBasicBlocksFromInstructions(this.Chunk, this.Instructions);
			CS$<>8__locals1.Enum = this.Random.Next(0, 255);
			CS$<>8__locals1.A = this.Random.Next(0, 255);
			CS$<>8__locals1.B = this.Random.Next(0, 255);
			CS$<>8__locals1.C = this.Random.Next(0, 255);
			CS$<>8__locals1.nA = this.ObfuscationContext.NumberEquations.Keys.ToList<long>().Random<long>();
			CS$<>8__locals1.nB = this.ObfuscationContext.NumberEquations.Keys.ToList<long>().Random<long>();
			CS$<>8__locals1.nC = this.ObfuscationContext.NumberEquations.Keys.ToList<long>().Random<long>();
			CS$<>8__locals1.iA = this.Random.Next(0, 2);
			CS$<>8__locals1.iB = this.Random.Next(0, 2);
			CS$<>8__locals1.iC = this.Random.Next(0, 2);
			CS$<>8__locals1.Virtual = new OpCustom();
			this.Virtuals.Add(CS$<>8__locals1.Virtual);
			foreach (BasicBlock Block in BasicBlocks)
			{
				List<Instruction> InstructionList = Block.Instructions.ToList<Instruction>();
				using (List<Instruction>.Enumerator enumerator2 = InstructionList.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						RegisterMutation.<>c__DisplayClass8_1 CS$<>8__locals2 = new RegisterMutation.<>c__DisplayClass8_1();
						CS$<>8__locals2.CS$<>8__locals1 = CS$<>8__locals1;
						CS$<>8__locals2.Instruction = enumerator2.Current;
						bool References = false;
						foreach (object Reference in CS$<>8__locals2.Instruction.References)
						{
							if (RegisterMutation.<>o__8.<>p__1 == null)
							{
								RegisterMutation.<>o__8.<>p__1 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof(RegisterMutation), new CSharpArgumentInfo[]
								{
									CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
								}));
							}
							Func<CallSite, object, bool> target = RegisterMutation.<>o__8.<>p__1.Target;
							CallSite <>p__ = RegisterMutation.<>o__8.<>p__1;
							if (RegisterMutation.<>o__8.<>p__0 == null)
							{
								RegisterMutation.<>o__8.<>p__0 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.NotEqual, typeof(RegisterMutation), new CSharpArgumentInfo[]
								{
									CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
									CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant, null)
								}));
							}
							bool flag = target(<>p__, RegisterMutation.<>o__8.<>p__0.Target(RegisterMutation.<>o__8.<>p__0, Reference, null));
							if (flag)
							{
								References = true;
							}
						}
						bool flag2 = References;
						if (!flag2)
						{
							bool isConstantA = CS$<>8__locals2.Instruction.IsConstantA;
							if (!isConstantA)
							{
								bool isConstantB = CS$<>8__locals2.Instruction.IsConstantB;
								if (!isConstantB)
								{
									bool isConstantC = CS$<>8__locals2.Instruction.IsConstantC;
									if (!isConstantC)
									{
										bool ignoreInstruction = CS$<>8__locals2.Instruction.IgnoreInstruction;
										if (!ignoreInstruction)
										{
											bool flag3 = CS$<>8__locals2.Instruction == Block.Instructions.First<Instruction>();
											if (!flag3)
											{
												bool flag4 = CS$<>8__locals2.Instruction.RegisterHandler != null;
												if (!flag4)
												{
													bool flag5 = this.Random.Next(1, 5) != 0;
													if (!flag5)
													{
														CS$<>8__locals2.Instruction.IgnoreInstruction = true;
														Instruction rInstruction = new Instruction(this.Chunk, OpCode.Custom, Array.Empty<object>());
														rInstruction.IgnoreInstruction = true;
														rInstruction.CustomInstructionData.OpCode = CS$<>8__locals2.CS$<>8__locals1.Virtual;
														rInstruction.References[1] = CS$<>8__locals2.Instruction;
														rInstruction.RequiresCustomData = true;
														rInstruction.InstructionType = InstructionType.ABx;
														Block.Instructions.Insert(1, rInstruction);
														rInstruction.RegisterHandler = delegate(Instruction Self)
														{
															rInstruction.B = CS$<>8__locals2.CS$<>8__locals1.<>4__this.Chunk.InstructionMap[CS$<>8__locals2.Instruction];
															VOpCode NoOpCode;
															CS$<>8__locals2.CS$<>8__locals1.<>4__this.ObfuscationContext.InstructionMapping.TryGetValue(OpCode.None, out NoOpCode);
															rInstruction.RequiresCustomData = true;
															rInstruction.CustomData = new List<int>
															{
																CS$<>8__locals2.Instruction.CustomInstructionData.OpCode.VIndex ^ CS$<>8__locals2.CS$<>8__locals1.Enum,
																(CS$<>8__locals2.CS$<>8__locals1.iA == 0) ? (CS$<>8__locals2.Instruction.A ^ CS$<>8__locals2.CS$<>8__locals1.A) : ((int)CS$<>8__locals2.CS$<>8__locals1.<>4__this.ObfuscationContext.NumberEquations[CS$<>8__locals2.CS$<>8__locals1.nA].ComputeExpression((long)CS$<>8__locals2.Instruction.A)),
																(CS$<>8__locals2.CS$<>8__locals1.iB == 0) ? (CS$<>8__locals2.Instruction.B ^ CS$<>8__locals2.CS$<>8__locals1.B) : ((int)CS$<>8__locals2.CS$<>8__locals1.<>4__this.ObfuscationContext.NumberEquations[CS$<>8__locals2.CS$<>8__locals1.nB].ComputeExpression((long)CS$<>8__locals2.Instruction.B)),
																(CS$<>8__locals2.CS$<>8__locals1.iC == 0) ? (CS$<>8__locals2.Instruction.C ^ CS$<>8__locals2.CS$<>8__locals1.C) : ((int)CS$<>8__locals2.CS$<>8__locals1.<>4__this.ObfuscationContext.NumberEquations[CS$<>8__locals2.CS$<>8__locals1.nC].ComputeExpression((long)CS$<>8__locals2.Instruction.C))
															};
															CS$<>8__locals2.CS$<>8__locals1.Virtual.Obfuscated = string.Concat(new string[]
															{
																"\n\n\t\t\t\t\t\tlocal oInstruction = Instructions[Instruction[OP_B]];\n\t\t\t\t\t\tlocal D = Instruction[OP_D];\n\n\t\t\t\t\t\t",
																string.Join("\n", new List<string>
																{
																	string.Concat(new string[]
																	{
																		"oInstruction[OP_ENUM] = BitXOR(D[",
																		Utility.IntegerToString(1, 2),
																		"], ",
																		Utility.IntegerToString(CS$<>8__locals2.CS$<>8__locals1.Enum, 2),
																		");"
																	}),
																	"oInstruction[OP_A] = " + ((CS$<>8__locals2.CS$<>8__locals1.iA == 0) ? string.Concat(new string[]
																	{
																		"BitXOR(D[",
																		Utility.IntegerToString(2, 2),
																		"], ",
																		Utility.IntegerToString(CS$<>8__locals2.CS$<>8__locals1.A, 2),
																		");"
																	}) : string.Format("CalculateVM({0}, D[{1}])", CS$<>8__locals2.CS$<>8__locals1.nA, Utility.IntegerToString(2, 2))),
																	"oInstruction[OP_B] = " + ((CS$<>8__locals2.CS$<>8__locals1.iB == 0) ? string.Concat(new string[]
																	{
																		"BitXOR(D[",
																		Utility.IntegerToString(3, 2),
																		"], ",
																		Utility.IntegerToString(CS$<>8__locals2.CS$<>8__locals1.B, 2),
																		");"
																	}) : string.Format("CalculateVM({0}, D[{1}])", CS$<>8__locals2.CS$<>8__locals1.nB, Utility.IntegerToString(3, 2))),
																	"oInstruction[OP_C] = " + ((CS$<>8__locals2.CS$<>8__locals1.iC == 0) ? string.Concat(new string[]
																	{
																		"BitXOR(D[",
																		Utility.IntegerToString(4, 2),
																		"], ",
																		Utility.IntegerToString(CS$<>8__locals2.CS$<>8__locals1.C, 2),
																		");"
																	}) : string.Format("CalculateVM({0}, D[{1}])", CS$<>8__locals2.CS$<>8__locals1.nC, Utility.IntegerToString(4, 2)))
																}.Shuffle<string>()),
																"\n\n\t\t\t\t\t\tInstruction[OP_ENUM] = (",
																Utility.IntegerToString(NoOpCode.VIndex, 0),
																");\n\n\t\t\t\t\t\t"
															});
															rInstruction.InstructionType = InstructionType.ABx;
															return rInstruction;
														};
														CS$<>8__locals2.Instruction.RegisterHandler = delegate(Instruction _)
														{
															CS$<>8__locals2.Instruction.WrittenVIndex = new int?(0);
															CS$<>8__locals2.Instruction.A = 0;
															CS$<>8__locals2.Instruction.B = 0;
															CS$<>8__locals2.Instruction.C = 0;
															return CS$<>8__locals2.Instruction;
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
			new IMutateCFlow(this.ObfuscationContext, this.Chunk, IList, this.Virtuals, this.Start, this.End).DoInstructions();
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
