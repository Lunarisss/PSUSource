using System;
using Obfuscator.Obfuscation.OpCodes;

namespace Obfuscator.Obfuscation
{
	[Serializable]
	public class CustomInstructionData
	{
		public VOpCode OpCode;

		public VOpCode WrittenOpCode;

		public bool Mutated = false;

		public bool Serialize = true;
	}
}
