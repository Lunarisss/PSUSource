using System;
using System.Collections.Generic;
using System.Linq;
using Obfuscator.Bytecode.IR;
using Obfuscator.Extensions;
using Obfuscator.Obfuscation.Generation;
using Obfuscator.Obfuscation.OpCodes;
using Obfuscator.Utility;

namespace Obfuscator.Obfuscation
{
	public class ObfuscationContext
	{
		public ObfuscationContext(Chunk HeadChunk)
		{
			this.HeadChunk = HeadChunk;
			this.InstructionMapping = new Dictionary<OpCode, VOpCode>();
			this.ChunkSteps = (from I in Enumerable.Range(0, 5)
			select (ChunkStep)I).ToArray<ChunkStep>();
			this.ChunkSteps.Shuffle<ChunkStep>();
			this.InstructionSteps = (from I in Enumerable.Range(0, 4)
			select (InstructionStep)I).ToArray<InstructionStep>();
			this.InstructionSteps.Shuffle<InstructionStep>();
			this.InstructionStepsABC = this.InstructionSteps.ToArray<InstructionStep>().Shuffle<InstructionStep>().ToArray<InstructionStep>();
			this.InstructionStepsABx = this.InstructionSteps.ToArray<InstructionStep>().Shuffle<InstructionStep>().ToArray<InstructionStep>();
			this.InstructionStepsAsBx = this.InstructionSteps.ToArray<InstructionStep>().Shuffle<InstructionStep>().ToArray<InstructionStep>();
			this.InstructionStepsAsBxC = this.InstructionSteps.ToArray<InstructionStep>().Shuffle<InstructionStep>().ToArray<InstructionStep>();
			this.PrimaryXORKey = this.Random.Next(0, 256);
			this.InitialPrimaryXORKey = this.PrimaryXORKey;
			this.XORKeys = new int[]
			{
				this.Random.Next(0, 256),
				this.Random.Next(0, 256),
				this.Random.Next(0, 256),
				this.Random.Next(0, 256),
				this.Random.Next(0, 256),
				this.Random.Next(0, 256),
				this.Random.Next(0, 256),
				this.Random.Next(0, 256),
				this.Random.Next(0, 256),
				this.Random.Next(0, 256)
			};
			this.Instruction.A = Utility.GetIndexListNoBrackets();
			this.Instruction.B = Utility.GetIndexListNoBrackets();
			this.Instruction.C = Utility.GetIndexListNoBrackets();
			this.Instruction.D = Utility.GetIndexListNoBrackets();
			this.Instruction.E = Utility.GetIndexListNoBrackets();
			this.Instruction.Enum = Utility.GetIndexListNoBrackets();
			this.Instruction.Data = Utility.GetIndexListNoBrackets();
			this.Chunk.ParameterCount = Utility.GetIndexListNoBrackets();
			this.Chunk.Instructions = Utility.GetIndexListNoBrackets();
			this.Chunk.Chunks = Utility.GetIndexListNoBrackets();
			this.Chunk.Constants = Utility.GetIndexListNoBrackets();
			this.Chunk.StackSize = Utility.GetIndexListNoBrackets();
			this.Chunk.InstructionPoint = Utility.GetIndexListNoBrackets();
			this.DeserializerInstructionSteps = new List<DeserializerInstructionStep>();
			this.PrimaryIndicies = new List<string>
			{
				"A",
				"B",
				"C"
			}.Shuffle<string>().ToList<string>();
			this.NumberEquations = new Dictionary<long, NumberEquation>();
			int Count = this.Random.Next(5, 15);
			for (int I2 = 0; I2 < Count; I2++)
			{
				this.NumberEquations.Add((long)this.Random.Next(0, 1000000000), new NumberEquation(this.Random.Next(3, 6)));
			}
			int _ = this.Random.Next(0, 16);
			int[] array = new int[6];
			array[0] = _;
			array[5] = (array[4] = (array[3] = (array[2] = (array[1] = _ + this.Random.Next(1, 16)) + this.Random.Next(1, 16)) + this.Random.Next(1, 16)) + this.Random.Next(1, 16)) + this.Random.Next(1, 16);
			this.ConstantMapping = array;
			this.ConstantMapping.Shuffle<int>();
		}

		public Chunk HeadChunk;

		public Dictionary<OpCode, VOpCode> InstructionMapping;

		public ChunkStep[] ChunkSteps;

		public InstructionStep[] InstructionStepsABC;

		public InstructionStep[] InstructionStepsABx;

		public InstructionStep[] InstructionStepsAsBx;

		public InstructionStep[] InstructionStepsAsBxC;

		public InstructionStep[] InstructionSteps;

		public int[] ConstantMapping;

		public int PrimaryXORKey;

		public int InitialPrimaryXORKey;

		public int[] XORKeys;

		public Obfuscator Obfuscator;

		private Random Random = new Random();

		public InstructionMap Instruction = default(InstructionMap);

		public ChunkMap Chunk = default(ChunkMap);

		public List<DeserializerInstructionStep> DeserializerInstructionSteps;

		public List<string> PrimaryIndicies;

		public string ByteCode;

		public string FormatTable;

		public Dictionary<long, NumberEquation> NumberEquations;
	}
}
