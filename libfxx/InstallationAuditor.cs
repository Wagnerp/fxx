using System;
using libfxx.iface;
using libfxx.persistence;
using System.IO;

namespace libfxx.core
{
	/// <summary>
	/// Performs audits of a given installation to determine what they are 
	/// and whether data about them has already been recorded
	/// </summary>

	public class InstallationAuditor
	{
		private IDatabase m_dbDatabase;
		private IHashCalculator m_hcCalculator;
	
		/// <summary>
		/// Whether component details should be retrieved even when there is a 
		/// successful match of an overall product hash
		/// </summary>
		/// <remarks>Generally this is unnecessary: It is slower and doesn't
		/// really reduce false positives, but is a useful debugging/safety
		/// measure</remarks>

		public bool RetrieveComponentsOnSuccessfulMatch 
		{
			get;
			set;
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="dbDatabase">Database to use for cross-referencing</param>
		/// <param name="hcCalculator">Hash calculator</param>

		public InstallationAuditor(IDatabase dbDatabase, IHashCalculator hcCalculator)
		{
			m_dbDatabase = dbDatabase;
			m_hcCalculator = hcCalculator;

			RetrieveComponentsOnSuccessfulMatch = false;
		}

		/// <summary>
		/// Audit the specified instInstallation.
		/// </summary>
		/// <param name="instInstallation">Installation to perform an audit
		/// of</param>

		public AuditResponse Audit (Installation instInstallation)
		{
			// Response
			AuditResponse arResponse = new AuditResponse();

			// Calculate the total hash of the installation
			string strHash = instInstallation.CalculateHash (m_hcCalculator);

			// Attempt to collect a product with this hash
			Product prdInstallProduct = m_dbDatabase.LoadProduct (strHash);

			// No total hash for this installation is known
			if (prdInstallProduct == null)
			{
				// Create a dummy product because this product is unidentified
				arResponse.Product = new UnidentifiedProduct();
				arResponse.Product.Hash = strHash;

				// Audit individual components
				AuditComponents(instInstallation, arResponse);
			} 
			else 
			{
				arResponse.Product = prdInstallProduct;

				// If we have a valid install but we're forcing component audits
				if (RetrieveComponentsOnSuccessfulMatch == true) 
				{
					AuditComponents(instInstallation, arResponse);
				}
			}

			return arResponse;
		}

		/// <summary>
		/// Audit each component in the installation individually
		/// </summary>
		/// <param name="instInstallation">The installation to audit the components
		/// of</param>
		/// <param name="arResponse">The response from auditing the components</param>

		private void AuditComponents (Installation instInstallation, 
		                             AuditResponse arResponse)
		{
			foreach (FileInfo fiFile in instInstallation.Files.Keys) 
			{
				string strComponentHash = instInstallation.Files[fiFile];

				// See if we know this component already
				Component compRetrieved = m_dbDatabase.LoadComponent(strComponentHash);

				// If we don't, create a new component and add it to the un-ID'd list
				if (compRetrieved == null)
				{
					Component compUnknown = new Component();
					compUnknown.Name = fiFile.Name;
					compUnknown.Hash = strComponentHash;

					arResponse.UnidentifiedComponents.Add(compUnknown);
				}
				else
				{
					// Otherwise store the retrieved component
					arResponse.IdentifiedComponents.Add(compRetrieved);
				}
			}
		}

	}
}

