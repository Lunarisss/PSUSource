using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using Microsoft.CSharp.RuntimeBinder;

namespace LuaGeneration
{
	public static class LuaGeneration
	{
		private static string GenerateVariableName(int Length = 0)
		{
			Length = ((Length > 0) ? Length : LuaGeneration.DEFAULT_VARIABLE_NAME_LENGTH);
			string String = LuaGeneration.Alphabet[LuaGeneration.Random.Next(0, LuaGeneration.Alphabet.Length)].ToString();
			for (int I = 0; I < Length - 1; I++)
			{
				String += LuaGeneration.Alphanumeric[LuaGeneration.Random.Next(0, LuaGeneration.Alphanumeric.Length)].ToString();
			}
			return String;
		}

		private static string GenerateNumber(int Length = 5, bool Integer = true)
		{
			Integer = true;
			string String = "";
			for (int I = 0; I < Length; I++)
			{
				String += LuaGeneration.Numbers[LuaGeneration.Random.Next(0, LuaGeneration.Numbers.Length)].ToString();
			}
			bool flag = !Integer;
			if (flag)
			{
				String += ".";
				for (int I2 = 0; I2 < Length; I2++)
				{
					String += LuaGeneration.Numbers[LuaGeneration.Random.Next(0, LuaGeneration.Numbers.Length)].ToString();
				}
			}
			return String;
		}

		private static string GenerateHexadecimal(int Length = 5)
		{
			string String = "0x";
			for (int I = 0; I < Length; I++)
			{
				String += LuaGeneration.Hexadecimal[LuaGeneration.Random.Next(0, LuaGeneration.Hexadecimal.Length)].ToString();
			}
			return String;
		}

		private static string GenerateString(int MaximumLength = 100)
		{
			string String = "";
			for (int I = 0; I < LuaGeneration.Random.Next(1, MaximumLength); I++)
			{
				String += LuaGeneration.Characters[LuaGeneration.Random.Next(0, LuaGeneration.Characters.Length)].ToString();
			}
			string result;
			switch (LuaGeneration.Random.Next(0, 3))
			{
			case 0:
				result = "\"" + String + "\"";
				break;
			case 1:
				result = "'" + String + "'";
				break;
			case 2:
			{
				string Chunk = new string('=', LuaGeneration.Random.Next(0, 10));
				result = string.Concat(new string[]
				{
					"[",
					Chunk,
					"[",
					String,
					"]",
					Chunk,
					"]"
				});
				break;
			}
			default:
				result = String;
				break;
			}
			return result;
		}

		private static string GenerateTable(int Depth = 0)
		{
			bool flag = Depth > 5;
			string result;
			if (flag)
			{
				result = "{}";
			}
			else
			{
				string String = "{";
				for (int I = 0; I < LuaGeneration.Random.Next(0, 10); I++)
				{
					int num = LuaGeneration.Random.Next(0, 2);
					int num2 = num;
					if (num2 != 0)
					{
						if (num2 == 1)
						{
							String = string.Concat(new string[]
							{
								String,
								"[(",
								LuaGeneration.GetRandomValue(Depth + 1, null),
								")] = ",
								LuaGeneration.GetRandomValue(Depth + 1, null),
								";"
							});
						}
					}
					else
					{
						String = String + LuaGeneration.GetRandomValue(Depth + 1, null) + ";";
					}
				}
				result = String + "}";
			}
			return result;
		}

		private static string GetEquation(int Depth = 0)
		{
			string String = LuaGeneration.GetRandomValue(Depth + 1, null) + LuaGeneration.Operators[LuaGeneration.Random.Next(0, LuaGeneration.Operators.Length)] + LuaGeneration.GetRandomValue(Depth + 1, null);
			bool flag = Depth <= 5;
			if (flag)
			{
				String = String + LuaGeneration.Operators[LuaGeneration.Random.Next(0, LuaGeneration.Operators.Length)] + LuaGeneration.GetEquation(Depth + 1);
			}
			return String;
		}

		private static string GenerateFunction(int Depth = 0)
		{
			bool flag = Depth > 5;
			string result;
			if (flag)
			{
				result = "(function(...) return; end)";
			}
			else
			{
				int VariableCount = LuaGeneration.Variables.Count;
				string String = "(function(";
				int Parameters = LuaGeneration.Random.Next(0, 10);
				for (int I = 0; I < Parameters; I++)
				{
					string Parameter = LuaGeneration.GenerateVariableName(0);
					String = String + Parameter + ", ";
					LuaGeneration.Variables.Add(Parameter);
				}
				String += "...)";
				LuaGeneration.GenerateBody(Depth + 1);
				String += "return ";
				int R = LuaGeneration.Random.Next(0, 10);
				for (int I2 = 0; I2 < R; I2++)
				{
					String = String + LuaGeneration.GetRandomValue(Depth + 1, null) + ((I2 < R - 1) ? ", " : "");
				}
				LuaGeneration.Variables.RemoveRange(VariableCount, LuaGeneration.Variables.Count - VariableCount);
				result = String + "; end)";
			}
			return result;
		}

		private static string GetRandomValue(int Depth = 0, dynamic Lock = null)
		{
			if (LuaGeneration.<>o__27.<>p__1 == null)
			{
				LuaGeneration.<>o__27.<>p__1 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof(LuaGeneration), new CSharpArgumentInfo[]
				{
					CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
				}));
			}
			Func<CallSite, object, bool> target = LuaGeneration.<>o__27.<>p__1.Target;
			CallSite <>p__ = LuaGeneration.<>o__27.<>p__1;
			if (LuaGeneration.<>o__27.<>p__0 == null)
			{
				LuaGeneration.<>o__27.<>p__0 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.NotEqual, typeof(LuaGeneration), new CSharpArgumentInfo[]
				{
					CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
					CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant, null)
				}));
			}
			bool flag = target(<>p__, LuaGeneration.<>o__27.<>p__0.Target(LuaGeneration.<>o__27.<>p__0, LuaGeneration.ValueLock, null));
			string String;
			if (flag)
			{
				int num = LuaGeneration.Random.Next(0, 11);
				int num2 = num;
				if (num2 != 0)
				{
					switch (num2)
					{
					case 7:
						if (Depth < 5)
						{
							String = LuaGeneration.GenerateTable(Depth + 1);
							goto IL_170;
						}
						break;
					case 8:
						if (Depth < 5)
						{
							String = LuaGeneration.GetEquation(Depth + 1);
							goto IL_170;
						}
						break;
					case 9:
						if (Depth < 5)
						{
							String = LuaGeneration.GenerateFunction(Depth + 1);
							goto IL_170;
						}
						break;
					}
				}
				else if (LuaGeneration.Variables.Count > 0)
				{
					String = LuaGeneration.Variables[LuaGeneration.Random.Next(0, LuaGeneration.Variables.Count)];
					goto IL_170;
				}
				String = LuaGeneration.Alphabet[LuaGeneration.Random.Next(0, LuaGeneration.Alphabet.Length)].ToString();
				IL_170:;
			}
			else
			{
				bool flag2 = Depth > 5;
				if (flag2)
				{
					switch (LuaGeneration.Random.Next(0, 7))
					{
					case 0:
						if (LuaGeneration.Variables.Count > 0)
						{
							String = LuaGeneration.Variables[LuaGeneration.Random.Next(0, LuaGeneration.Variables.Count)];
							goto IL_24D;
						}
						break;
					case 1:
						String = LuaGeneration.GenerateString(100);
						goto IL_24D;
					case 2:
						String = LuaGeneration.GenerateNumber(5, true);
						goto IL_24D;
					case 3:
						String = LuaGeneration.GenerateHexadecimal(5);
						goto IL_24D;
					case 4:
						String = "...";
						goto IL_24D;
					case 5:
						String = ((LuaGeneration.Random.Next(0, 2) == 0) ? "false" : "true");
						goto IL_24D;
					case 6:
						String = "nil";
						goto IL_24D;
					}
					String = LuaGeneration.GenerateString(100);
					IL_24D:;
				}
				else
				{
					switch (LuaGeneration.Random.Next(0, 11))
					{
					case 0:
						if (LuaGeneration.Variables.Count > 0)
						{
							String = LuaGeneration.Variables[LuaGeneration.Random.Next(0, LuaGeneration.Variables.Count)];
							goto IL_368;
						}
						break;
					case 1:
						String = LuaGeneration.GenerateString(100);
						goto IL_368;
					case 2:
						String = LuaGeneration.GenerateNumber(5, true);
						goto IL_368;
					case 3:
						String = LuaGeneration.GenerateHexadecimal(5);
						goto IL_368;
					case 4:
						String = "...";
						goto IL_368;
					case 5:
						String = ((LuaGeneration.Random.Next(0, 2) == 0) ? "false" : "true");
						goto IL_368;
					case 6:
						String = "nil";
						goto IL_368;
					case 7:
						if (Depth < 5)
						{
							String = LuaGeneration.GenerateTable(Depth + 1);
							goto IL_368;
						}
						break;
					case 8:
						if (Depth < 5)
						{
							String = LuaGeneration.GetEquation(Depth + 1);
							goto IL_368;
						}
						break;
					case 9:
						if (Depth < 5)
						{
							String = LuaGeneration.GenerateFunction(Depth + 1);
							goto IL_368;
						}
						break;
					}
					String = LuaGeneration.GenerateString(100);
					IL_368:;
				}
			}
			bool flag3 = LuaGeneration.Random.Next(0, 2) == 0;
			if (flag3)
			{
				String = "(not " + String + ")";
			}
			bool flag4 = LuaGeneration.Random.Next(0, 2) == 0;
			if (flag4)
			{
				String = "#" + String;
			}
			bool flag5 = LuaGeneration.Random.Next(0, 2) == 0;
			if (flag5)
			{
				String = "(-" + String + ")";
			}
			bool flag6 = LuaGeneration.Random.Next(0, 2) == 0;
			if (flag6)
			{
				String = "(" + String + ")." + LuaGeneration.GenerateVariableName(0);
			}
			bool flag7 = LuaGeneration.Random.Next(0, 2) == 0;
			if (flag7)
			{
				String = "(" + String + ")()";
			}
			return String;
		}

		private static void GenerateBody(int Depth = 1)
		{
			bool flag = Depth > 2;
			if (!flag)
			{
				int VariableCount = LuaGeneration.Variables.Count;
				int I = 0;
				while ((double)I < LuaGeneration.GenerationIntensity - (double)Depth)
				{
					switch (LuaGeneration.Random.Next(0, 6))
					{
					case 0:
					{
						string VariableName = LuaGeneration.GenerateVariableName(0);
						LuaGeneration.Source = string.Concat(new string[]
						{
							LuaGeneration.Source,
							"local ",
							VariableName,
							" = ",
							LuaGeneration.GetRandomValue(Depth + 1, null),
							";"
						});
						LuaGeneration.Variables.Add(VariableName);
						break;
					}
					case 1:
						LuaGeneration.Source = LuaGeneration.Source + "if (" + LuaGeneration.GetEquation(Depth + 1) + ") then ";
						LuaGeneration.GenerateBody(Depth + 1);
						LuaGeneration.Source += " end;";
						break;
					case 2:
						LuaGeneration.Source = LuaGeneration.Source + "while (" + LuaGeneration.GetEquation(Depth + 1) + ") do ";
						LuaGeneration.GenerateBody(Depth + 1);
						LuaGeneration.Source += " end;";
						break;
					case 3:
					{
						string VariableName2 = LuaGeneration.GenerateVariableName(0);
						LuaGeneration.Source = string.Concat(new string[]
						{
							LuaGeneration.Source,
							"for ",
							VariableName2,
							" = ",
							LuaGeneration.GetEquation(Depth + 1),
							", ",
							LuaGeneration.GetEquation(Depth + 1),
							", ",
							LuaGeneration.GetEquation(Depth + 1),
							" do "
						});
						LuaGeneration.Variables.Add(VariableName2);
						LuaGeneration.GenerateBody(Depth + 1);
						LuaGeneration.Source += " end;";
						LuaGeneration.Variables.Remove(VariableName2);
						break;
					}
					case 4:
					{
						string VariableName3 = LuaGeneration.GenerateVariableName(0);
						LuaGeneration.Source = LuaGeneration.Source + "local function " + VariableName3 + "(...) ";
						LuaGeneration.Variables.Add(VariableName3);
						LuaGeneration.GenerateBody(Depth + 1);
						LuaGeneration.Source += " end;";
						break;
					}
					}
					I++;
				}
				LuaGeneration.Variables.RemoveRange(VariableCount, LuaGeneration.Variables.Count - VariableCount);
			}
		}

		public static string GenerateSingleVariable()
		{
			string result;
			switch (LuaGeneration.Random.Next(0, 4))
			{
			case 0:
				result = "local _ = " + LuaGeneration.GenerateNumber(5, true) + ";";
				break;
			case 1:
				result = "local _ = " + LuaGeneration.GenerateString(100) + ";";
				break;
			case 2:
				result = "local _ = " + LuaGeneration.GenerateHexadecimal(5) + ";";
				break;
			case 3:
				result = "local _ = ({});";
				break;
			default:
				result = "local _ = " + LuaGeneration.GenerateString(100) + ";";
				break;
			}
			return result;
		}

		public static string GenerateRandomFile(int Iterations, int GenerationIntensity)
		{
			List<string> Chunks = new List<string>();
			LuaGeneration.Alphabet = "_";
			LuaGeneration.Alphanumeric = "_";
			LuaGeneration.DEFAULT_VARIABLE_NAME_LENGTH = 1;
			LuaGeneration.Source = "";
			for (double I = 0.0; I < (double)Iterations; I += 1.0)
			{
				LuaGeneration.Source = "";
				switch (LuaGeneration.Random.Next(0, 5))
				{
				case 0:
				{
					string VariableName = LuaGeneration.GenerateVariableName(0);
					LuaGeneration.Source = string.Concat(new string[]
					{
						LuaGeneration.Source,
						"local ",
						VariableName,
						" = ",
						LuaGeneration.GetEquation(1),
						";"
					});
					LuaGeneration.Variables.Add(VariableName);
					break;
				}
				case 1:
					LuaGeneration.Source = LuaGeneration.Source + "if (" + LuaGeneration.GetEquation(1) + ") then ";
					LuaGeneration.GenerateBody(1);
					LuaGeneration.Source += " end;";
					break;
				case 2:
					LuaGeneration.Source = LuaGeneration.Source + "while (" + LuaGeneration.GetEquation(1) + ") do ";
					LuaGeneration.GenerateBody(1);
					LuaGeneration.Source += " end;";
					break;
				case 3:
				{
					string VariableName2 = LuaGeneration.GenerateVariableName(0);
					LuaGeneration.Source = string.Concat(new string[]
					{
						LuaGeneration.Source,
						"for ",
						VariableName2,
						" = ",
						LuaGeneration.GetEquation(1),
						", ",
						LuaGeneration.GetEquation(1),
						", ",
						LuaGeneration.GetEquation(1),
						" do "
					});
					LuaGeneration.Variables.Add(VariableName2);
					LuaGeneration.GenerateBody(1);
					LuaGeneration.Source += " end;";
					LuaGeneration.Variables.Remove(VariableName2);
					break;
				}
				case 4:
				{
					string VariableName3 = LuaGeneration.GenerateVariableName(0);
					LuaGeneration.Source = LuaGeneration.Source + "local function " + VariableName3 + "(...) ";
					LuaGeneration.Variables.Add(VariableName3);
					LuaGeneration.GenerateBody(1);
					LuaGeneration.Source += " end;";
					break;
				}
				}
				LuaGeneration.Source += "\n";
				Chunks.Add(LuaGeneration.Source);
			}
			LuaGeneration.Source = "";
			LuaGeneration.Source = string.Join("", Chunks);
			return LuaGeneration.Source;
		}

		public static string VarArgSpam(int Iterations, int GenerationIntensity)
		{
			LuaGeneration.ValueLock = new string[]
			{
				"..."
			};
			LuaGeneration.DEFAULT_VARIABLE_NAME_LENGTH = 1;
			List<string> Chunks = new List<string>();
			LuaGeneration.Source = "";
			for (double I = 0.0; I < (double)Iterations; I += 1.0)
			{
				LuaGeneration.Source = "";
				switch (LuaGeneration.Random.Next(0, 5))
				{
				case 0:
				{
					string VariableName = LuaGeneration.GenerateVariableName(0);
					LuaGeneration.Source = string.Concat(new string[]
					{
						LuaGeneration.Source,
						"local ",
						VariableName,
						" = ",
						LuaGeneration.GetEquation(1),
						";"
					});
					LuaGeneration.Variables.Add(VariableName);
					break;
				}
				case 1:
					LuaGeneration.Source = LuaGeneration.Source + "if (" + LuaGeneration.GetEquation(1) + ") then ";
					LuaGeneration.GenerateBody(1);
					LuaGeneration.Source += " end;";
					break;
				case 2:
					LuaGeneration.Source = LuaGeneration.Source + "while (" + LuaGeneration.GetEquation(1) + ") do ";
					LuaGeneration.GenerateBody(1);
					LuaGeneration.Source += " end;";
					break;
				case 3:
				{
					string VariableName2 = LuaGeneration.GenerateVariableName(0);
					LuaGeneration.Source = string.Concat(new string[]
					{
						LuaGeneration.Source,
						"for ",
						VariableName2,
						" = ",
						LuaGeneration.GetEquation(1),
						", ",
						LuaGeneration.GetEquation(1),
						", ",
						LuaGeneration.GetEquation(1),
						" do "
					});
					LuaGeneration.Variables.Add(VariableName2);
					LuaGeneration.GenerateBody(1);
					LuaGeneration.Source += " end;";
					LuaGeneration.Variables.Remove(VariableName2);
					break;
				}
				case 4:
				{
					string VariableName3 = LuaGeneration.GenerateVariableName(0);
					LuaGeneration.Source = LuaGeneration.Source + "local function " + VariableName3 + "(...) ";
					LuaGeneration.Variables.Add(VariableName3);
					LuaGeneration.GenerateBody(1);
					LuaGeneration.Source += " end;";
					break;
				}
				}
				LuaGeneration.Source += "\n";
				Chunks.Add(LuaGeneration.Source);
			}
			LuaGeneration.Source = "";
			LuaGeneration.Source = string.Join("", Chunks);
			LuaGeneration.Variables.Clear();
			return LuaGeneration.Source;
		}

		public static string OperatorSpam(int Iterations, int GenerationIntensity)
		{
			LuaGeneration.DEFAULT_VARIABLE_NAME_LENGTH = 1;
			LuaGeneration.Source = "";
			List<string> Chunks = new List<string>();
			for (double I = 0.0; I < (double)Iterations; I += 1.0)
			{
				string Chunk = "";
				Chunk = string.Concat(new string[]
				{
					Chunk,
					"local ",
					LuaGeneration.GenerateVariableName(2),
					" = ",
					LuaGeneration.GenerateVariableName(0)
				});
				for (int K = 0; K < GenerationIntensity; K++)
				{
					Chunk = Chunk + LuaGeneration.Operators[LuaGeneration.Random.Next(0, LuaGeneration.Operators.Length)] + LuaGeneration.GenerateVariableName(0);
				}
				Chunk += ";\n";
				Chunks.Add(Chunk);
			}
			LuaGeneration.Source = string.Join("", Chunks);
			LuaGeneration.Variables.Clear();
			return LuaGeneration.Source;
		}

		private static string Symbols = "`~!@#$%^&*()_-+={}|;:<,>.?/";

		private static string Uppercase = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

		private static string Lowercase = "abcdefghijklmnnopqrstuvwxyz";

		private static string Numbers = "0123456789";

		private static string Hexadecimal = "ABCDEFabcdef0123456789";

		private static string Alphabet = LuaGeneration.Uppercase + LuaGeneration.Lowercase;

		private static string Alphanumeric = LuaGeneration.Alphabet + LuaGeneration.Numbers;

		private static string Characters = LuaGeneration.Alphabet + LuaGeneration.Numbers + LuaGeneration.Symbols;

		private static string[] Operators = new string[]
		{
			" + ",
			" - ",
			" * ",
			" / ",
			" ^ ",
			" % ",
			" or ",
			" and ",
			" == ",
			" <= ",
			" >= ",
			" < ",
			" > "
		};

		private static Random Random = new Random();

		private static List<string> Variables = new List<string>();

		private static double GenerationIntensity = 10.0;

		private static string Source = "";

		[Dynamic]
		private static dynamic ValueLock = null;

		private static int DEFAULT_VARIABLE_NAME_LENGTH = 10;

		private const int DEFAULT_NUMBER_LENGTH = 5;

		private const int MAXIMUM_TABLE_LENGTH = 10;

		private const int MAXIMUM_STRING_LENGTH = 100;

		private const int MAXIMUM_DEPTH = 5;

		private const int MAXIMUM_FUNCTION_DEPTH = 2;
	}
}
