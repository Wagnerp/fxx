using System;
using libfxx.iface;
using System.Collections.Generic;
using System.Text;

namespace libfxx.core
{
	/// <summary>
	/// Generates combined (snowball) hashes from an input set of hashes
	/// </summary>

	public class SnowballHashCalculator
	{
		private IHashCalculator m_hcCalculator;

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="hcCalculator">Calculator to use for hash creation</param>

		public SnowballHashCalculator(IHashCalculator hcCalculator)
		{
			m_hcCalculator = hcCalculator;
		}

		/// <summary>
		/// Calculate a snowball hash from a given collection of hashes
		/// </summary>
		/// <param name="enHashes">Hashes to create a combined hash from</param>

		public string Calculate (IEnumerable<string> enHashes)
		{
			StringBuilder sbCombinedHash = new StringBuilder ();

			foreach (string strHash in enHashes) 
			{
				sbCombinedHash.Append(strHash);
			}

			string strCombinedHash = sbCombinedHash.ToString();
			byte[] arrHashInBytes = System.Text.Encoding.Unicode.GetBytes(strCombinedHash);

			return m_hcCalculator.Calculate(arrHashInBytes);
		}
	}
}

