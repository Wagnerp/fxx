using System;
using libfxx.iface;
using System.Security.Cryptography;

namespace libfxx.core
{
	/// <summary>
	/// Calculates SHA-1 hashes 
	/// </summary>

	public class Sha1HashCalculator : IHashCalculator
	{
		private SHA1CryptoServiceProvider m_cspProvider;

		public Sha1HashCalculator()
		{
			m_cspProvider = new SHA1CryptoServiceProvider();
		}

		#region IHashCalculator implementation
		public string Calculate (byte[] bufBuffer)
		{
			byte[] arrHash = m_cspProvider.ComputeHash(bufBuffer);
			string strStringHash = BitConverter.ToString(arrHash);

			// Remove dashes from hash
			return strStringHash.Replace("-", String.Empty);
		}
		#endregion
	}
}

