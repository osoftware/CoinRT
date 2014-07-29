﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoinRT
{
	/// <summary>
	/// A key in form of [1 byte version] : [n bytes raw key] : [4 bytes checksum].
	/// </summary>
	public class EncodedKey
	{
		private readonly Base58 data;

		public EncodedKey(byte version, byte[] key)
		{
			var rawData = new List<byte>(20);
			rawData.Add(version);
			rawData.AddRange(key);
			rawData.AddRange(rawData.Sha256().Sha256().Take(4));

			this.data = new Base58(rawData);
		}

		public EncodedKey(string encoded)
		{
			this.data = new Base58(encoded);

			if(!this.Checksum.SequenceEqual(this.Version.Concat(this.RawKey).Sha256().Sha256().Take(4)))
			{
				throw new ArgumentException("Checksum is invalid");
			}
		}

		public byte Version
		{
			get
			{
				return this.data.ToByteArray()[0];
			}
		}

		public byte[] RawKey
		{
			get
			{
				var data = this.data.ToByteArray();
				return data.Skip(1).Take(data.Length - 5).ToArray();
			}
		}

		public byte[] Checksum
		{
			get
			{
				var data = this.data.ToByteArray();
				return data.Skip(data.Length - 4).ToArray();
			}
		}

		public override string ToString()
		{
			return this.data.ToString();
		}

		public override int GetHashCode()
		{
			return this.data.GetHashCode();
		}
	}
}