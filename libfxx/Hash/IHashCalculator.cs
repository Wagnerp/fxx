using System;
using System.Collections.Generic;
using System.IO;

namespace libfxx.iface
{
	/// <summary>
	/// Interface for defining the calculation of hashes
	/// </summary>

	public interface IHashCalculator
	{
		/// <summary>
		/// Calculate the hash for the provided buffer of data
		/// </summary>
		/// <param name="bufBuffer">Data buffer (generally file data)</param>

		string Calculate(byte[] bufBuffer);
	}
}

