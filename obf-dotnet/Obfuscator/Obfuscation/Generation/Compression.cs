using System;
using System.Collections.Generic;
using System.Text;

namespace Obfuscator.Obfuscation.Generation
{
	public static class Compression
	{
		public static string ToBase36(ulong Value)
		{
			Random Random = new Random();
			StringBuilder StringBuilder = new StringBuilder(13);
			do
			{
				bool flag = Random.Next(0, 2) == 0;
				if (flag)
				{
					StringBuilder.Insert(0, "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ"[(int)((byte)(Value % 36UL))]);
				}
				else
				{
					StringBuilder.Insert(0, "0123456789abcdefghijklmnopqrstuvwxyz"[(int)((byte)(Value % 36UL))]);
				}
				Value /= 36UL;
			}
			while (Value > 0UL);
			return StringBuilder.ToString();
		}

		public static string CompressedToString(List<int> Compressed, ObfuscationSettings ObfuscationSettings)
		{
			StringBuilder StringBuilder = new StringBuilder();
			foreach (int Integer in Compressed)
			{
				string String = Compression.ToBase36((ulong)((long)Integer));
				String = Compression.ToBase36((ulong)((long)String.Length)) + String;
				byte[] Bytes = Compression.LuaEncoding.GetBytes(String);
				String = "";
				for (int I = 0; I < Bytes.Length; I++)
				{
					String += Compression.LuaEncoding.GetString(new byte[]
					{
						Bytes[I]
					});
				}
				StringBuilder.Append(String);
			}
			return StringBuilder.ToString();
		}

		public static List<int> Compress(byte[] Bytes)
		{
			Dictionary<string, int> Dictionary = new Dictionary<string, int>();
			for (int Integer = 0; Integer < 256; Integer++)
			{
				Dictionary.Add(((char)Integer).ToString(), Integer);
			}
			string String = string.Empty;
			List<int> Compressed = new List<int>();
			foreach (byte Byte in Bytes)
			{
				string str = String;
				char c = (char)Byte;
				string W = str + c.ToString();
				bool flag = Dictionary.ContainsKey(W);
				if (flag)
				{
					String = W;
				}
				else
				{
					Compressed.Add(Dictionary[String]);
					Dictionary.Add(W, Dictionary.Count);
					c = (char)Byte;
					String = c.ToString();
				}
			}
			bool flag2 = !string.IsNullOrEmpty(String);
			if (flag2)
			{
				Compressed.Add(Dictionary[String]);
			}
			return Compressed;
		}

		public static Encoding LuaEncoding = Encoding.GetEncoding(28591);

		public const string UpperCaseBase36 = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";

		public const string LowerCaseBase36 = "0123456789abcdefghijklmnopqrstuvwxyz";
	}
}
