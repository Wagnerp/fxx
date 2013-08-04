using System;
using libfxx.iface;
using System.Collections.Generic;
using System.Text;
using System.Linq;

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
			try
			{
				StringBuilder sbCombinedHash = new StringBuilder ();

				// Construct a giant digest of all hashes in a single string
				foreach (string strHash in enHashes)
				{
					sbCombinedHash.Append (strHash);
				}

				// Create a string
				string strCombinedHash = sbCombinedHash.ToString ();

				// Turn into bytes
				byte[] arrHashInBytes = System.Text.Encoding.Unicode.GetBytes (strCombinedHash);

				// Hash the bytes
				return m_hcCalculator.Calculate (arrHashInBytes);
			}
			catch (Exception ex)
			{
				throw new Exception("Failed to calculate snowball hash", ex);
			}
		}
	}
}

