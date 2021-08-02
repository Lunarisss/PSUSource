using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using GParse.Collections;
using GParse.Lexing;
using Loretta;
using Loretta.Lexing;
using Loretta.Parsing;
using Loretta.Parsing.AST;
using Loretta.Parsing.Visitor;
using LuaGeneration;
using Microsoft.CSharp.RuntimeBinder;
using Obfuscator.Bytecode;
using Obfuscator.Bytecode.IR;
using Obfuscator.Extensions;
using Obfuscator.Obfuscation.OpCodes;
using Obfuscator.Utility;

namespace Obfuscator.Obfuscation.Generation
{
	public class ScriptBuilder
	{
		private List<string> GenerateIndicies()
		{
			bool flag = ScriptBuilder.Random.Next(0, 2) == 0;
			List<string> result;
			if (flag)
			{
				long Index = (long)ScriptBuilder.Random.Next(0, 1000000000);
				while (this.UsedIndicies.Contains(Index))
				{
					Index = (long)ScriptBuilder.Random.Next(0, 1000000000);
				}
				this.UsedIndicies.Add("[" + Index.ToString() + "]");
				result = new List<string>
				{
					"[" + Index.ToString() + "]"
				};
			}
			else
			{
				List<string> Indicies = Utility.GetIndexList();
				while (this.UsedIndicies.Contains(Indicies.First<string>()))
				{
					Indicies = Utility.GetIndexList();
				}
				foreach (string Index2 in Indicies)
				{
					this.UsedIndicies.Add(Indicies.First<string>());
				}
				result = Indicies;
			}
			return result;
		}

		private long GenerateNumericIndex()
		{
			long Index = (long)ScriptBuilder.Random.Next(0, 1000000000);
			while (this.UsedIndicies.Contains(Index))
			{
				Index = (long)ScriptBuilder.Random.Next(0, 1000000000);
			}
			this.UsedIndicies.Add("[" + Index.ToString() + "]");
			return Index;
		}

		private ScriptBuilder.Expression AddExpression(dynamic Value)
		{
			if (ScriptBuilder.<>o__19.<>p__1 == null)
			{
				ScriptBuilder.<>o__19.<>p__1 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof(ScriptBuilder), new CSharpArgumentInfo[]
				{
					CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
				}));
			}
			Func<CallSite, object, bool> target = ScriptBuilder.<>o__19.<>p__1.Target;
			CallSite <>p__ = ScriptBuilder.<>o__19.<>p__1;
			if (ScriptBuilder.<>o__19.<>p__0 == null)
			{
				ScriptBuilder.<>o__19.<>p__0 = CallSite<Func<CallSite, Dictionary<object, ScriptBuilder.Expression>, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ContainsKey", null, typeof(ScriptBuilder), new CSharpArgumentInfo[]
				{
					CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null),
					CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
				}));
			}
			bool flag = target(<>p__, ScriptBuilder.<>o__19.<>p__0.Target(ScriptBuilder.<>o__19.<>p__0, this.ExpressionMap, Value));
			ScriptBuilder.Expression result;
			if (flag)
			{
				if (ScriptBuilder.<>o__19.<>p__3 == null)
				{
					ScriptBuilder.<>o__19.<>p__3 = CallSite<Func<CallSite, object, ScriptBuilder.Expression>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(ScriptBuilder.Expression), typeof(ScriptBuilder)));
				}
				Func<CallSite, object, ScriptBuilder.Expression> target2 = ScriptBuilder.<>o__19.<>p__3.Target;
				CallSite <>p__2 = ScriptBuilder.<>o__19.<>p__3;
				if (ScriptBuilder.<>o__19.<>p__2 == null)
				{
					ScriptBuilder.<>o__19.<>p__2 = CallSite<Func<CallSite, Dictionary<object, ScriptBuilder.Expression>, object, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(ScriptBuilder), new CSharpArgumentInfo[]
					{
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null),
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
					}));
				}
				result = target2(<>p__2, ScriptBuilder.<>o__19.<>p__2.Target(ScriptBuilder.<>o__19.<>p__2, this.ExpressionMap, Value));
			}
			else
			{
				ScriptBuilder.Expression Expression = new ScriptBuilder.Expression();
				Expression.Data = Value;
				Expression.Indicies = this.GenerateIndicies();
				bool flag2 = Value is long;
				if (flag2)
				{
					this.NumberExpressions.Add(Expression);
					if (ScriptBuilder.<>o__19.<>p__4 == null)
					{
						ScriptBuilder.<>o__19.<>p__4 = CallSite<Func<CallSite, Dictionary<long, ScriptBuilder.Expression>, object, ScriptBuilder.Expression, object>>.Create(Binder.SetIndex(CSharpBinderFlags.None, typeof(ScriptBuilder), new CSharpArgumentInfo[]
						{
							CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null),
							CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
							CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null)
						}));
					}
					ScriptBuilder.<>o__19.<>p__4.Target(ScriptBuilder.<>o__19.<>p__4, this.NumberExpressionMap, Value, Expression);
				}
				this.Expressions.Add(Expression);
				if (ScriptBuilder.<>o__19.<>p__5 == null)
				{
					ScriptBuilder.<>o__19.<>p__5 = CallSite<Func<CallSite, Dictionary<object, ScriptBuilder.Expression>, object, ScriptBuilder.Expression, object>>.Create(Binder.SetIndex(CSharpBinderFlags.None, typeof(ScriptBuilder), new CSharpArgumentInfo[]
					{
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null),
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null)
					}));
				}
				ScriptBuilder.<>o__19.<>p__5.Target(ScriptBuilder.<>o__19.<>p__5, this.ExpressionMap, Value, Expression);
				result = Expression;
			}
			return result;
		}

		private string ToExpression(dynamic Value, string Type)
		{
			bool flag = Type == "String";
			string result;
			if (flag)
			{
				if (ScriptBuilder.<>o__20.<>p__1 == null)
				{
					ScriptBuilder.<>o__20.<>p__1 = CallSite<Func<CallSite, object, byte[]>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(byte[]), typeof(ScriptBuilder)));
				}
				Func<CallSite, object, byte[]> target = ScriptBuilder.<>o__20.<>p__1.Target;
				CallSite <>p__ = ScriptBuilder.<>o__20.<>p__1;
				if (ScriptBuilder.<>o__20.<>p__0 == null)
				{
					ScriptBuilder.<>o__20.<>p__0 = CallSite<Func<CallSite, Encoding, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "GetBytes", null, typeof(ScriptBuilder), new CSharpArgumentInfo[]
					{
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null),
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
					}));
				}
				byte[] Bytes = target(<>p__, ScriptBuilder.<>o__20.<>p__0.Target(ScriptBuilder.<>o__20.<>p__0, ScriptBuilder.LuaEncoding, Value));
				string String = "\"";
				bool IsString = true;
				foreach (byte Byte in Bytes)
				{
					int num = ScriptBuilder.Random.Next(0, 2);
					int num2 = num;
					if (num2 != 0)
					{
						string Escape = "\\" + Byte.ToString();
						string Chunk = "T" + this.AddExpression("\"" + Escape + "\"").Indicies.Random<string>();
						bool flag2 = IsString;
						if (flag2)
						{
							String = String + "\".." + Chunk;
							IsString = false;
						}
						else
						{
							String = String + ".." + Chunk;
							IsString = false;
						}
					}
					else
					{
						string Chunk2 = "\\" + Byte.ToString();
						bool flag3 = !IsString;
						if (flag3)
						{
							String = String + "..\"" + Chunk2;
							IsString = true;
						}
						else
						{
							String += Chunk2;
							IsString = true;
						}
					}
				}
				result = String + (IsString ? "\"" : "");
			}
			else
			{
				bool flag4 = Type == "Number";
				if (flag4)
				{
					string str = "T";
					if (ScriptBuilder.<>o__20.<>p__4 == null)
					{
						ScriptBuilder.<>o__20.<>p__4 = CallSite<Func<CallSite, object, List<string>>>.Create(Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof(List<string>), typeof(ScriptBuilder)));
					}
					Func<CallSite, object, List<string>> target2 = ScriptBuilder.<>o__20.<>p__4.Target;
					CallSite <>p__2 = ScriptBuilder.<>o__20.<>p__4;
					if (ScriptBuilder.<>o__20.<>p__3 == null)
					{
						ScriptBuilder.<>o__20.<>p__3 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Indicies", typeof(ScriptBuilder), new CSharpArgumentInfo[]
						{
							CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
						}));
					}
					Func<CallSite, object, object> target3 = ScriptBuilder.<>o__20.<>p__3.Target;
					CallSite <>p__3 = ScriptBuilder.<>o__20.<>p__3;
					if (ScriptBuilder.<>o__20.<>p__2 == null)
					{
						ScriptBuilder.<>o__20.<>p__2 = CallSite<Func<CallSite, ScriptBuilder, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.InvokeSimpleName, "AddExpression", null, typeof(ScriptBuilder), new CSharpArgumentInfo[]
						{
							CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null),
							CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
						}));
					}
					result = str + target2(<>p__2, target3(<>p__3, ScriptBuilder.<>o__20.<>p__2.Target(ScriptBuilder.<>o__20.<>p__2, this, Value))).Random<string>();
				}
				else
				{
					bool flag5 = Type == "Raw String";
					if (flag5)
					{
						string str2 = "T";
						if (ScriptBuilder.<>o__20.<>p__7 == null)
						{
							ScriptBuilder.<>o__20.<>p__7 = CallSite<Func<CallSite, object, List<string>>>.Create(Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof(List<string>), typeof(ScriptBuilder)));
						}
						Func<CallSite, object, List<string>> target4 = ScriptBuilder.<>o__20.<>p__7.Target;
						CallSite <>p__4 = ScriptBuilder.<>o__20.<>p__7;
						if (ScriptBuilder.<>o__20.<>p__6 == null)
						{
							ScriptBuilder.<>o__20.<>p__6 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Indicies", typeof(ScriptBuilder), new CSharpArgumentInfo[]
							{
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
							}));
						}
						Func<CallSite, object, object> target5 = ScriptBuilder.<>o__20.<>p__6.Target;
						CallSite <>p__5 = ScriptBuilder.<>o__20.<>p__6;
						if (ScriptBuilder.<>o__20.<>p__5 == null)
						{
							ScriptBuilder.<>o__20.<>p__5 = CallSite<Func<CallSite, ScriptBuilder, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.InvokeSimpleName, "AddExpression", null, typeof(ScriptBuilder), new CSharpArgumentInfo[]
							{
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null),
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
							}));
						}
						result = str2 + target4(<>p__4, target5(<>p__5, ScriptBuilder.<>o__20.<>p__5.Target(ScriptBuilder.<>o__20.<>p__5, this, Value))).Random<string>();
					}
					else
					{
						result = "";
					}
				}
			}
			return result;
		}

		private string GetLuaGeneration()
		{
			bool flag = !this.ObfuscationSettings.EnhancedOutput;
			string result;
			if (flag)
			{
				result = "";
			}
			else
			{
				result = "PSU_LUA_GENERATION";
			}
			return result;
		}

		public string BasicRandomizeNumberStatement(long Number)
		{
			bool flag = ScriptBuilder.Random.Next(0, 2) == 0;
			string Replacement;
			if (flag)
			{
				int XOR = ScriptBuilder.Random.Next(0, 1000000000);
				Number ^= (long)XOR;
				Replacement = string.Format("BitXOR({0}, {1})", XOR, Number);
			}
			else
			{
				bool flag2 = this.NumberExpressions.Count > 0 && ScriptBuilder.Random.Next(0, 2) == 0;
				if (flag2)
				{
					ScriptBuilder.Expression Expression = this.NumberExpressions.Random<ScriptBuilder.Expression>();
					long num = Number;
					if (ScriptBuilder.<>o__23.<>p__0 == null)
					{
						ScriptBuilder.<>o__23.<>p__0 = CallSite<Func<CallSite, object, long>>.Create(Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof(long), typeof(ScriptBuilder)));
					}
					Number = (num ^ ScriptBuilder.<>o__23.<>p__0.Target(ScriptBuilder.<>o__23.<>p__0, Expression.Data));
					Replacement = string.Format("BitXOR({0}, T{1})", Number, Expression.Indicies.Random<string>());
				}
				else
				{
					long XOR2 = (long)ScriptBuilder.Random.Next(0, 1000000000);
					ScriptBuilder.Expression Expression2 = this.AddExpression(XOR2);
					Number ^= XOR2;
					Replacement = string.Format("BitXOR({0}, T{1})", Number, Expression2.Indicies.Random<string>());
				}
			}
			return Replacement;
		}

		public string RandomizeNumberStatement(long Number)
		{
			string Replacement = Number.ToString();
			bool maximumSecurityEnabled = this.ObfuscationSettings.MaximumSecurityEnabled;
			if (maximumSecurityEnabled)
			{
				switch (ScriptBuilder.Random.Next(0, 7))
				{
				case 0:
				{
					long Index = this.NumberEquations.Keys.ToList<long>().Random<long>();
					NumberEquation NumberEquation = this.NumberEquations[Index];
					Replacement = string.Format("Calculate({0}, {1})", Index, NumberEquation.ComputeExpression(Number));
					break;
				}
				case 1:
				{
					long Index2 = this.NumberEquations.Keys.ToList<long>().Random<long>();
					NumberEquation NumberEquation2 = this.NumberEquations[Index2];
					Replacement = string.Format("Calculate({0}, {1})", this.BasicRandomizeNumberStatement(Index2), NumberEquation2.ComputeExpression(Number));
					break;
				}
				case 2:
				{
					long Index3 = this.NumberEquations.Keys.ToList<long>().Random<long>();
					NumberEquation NumberEquation3 = this.NumberEquations[Index3];
					Replacement = string.Concat(new string[]
					{
						"Calculate(",
						this.BasicRandomizeNumberStatement(Index3),
						", ",
						this.BasicRandomizeNumberStatement(NumberEquation3.ComputeExpression(Number)),
						")"
					});
					break;
				}
				case 3:
				{
					NumberEquation NumberEquation4 = new NumberEquation(ScriptBuilder.Random.Next(3, 6));
					long Index4 = this.GenerateNumericIndex();
					Replacement = string.Format("((Storage[{0}]) or (", Index4) + string.Format("(function(Value) Storage[{0}] = {1}; return (Storage[{2}]); end)({3})))", new object[]
					{
						Index4,
						NumberEquation4.WriteStatement(),
						Index4,
						this.BasicRandomizeNumberStatement(NumberEquation4.ComputeExpression(Number))
					});
					break;
				}
				case 4:
					Replacement = this.BasicRandomizeNumberStatement(Number);
					break;
				case 5:
				{
					NumberEquation NumberEquation5 = new NumberEquation(ScriptBuilder.Random.Next(3, 6));
					long Index5 = this.GenerateNumericIndex();
					string Function = "(function(Value, BitXOR, Storage, Index) Storage[Index] = " + NumberEquation5.WriteStatement() + "; return (Storage[Index]); end)";
					Replacement = string.Format("((Storage[{0}]) or (", Index5) + this.ToExpression(Function, "Raw String") + string.Format("({0}, BitXOR, Storage, {1})))", this.BasicRandomizeNumberStatement(NumberEquation5.ComputeExpression(Number)), Index5);
					break;
				}
				}
			}
			return Replacement;
		}

		public string ExpandNumberStatements(string Source)
		{
			int SearchPosition = 0;
			while (SearchPosition < Source.Length)
			{
				string Substring = Source.Substring(SearchPosition);
				Match Match = Regex.Match(Substring, "[^\\\\0-9a-zA-Z\\.\"'](\\d+)[^0-9a-zA-Z\\.\"']");
				bool flag = !Match.Success;
				if (flag)
				{
					break;
				}
				int Number;
				bool Success = int.TryParse(Match.Groups[1].Value, out Number);
				bool flag2 = !Success;
				if (flag2)
				{
					SearchPosition += Match.Index + Match.Length;
				}
				else
				{
					string Replacement = "(" + Utility.IntegerToString(Number, 0) + ")";
					Source = Source.Substring(0, SearchPosition + Match.Index + 1) + Replacement + Source.Substring(SearchPosition + Match.Index + Match.Length - 1);
					SearchPosition += Match.Index + Replacement.Length;
				}
			}
			return Source;
		}

		public string ReplaceNumbers(string Source)
		{
			List<int> Variables = new List<int>();
			int SearchPosition = 0;
			while (SearchPosition < Source.Length)
			{
				string Substring = Source.Substring(SearchPosition);
				Match Match = Regex.Match(Substring, "[^\\\\0-9a-zA-Z_.](\\d+)[^0-9a-zA-Z_.]");
				bool flag = !Match.Success;
				if (flag)
				{
					break;
				}
				int Number;
				bool Success = int.TryParse(Match.Groups[1].Value, out Number);
				bool flag2 = !Success;
				if (flag2)
				{
					SearchPosition = Match.Index + Match.Length;
				}
				else
				{
					string Replacement = this.ToExpression(Number, "Number");
					bool flag3 = !Variables.Contains(Number);
					if (flag3)
					{
						bool flag4 = Variables.Count > 32;
						if (flag4)
						{
							SearchPosition = SearchPosition + Match.Index + Match.Length;
							continue;
						}
						Variables.Add(Number);
					}
					Source = Source.Substring(0, SearchPosition + Match.Index + 1) + string.Format("V{0}", Number) + Source.Substring(SearchPosition + Match.Index + Match.Length - 1);
					SearchPosition = Match.Index + Replacement.Length;
				}
			}
			Variables.Shuffle<int>();
			foreach (int Number2 in Variables)
			{
				Source = string.Format("local V{0} = T{1}; \n", Number2, this.ExpressionMap[Number2].Indicies.Random<string>()) + Source;
			}
			return Source;
		}

		public ScriptBuilder(Chunk HeadChunk, ObfuscationContext ObfuscationContext, ObfuscationSettings ObfuscationSettings, List<VOpCode> Virtuals)
		{
			this.HeadChunk = HeadChunk;
			this.ObfuscationSettings = ObfuscationSettings;
			this.ObfuscationContext = ObfuscationContext;
			this.Virtuals = Virtuals;
			int Count = ScriptBuilder.Random.Next(5, 15);
			for (int I = 0; I < Count; I++)
			{
				this.NumberEquations.Add((long)ScriptBuilder.Random.Next(0, 1000000000), new NumberEquation(ScriptBuilder.Random.Next(3, 6)));
			}
		}

		private void GenerateDeserializer()
		{
			this.Deserializer = string.Concat(new string[]
			{
				"\n\n",
				this.GetLuaGeneration(),
				"\n\n",
				this.ObfuscationSettings.MaximumSecurityEnabled ? "PSU_MAX_SECURITY_START()" : "",
				"\n\nlocal function Deserialize(...) \n\t\t\t\t\t\n",
				string.Join("\n", new List<string>
				{
					"\tlocal Instructions = ({});",
					"\tlocal Constants = ({});",
					"\tlocal Functions = ({});"
				}.Shuffle<string>()),
				"  \n\n\t\t\t"
			});
			string InlinedGetBits32 = "";
			string InlinedGetBits33 = "";
			string InlinedGetBits34 = "";
			ChunkStep[] chunkSteps = this.ObfuscationContext.ChunkSteps;
			for (int i = 0; i < chunkSteps.Length; i++)
			{
				switch (chunkSteps[i])
				{
				case ChunkStep.ParameterCount:
					this.Deserializer = this.Deserializer + "\n\t" + ScriptBuilder.<GenerateDeserializer>g__GetInlinedOrDefault|28_0("local ParameterCount = gBits8(PrimaryXORKey);", "local ParameterCount = Value;", InlinedGetBits34, "PrimaryXORKey") + "\n";
					break;
				case ChunkStep.Instructions:
				{
					this.Deserializer += string.Format(" \n\n\t\t\t\t\t\t\t{0}\n\t\t\t\t\t\t\t\t{1}\n\t\t\t\t\t\t\t\t\n\t\t\t\t\t\t\t\tif (Type == {2}) then\n\t\t\t\t\t\t\t\t\t\n\t\t\t\t\t\t\t\t\t{3}\n\t\t\t\t\t\t\t\t\tConstants[Index] = (Bool ~= 0);\n\n\t\t\t\t\t\t\t\telseif (Type == {4}) then\n\n\t\t\t\t\t\t\t\t\twhile (true) do\n\t\t\t\t\t\t\t\t\t\t{5}\n\t\t\t\t\t\t\t\t\t\t{6}                                   \n\t\t\t\t\t\t\t\t\t\tlocal IsNormal = 1;\n\t\t\t\t\t\t\t\t\t\tlocal Mantissa = (gBit(Right, 1, 20) * (2 ^ 32)) + Left;\n\t\t\t\t\t\t\t\t\t\tlocal Exponent = gBit(Right, 21, 31);\n\t\t\t\t\t\t\t\t\t\tlocal Sign = ((-1) ^ gBit(Right, 32));\n\t\t\t\t\t\t\t\t\t\tif (Exponent == 0) then\n\t\t\t\t\t\t\t\t\t\t\tif (Mantissa == 0) then\n\t\t\t\t\t\t\t\t\t\t\t\tConstants[Index] = (Sign * 0);\n\t\t\t\t\t\t\t\t\t\t\t\tbreak;\n\t\t\t\t\t\t\t\t\t\t\telse\n\t\t\t\t\t\t\t\t\t\t\t\tExponent = 1;\n\t\t\t\t\t\t\t\t\t\t\t\tIsNormal = 0;\n\t\t\t\t\t\t\t\t\t\t\tend;\n\t\t\t\t\t\t\t\t\t\telseif(Exponent == 2047) then\n\t\t\t\t\t\t\t\t\t\t\tConstants[Index] = (Mantissa == 0) and (Sign * (1 / 0)) or (Sign * (0 / 0));\n\t\t\t\t\t\t\t\t\t\t\tbreak;\n\t\t\t\t\t\t\t\t\t\tend;\n\t\t\t\t\t\t\t\t\t\tConstants[Index] = LDExp(Sign, Exponent - 1023) * (IsNormal + (Mantissa / (2 ^ 52)));\n\t\t\t\t\t\t\t\t\t\tbreak;\n\t\t\t\t\t\t\t\t\tend;\n\n\t\t\t\t\t\t\t\telseif (Type == {7}) then\n\t\t\t\t\t\t\t\t   \n\t\t\t\t\t\t\t\t\twhile (true) do\n\t\t\t\t\t\t\t\t\t\t{8}\t\t\t                        \n\t\t\t\t\t\t\t\t\t\tif (Length == 0) then Constants[Index] = (''); break; end;\n\t\t\t\t\t\t\t\t\t   \n\t\t\t\t\t\t\t\t\t\tif (Length > 5000) then\n\t\t\t\t\t\t\t\t\t\t\tlocal Constant, ByteString = (''), (SubString(ByteString, Position, Position + Length - 1));\n\t\t\t\t\t\t\t\t\t\t\tPosition = Position + Length;\n\t\t\t\t\t\t\t\t\t\t\tfor Index = 1, #ByteString, 1 do local Byte = BitXOR(Byte(SubString(ByteString, Index, Index)), PrimaryXORKey); PrimaryXORKey = Byte % 256; Constant = Constant .. Dictionary[Byte]; end;\n\t\t\t\t\t\t\t\t\t\t\tConstants[Index] = Constant;\n\t\t\t\t\t\t\t\t\t\telse\n\t\t\t\t\t\t\t\t\t\t\tlocal Constant, Bytes = (''), ({{Byte(ByteString, Position, Position + Length - 1)}});\n\t\t\t\t\t\t\t\t\t\t\tPosition = Position + Length;        \n\t\t\t\t\t\t\t\t\t\t\tfor Index, Byte in Pairs(Bytes) do local Byte = BitXOR(Byte, PrimaryXORKey); PrimaryXORKey = Byte % 256; Constant = Constant .. Dictionary[Byte]; end;\t\t\t\t                        \n\t\t\t\t\t\t\t\t\t\t\tConstants[Index] = Constant;\n\t\t\t\t\t\t\t\t\t\tend;\n\n\t\t\t\t\t\t\t\t\t\tbreak;\n\t\t\t\t\t\t\t\t\tend;\n\t\t\t\t\t\t\t\telseif (Type == {9}) then\n\t\t\t\t\t\t\t\t\twhile (true) do\n\t\t\t\t\t\t\t\t\t\t{10}\n\t\t\t\t\t\t\t\t\t\tConstants[Index] = SubString(ByteString, Position, Position + Length - 1);\n\t\t\t\t\t\t\t\t\t\tPosition = Position + Length;\n\n\t\t\t\t\t\t\t\t\t\tbreak;\n\t\t\t\t\t\t\t\t\tend;\n\t\t\t\t\t\t\t\telse\n\n\t\t\t\t\t\t\t\t   Constants[Index] = (nil);\n\n\t\t\t\t\t\t\t\tend;\n\t\t\t\t\t\t\tend;", new object[]
					{
						ScriptBuilder.<GenerateDeserializer>g__GetInlinedOrDefault|28_0("for Index = 0, gBits32(PrimaryXORKey) - 1, 1 do", "for Index = 0, Value - 1, 1 do", InlinedGetBits32, "PrimaryXORKey"),
						ScriptBuilder.<GenerateDeserializer>g__GetInlinedOrDefault|28_0("local Type = gBits8(PrimaryXORKey);", "local Type = Value;", InlinedGetBits34, "PrimaryXORKey"),
						this.ObfuscationContext.ConstantMapping[1],
						ScriptBuilder.<GenerateDeserializer>g__GetInlinedOrDefault|28_0("local Bool = gBits8(PrimaryXORKey);", "local Bool = Value;", InlinedGetBits34, "PrimaryXORKey"),
						this.ObfuscationContext.ConstantMapping[2],
						ScriptBuilder.<GenerateDeserializer>g__GetInlinedOrDefault|28_0("local Left = gBits32(PrimaryXORKey);", "local Left = Value;", InlinedGetBits32, "PrimaryXORKey"),
						ScriptBuilder.<GenerateDeserializer>g__GetInlinedOrDefault|28_0("local Right = gBits32(PrimaryXORKey);", "local Right = Value;", InlinedGetBits32, "PrimaryXORKey"),
						this.ObfuscationContext.ConstantMapping[3],
						ScriptBuilder.<GenerateDeserializer>g__GetInlinedOrDefault|28_0("local Length = gBits32(PrimaryXORKey);", "local Length = Value;", InlinedGetBits32, "PrimaryXORKey"),
						this.ObfuscationContext.ConstantMapping[4],
						ScriptBuilder.<GenerateDeserializer>g__GetInlinedOrDefault|28_0("local Length = gBits32(PrimaryXORKey);", "local Length = Value;", InlinedGetBits32, "PrimaryXORKey")
					});
					this.Deserializer = string.Concat(new string[]
					{
						this.Deserializer,
						"\n\n\t\t\t\t\t\t\t",
						ScriptBuilder.<GenerateDeserializer>g__GetInlinedOrDefault|28_0("local Count = gBits32(PrimaryXORKey);", "local Count = Value;", InlinedGetBits32, "PrimaryXORKey"),
						" \n\t\t\t\t\t\t\tfor Index = 0, Count - 1, 1 do Instructions[Index] = ({}); end;\n\n\t\t\t\t\t\t\tfor Index = 0, Count - 1, 1 do\n\t\t\t\t\t\t\t\t",
						ScriptBuilder.<GenerateDeserializer>g__GetInlinedOrDefault|28_0("local InstructionData = gBits8(PrimaryXORKey);", "local InstructionData = Value;", InlinedGetBits34, "PrimaryXORKey"),
						" \n\t\t\t\t\t\t\t\tif (InstructionData ~= 0) then \n\t\t\t\t\t\t\t\t\tInstructionData = InstructionData - 1;\n\t\t\t\t\t\t\t\t\tlocal ",
						string.Join(", ", new List<string>
						{
							"Enum",
							"A",
							"B",
							"C",
							"D",
							"E"
						}.Shuffle<string>()),
						" = 0, 0, 0, 0, 0, 0;\n\t\t\t\t\t\t\t\t\tlocal InstructionType = gBit(InstructionData, 1, 3);\n\t\t\n\t\t\t\t\t\t\t"
					});
					List<InstructionType> InstructionTypes = new List<InstructionType>
					{
						InstructionType.ABC,
						InstructionType.ABx,
						InstructionType.AsBx,
						InstructionType.AsBxC,
						InstructionType.Closure,
						InstructionType.Compressed
					}.Shuffle<InstructionType>().ToList<InstructionType>();
					foreach (InstructionType InstructionType in InstructionTypes)
					{
						this.Deserializer += string.Format("if (InstructionType == {0}) then ", (int)InstructionType);
						switch (InstructionType)
						{
						case InstructionType.ABC:
						{
							InstructionStep[] instructionSteps = this.ObfuscationContext.InstructionSteps;
							for (int j = 0; j < instructionSteps.Length; j++)
							{
								switch (instructionSteps[j])
								{
								case InstructionStep.Enum:
									this.Deserializer += ScriptBuilder.<GenerateDeserializer>g__GetInlinedOrDefault|28_0(" Enum = (gBits8(PrimaryXORKey));", " Enum = (Value);", InlinedGetBits34, "PrimaryXORKey");
									break;
								case InstructionStep.A:
									this.Deserializer += ScriptBuilder.<GenerateDeserializer>g__GetInlinedOrDefault|28_0(" A = (gBits16(PrimaryXORKey));", " A = (Value);", InlinedGetBits33, "PrimaryXORKey");
									break;
								case InstructionStep.B:
									this.Deserializer += ScriptBuilder.<GenerateDeserializer>g__GetInlinedOrDefault|28_0(" B = (gBits16(PrimaryXORKey));", " B = (Value);", InlinedGetBits33, "PrimaryXORKey");
									break;
								case InstructionStep.C:
									this.Deserializer += ScriptBuilder.<GenerateDeserializer>g__GetInlinedOrDefault|28_0(" C = (gBits16(PrimaryXORKey));", " C = (Value);", InlinedGetBits33, "PrimaryXORKey");
									break;
								}
							}
							break;
						}
						case InstructionType.ABx:
						{
							InstructionStep[] instructionSteps2 = this.ObfuscationContext.InstructionSteps;
							for (int k = 0; k < instructionSteps2.Length; k++)
							{
								switch (instructionSteps2[k])
								{
								case InstructionStep.Enum:
									this.Deserializer += ScriptBuilder.<GenerateDeserializer>g__GetInlinedOrDefault|28_0(" Enum = (gBits8(PrimaryXORKey));", " Enum = (Value);", InlinedGetBits34, "PrimaryXORKey");
									break;
								case InstructionStep.A:
									this.Deserializer += ScriptBuilder.<GenerateDeserializer>g__GetInlinedOrDefault|28_0(" A = (gBits16(PrimaryXORKey));", " A = (Value);", InlinedGetBits33, "PrimaryXORKey");
									break;
								case InstructionStep.B:
									this.Deserializer += ScriptBuilder.<GenerateDeserializer>g__GetInlinedOrDefault|28_0(" B = (gBits32(PrimaryXORKey));", " B = (Value);", InlinedGetBits32, "PrimaryXORKey");
									break;
								}
							}
							break;
						}
						case InstructionType.AsBx:
						{
							InstructionStep[] instructionSteps3 = this.ObfuscationContext.InstructionSteps;
							for (int l = 0; l < instructionSteps3.Length; l++)
							{
								switch (instructionSteps3[l])
								{
								case InstructionStep.Enum:
									this.Deserializer += ScriptBuilder.<GenerateDeserializer>g__GetInlinedOrDefault|28_0(" Enum = (gBits8(PrimaryXORKey));", " Enum = (Value);", InlinedGetBits34, "PrimaryXORKey");
									break;
								case InstructionStep.A:
									this.Deserializer += ScriptBuilder.<GenerateDeserializer>g__GetInlinedOrDefault|28_0(" A = (gBits16(PrimaryXORKey));", " A = (Value);", InlinedGetBits33, "PrimaryXORKey");
									break;
								case InstructionStep.B:
									this.Deserializer += ScriptBuilder.<GenerateDeserializer>g__GetInlinedOrDefault|28_0(" B = Instructions[(gBits32(PrimaryXORKey))];", " B = (Value);", InlinedGetBits32, "PrimaryXORKey");
									break;
								}
							}
							break;
						}
						case InstructionType.AsBxC:
						{
							InstructionStep[] instructionSteps4 = this.ObfuscationContext.InstructionSteps;
							for (int m = 0; m < instructionSteps4.Length; m++)
							{
								switch (instructionSteps4[m])
								{
								case InstructionStep.Enum:
									this.Deserializer += ScriptBuilder.<GenerateDeserializer>g__GetInlinedOrDefault|28_0(" Enum = (gBits8(PrimaryXORKey));", " Enum = (Value);", InlinedGetBits34, "PrimaryXORKey");
									break;
								case InstructionStep.A:
									this.Deserializer += ScriptBuilder.<GenerateDeserializer>g__GetInlinedOrDefault|28_0(" A = (gBits16(PrimaryXORKey));", " A = (Value);", InlinedGetBits33, "PrimaryXORKey");
									break;
								case InstructionStep.B:
									this.Deserializer += ScriptBuilder.<GenerateDeserializer>g__GetInlinedOrDefault|28_0(" B = Instructions[(gBits32(PrimaryXORKey))];", " B = Instructions[(Value)];", InlinedGetBits32, "PrimaryXORKey");
									break;
								case InstructionStep.C:
									this.Deserializer += ScriptBuilder.<GenerateDeserializer>g__GetInlinedOrDefault|28_0(" C = (gBits16(PrimaryXORKey));", " C = (Value);", InlinedGetBits33, "PrimaryXORKey");
									break;
								}
							}
							break;
						}
						case InstructionType.Closure:
						{
							InstructionStep[] instructionSteps5 = this.ObfuscationContext.InstructionSteps;
							for (int n = 0; n < instructionSteps5.Length; n++)
							{
								switch (instructionSteps5[n])
								{
								case InstructionStep.Enum:
									this.Deserializer += ScriptBuilder.<GenerateDeserializer>g__GetInlinedOrDefault|28_0(" Enum = (gBits8(PrimaryXORKey));", " Enum = (Value);", InlinedGetBits34, "PrimaryXORKey");
									break;
								case InstructionStep.A:
									this.Deserializer += ScriptBuilder.<GenerateDeserializer>g__GetInlinedOrDefault|28_0(" A = (gBits16(PrimaryXORKey));", " A = (Value);", InlinedGetBits33, "PrimaryXORKey");
									break;
								case InstructionStep.B:
									this.Deserializer += ScriptBuilder.<GenerateDeserializer>g__GetInlinedOrDefault|28_0(" B = (gBits32(PrimaryXORKey));", " B = (Value);", InlinedGetBits32, "PrimaryXORKey");
									break;
								case InstructionStep.C:
									this.Deserializer += ScriptBuilder.<GenerateDeserializer>g__GetInlinedOrDefault|28_0(" C = (gBits16(PrimaryXORKey));", " C = (Value);", InlinedGetBits33, "PrimaryXORKey");
									break;
								}
							}
							this.Deserializer += " D = ({}); for Index = 1, C, 1 do D[Index] = ({[0] = gBits8(PrimaryXORKey), [1] = gBits16(PrimaryXORKey)}); end; ";
							break;
						}
						}
						bool flag = InstructionType != InstructionTypes.Last<InstructionType>();
						if (flag)
						{
							this.Deserializer += " else";
						}
					}
					this.Deserializer = string.Concat(new string[]
					{
						this.Deserializer,
						" end; \n\n\t\t\t\t\t\t\t",
						string.Join(" ", new List<string>
						{
							"if (gBit(InstructionData, 4, 4) == 1) then A = Constants[A]; end;",
							"if (gBit(InstructionData, 5, 5) == 1) then B = Constants[B]; end;",
							"if (gBit(InstructionData, 6, 6) == 1) then C = Constants[C]; end;",
							"if (gBit(InstructionData, 8, 8) == 1) then E = Instructions[gBits32(PrimaryXORKey)]; else E = Instructions[Index + 1]; end;"
						}.Shuffle<string>()),
						"\n\n\t\t\t\t\t\t\tif (gBit(InstructionData, 7, 7) == 1) then D = ({}); for Index = 1, gBits8(), 1 do D[Index] = gBits32(); end; end;\n\n\t\t\t\t\t\t\tlocal Instruction = Instructions[Index];\n\n\t\t\t\t\t\t\t",
						string.Join(" ", new List<string>
						{
							"Instruction[" + this.ObfuscationContext.Instruction.Enum.Random<string>() + "] = Enum;",
							"Instruction[" + this.ObfuscationContext.Instruction.A.Random<string>() + "] = A;",
							"Instruction[" + this.ObfuscationContext.Instruction.B.Random<string>() + "] = B;",
							"Instruction[" + this.ObfuscationContext.Instruction.C.Random<string>() + "] = C;",
							"Instruction[" + this.ObfuscationContext.Instruction.D.Random<string>() + "] = D;",
							"Instruction[" + this.ObfuscationContext.Instruction.E.Random<string>() + "] = E;"
						}.Shuffle<string>()),
						" end; end;"
					});
					break;
				}
				case ChunkStep.Chunks:
					this.Deserializer = this.Deserializer + "\n\t" + ScriptBuilder.<GenerateDeserializer>g__GetInlinedOrDefault|28_0("for Index = 0, gBits32(PrimaryXORKey) - 1, 1 do", "for Index = 0, Value - 1, 1 do", InlinedGetBits32, "PrimaryXORKey") + " Functions[Index] = Deserialize(); end;\n";
					break;
				case ChunkStep.StackSize:
					this.Deserializer = this.Deserializer + "\n\t" + ScriptBuilder.<GenerateDeserializer>g__GetInlinedOrDefault|28_0("local StackSize = gBits16(PrimaryXORKey);", "local StackSize = Value;", InlinedGetBits33, "PrimaryXORKey") + "\n";
					break;
				}
			}
			this.Deserializer = string.Concat(new string[]
			{
				this.Deserializer,
				"\n\n\treturn ({\n\n",
				string.Join("\n", new List<string>
				{
					"\t[" + this.ObfuscationContext.Chunk.InstructionPoint.Random<string>() + "] = 0;",
					"\t[" + this.ObfuscationContext.Chunk.Instructions.Random<string>() + "] = Instructions;",
					"\t[" + this.ObfuscationContext.Chunk.Constants.Random<string>() + "] = Constants;",
					"\t[" + this.ObfuscationContext.Chunk.Chunks.Random<string>() + "] = Functions;",
					"\t[" + this.ObfuscationContext.Chunk.StackSize.Random<string>() + "] = StackSize;",
					"\t[" + this.ObfuscationContext.Chunk.ParameterCount.Random<string>() + "] = ParameterCount;"
				}.Shuffle<string>()),
				"\n\n\t}); \n\nend; \n\n",
				this.ObfuscationSettings.MaximumSecurityEnabled ? "PSU_MAX_SECURITY_END()" : "",
				"\n"
			});
		}

		private void GenerateVM()
		{
			this.VM = string.Concat(new string[]
			{
				"\n\n",
				this.GetLuaGeneration(),
				"\n\nlocal function Wrap(Chunk, UpValues, Environment, ...)\n\t\t\t\t\n\t",
				string.Join("\n", new List<string>
				{
					"\tlocal Instructions = Chunk[" + this.ObfuscationContext.Chunk.Instructions.Random<string>() + "];",
					"\tlocal Functions = Chunk[" + this.ObfuscationContext.Chunk.Chunks.Random<string>() + "];",
					"\tlocal ParameterCount = Chunk[" + this.ObfuscationContext.Chunk.ParameterCount.Random<string>() + "];",
					"\tlocal Constants = Chunk[" + this.ObfuscationContext.Chunk.Constants.Random<string>() + "];",
					"\tlocal InitialInstructionPoint = 0;",
					"\tlocal StackSize = Chunk[" + this.ObfuscationContext.Chunk.StackSize.Random<string>() + "];"
				}.Shuffle<string>()),
				"\n\t\n\treturn (function(...)\n\n\t\t",
				string.Join("\n", new List<string>
				{
					"\t\tlocal OP_A = " + this.ObfuscationContext.Instruction.A.Random<string>() + ";",
					"\t\tlocal OP_B = " + this.ObfuscationContext.Instruction.B.Random<string>() + ";",
					"\t\tlocal OP_C = " + this.ObfuscationContext.Instruction.C.Random<string>() + ";",
					"\t\tlocal OP_D = " + this.ObfuscationContext.Instruction.D.Random<string>() + ";",
					"\t\tlocal OP_E = " + this.ObfuscationContext.Instruction.E.Random<string>() + ";",
					"\t\tlocal OP_ENUM = " + this.ObfuscationContext.Instruction.Enum.Random<string>() + ";",
					"\t\tlocal Stack = {};",
					"\t\tlocal Top = -(1);",
					"\t\tlocal VarArg = {};",
					"\t\tlocal Arguments = {...};",
					"\t\tlocal PCount = (Select(Mode, ...) - 1);",
					"\t\tlocal InstructionPoint = Instructions[InitialInstructionPoint];",
					"\t\tlocal lUpValues = ({});",
					string.Format("\t\tlocal VMKey = ({0});", ScriptBuilder.Random.Next(0, 1000000000)),
					"\t\tlocal DecryptConstants = (true);"
				}.Shuffle<string>()),
				"\n\n\t\tfor Index = 0, PCount, 1 do\n\t\t\tif (Index >= ParameterCount) then\n\t\t\t\tVarArg[Index - ParameterCount] = Arguments[Index + 1];\n\t\t\telse\n\t\t\t\tStack[Index] = Arguments[Index + 1];\n\t\t\tend;\n\t\tend;\n\n\t\tlocal VarArgs = PCount - ParameterCount + 1;\n\n\t\twhile (true) do\n\t\t\tlocal Instruction = InstructionPoint;\t\n\t\t\tlocal Enum = Instruction[OP_ENUM];\n\t\t\tInstructionPoint = Instruction[OP_E];"
			});
			this.VM += this.<GenerateVM>g__GetString|29_2(Enumerable.Range(0, this.Virtuals.Count).ToList<int>());
			this.VM = this.VM + "\n\n\t\t\t\t\tend;\n\t\t\t\tend);\n\t\t\tend;\t\n\n\t\t\t" + this.GetLuaGeneration() + "\n\n\t\t\treturn Wrap(Deserialize(), {}, GetFEnv())(...);";
		}

		private void GenerateHeader()
		{
			List<byte> Bytes = new Serializer(this.ObfuscationContext, this.ObfuscationSettings).Serialize(this.HeadChunk);
			string ByteString = Compression.CompressedToString(Compression.Compress(Bytes.ToArray()), this.ObfuscationSettings);
			string ByteCodeFormattingTable = "{";
			Dictionary<char, string> Replacements = new Dictionary<char, string>();
			string Base36 = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
			string Pattern = "";
			bool flag = this.ObfuscationSettings.ByteCodeMode == "Chinese";
			if (flag)
			{
				string Format = "\ud847\udfc2\ud847\udfc3\ud847\udfc4\ud847\udfc5\ud847\udfc6\ud847\udfc7\ud847\udfc8\ud847\udfc9\ud847\udfca\ud847\udfcb\ud847\udfcc\ud847\udfcd\ud847\udfce\ud847\udfcf\ud847\udfd0\ud847\udfd1\ud847\udfd2\ud847\udfd3\ud847\udfd4\ud847\udfd5\ud847\udfd6\ud847\udfd7\ud847\udfd8\ud847\udfd9\ud847\udfda\ud847\udfdb\ud847\udfdc\ud847\udfdd\ud847\udfde\ud847\udfdf\ud847\udfe0\ud847\udfe1\ud847\udfe2\ud847\udfe3\ud847\udfe4\ud847\udfe5\ud845\udcf7\ud845\udcf8\ud845\udcf9\ud845\udcfa\ud845\udcfb\ud845\udcfc\ud845\udcfd\ud845\udcfe\ud845\udcff\ud845\udd00\ud845\udd01\ud845\udd02\ud845\udd03\ud845\udd04\ud845\udd05\ud845\udd06\ud845\udd07\ud845\udd08\ud845\udd09\ud845\udd0a\ud845\udd0b\ud845\udd0c\ud845\udd0d\ud845\udd0e\ud845\udd0f\ud845\udd10\ud845\udd11\ud845\udd12\ud845\udd13\ud845\udd14\ud845\udd15\ud845\udd16\ud845\udd17\ud845\udd18\ud845\udd19\ud845\udd1a";
				Pattern = "....";
				List<string> Characters = new List<string>();
				for (int I = 0; I < Format.Length; I += 2)
				{
					string Section = Format.Substring(I, 2);
					Characters.Add(Section);
				}
				for (int I2 = 0; I2 < Base36.Length; I2++)
				{
					ByteString = ByteString.Replace(Base36[I2].ToString(), Characters[I2]);
					Replacements[Base36[I2]] = Characters[I2];
					ByteCodeFormattingTable += string.Format("[\"{0}\"]=\"{1}\";", Characters[I2], Base36[I2]);
				}
				ByteString = "[=[PSU|" + ByteString + "]=]";
			}
			else
			{
				bool flag2 = this.ObfuscationSettings.ByteCodeMode == "Arabic";
				if (flag2)
				{
					string Format2 = "ٶٷٸٺٻټٽپٿڀځڂڃڄڅچڇڈډڊڋڌڍڎڏڐڑڒݐݑݒݓݔݕݖݗݘݙݚݛݜݝݞݟݠݡݢݣݤݥݦݧݨݩݪݫݬݭݮݯݰݱݲݳݴݵݶݷݸݹݺݻݼݽݾݿ";
					Pattern = "..";
					List<string> Characters2 = new List<string>();
					for (int I3 = 0; I3 < Format2.Length; I3++)
					{
						string Section2 = Format2.Substring(I3, 1);
						Characters2.Add(Section2);
					}
					for (int I4 = 0; I4 < Base36.Length; I4++)
					{
						ByteString = ByteString.Replace(Base36[I4].ToString(), Characters2[I4]);
						Replacements[Base36[I4]] = Characters2[I4];
						ByteCodeFormattingTable += string.Format("[\"{0}\"]=\"{1}\";", Characters2[I4], Base36[I4]);
					}
					ByteString = "[=[PSU|" + ByteString + "]=]";
				}
				else
				{
					bool flag3 = this.ObfuscationSettings.ByteCodeMode == "Symbols1";
					if (flag3)
					{
						string Format3 = "ꀀꀁꀂꀃꀄꀅꀆꀇꀈꀉꀊꀋꀌꀍꀎꀏꀐꀑꀒꀓꀔꀕꀖꀗꀘꀙꀚꀛꀜꀝꀞꀟꀠꀡꀢꀣꀤꀥꀦꀧꀨꀩꀪꀫꀬꀭꀮꀯꀰꀱꀲꀳꀴꀵꀶꀷꀸꀹꀺꀻꀼꀽꀾꀿꁀꁁꁂꁃꁄꁅꁆꁇꁈꁉꁊꁋꁌꁍꁎꁏꁐꁑꁒꁓꁔꁕꁖꁗꁘꁙꁚꁛ";
						Pattern = "...";
						List<string> Characters3 = new List<string>();
						for (int I5 = 0; I5 < Format3.Length; I5++)
						{
							string Section3 = Format3.Substring(I5, 1);
							Characters3.Add(Section3);
						}
						for (int I6 = 0; I6 < Base36.Length; I6++)
						{
							ByteString = ByteString.Replace(Base36[I6].ToString(), Characters3[I6]);
							Replacements[Base36[I6]] = Characters3[I6];
							ByteCodeFormattingTable += string.Format("[\"{0}\"]=\"{1}\";", Characters3[I6], Base36[I6]);
						}
						ByteString = "[=[PSU|" + ByteString + "]=]";
					}
					else
					{
						bool flag4 = this.ObfuscationSettings.ByteCodeMode == "Korean";
						if (flag4)
						{
							string Format4 = "뗬뗭뗮뗯뗰뗱뗲뗳뗴뗵뗶뗷뗸뗹뗺뗻뗼뗽뗾뗿똀똁똂똃똄똅똆똇똈똉똊똋똌똍똎똏또똑똒똓똔똕똖똗똘똙똚똛똜똝똞똟똠똡똢똣똤똥똦똧똨똩똪똫똬똭똮똯똰똱똲똳똴똵똶똷똸똹똺똻";
							Pattern = "...";
							List<string> Characters4 = new List<string>();
							for (int I7 = 0; I7 < Format4.Length; I7++)
							{
								string Section4 = Format4.Substring(I7, 1);
								Characters4.Add(Section4);
							}
							for (int I8 = 0; I8 < Base36.Length; I8++)
							{
								ByteString = ByteString.Replace(Base36[I8].ToString(), Characters4[I8]);
								Replacements[Base36[I8]] = Characters4[I8];
								ByteCodeFormattingTable += string.Format("[\"{0}\"]=\"{1}\";", Characters4[I8], Base36[I8]);
							}
							ByteString = "[=[PSU|" + ByteString + "]=]";
						}
						else
						{
							bool flag5 = this.ObfuscationSettings.ByteCodeMode == "Symbols2";
							if (flag5)
							{
								string Format5 = "\ud808\ude67\ud808\ude68\ud808\ude69\ud808\ude6a\ud808\ude6b\ud808\ude6c\ud808\ude6d\ud808\ude6e\ud808\ude6f\ud808\ude70\ud808\ude71\ud808\ude72\ud808\ude73\ud808\ude74\ud808\ude75\ud808\ude76\ud808\ude77\ud808\ude78\ud808\ude79\ud808\ude7a\ud808\ude7b\ud808\ude7c\ud808\ude7d\ud808\ude7e\ud808\ude7f\ud808\ude80\ud808\ude81\ud808\ude82\ud808\ude83\ud808\ude84\ud808\ude85\ud808\ude86\ud808\ude87\ud808\ude88\ud808\ude89\ud808\ude8a\ud808\ude8b\ud808\ude8c\ud808\ude8d\ud808\ude8e\ud808\ude8f\ud808\ude90\ud808\ude91\ud808\ude92\ud808\ude93\ud808\ude94\ud808\ude95\ud808\ude96\ud808\ude97\ud808\ude98\ud808\ude99\ud808\ude9a\ud808\ude9b\ud808\ude9c\ud808\ude9d\ud808\ude9e\ud808\ude9f\ud808\udea0\ud808\udea1\ud808\udea2\ud808\udea3\ud808\udea4\ud808\udea5\ud808\udea6\ud808\udea7\ud808\udea8\ud808\udea9\ud808\udeaa\ud808\udeab\ud808\udeac\ud808\udead\ud808\udeae\ud808\udeaf\ud808\udeb0\ud808\udeb1\ud808\udeb2\ud808\udeb3\ud808\udeb4\ud808\udeb5\ud808\udeb6\ud808\udeb7\ud808\udeb8\ud808\udeb9\ud808\udeba\ud808\udebb\ud808\udebc\ud808\udebd\ud808\udebe\ud808\udebf\ud808\udec0\ud808\udec1\ud808\udec2\ud808\udec3\ud808\udec4\ud808\udec5\ud808\udec6\ud808\udec7";
								Pattern = "....";
								List<string> Characters5 = new List<string>();
								for (int I9 = 0; I9 < Format5.Length; I9 += 2)
								{
									string Section5 = Format5.Substring(I9, 2);
									Characters5.Add(Section5);
								}
								for (int I10 = 0; I10 < Base36.Length; I10++)
								{
									ByteString = ByteString.Replace(Base36[I10].ToString(), Characters5[I10]);
									Replacements[Base36[I10]] = Characters5[I10];
									ByteCodeFormattingTable += string.Format("[\"{0}\"]=\"{1}\";", Characters5[I10], Base36[I10]);
								}
								ByteString = "[=[PSU|" + ByteString + "]=]";
							}
							else
							{
								bool flag6 = this.ObfuscationSettings.ByteCodeMode == "Symbols3";
								if (flag6)
								{
									string Format6 = "\ud809\udc0b\ud809\udc0c\ud809\udc0d\ud809\udc0e\ud809\udc0f\ud809\udc10\ud809\udc11\ud809\udc12\ud809\udc13\ud809\udc14\ud809\udc15\ud809\udc16\ud809\udc17\ud809\udc18\ud809\udc19\ud809\udc1a\ud809\udc1b\ud809\udc1c\ud809\udc1d\ud809\udc1e\ud809\udc1f\ud809\udc20\ud809\udc21\ud809\udc22\ud809\udc23\ud809\udc24\ud809\udc25\ud809\udc26\ud809\udc27\ud809\udc28\ud809\udc29\ud809\udc2a\ud809\udc2b\ud809\udc2c\ud809\udc2d\ud809\udc2e\ud809\udc2f\ud809\udc30\ud809\udc31\ud809\udc32\ud809\udc33\ud809\udc34\ud809\udc35\ud809\udc36\ud809\udc37\ud809\udc38\ud809\udc39\ud809\udc3a\ud809\udc3b\ud809\udc3c\ud809\udc3d\ud809\udc3e\ud809\udc3f\ud809\udc40\ud809\udc41\ud809\udc42\ud809\udc43\ud809\udc44\ud809\udc45\ud809\udc46\ud809\udc47\ud809\udc48\ud809\udc49\ud809\udc4a\ud809\udc4b\ud809\udc4c\ud809\udc4d\ud809\udc4e\ud809\udc4f\ud809\udc50\ud809\udc51\ud809\udc52\ud809\udc53\ud809\udc54\ud809\udc55\ud809\udc56\ud809\udc57\ud809\udc58\ud809\udc59\ud809\udc5a\ud809\udc5b\ud809\udc5c\ud809\udc5d\ud809\udc5e\ud809\udc5f\ud809\udc60\ud809\udc61\ud809\udc62\ud809\udc63\ud809\udc64\ud809\udc65\ud809\udc66\ud809\udc67\ud809\udc68\ud809\udc69\ud809\udc6a";
									Pattern = "....";
									List<string> Characters6 = new List<string>();
									for (int I11 = 0; I11 < Format6.Length; I11 += 2)
									{
										string Section6 = Format6.Substring(I11, 2);
										Characters6.Add(Section6);
									}
									for (int I12 = 0; I12 < Base36.Length; I12++)
									{
										ByteString = ByteString.Replace(Base36[I12].ToString(), Characters6[I12]);
										Replacements[Base36[I12]] = Characters6[I12];
										ByteCodeFormattingTable += string.Format("[\"{0}\"]=\"{1}\";", Characters6[I12], Base36[I12]);
									}
									ByteString = "[=[PSU|" + ByteString + "]=]";
								}
								else
								{
									bool flag7 = this.ObfuscationSettings.ByteCodeMode == "Emoji";
									if (flag7)
									{
										string Format7 = "\ud83c\udf45\ud83c\udf46\ud83c\udf47\ud83c\udf48\ud83c\udf49\ud83c\udf4a\ud83c\udf4b\ud83c\udf4c\ud83c\udf4d\ud83c\udf4e\ud83c\udf4f\ud83c\udf50\ud83c\udf51\ud83c\udf52\ud83c\udf53\ud83c\udf54\ud83c\udf55\ud83c\udf56\ud83c\udf57\ud83c\udf58\ud83d\udd0f\ud83d\udd10\ud83d\udd11\ud83d\udd12\ud83d\udd13\ud83d\udd14\ud83d\ude00\ud83d\ude01\ud83d\ude02\ud83d\ude03\ud83d\ude04\ud83d\ude05\ud83d\ude06\ud83d\ude07\ud83d\ude08\ud83d\ude09\ud83d\ude0a\ud83d\ude0b\ud83d\ude0c\ud83d\ude0d\ud83d\ude0e\ud83d\ude0f\ud83d\ude10\ud83d\ude11\ud83d\ude12\ud83d\ude13\ud83d\ude14\ud83d\ude15\ud83d\ude16\ud83d\ude17\ud83d\ude18\ud83d\ude19\ud83d\ude1a\ud83d\ude1b\ud83d\ude1c\ud83d\ude1d\ud83d\ude1e\ud83d\ude1f\ud83d\ude20\ud83d\ude21\ud83d\ude22\ud83d\ude23\ud83d\ude24\ud83d\ude25\ud83d\ude26\ud83d\ude27\ud83d\ude28\ud83d\ude29\ud83d\ude2a\ud83d\ude2b\ud83d\ude2c\ud83d\ude2d\ud83d\ude2e\ud83d\ude2f\ud83d\ude30\ud83d\ude31";
										Pattern = "....";
										List<string> Characters7 = new List<string>();
										for (int I13 = 0; I13 < Format7.Length; I13 += 2)
										{
											string Section7 = Format7.Substring(I13, 2);
											Characters7.Add(Section7);
										}
										for (int I14 = 0; I14 < Base36.Length; I14++)
										{
											ByteString = ByteString.Replace(Base36[I14].ToString(), Characters7[I14]);
											Replacements[Base36[I14]] = Characters7[I14];
											ByteCodeFormattingTable += string.Format("[\"{0}\"]=\"{1}\";", Characters7[I14], Base36[I14]);
										}
										ByteString = "[=[PSU|" + ByteString + "]=]";
									}
									else
									{
										bool flag8 = this.ObfuscationSettings.ByteCodeMode == "Greek";
										if (flag8)
										{
											string Format8 = "\ud835\udf44\ud835\udf45\ud835\udf46\ud835\udf47\ud835\udf48\ud835\udf49\ud835\udf4a\ud835\udf4b\ud835\udf4c\ud835\udf4d\ud835\udf4e\ud835\udf4f\ud835\udf50\ud835\udf51\ud835\udf52\ud835\udf53\ud835\udf54\ud835\udf55\ud835\udf56\ud835\udf57\ud835\udf58\ud835\udf59\ud835\udf5a\ud835\udf5b\ud835\udf5c\ud835\udf5d\ud835\udf5e\ud835\udf5f\ud835\udf60\ud835\udf61\ud835\udf62\ud835\udf63\ud835\udf64\ud835\udf65\ud835\udf66\ud835\udf67\ud835\udf68\ud835\udf69\ud835\udf6a\ud835\udf6b\ud835\udf6c\ud835\udf6d\ud835\udf6e\ud835\udf6f\ud835\udf70\ud835\udf71\ud835\udf72\ud835\udf73\ud835\udf74\ud835\udf75\ud835\udf76\ud835\udf77\ud835\udf78\ud835\udf79\ud835\udf7a\ud835\udf7b\ud835\udf7c\ud835\udf7d\ud835\udf7e\ud835\udf7f\ud835\udf80\ud835\udf81\ud835\udf82\ud835\udf83\ud835\udf84\ud835\udf85\ud835\udf86";
											Pattern = "....";
											List<string> Characters8 = new List<string>();
											for (int I15 = 0; I15 < Format8.Length; I15 += 2)
											{
												string Section8 = Format8.Substring(I15, 2);
												Characters8.Add(Section8);
											}
											for (int I16 = 0; I16 < Base36.Length; I16++)
											{
												ByteString = ByteString.Replace(Base36[I16].ToString(), Characters8[I16]);
												Replacements[Base36[I16]] = Characters8[I16];
												ByteCodeFormattingTable += string.Format("[\"{0}\"]=\"{1}\";", Characters8[I16], Base36[I16]);
											}
											ByteString = "[=[PSU|" + ByteString + "]=]";
										}
										else
										{
											bool flag9 = this.ObfuscationSettings.ByteCodeMode == "Default";
											if (flag9)
											{
												ByteString = "\"PSU|" + ByteString + "\"";
											}
											else
											{
												ByteString = "\"PSU|" + ByteString + "\"";
											}
										}
									}
								}
							}
						}
					}
				}
			}
			this.ObfuscationContext.ByteCode = ByteString;
			ByteCodeFormattingTable += "}";
			bool flag10 = this.ObfuscationSettings.ByteCodeMode != "Default";
			if (flag10)
			{
				this.ObfuscationContext.FormatTable = ByteCodeFormattingTable;
				ByteCodeFormattingTable = "ByteString = GSub(ByteString, \"" + Pattern + "\", PSU_FORMAT_TABLE);";
			}
			else
			{
				ByteCodeFormattingTable = "";
			}
			this.Variables = string.Format("\n\nlocal GetFEnv = ((getfenv) or (function(...) return (_ENV); end));\nlocal Storage, _, Environment = ({{}}), (\"\"), (GetFEnv(1));\n\nlocal bit32 = ((Environment[{0}]) or (Environment[{1}]) or ({{}})); \nlocal BitXOR = (((bit32) and (bit32[{2}])) or (function(A, B) local P, C = 1, 0; while ((A > 0) and (B > 0)) do local X, Y = A % 2, B % 2; if X ~= Y then C = C + P; end; A, B, P = (A - X) / 2, (B - Y) / 2, P * 2; end; if A < B then A = B; end; while A > 0 do local X = A % 2; if X > 0 then C = C + P; end; A, P =(A - X) / 2, P * 2; end; return (C); end));\n\nlocal MOD = (2 ^ 32);\nlocal MODM = (MOD - 1);\nlocal BitSHL, BitSHR, BitAND;\n\n{3}\n\n{4}\n\n{5}\n\n{6}\n\n{7}\n\n{8}\n\nif ((not (Environment[{9}])) and (not (Environment[{10}]))) then\n\n{11}\n\nend;\n\n{12}\n\n{13} \n\nEnvironment[{14}] = bit32;\n\nlocal PrimaryXORKey = ({15});\n\n{16}\n\nlocal F = (#TEXT + 165); local G, Dictionary = ({{}}), ({{}}); for H = 0, F - 1 do local Value = Character(H); G[H] = Value; Dictionary[H] = Value; Dictionary[Value] = H; end;\nlocal ByteString, Position = (function(ByteString) local X, Y, Z = Byte(ByteString, 1, 3); if ((X + Y + Z) ~= 248) then PrimaryXORKey = PrimaryXORKey + {17}; F = F + {18}; end; ByteString = SubString(ByteString, 5); {19} local C, D, E = (\"\"), (\"\"), ({{}}); local I = 1; local function K() local L = ToNumber(SubString(ByteString, I, I), 36); I = I + 1; local M = ToNumber(SubString(ByteString, I, I + L - 1), 36); I = I + L; return (M); end; C = Dictionary[K()]; E[1] = C; while (I < #ByteString) do local N = K(); if G[N] then D = G[N]; else D = C .. SubString(C, 1, 1); end; G[F] = C .. SubString(D, 1, 1); E[#E + 1], C, F = D, D, F + 1; end; return (Concatenate(E)); end)(PSU_BYTECODE), (#TEXT - 90);", new object[]
			{
				this.ToExpression("bit32", "String"),
				this.ToExpression("bit", "String"),
				this.ToExpression("bxor", "String"),
				this.GetLuaGeneration(),
				string.Join("\n", new List<string>
				{
					"local Byte = (_[" + this.ToExpression("byte", "String") + "]);",
					"local Character = (_[" + this.ToExpression("char", "String") + "]);",
					"local SubString = (_[" + this.ToExpression("sub", "String") + "]);",
					"local GSub = (_[" + this.ToExpression("gsub", "String") + "]);"
				}.Shuffle<string>()),
				this.GetLuaGeneration(),
				string.Join("\n", new List<string>
				{
					"local RawSet = (Environment[" + this.ToExpression("rawset", "String") + "]);",
					"local Pairs = (Environment[" + this.ToExpression("pairs", "String") + "]);",
					"local ToNumber = (Environment[" + this.ToExpression("tonumber", "String") + "]);",
					"local SetMetaTable = (Environment[" + this.ToExpression("setmetatable", "String") + "]);",
					"local Select = (Environment[" + this.ToExpression("select", "String") + "]);",
					"local Type = (Environment[" + this.ToExpression("type", "String") + "]);",
					string.Concat(new string[]
					{
						"local UnPack = ((Environment[",
						this.ToExpression("unpack", "String"),
						"]) or (Environment[",
						this.ToExpression("table", "String"),
						"][",
						this.ToExpression("unpack", "String"),
						"]));"
					}),
					string.Concat(new string[]
					{
						"local LDExp = ((Environment[",
						this.ToExpression("math", "String"),
						"][",
						this.ToExpression("ldexp", "String"),
						"]) or (function(Value, Exponent, ...) return ((Value * 2) ^ Exponent); end));"
					}),
					string.Concat(new string[]
					{
						"local Floor = (Environment[",
						this.ToExpression("math", "String"),
						"][",
						this.ToExpression("floor", "String"),
						"]);"
					})
				}.Shuffle<string>()),
				this.GetLuaGeneration(),
				string.Join("\n", new List<string>
				{
					"BitAND = (bit32[" + this.ToExpression("band", "String") + "]) or (function(A, B, ...) return (((A + B) - BitXOR(A, B)) / 2); end);",
					"local BitOR = (bit32[" + this.ToExpression("bor", "String") + "]) or (function(A, B, ...) return (MODM - BitAND(MODM - A, MODM - B)); end);",
					"local BitNOT = (bit32[" + this.ToExpression("bnot", "String") + "]) or (function(A, ...) return (MODM - A); end);",
					"BitSHL = ((bit32[" + this.ToExpression("lshift", "String") + "]) or (function(A, B, ...) if (B < 0) then return (BitSHR(A, -(B))); end; return ((A * 2 ^ B) % 2 ^ 32); end));",
					"BitSHR = ((bit32[" + this.ToExpression("rshift", "String") + "]) or (function(A, B, ...) if (B < 0) then return (BitSHL(A, -(B))); end; return (Floor(A % 2 ^ 32 / 2 ^ B)); end));"
				}.Shuffle<string>()),
				this.ToExpression("bit32", "String"),
				this.ToExpression("bit", "String"),
				string.Join("\n", new List<string>
				{
					"bit32[" + this.ToExpression("bxor", "String") + "] = BitXOR;",
					"bit32[" + this.ToExpression("band", "String") + "] = BitAND;",
					"bit32[" + this.ToExpression("bor", "String") + "] = BitOR;",
					"bit32[" + this.ToExpression("bnot", "String") + "] = BitNOT;",
					"bit32[" + this.ToExpression("lshift", "String") + "] = BitSHL;",
					"bit32[" + this.ToExpression("rshift", "String") + "] = BitSHR;"
				}.Shuffle<string>()),
				this.GetLuaGeneration(),
				string.Join("\n", new List<string>
				{
					string.Concat(new string[]
					{
						"local Remove = (Environment[",
						this.ToExpression("table", "String"),
						"][",
						this.ToExpression("remove", "String"),
						"]);"
					}),
					string.Concat(new string[]
					{
						"local Insert = (Environment[",
						this.ToExpression("table", "String"),
						"][",
						this.ToExpression("insert", "String"),
						"]);"
					}),
					string.Concat(new string[]
					{
						"local Concatenate = (Environment[",
						this.ToExpression("table", "String"),
						"][",
						this.ToExpression("concat", "String"),
						"]);"
					}),
					string.Concat(new string[]
					{
						"local Create = (((Environment[",
						this.ToExpression("table", "String"),
						"][",
						this.ToExpression("create", "String"),
						"])) or ((function(Size, ...) return ({ UnPack({}, 0, Size); }); end)));"
					})
				}.Shuffle<string>()),
				this.ToExpression("bit32", "String"),
				this.ObfuscationContext.InitialPrimaryXORKey,
				this.GetLuaGeneration(),
				ScriptBuilder.Random.Next(0, 256),
				ScriptBuilder.Random.Next(0, 256),
				ByteCodeFormattingTable
			});
			this.Functions = string.Concat(new string[]
			{
				"\n\n",
				this.GetLuaGeneration(),
				"\n\n",
				string.Join("\n", new List<string>
				{
					"local function gBits32() local W, X, Y, Z = Byte(ByteString, Position, Position + 3); W = BitXOR(W, PrimaryXORKey); PrimaryXORKey = W % 256; X = BitXOR(X, PrimaryXORKey); PrimaryXORKey = X % 256; Y = BitXOR(Y, PrimaryXORKey); PrimaryXORKey = Y % 256; Z = BitXOR(Z, PrimaryXORKey); PrimaryXORKey = Z % 256; Position = Position + 4; return ((Z * 16777216) + (Y * 65536) + (X * 256) + W); end;",
					"local function gBits16() local W, X = Byte(ByteString, Position, Position + 2); W = BitXOR(W, PrimaryXORKey); PrimaryXORKey = W % 256; X = BitXOR(X, PrimaryXORKey); PrimaryXORKey = X % 256; Position = Position + 2; return ((X * 256) + W); end;",
					"local function gBits8() local F = BitXOR(Byte(ByteString, Position, Position), PrimaryXORKey); PrimaryXORKey = F % 256; Position = (Position + 1); return (F); end;",
					"local function gBit(Bit, Start, End) if (End) then local R = (Bit / 2 ^ (Start - 1)) % 2 ^ ((End - 1) - (Start - 1) + 1); return (R - (R % 1)); else local P = 2 ^ (Start - 1); return (((Bit % (P + P) >= P) and (1)) or(0)); end; end;"
				}.Shuffle<string>()),
				" \n\nlocal Mode = ",
				this.ToExpression("#", "String"),
				"; local function _R(...) return ({...}), Select(Mode, ...); end;"
			});
		}

		public string BuildScript(string Location)
		{
			this.GenerateHeader();
			this.GenerateDeserializer();
			this.GenerateVM();
			bool flag = !this.ObfuscationSettings.CompressedOutput;
			if (flag)
			{
				this.Deserializer = this.ReplaceNumbers(this.Deserializer);
				this.Deserializer = this.ExpandNumberStatements(this.Deserializer);
				this.Deserializer = "local function Deserialize(...) " + this.Deserializer + " return (Deserialize(...)); end;";
				this.Variables = this.ReplaceNumbers(this.Variables);
				this.Variables = this.ExpandNumberStatements(this.Variables);
				bool maximumSecurityEnabled = this.ObfuscationSettings.MaximumSecurityEnabled;
				if (maximumSecurityEnabled)
				{
					this.Variables += "local function Calculate(Index, Value, ...)";
					foreach (KeyValuePair<long, NumberEquation> NumberEquationPair in this.NumberEquations)
					{
						this.Variables += string.Format("if (Index == {0}) then return ({1});", NumberEquationPair.Key, NumberEquationPair.Value.WriteStatement());
						bool flag2 = NumberEquationPair.Key != this.NumberEquations.Last<KeyValuePair<long, NumberEquation>>().Key;
						if (flag2)
						{
							this.Variables += "else";
						}
					}
					this.Variables += " end; end;";
				}
				this.Functions = this.ReplaceNumbers(this.Functions);
				this.Functions = this.ExpandNumberStatements(this.Functions);
			}
			this.Variables += "local function CalculateVM(Index, Value, ...)";
			foreach (KeyValuePair<long, NumberEquation> NumberEquationPair2 in this.ObfuscationContext.NumberEquations)
			{
				this.Variables += string.Format("if (Index == {0}) then return ({1});", NumberEquationPair2.Key, NumberEquationPair2.Value.WriteStatement());
				bool flag3 = NumberEquationPair2.Key != this.NumberEquations.Last<KeyValuePair<long, NumberEquation>>().Key;
				if (flag3)
				{
					this.Variables += "else";
				}
			}
			this.Variables += " end; end;";
			string VMTable = "";
			this.Expressions.Shuffle<ScriptBuilder.Expression>();
			foreach (ScriptBuilder.Expression Expression in this.Expressions)
			{
				string Index = Expression.Indicies.Random<string>();
				bool flag4 = Index.StartsWith(".");
				if (flag4)
				{
					Index = Index.Remove(0, 1);
				}
				VMTable += string.Format("{0}=({1});", Index, Expression.Data);
			}
			bool flag5 = !this.ObfuscationSettings.CompressedOutput;
			if (flag5)
			{
				VMTable = this.ExpandNumberStatements(VMTable);
			}
			string Source = string.Concat(new string[]
			{
				"return (function(T, ...) local TEXT = \"This file was obfuscated using PSU Obfuscator 4.0.A | https://www.psu.dev/ & discord.gg/psu\"; ",
				this.Variables,
				this.Functions,
				this.Deserializer,
				this.VM
			});
			Source = Source + " \nend)(({" + VMTable + "}), ...);";
			bool enhancedOutput = this.ObfuscationSettings.EnhancedOutput;
			if (enhancedOutput)
			{
				Match Match;
				string Replacement;
				for (int SearchPosition = 0; SearchPosition < Source.Length; SearchPosition += Match.Index + Replacement.Length)
				{
					string Substring = Source.Substring(SearchPosition);
					Match = Regex.Match(Substring, "PSU_LUA_GENERATION");
					bool success = Match.Success;
					if (!success)
					{
						break;
					}
					Replacement = this.<BuildScript>g__GenerateCode|31_0();
					Source = Source.Substring(0, SearchPosition + Match.Index) + Replacement + Source.Substring(SearchPosition + Match.Index + Match.Length);
				}
			}
			bool maximumSecurityEnabled2 = this.ObfuscationSettings.MaximumSecurityEnabled;
			if (maximumSecurityEnabled2)
			{
				LuaOptions LuaOptions = new LuaOptions(true, false, true, false, true, true, true, true, true, false, true, true, ContinueType.ContextualKeyword);
				LuaLexerBuilder LexerBuilder = new LuaLexerBuilder(LuaOptions);
				LuaParserBuilder ParserBuilder = new LuaParserBuilder(LuaOptions);
				DiagnosticList Diagnostics = new DiagnosticList();
				ILexer<LuaTokenType> Lexer = LexerBuilder.CreateLexer(Source, Diagnostics);
				TokenReader<LuaTokenType> TokenReader = new TokenReader<LuaTokenType>(Lexer);
				LuaParser Parser = ParserBuilder.CreateParser(TokenReader, Diagnostics);
				StatementList Tree = Parser.Parse();
				Source = VMFormattedLuaCodeSerializer.Format(LuaOptions.All, Tree);
			}
			File.WriteAllText(Path.Combine(Location, "VM.lua"), Source);
			Process Process = new Process
			{
				StartInfo = 
				{
					WorkingDirectory = "Lua/lua",
					FileName = "Lua/LuaJIT.exe",
					Arguments = string.Concat(new string[]
					{
						"LuaSrcDiet.lua --maximum --opt-entropy --opt-emptylines --opt-eols --opt-numbers --opt-whitespace --opt-locals --noopt-strings \"",
						Path.GetFullPath(Path.Combine(Location, "VM.lua")),
						"\" -o \"",
						Path.GetFullPath(Path.Combine(Location, "Output.lua")),
						"\""
					}),
					UseShellExecute = false,
					RedirectStandardError = !this.ObfuscationSettings.DebugMode,
					RedirectStandardOutput = !this.ObfuscationSettings.DebugMode
				}
			};
			Process.Start();
			Process.WaitForExit();
			Source = File.ReadAllText(Path.Combine(Location, "Output.lua"), ScriptBuilder.LuaEncoding).Replace("\n", " ");
			Source = Source.Replace("[.", "[0.");
			Source = Utility.FinalReplaceStrings(Source);
			File.WriteAllText(Path.Combine(Location, "Output.lua"), Source.Replace("PSU_BYTECODE", this.ObfuscationContext.ByteCode).Replace("PSU_FORMAT_TABLE", this.ObfuscationContext.FormatTable));
			return Source;
		}

		[CompilerGenerated]
		internal static string <GenerateDeserializer>g__GetInlinedOrDefault|28_0(string ToInline, string Inlined, string Function, dynamic XORKey)
		{
			bool flag = ScriptBuilder.Random.Next(1, 2) == 0;
			string result;
			if (flag)
			{
				if (ScriptBuilder.<>o__28.<>p__2 == null)
				{
					ScriptBuilder.<>o__28.<>p__2 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(ScriptBuilder)));
				}
				Func<CallSite, object, string> target = ScriptBuilder.<>o__28.<>p__2.Target;
				CallSite <>p__ = ScriptBuilder.<>o__28.<>p__2;
				if (ScriptBuilder.<>o__28.<>p__1 == null)
				{
					ScriptBuilder.<>o__28.<>p__1 = CallSite<Func<CallSite, string, string, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "Replace", null, typeof(ScriptBuilder), new CSharpArgumentInfo[]
					{
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null),
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, null),
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
					}));
				}
				Func<CallSite, string, string, object, object> target2 = ScriptBuilder.<>o__28.<>p__1.Target;
				CallSite <>p__2 = ScriptBuilder.<>o__28.<>p__1;
				string arg = Function + "\n" + Inlined;
				string arg2 = "XOR_KEY";
				if (ScriptBuilder.<>o__28.<>p__0 == null)
				{
					ScriptBuilder.<>o__28.<>p__0 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", null, typeof(ScriptBuilder), new CSharpArgumentInfo[]
					{
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
					}));
				}
				result = target(<>p__, target2(<>p__2, arg, arg2, ScriptBuilder.<>o__28.<>p__0.Target(ScriptBuilder.<>o__28.<>p__0, XORKey)));
			}
			else
			{
				result = ToInline;
			}
			return result;
		}

		[CompilerGenerated]
		private string <GenerateVM>g__FormatVMHandle|29_0(VOpCode Virtual)
		{
			string Obfuscated = Virtual.GetObfuscated(this.ObfuscationContext);
			bool flag = !this.ObfuscationSettings.ConstantEncryption || this.ObfuscationSettings.EnhancedConstantEncryption;
			if (flag)
			{
				Obfuscated = Obfuscated.Replace("Constants[Instruction[OP_A]]", "Instruction[OP_A]");
				Obfuscated = Obfuscated.Replace("Constants[Instruction[OP_B]]", "Instruction[OP_B]");
				Obfuscated = Obfuscated.Replace("Constants[Instruction[OP_C]]", "Instruction[OP_C]");
			}
			return Obfuscated;
		}

		[CompilerGenerated]
		private string <GenerateVM>g__FormatEnum|29_1(int Enum)
		{
			return this.RandomizeNumberStatement((long)Enum);
		}

		[CompilerGenerated]
		private string <GenerateVM>g__GetString|29_2(List<int> OpCodes)
		{
			string String = "";
			bool flag = OpCodes.Count == 1;
			if (flag)
			{
				string Obfuscated = this.<GenerateVM>g__FormatVMHandle|29_0(this.Virtuals[OpCodes[0]]);
				String += Obfuscated;
			}
			else
			{
				bool flag2 = OpCodes.Count == 2;
				if (flag2)
				{
					int num = ScriptBuilder.Random.Next(0, 2);
					int num2 = num;
					if (num2 != 0)
					{
						if (num2 == 1)
						{
							String = string.Concat(new string[]
							{
								String,
								"if (Enum == ",
								this.<GenerateVM>g__FormatEnum|29_1(this.Virtuals[OpCodes[0]].VIndex),
								") then\n",
								this.<GenerateVM>g__FormatVMHandle|29_0(this.Virtuals[OpCodes[0]])
							});
							String = string.Concat(new string[]
							{
								String,
								"elseif (Enum <= ",
								this.<GenerateVM>g__FormatEnum|29_1(this.Virtuals[OpCodes[1]].VIndex),
								") then\n",
								this.<GenerateVM>g__FormatVMHandle|29_0(this.Virtuals[OpCodes[1]])
							});
							String += "end;";
						}
					}
					else
					{
						String = string.Concat(new string[]
						{
							String,
							"if (Enum > ",
							this.<GenerateVM>g__FormatEnum|29_1(this.Virtuals[OpCodes[0]].VIndex),
							") then\n",
							this.<GenerateVM>g__FormatVMHandle|29_0(this.Virtuals[OpCodes[1]])
						});
						String = string.Concat(new string[]
						{
							String,
							"elseif (Enum < ",
							this.<GenerateVM>g__FormatEnum|29_1(this.Virtuals[OpCodes[1]].VIndex),
							") then\n\n",
							this.<GenerateVM>g__FormatVMHandle|29_0(this.Virtuals[OpCodes[0]])
						});
						String += "end;";
					}
				}
				else
				{
					List<int> Ordered = (from OpCode in OpCodes
					orderby OpCode
					select OpCode).ToList<int>();
					List<int>[] Sorted = new List<int>[]
					{
						Ordered.Take(Ordered.Count / 2).ToList<int>(),
						Ordered.Skip(Ordered.Count / 2).ToList<int>()
					};
					String = String + "if (Enum <= " + this.<GenerateVM>g__FormatEnum|29_1(Sorted[0].Last<int>()) + ") then ";
					String += this.<GenerateVM>g__GetString|29_2(Sorted[0]);
					String += "else";
					String += this.<GenerateVM>g__GetString|29_2(Sorted[1]);
				}
			}
			return String;
		}

		[CompilerGenerated]
		private string <BuildScript>g__GenerateCode|31_0()
		{
			bool flag = !this.ObfuscationSettings.EnhancedOutput;
			string result;
			if (flag)
			{
				result = "";
			}
			else
			{
				switch (ScriptBuilder.Random.Next(0, 3))
				{
				case 0:
					result = " do local function _(...) " + LuaGeneration.VarArgSpam(1, 1) + " end; end; ";
					break;
				case 1:
					result = " do local function _(...) " + LuaGeneration.GenerateRandomFile(1, 1) + " end; end; ";
					break;
				case 2:
					result = "";
					break;
				default:
					result = "";
					break;
				}
			}
			return result;
		}

		private List<string> extraStrings;

		private static Random Random = new Random();

		public static Encoding LuaEncoding = Encoding.GetEncoding(28591);

		private Chunk HeadChunk;

		private ObfuscationContext ObfuscationContext;

		private ObfuscationSettings ObfuscationSettings;

		private List<VOpCode> Virtuals;

		private string Variables;

		private string Functions;

		private string Deserializer;

		private string VM;

		private List<ScriptBuilder.Expression> Expressions = new List<ScriptBuilder.Expression>();

		[Dynamic(new bool[]
		{
			false,
			true,
			false
		})]
		private Dictionary<dynamic, ScriptBuilder.Expression> ExpressionMap = new Dictionary<object, ScriptBuilder.Expression>();

		private List<ScriptBuilder.Expression> NumberExpressions = new List<ScriptBuilder.Expression>();

		private Dictionary<long, ScriptBuilder.Expression> NumberExpressionMap = new Dictionary<long, ScriptBuilder.Expression>();

		[Dynamic(new bool[]
		{
			false,
			true
		})]
		private List<dynamic> UsedIndicies = new List<object>();

		private Dictionary<long, NumberEquation> NumberEquations = new Dictionary<long, NumberEquation>();

		public class Expression
		{
			[Dynamic]
			public dynamic Data;

			public List<string> Indicies = new List<string>();
		}
	}
}
