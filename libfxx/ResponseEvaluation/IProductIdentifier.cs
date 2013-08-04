using System;
using libfxx.iface;

namespace libfxx.core
{
	/// <summary>
	/// Interface for a class producing identification results for a given
	/// database and installationm
	/// </summary>

	public interface IProductIdentifier
	{
		/// <summary>
		/// Identify the given installation using the provided database
		/// </summary>
		/// <param name="instInstallation">Product installation</param>
		/// <param name="dbDatabase">Database connection</param>

		IdentificationResults Identify(Installation instInstallation,
		                               IDatabase dbDatabase);

		/// <summary>
		/// Save details associated with the provided results back to the database
		/// </summary>
		/// <param name="irResults">Results in question</param>
		/// <param name="dbDatabase">Database connection</param>

		void SaveResults(IdentificationResults irResults, IDatabase dbDatabase);
	}
}

