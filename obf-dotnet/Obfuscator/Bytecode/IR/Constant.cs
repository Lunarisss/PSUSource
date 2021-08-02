using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Obfuscator.Bytecode.IR
{
	[Serializable]
	public class Constant
	{
		public Constant()
		{
			this.BackReferences = new List<Instruction>();
		}

		public Constant(Constant Constant)
		{
			this.Type = Constant.Type;
			this.Data = Constant.Data;
			this.BackReferences = Constant.BackReferences.ToList<Instruction>();
		}

		public List<Instruction> BackReferences;

		public ConstantType Type;

		[Dynamic]
		public dynamic Data;
	}
}
