using System;
using System.Collections.Generic;

namespace Obfuscator.Bytecode.IR
{
	[Serializable]
	public class Chunk
	{
		public void UpdateMappings()
		{
			this.InstructionMap.Clear();
			this.ConstantMap.Clear();
			this.ChunkMap.Clear();
			for (int I = 0; I < this.Instructions.Count; I++)
			{
				this.InstructionMap.Add(this.Instructions[I], I);
			}
			for (int I2 = 0; I2 < this.Constants.Count; I2++)
			{
				this.ConstantMap.Add(this.Constants[I2], I2);
			}
			for (int I3 = 0; I3 < this.Chunks.Count; I3++)
			{
				this.ChunkMap.Add(this.Chunks[I3], I3);
			}
		}

		public string Name = "";

		public byte ParameterCount;

		public byte VarArgFlag;

		public byte StackSize;

		public int Line;

		public int LastLine;

		public int CurrentOffset;

		public int CurrentParameterOffset;

		public byte UpValueCount;

		public List<string> UpValues;

		public List<Instruction> Instructions;

		public Dictionary<Instruction, int> InstructionMap;

		public List<Constant> Constants;

		public Dictionary<Constant, int> ConstantMap;

		public List<Chunk> Chunks;

		public Dictionary<Chunk, int> ChunkMap;

		public int XORKey = new Random().Next(0, 256);
	}
}
