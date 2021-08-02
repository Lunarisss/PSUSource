using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.CSharp.RuntimeBinder;
using Obfuscator.Bytecode.IR;
using Obfuscator.Extensions;

namespace Obfuscator.Utility
{
	public static class Utility
	{
		[DllImport("ObfuscatorDLL.dll")]
		private static extern double StringToDouble(string String, ref bool Success);

		public static void VoidInstruction(Instruction Instruction)
		{
			Instruction.OpCode = OpCode.None;
			Instruction.A = 0;
			Instruction.B = 0;
			Instruction.C = 0;
			Instruction.ConstantType = InstructionConstantType.NK;
			Instruction.InstructionType = InstructionType.ABC;
			foreach (object Reference in Instruction.References)
			{
				Instruction ReferencedInstruction = Reference as Instruction;
				bool flag = ReferencedInstruction != null;
				if (flag)
				{
					ReferencedInstruction.BackReferences.Remove(Instruction);
				}
				else
				{
					Constant ReferencedConstant = Reference as Constant;
					bool flag2 = ReferencedConstant != null;
					if (flag2)
					{
						ReferencedConstant.BackReferences.Remove(Instruction);
					}
				}
			}
			Instruction.References = new List<object>
			{
				null,
				null,
				null,
				null,
				null
			};
		}

		public static void SwapBackReferences(Instruction Original, Instruction Instruction)
		{
			foreach (Instruction BackReference in Original.BackReferences)
			{
				Instruction.BackReferences.Add(BackReference);
				bool flag = BackReference.JumpTo == Original;
				if (flag)
				{
					BackReference.JumpTo = Instruction;
				}
				else
				{
					BackReference.References[BackReference.References.IndexOf(Original)] = Instruction;
				}
			}
			Original.BackReferences.Clear();
		}

		public static Constant GetOrAddConstant(Chunk Chunk, ConstantType Type, dynamic Constant, out int ConstantIndex)
		{
			Constant Current = Chunk.Constants.FirstOrDefault(delegate(Constant C)
			{
				if (Utility.<>o__4.<>p__2 == null)
				{
					Utility.<>o__4.<>p__2 = CallSite<Func<CallSite, object, bool>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(bool), typeof(Utility)));
				}
				Func<CallSite, object, bool> target = Utility.<>o__4.<>p__2.Target;
				CallSite <>p__ = Utility.<>o__4.<>p__2;
				bool flag2 = C.Type == Type;
				object arg2;
				if (flag2)
				{
					if (Utility.<>o__4.<>p__1 == null)
					{
						Utility.<>o__4.<>p__1 = CallSite<Func<CallSite, bool, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.BinaryOperationLogical, ExpressionType.And, typeof(Utility), new CSharpArgumentInfo[]
						{
							CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null),
							CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
						}));
					}
					Func<CallSite, bool, object, object> target2 = Utility.<>o__4.<>p__1.Target;
					CallSite <>p__2 = Utility.<>o__4.<>p__1;
					bool arg = flag2;
					if (Utility.<>o__4.<>p__0 == null)
					{
						Utility.<>o__4.<>p__0 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Equal, typeof(Utility), new CSharpArgumentInfo[]
						{
							CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
							CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
						}));
					}
					arg2 = target2(<>p__2, arg, Utility.<>o__4.<>p__0.Target(Utility.<>o__4.<>p__0, C.Data, Constant));
				}
				else
				{
					arg2 = flag2;
				}
				return target(<>p__, arg2);
			});
			bool flag = Current != null;
			Constant result;
			if (flag)
			{
				ConstantIndex = Chunk.Constants.IndexOf(Current);
				result = Current;
			}
			else
			{
				Constant nConstant = new Constant
				{
					Type = Type,
					Data = Constant
				};
				ConstantIndex = Chunk.Constants.Count;
				Chunk.Constants.Add(nConstant);
				Chunk.ConstantMap.Add(nConstant, ConstantIndex);
				result = nConstant;
			}
			return result;
		}

		public static int[] GetRegistersThatInstructionWritesTo(Instruction Instruction, int StackTop = 256)
		{
			int A = Instruction.A;
			int B = Instruction.B;
			int C = Instruction.C;
			switch (Instruction.OpCode)
			{
			case OpCode.OpMove:
				return new int[]
				{
					A
				};
			case OpCode.OpLoadK:
				return new int[]
				{
					A
				};
			case OpCode.OpLoadBool:
				return new int[]
				{
					A
				};
			case OpCode.OpLoadNil:
				return Enumerable.Range(A, B - A + 1).ToArray<int>();
			case OpCode.OpGetUpValue:
				return new int[]
				{
					A
				};
			case OpCode.OpGetGlobal:
				return new int[]
				{
					A
				};
			case OpCode.OpGetTable:
				return new int[]
				{
					A
				};
			case OpCode.OpNewTable:
				return new int[]
				{
					A
				};
			case OpCode.OpSelf:
				return new int[]
				{
					A,
					A + 1
				};
			case OpCode.OpAdd:
				return new int[]
				{
					A
				};
			case OpCode.OpSub:
				return new int[]
				{
					A
				};
			case OpCode.OpMul:
				return new int[]
				{
					A
				};
			case OpCode.OpDiv:
				return new int[]
				{
					A
				};
			case OpCode.OpMod:
				return new int[]
				{
					A
				};
			case OpCode.OpPow:
				return new int[]
				{
					A
				};
			case OpCode.OpUnm:
				return new int[]
				{
					A
				};
			case OpCode.OpNot:
				return new int[]
				{
					A
				};
			case OpCode.OpLen:
				return new int[]
				{
					A
				};
			case OpCode.OpConcat:
				return new int[]
				{
					A
				};
			case OpCode.OpTestSet:
				return new int[]
				{
					A
				};
			case OpCode.OpCall:
				if (C >= 2)
				{
					return Enumerable.Range(A, C + 1).ToArray<int>();
				}
				if (C == 0)
				{
					return new int[]
					{
						A
					};
				}
				break;
			case OpCode.OpForLoop:
				return new int[]
				{
					A,
					A + 3
				};
			case OpCode.OpForPrep:
				return new int[]
				{
					A
				};
			case OpCode.OpTForLoop:
				return Enumerable.Range(A + 2, A + 2 + C - A + 1).ToArray<int>();
			case OpCode.OpClose:
				return Enumerable.Range(0, StackTop).ToArray<int>();
			case OpCode.OpClosure:
				return new int[]
				{
					A
				};
			case OpCode.OpVarArg:
				if (B >= 2)
				{
					return Enumerable.Range(A, B - 1).ToArray<int>();
				}
				if (B == 0)
				{
					return new int[]
					{
						A
					};
				}
				break;
			}
			return new int[0];
		}

		public static int[] GetRegistersThatInstructionReadsFrom(Instruction Instruction, int StackTop = 256)
		{
			int A = Instruction.A;
			int B = Instruction.B;
			int C = Instruction.C;
			switch (Instruction.OpCode)
			{
			case OpCode.OpMove:
				return new int[]
				{
					B
				};
			case OpCode.OpGetTable:
			{
				List<int> List = new List<int>
				{
					B
				};
				bool flag = C < 1024;
				if (flag)
				{
					List.Add(C);
				}
				return List.ToArray();
			}
			case OpCode.OpSetGlobal:
				return new int[]
				{
					A
				};
			case OpCode.OpSetUpValue:
				return new int[]
				{
					A
				};
			case OpCode.OpSetTable:
			{
				List<int> List2 = new List<int>
				{
					A
				};
				bool flag2 = B < 1024;
				if (flag2)
				{
					List2.Add(B);
				}
				bool flag3 = C < 1024;
				if (flag3)
				{
					List2.Add(C);
				}
				return List2.ToArray();
			}
			case OpCode.OpSelf:
			{
				int[] List3 = new int[]
				{
					B
				};
				bool flag4 = C < 1024;
				if (flag4)
				{
					List3[1] = C;
				}
				return List3.ToArray<int>();
			}
			case OpCode.OpAdd:
			{
				List<int> List4 = new List<int>();
				bool flag5 = B < 1024;
				if (flag5)
				{
					List4.Add(B);
				}
				bool flag6 = C < 1024;
				if (flag6)
				{
					List4.Add(C);
				}
				return List4.ToArray();
			}
			case OpCode.OpSub:
			{
				List<int> List5 = new List<int>();
				bool flag7 = B < 1024;
				if (flag7)
				{
					List5.Add(B);
				}
				bool flag8 = C < 1024;
				if (flag8)
				{
					List5.Add(C);
				}
				return List5.ToArray();
			}
			case OpCode.OpMul:
			{
				List<int> List6 = new List<int>();
				bool flag9 = B < 1024;
				if (flag9)
				{
					List6.Add(B);
				}
				bool flag10 = C < 1024;
				if (flag10)
				{
					List6.Add(C);
				}
				return List6.ToArray();
			}
			case OpCode.OpDiv:
			{
				List<int> List7 = new List<int>();
				bool flag11 = B < 1024;
				if (flag11)
				{
					List7.Add(B);
				}
				bool flag12 = C < 1024;
				if (flag12)
				{
					List7.Add(C);
				}
				return List7.ToArray();
			}
			case OpCode.OpMod:
			{
				List<int> List8 = new List<int>();
				bool flag13 = B < 1024;
				if (flag13)
				{
					List8.Add(B);
				}
				bool flag14 = C < 1024;
				if (flag14)
				{
					List8.Add(C);
				}
				return List8.ToArray();
			}
			case OpCode.OpPow:
			{
				List<int> List9 = new List<int>();
				bool flag15 = B < 1024;
				if (flag15)
				{
					List9.Add(B);
				}
				bool flag16 = C < 1024;
				if (flag16)
				{
					List9.Add(C);
				}
				return List9.ToArray();
			}
			case OpCode.OpUnm:
				return new int[]
				{
					B
				};
			case OpCode.OpNot:
				return new int[]
				{
					B
				};
			case OpCode.OpLen:
				return new int[]
				{
					B
				};
			case OpCode.OpConcat:
				return Enumerable.Range(B, C - B + 1).ToArray<int>();
			case OpCode.OpEq:
			{
				List<int> List10 = new List<int>();
				bool flag17 = B < 1024;
				if (flag17)
				{
					List10.Add(B);
				}
				bool flag18 = C < 1024;
				if (flag18)
				{
					List10.Add(C);
				}
				return List10.ToArray();
			}
			case OpCode.OpLt:
			{
				List<int> List11 = new List<int>();
				bool flag19 = B < 1024;
				if (flag19)
				{
					List11.Add(B);
				}
				bool flag20 = C < 1024;
				if (flag20)
				{
					List11.Add(C);
				}
				return List11.ToArray();
			}
			case OpCode.OpLe:
			{
				List<int> List12 = new List<int>();
				bool flag21 = B < 1024;
				if (flag21)
				{
					List12.Add(B);
				}
				bool flag22 = C < 1024;
				if (flag22)
				{
					List12.Add(C);
				}
				return List12.ToArray();
			}
			case OpCode.OpTest:
				return new int[]
				{
					A
				};
			case OpCode.OpTestSet:
				return new int[]
				{
					A,
					B
				};
			case OpCode.OpCall:
				if (B >= 2)
				{
					return Enumerable.Range(A, B).ToArray<int>();
				}
				if (B == 0)
				{
					return Enumerable.Range(A, StackTop - A).ToArray<int>();
				}
				if (B == 1)
				{
					return new int[]
					{
						A
					};
				}
				break;
			case OpCode.OpTailCall:
				if (B == 0)
				{
					return Enumerable.Range(A, StackTop - A).ToArray<int>();
				}
				if (B >= 2)
				{
					return Enumerable.Range(A, B).ToArray<int>();
				}
				break;
			case OpCode.OpReturn:
				if (B >= 2)
				{
					return Enumerable.Range(A, B - 1).ToArray<int>();
				}
				if (B == 0)
				{
					return Enumerable.Range(A, StackTop - A).ToArray<int>();
				}
				break;
			case OpCode.OpForLoop:
				return new int[]
				{
					A,
					A + 1,
					A + 2,
					A + 3
				};
			case OpCode.OpForPrep:
				return new int[]
				{
					A + 2
				};
			case OpCode.OpTForLoop:
				return new int[]
				{
					A,
					A + 1,
					A + 2,
					A + 3
				};
			case OpCode.OpSetList:
				return Enumerable.Range(A, StackTop - A).ToArray<int>();
			}
			return new int[0];
		}

		public static void GetExtraStrings(string source)
		{
			Utility.NoExtraString = false;
			Utility.OverrideStrings = false;
			Utility.extra.Clear();
			Match mtch = Regex.Match(source, "^\\s*--\\[(=*)\\[([\\S\\s]*?)\\]\\1\\]");
			bool flag = !mtch.Success;
			if (!flag)
			{
				bool flag2 = mtch.Groups.Count < 3;
				if (!flag2)
				{
					string comment = mtch.Groups[2].Value.Trim();
					string[] lines = comment.Split('\n', StringSplitOptions.None);
					bool flag3 = lines.Length == 0;
					if (!flag3)
					{
						string line = lines[0].Trim().ToLower();
						bool flag4 = line != "strings" && line != "strings-override";
						if (!flag4)
						{
							bool flag5 = line == "strings-override";
							if (flag5)
							{
								Utility.OverrideStrings = true;
							}
							for (int i = 1; i < lines.Length; i++)
							{
								string line2 = lines[i].Trim();
								Utility.extra.Add(line2);
							}
						}
					}
				}
			}
		}

		public static int CompatLength(string str)
		{
			return Encoding.ASCII.GetString(Encoding.Default.GetBytes(str)).Length;
		}

		public static string FinalReplaceStrings(string source)
		{
			return Regex.Replace(source, "EXTRASTRING(\\d+)", (Match mtch) => Utility.extra[int.Parse(mtch.Groups[1].Value)].Replace("\"", "\\\""));
		}

		public static string IntegerToHex(int Integer)
		{
			return "0x" + Integer.ToString("X3");
		}

		public static string IntegerToString(int Integer, int Minimum = 0)
		{
			string result;
			switch (Utility.Random.Next(Minimum, 4))
			{
			case 0:
				result = Integer.ToString();
				break;
			case 1:
				result = Utility.IntegerToHex(Integer);
				break;
			case 2:
				result = Utility.IntegerToTable(Integer);
				break;
			case 3:
			{
				Random rand = new Random();
				bool flag = Utility.extra.Count == 0 || (Utility.extra.Count == 1 && Utility.OverrideStrings) || Utility.NoExtraString;
				if (flag)
				{
					string String = Utility.VMStrings.Random<string>();
					result = string.Format("({0} - #(\"{1}\"))", Integer + String.Length, String);
				}
				else
				{
					bool overrideStrings = Utility.OverrideStrings;
					if (overrideStrings)
					{
						int idx = rand.Next(0, Utility.extra.Count);
						string String2 = Utility.extra[idx];
						result = string.Format("({0} - #(\"EXTRASTRING{1}\"))", Integer + Utility.CompatLength(String2), idx);
					}
					else
					{
						bool flag2 = rand.Next(0, 2) == 0;
						if (flag2)
						{
							int idx2 = rand.Next(0, Utility.extra.Count);
							string String3 = Utility.extra[idx2];
							result = string.Format("({0} - #(\"EXTRASTRING{1}\"))", Integer + Utility.CompatLength(String3), idx2);
						}
						else
						{
							string String4 = Utility.VMStrings.Random<string>();
							result = string.Format("({0} - #(\"{1}\"))", Integer + String4.Length, String4);
						}
					}
				}
				break;
			}
			default:
				result = Integer.ToString();
				break;
			}
			return result;
		}

		public static string IntegerToStringBasic(int Integer)
		{
			int num = Utility.Random.Next(0, 2);
			int num2 = num;
			string result;
			if (num2 != 0)
			{
				if (num2 != 1)
				{
					result = Integer.ToString();
				}
				else
				{
					result = Utility.IntegerToHex(Integer);
				}
			}
			else
			{
				result = Integer.ToString();
			}
			return result;
		}

		public static string IntegerToTable(int Value)
		{
			string Table = "(#{";
			int Values = Utility.Random.Next(0, 5);
			Value -= Values;
			for (int Count = 0; Count < Values; Count++)
			{
				Table = Table + Utility.IntegerToStringBasic(Utility.Random.Next(0, 1000)) + ";";
			}
			bool flag = Utility.Random.Next(0, 2) == 0;
			if (flag)
			{
				int ReturnValues = Utility.Random.Next(0, 5);
				int ReturnCount = 0;
				Value -= ReturnValues;
				Table += "(function(...)return ";
				while (ReturnCount < ReturnValues)
				{
					Table += Utility.IntegerToStringBasic(Utility.Random.Next(0, 1000));
					bool flag2 = ReturnCount < ReturnValues - 1;
					if (flag2)
					{
						Table += ",";
					}
					ReturnCount++;
				}
				bool HasVarArg = Utility.Random.Next(0, 3) == 0;
				bool flag3 = HasVarArg;
				if (flag3)
				{
					bool flag4 = ReturnValues > 0;
					if (flag4)
					{
						Table += ",";
					}
					Table += "...";
				}
				Table += ";end)(";
				bool flag5 = HasVarArg;
				if (flag5)
				{
					int VarArgValues = Utility.Random.Next(0, 5);
					int VarArgCount = 0;
					Value -= VarArgValues;
					while (VarArgCount < VarArgValues)
					{
						Table += Utility.IntegerToStringBasic(Utility.Random.Next(0, 1000));
						bool flag6 = VarArgCount < VarArgValues - 1;
						if (flag6)
						{
							Table += ",";
						}
						VarArgCount++;
					}
				}
				Table += ")";
			}
			return string.Concat(new string[]
			{
				Table,
				"}",
				(Math.Sign(Value) < 0) ? " - " : " + ",
				Utility.IntegerToStringBasic(Math.Abs(Value)),
				")"
			});
		}

		public static List<string> GetIndexList()
		{
			List<string> Indicies = new List<string>();
			int num = Utility.Random.Next(0, 2);
			int num2 = num;
			if (num2 != 0)
			{
				if (num2 == 1)
				{
					int Length = Utility.Random.Next(4, 10);
					string Index = Utility.Characters.Random<char>().ToString();
					for (int I = 0; I < Length; I++)
					{
						Index += Utility.AlphaNumeric.Random<char>().ToString();
					}
					Indicies.Add("." + Index);
					Indicies.Add("[\"" + Index + "\"]");
					Indicies.Add("['" + Index + "']");
				}
			}
			else
			{
				double Index2 = (double)Utility.Random.Next(0, 1000000000);
				Indicies.Add(string.Format("[{0}]", Index2));
			}
			return Indicies;
		}

		public static List<string> GetIndexListNoBrackets()
		{
			List<string> Indicies = new List<string>();
			int num = Utility.Random.Next(0, 2);
			int num2 = num;
			if (num2 != 0)
			{
				if (num2 == 1)
				{
					int Length = Utility.Random.Next(2, 10);
					string Index = Utility.Characters.Random<char>().ToString();
					for (int I = 0; I < Length; I++)
					{
						Index += Utility.AlphaNumeric.Random<char>().ToString();
					}
					Indicies.Add("\"" + Index + "\"");
					Indicies.Add("'" + Index + "'");
				}
			}
			else
			{
				double Index2 = 0.0;
				Index2 += (double)Utility.Random.Next(0, 1000000);
				bool flag = Utility.Random.Next(0, 4) == 0;
				if (flag)
				{
					Index2 += Utility.Random.NextDouble();
				}
				bool flag2 = Utility.Random.Next(0, 2) == 0;
				if (flag2)
				{
					Index2 = -Index2;
				}
				Indicies.Add(string.Format("{0}", Index2));
			}
			return Indicies;
		}

		private static List<string> VMStrings = new List<string>
		{
			"'psu > luraph' - memcorrupt 2020",
			"ililililililili guys look at me i'm intimidating",
			"psu 60fps, luraph 5fps, xen 0fps",
			"psu premium chads winning (only losers use the free version)",
			"woooow u hooked an opcode, congratulations! i do NOT give a fuck.",
			"you dumped constants by printing the deserializer??? ladies and gentlemen stand clear we have a genius in the building.",
			"ironbrew deobfuscator go brrrrrrrrrrrrrr",
			"LuraphDeobfuscator.zip (oh god DMCA incoming everyone hide)",
			"I'm not ignoring you, my DMs are full. Can't DM me? Shoot me a email: mem@mem.rip (Business enquiries only)",
			"Luraph: Probably considered the worst out of the three, Luraph is another Lua Obfuscator. It isnt remotely as secure as Ironbrew or Synapse Xen, and it isn't as fast as Ironbrew either.",
			"Are you using AztupBrew, clvbrew, or IB2? Congratulations! You're deobfuscated!",
			"this isn't krnl support you bonehead moron",
			"why the fuck would we sell a deobfuscator for a product we created.....",
			"i am not wally stop asking me for wally hub support please fuck off",
			"guys someone play Among Us with memcorrupt he is so lonely :(",
			"Luraph v12.6 has been released!: changed absolutely fucking nothing but donate to my patreon!",
			"@everyone designs are done. luraph website coming.... eta JULY 2020",
			"why does psu.dev attract so many ddosing retards wtf",
			"still waiting for luci to fix the API :|",
			"luraph is now down until further notice for an emergency major security update",
			"who the fuck looked at synapse xen and said 'yeah this is good enough for release'",
			"uh oh everyone watch out pain exist coming in with the backspace method one dot two dot man dot",
			"oh Mr. Pools, thats a little close please dont touch me there... please Mr. Pools I am only eight years old please stop..."
		};

		public static Random Random = new Random();

		public static List<char> HexDecimal = "abcdefABCDEF0123456789".ToCharArray().ToList<char>();

		public static List<char> Decimal = "0123456789".ToCharArray().ToList<char>();

		public static List<char> Characters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJLMNOPQRSTUVWXYZ".ToCharArray().ToList<char>();

		public static List<char> AlphaNumeric = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJLMNOPQRSTUVWXYZ01234567890".ToCharArray().ToList<char>();

		public static bool OverrideStrings = false;

		public static bool NoExtraString = false;

		public static List<string> extra = new List<string>();
	}
}
