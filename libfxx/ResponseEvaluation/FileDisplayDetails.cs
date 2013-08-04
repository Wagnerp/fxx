using System;

namespace libfxx.core
{
	/// <summary>
	/// File details collected together expressly for being displayed on screen
	/// </summary>

	public class FileDisplayDetails
	{
		/// <summary>
		/// Name of the file
		/// </summary>

		public string Name
		{
			get;
			set;
		}

		/// <summary>
		/// Hash of the file, truncated to 7 characters
		/// </summary>

		public string TruncatedHash
		{
			get;
			set;
		}

		/// <summary>
		/// Status of the file (what product it is associated with or
		/// whether it is identified or not)
		/// </summary>

		public string Status
		{
			get;
			set;
		}

		/// <summary>
		/// Additional flag/information 
		/// </summary>

		public FileDisplayFlag Flag
		{
			get;
			set;
		}

		/// <summary>
		/// Constructor
		/// </summary>

		public FileDisplayDetails ()
		{
		}

		/// <summary>
		/// Format the file details for display (e.g. command line display)
		/// </summary>
		/// <returns>A <see cref="System.String"/> that represents the current <see cref="libfxx.core.FileDisplayDetails"/>.</returns>

		public override string ToString()
		{
			return String.Format("{0,-30}{1,-9}{2,-40}", 
			                     Name, TruncatedHash.Substring(0, 7),
			                     Status);
		}

	}
}

