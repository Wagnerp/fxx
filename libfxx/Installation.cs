using System;
using libfxx.core;
using System.IO;
using System.Collections.Generic;
using libfxx.iface;

namespace libfxx.core
{
	/// <summary>
	/// Represents an installation of a product or application
	/// </summary>

	public class Installation
	{
		private IDictionary<FileInfo, string> m_dicFiles;

		/// <summary>
		/// Path of the installation (directory)
		/// </summary>

		public string Path
		{
			get;
			set;
		}

		/// <summary>
		/// Set of files as a dictionary between file details and the relevant
		/// hash
		/// </summary>

		public IDictionary<FileInfo, string> Files 
		{
			get 
			{
				return m_dicFiles;
			}
		}

		/// <summary>
		/// Hash of the overall installation
		/// </summary>

		public string Hash
		{
			get;
			set;
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="strDirectory">Directory of the installation</param>

		public Installation (string strDirectory)
		{
			m_dicFiles = new Dictionary<FileInfo, string>();
			Path = strDirectory;
		}

		/// <summary>
		/// Calculate the total hash of the installation
		/// </summary>
		/// <param name="hcCalculator">Calculator for creating hashes</param>
		/// <returns>Total hash of the installation</returns>

		public string CalculateHash(IHashCalculator hcCalculator)
		{
			FileHashCalculator fhcCalculator = new FileHashCalculator(hcCalculator);

			m_dicFiles = fhcCalculator.Calculate(Directory.EnumerateFiles(Path));
			Hash = fhcCalculator.CalculateSnowball(Files.Values);

			return Hash;
		}


	}
}

