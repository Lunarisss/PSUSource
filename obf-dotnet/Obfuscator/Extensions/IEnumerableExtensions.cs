using System;
using System.Collections.Generic;

namespace Obfuscator.Extensions
{
	public static class IEnumerableExtensions
	{
		public static IList<T> Shuffle<T>(this IList<T> List)
		{
			for (int I = 0; I < List.Count; I++)
			{
				List.Swap(I, IEnumerableExtensions.Generator.Next(I, List.Count));
			}
			return List;
		}

		public static void Swap<T>(this IList<T> List, int I, int J)
		{
			T Temp = List[I];
			List[I] = List[J];
			List[J] = Temp;
		}

		public static T Random<T>(this IList<T> List)
		{
			return List[IEnumerableExtensions.Generator.Next(0, List.Count)];
		}

		private static Random Generator = new Random();
	}
}
