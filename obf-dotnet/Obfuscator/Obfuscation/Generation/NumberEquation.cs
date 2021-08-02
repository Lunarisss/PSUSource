using System;
using System.Collections.Generic;

namespace Obfuscator.Obfuscation.Generation
{
	public class NumberEquation
	{
		public NumberEquation(int StepCount)
		{
			for (int I = 0; I < StepCount; I++)
			{
				int Step = this.Random.Next(0, 2);
				this.Steps.Add(Step);
				int num = Step;
				int num2 = num;
				if (num2 != 0)
				{
					if (num2 == 1)
					{
						this.Values.Add((long)this.Random.Next(0, 1000000));
					}
				}
				else
				{
					this.Values.Add((long)this.Random.Next(0, 1000000));
				}
			}
		}

		public long ComputeExpression(long Value)
		{
			int Index = 0;
			foreach (int Step in this.Steps)
			{
				bool flag = Step == 0;
				if (flag)
				{
					Value ^= this.Values[Index];
				}
				else
				{
					bool flag2 = Step == 1;
					if (flag2)
					{
						Value += this.Values[Index];
					}
				}
				Index++;
			}
			return Value;
		}

		public string WriteStatement()
		{
			string WrittenExpression = "Value";
			int Index = this.Steps.Count - 1;
			for (int I = this.Steps.Count - 1; I >= 0; I--)
			{
				int Step = this.Steps[I];
				bool flag = Step == 0;
				if (flag)
				{
					WrittenExpression = string.Format("BitXOR({0}, {1})", WrittenExpression, this.Values[Index]);
				}
				else
				{
					bool flag2 = Step == 1;
					if (flag2)
					{
						WrittenExpression = string.Format("({0}) - {1}", WrittenExpression, this.Values[Index]);
					}
				}
				Index--;
			}
			return WrittenExpression;
		}

		public List<int> Steps = new List<int>();

		public List<long> Values = new List<long>();

		private Random Random = new Random();
	}
}
