using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using GParse;
using GParse.Collections;
using GParse.Lexing;
using Loretta;
using Loretta.Lexing;
using Loretta.Parsing;
using Loretta.Parsing.AST;
using Loretta.Parsing.Visitor;
using Obfuscator.Bytecode;
using Obfuscator.Bytecode.IR;
using Obfuscator.Extensions;
using Obfuscator.Obfuscation;
using Obfuscator.Obfuscation.Generation;
using Obfuscator.Obfuscation.Generation.Macros;
using Obfuscator.Obfuscation.OpCodes;
using Obfuscator.Obfuscation.Security;
using Obfuscator.Utility;

namespace Obfuscator
{
	public class Obfuscator
	{
		public Obfuscator(ObfuscationSettings ObfuscationSettings, string Location)
		{
			this.ObfuscationSettings = ObfuscationSettings;
			this.Location = Location;
		}

		private bool IsUsed(Chunk Chunk, VOpCode Virtual)
		{
			bool Return = false;
			foreach (Instruction Instruction in Chunk.Instructions)
			{
				bool flag = Virtual.IsInstruction(Instruction);
				if (flag)
				{
					bool flag2 = !this.ObfuscationContext.InstructionMapping.ContainsKey(Instruction.OpCode);
					if (flag2)
					{
						this.ObfuscationContext.InstructionMapping.Add(Instruction.OpCode, Virtual);
					}
					Instruction.CustomInstructionData = new CustomInstructionData
					{
						OpCode = Virtual
					};
					Return = true;
				}
			}
			foreach (Chunk sChunk in Chunk.Chunks)
			{
				Return |= this.IsUsed(sChunk, Virtual);
			}
			return Return;
		}

		public string ObfuscateString(string Source)
		{
			bool flag = !Directory.Exists(this.Location);
			if (flag)
			{
				Directory.CreateDirectory(this.Location);
			}
			File.WriteAllText(Path.Combine(this.Location, "Input.lua"), Source);
			File.WriteAllText(Path.Combine(this.Location, "Output.lua"), "");
			Obfuscator Obfuscator = new Obfuscator(new ObfuscationSettings(this.ObfuscationSettings), this.Location);
			string Error = "";
			Obfuscator.Compile(out Error);
			Obfuscator.Deserialize(out Error);
			Obfuscator.Obfuscate(out Error);
			return File.ReadAllText(Path.Combine(this.Location, "Output.lua"));
		}

		public bool Compile(out string Error)
		{
			Error = "";
			bool flag = !Directory.Exists(this.Location);
			bool result;
			if (flag)
			{
				Error = "[Error #1] File Directory Does Not Exist!";
				result = false;
			}
			else
			{
				bool flag2 = !File.Exists(Path.Combine(this.Location, "Input.lua"));
				if (flag2)
				{
					Error = "[Error #2] Input File Does Not Exist!";
					result = false;
				}
				else
				{
					for (;;)
					{
						bool flag3 = File.Exists(string.Format("{0}/LuaC.out", this.Location));
						if (flag3)
						{
							string Input = Path.Combine(this.Location, "Input.lua");
							string ByteCode = Path.Combine(this.Location, "LuaC.out");
							string src = File.ReadAllText(Input);
							Utility.GetExtraStrings(src);
							File.WriteAllText(Input, Obfuscator.ExtraHeader + "\n" + src);
							File.WriteAllText(Input, Obfuscator.ExtraHeader + "\n" + src);
							bool flag4 = !this.ObfuscationSettings.DisableAllMacros;
							if (flag4)
							{
								LuaOptions LuaOptions = new LuaOptions(true, true, true, false, true, true, true, true, true, true, true, true, ContinueType.ContextualKeyword);
								LuaLexerBuilder LexerBuilder = new LuaLexerBuilder(LuaOptions);
								LuaParserBuilder ParserBuilder = new LuaParserBuilder(LuaOptions);
								DiagnosticList Diagnostics = new DiagnosticList();
								ILexer<LuaTokenType> Lexer = LexerBuilder.CreateLexer(File.ReadAllText(Input), Diagnostics);
								TokenReader<LuaTokenType> TokenReader = new TokenReader<LuaTokenType>(Lexer);
								LuaParser Parser = ParserBuilder.CreateParser(TokenReader, Diagnostics);
								StatementList Tree = Parser.Parse();
								bool flag5 = Diagnostics.Any((Diagnostic Diagnostic) => Diagnostic.Severity == DiagnosticSeverity.Error);
								if (flag5)
								{
									break;
								}
								File.WriteAllText(Input, FormattedLuaCodeSerializer.Format(LuaOptions.All, Tree, new Func<string, string>(this.ObfuscateString), this.ObfuscationSettings.EncryptAllStrings, this.Location, this.ObfuscationSettings.PremiumFormat));
							}
							Process Process = new Process
							{
								StartInfo = 
								{
									FileName = "Lua/LuaC.exe",
									Arguments = string.Concat(new string[]
									{
										"-o \"",
										ByteCode,
										"\" \"",
										Input,
										"\""
									}),
									UseShellExecute = false,
									RedirectStandardError = true,
									RedirectStandardOutput = true
								}
							};
							Process.Start();
							Process.WaitForExit();
							bool flag6 = !this.Obfuscating;
							if (flag6)
							{
								goto Block_7;
							}
							bool flag7 = !File.Exists(ByteCode);
							if (flag7)
							{
								goto Block_8;
							}
						}
					}
					Error = "[?] [Parsing] Syntax Error";
					return false;
					Block_7:
					Error = "[?] Process Terminated.";
					return false;
					Block_8:
					Error = "[Error #3] Lua Error: Error While Compiling Script! (Syntax Error?)";
					result = false;
				}
			}
			return result;
		}

		public bool Deserialize(out string Error)
		{
			Error = "";
			Deserializer Deserializer = new Deserializer(File.ReadAllBytes(Path.Combine(this.Location, "LuaC.out")));
			try
			{
				this.HeadChunk = Deserializer.DecodeFile();
			}
			catch
			{
				Error = "[Error #4] Error While Deserializing File!";
				return false;
			}
			bool flag = !this.Obfuscating;
			bool result;
			if (flag)
			{
				Error = "[?] Process Terminated.";
				result = false;
			}
			else
			{
				this.ObfuscationContext = new ObfuscationContext(this.HeadChunk);
				this.ObfuscationContext.Obfuscator = this;
				result = true;
			}
			return result;
		}

		public bool Obfuscate(out string Error)
		{
			Error = "";
			List<VOpCode> AdditionalVirtuals = new List<VOpCode>();
			bool flag = !this.ObfuscationSettings.DisableAllMacros;
			if (flag)
			{
				new BytecodeSecurity(this.HeadChunk, this.ObfuscationSettings, this.ObfuscationContext, AdditionalVirtuals).DoChunks();
			}
			bool flag2 = !this.ObfuscationSettings.DisableAllMacros;
			if (flag2)
			{
				new LuaMacros(this.HeadChunk, AdditionalVirtuals).DoChunks();
			}
			bool controlFlowObfuscation = this.ObfuscationSettings.ControlFlowObfuscation;
			if (controlFlowObfuscation)
			{
				Obfuscator.<Obfuscate>g__ShuffleControlFlow|13_0(this.HeadChunk);
			}
			List<VOpCode> Virtuals = (from VOpCode T in (from T in Assembly.GetExecutingAssembly().GetTypes()
			where T.IsSubclassOf(typeof(VOpCode))
			select T).Select(new Func<Type, object>(Activator.CreateInstance))
			where this.IsUsed(this.HeadChunk, T)
			select T).ToList<VOpCode>();
			foreach (VOpCode Virtual in AdditionalVirtuals)
			{
				Virtuals.Add(Virtual);
			}
			bool flag3 = !this.ObfuscationSettings.DisableSuperOperators;
			if (flag3)
			{
				new SuperOperators().DoChunk(this.HeadChunk, Virtuals);
			}
			Virtuals.Shuffle<VOpCode>();
			for (int I = 0; I < Virtuals.Count; I++)
			{
				Virtuals[I].VIndex = I;
			}
			bool premiumFormat = this.ObfuscationSettings.PremiumFormat;
			if (premiumFormat)
			{
				Utility.NoExtraString = true;
				PremiumScriptBuilder ScriptBuilder = new PremiumScriptBuilder(this.HeadChunk, this.ObfuscationContext, this.ObfuscationSettings, Virtuals);
				string Source = ScriptBuilder.BuildScript(this.Location);
			}
			else
			{
				ScriptBuilder ScriptBuilder2 = new ScriptBuilder(this.HeadChunk, this.ObfuscationContext, this.ObfuscationSettings, Virtuals);
				string Source2 = ScriptBuilder2.BuildScript(this.Location);
			}
			return true;
		}

		[CompilerGenerated]
		internal static void <Obfuscate>g__ShuffleControlFlow|13_0(Chunk Chunk)
		{
			foreach (Chunk SubChunk in Chunk.Chunks)
			{
				Obfuscator.<Obfuscate>g__ShuffleControlFlow|13_0(SubChunk);
			}
			List<BasicBlock> BasicBlocks = new BasicBlock().GenerateBasicBlocks(Chunk);
			Instruction EntryPoint = Chunk.Instructions.First<Instruction>();
			Dictionary<int, BasicBlock> BlockMap = new Dictionary<int, BasicBlock>();
			BasicBlocks.Shuffle<BasicBlock>();
			int InstructionPoint = 0;
			foreach (BasicBlock Block in BasicBlocks)
			{
				foreach (Instruction Instruction in Block.Instructions)
				{
					BlockMap[InstructionPoint] = Block;
					InstructionPoint++;
				}
			}
			foreach (BasicBlock Block2 in BasicBlocks)
			{
				bool flag = Block2.Instructions.Count == 0;
				if (!flag)
				{
					Instruction Instruction2 = Block2.Instructions.Last<Instruction>();
					switch (Instruction2.OpCode)
					{
					case OpCode.OpJump:
						break;
					case OpCode.OpEq:
					case OpCode.OpLt:
					case OpCode.OpLe:
					case OpCode.OpTest:
					case OpCode.OpTestSet:
					case OpCode.OpTForLoop:
						Block2.Instructions.Add(new Instruction(Chunk, OpCode.OpJump, new object[]
						{
							Block2.References[0].Instructions[0]
						}));
						break;
					case OpCode.OpCall:
					case OpCode.OpTailCall:
						goto IL_1DF;
					case OpCode.OpReturn:
						break;
					case OpCode.OpForLoop:
					case OpCode.OpForPrep:
						Block2.Instructions.Add(new Instruction(Chunk, OpCode.OpJump, new object[]
						{
							Block2.References[0].Instructions[0]
						}));
						break;
					default:
						goto IL_1DF;
					}
					continue;
					IL_1DF:
					Block2.Instructions.Add(new Instruction(Chunk, OpCode.OpJump, new object[]
					{
						Block2.References[0].Instructions[0]
					}));
				}
			}
			Chunk.Instructions.Clear();
			Chunk.Instructions.Add(new Instruction(Chunk, OpCode.OpJump, new object[]
			{
				EntryPoint
			}));
			foreach (BasicBlock Block3 in BasicBlocks)
			{
				foreach (Instruction Instruction3 in Block3.Instructions)
				{
					Chunk.Instructions.Add(Instruction3);
				}
			}
			Chunk.UpdateMappings();
		}

		private Encoding LuaEncoding = Encoding.GetEncoding(28591);

		private Random Random = new Random();

		private ObfuscationContext ObfuscationContext;

		private ObfuscationSettings ObfuscationSettings;

		private Chunk HeadChunk;

		private string Location = "";

		public bool Obfuscating = true;

		public static string ExtraHeader = "local numberoffakes = 2000\nlocal fakes = {'DefaultChatSystemChatEvents','secrun','is_beta','secure_call','cache_replace','get_thread_identity','request','protect_gui','run_secure_lua','cache_invalidate','queue_on_teleport','is_cached','set_thread_identity','write_clipboard','run_secure_function','crypto','websocket','unprotect_gui','create_secure_function','crypt','syn','request','SayMessageRequest','FireServer','InvokeServer','tick','pcall','spawn','print','warn','game','GetService','getgc','getreg','getrenv','getgenv','getfenv','debug','require','ModuleScript','LocalScript','GetChildren','GetDescendants','function','settings','GameSettings','RenderSettings','string','sub','service','IsA','Parent','Name','RunService','Stepped','wait','Changed','FindFirstChild','Terrain','Lighting','Enabled','getconnections','firesignal','workspace','true','false','tostring','table','math','random','floor','Humanoid','Character','LocalPlayer','plr','Players','Player','WalkSpeed','Enum','KeyCode','_G','BreakJoints','Health','Chatted','RemoteEvent','RemoteFunction','getrawmetatable','make_writable','setreadonly','PointsService','JointsService','VRService','Ragdoll','SimulationRadiusLocaleId','gethiddenproperty','sethiddenproperty','syn','Zombies','GameId','JobId','Tool','Accessory','RightGrip','Weld','HumanoidRootPart','GuiService','CoreGui','BindableEvent','fire','BodyForce','Chat','PlayerGui','NetworkMarker','Geometry','TextService','LogService','error','LuaSettings','UserInputService','fireclickdetector','Trail','Camera','CurrentCamera','FOV','Path','InputObject','Frame','TextBox','ScreenGui','hookfunction','Debris','ReplicatedStorage','ReplicatedFirst','decompile','saveinstance','TweenService','SoundService','Teams','Tween','BasePart','Seat','Decal','Instance','new','Ray','TweenInfo','Color3','CFrame','Vector3','Vector2','UDim','UDim2','NumberRange','NumberSequence','Handle','Gravity','HopperBin','Shirt','Pants','Mouse','IntValue','StringValue','Value','VirtualUser','MouseButton1Click','Activated','FileMesh','TeleportService','Teleport','userdata','string','int','number','bool','BodyGyro','Backpack','SkateboardPlatform','FilteringEnabled','Shoot','Shell','Asset','checkifgay','create','god','BrianSucksVexu','checkifalive','getteams','getnearest','getcross','autoshoot','chatspam','changeupvalues','modifyguns','infammo','godmode','aimbot','esp','crashserver','antiaim'}\nlocal faked = {}\nfor i = 1,numberoffakes do\ntable.insert(faked, 'a34534345 = \\'' ..tostring(fakes[math.random(1,#fakes)])..'\\'')\nend\ntable.concat(faked,'\\n')";
	}
}
