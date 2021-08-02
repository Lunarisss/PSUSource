using System;
using System.Collections.Generic;

namespace Obfuscator.Obfuscation
{
	public struct ChunkMap
	{
		public List<string> ParameterCount;

		public List<string> Instructions;

		public List<string> Chunks;

		public List<string> Constants;

		public List<string> StackSize;

		public List<string> InstructionPoint;
	}
}
