using System;
using System.Collections.Generic;
using System.IO;
using libfxx.iface;

namespace libfxx.core
{
	/// <summary>
	/// Calculate hashes for files
	/// </summary>

	public class FileHashCalculator
	{
		private IHashCalculator m_hcCalculator;
		private SnowballHashCalculator m_sbhSnowball;

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="hcCalculator">Calculator used for hashing files</param>

		public FileHashCalculator (IHashCalculator hcCalculator)
		{
			m_hcCalculator = hcCalculator;
			m_sbhSnowball = new SnowballHashCalculator(hcCalculator);
		}

		/// <summary>
		/// Calculate a hash for a file via the provided filename
		/// </summary>
		/// <param name="strFilename">Filename of the file to hash</param>

		public string Calculate (string strFilename)
		{
			return m_hcCalculator.Calculate(File.ReadAllBytes(strFilename));
		}

		/// <summary>
		/// Calculate hashes for all files in the provided enumerable
		/// </summary>
		/// <param name="enPaths">Set of full paths to files which require
		/// hashing</param>

		public IDictionary<FileInfo, string> Calculate (IEnumerable<string> enPaths)
		{
			Dictionary<FileInfo, string> dicResults = new Dictionary<FileInfo, string>();

			foreach (string strFilename in enPaths) 
			{
				FileInfo fiInfo = new FileInfo(strFilename);
				dicResults.Add(fiInfo, Calculate(strFilename));
			}

			return dicResults;
		}

		/// <summary>
		/// Calculate a snowball (hash of hashes) for the given hash values
		/// </summary>
		/// <returns>Combined (snowball) hash</returns>
		/// <param name="enHashes">Incoming hashes to generate a hash from</param>

		public string CalculateSnowball (IEnumerable<string> enHashes)
		{
			return m_sbhSnowball.Calculate(enHashes);
		}


	}
}

