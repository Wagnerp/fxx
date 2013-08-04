using System;
using libfxx.iface;
using libfxx.persistence;
using System.IO;
using System.Collections.Generic;
using System.Linq;

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
			RetrieveComponentsOnSuccessfulMatch = true;
		}

		/// <summary>
		/// Audit the specified instInstallation.
		/// </summary>
		/// <param name="instInstallation">Installation to perform an audit
		/// of</param>

		public AuditResponse Audit (Installation instInstallation)
		{
			try
			{
				// Response
				AuditResponse arResponse = new AuditResponse ();

				// Calculate the total hash of the installation
				string strHash = instInstallation.CalculateHash (m_hcCalculator);

				// Attempt to collect a product with this hash
				Product prdInstallProduct = m_dbDatabase.LoadProduct (strHash);

				// No total hash for this installation is known
				if (prdInstallProduct == null)
				{
					// Create a dummy product because this product is unidentified
					arResponse.Product = new UnidentifiedProduct ();
					arResponse.Product.Hash = strHash;

					// Audit individual components
					AuditComponents (instInstallation, arResponse);
				}
				else
				{
					arResponse.Product = prdInstallProduct;

					// If we have a valid install but we're forcing component audits
					if (RetrieveComponentsOnSuccessfulMatch == true)
					{
						AuditComponents (instInstallation, arResponse);
					}
				}

				return arResponse;
			}
			catch (Exception ex)
			{
				throw new Exception(String.Format("Failed to perform audit " +
				       "for installation at [{0}]", instInstallation.Path), ex);
			}
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
			try
			{
				foreach (FileInfo fiFile in instInstallation.Files.Keys)
				{
					string strComponentHash = instInstallation.Files [fiFile];

					// See if we know this component already
					Component compRetrieved = m_dbDatabase.LoadComponent (strComponentHash);

					// If we don't, create a new component and add it to the un-ID'd list
					if (compRetrieved == null)
					{
						Component compUnknown = new Component ();
						compUnknown.Name = fiFile.Name;
						compUnknown.Hash = strComponentHash;

						arResponse.UnidentifiedComponents.Add (compUnknown);
					}
					else
					{
						// Otherwise store the retrieved component
						arResponse.IdentifiedComponents.Add (compRetrieved);
					}
				}
			}
			catch (Exception ex)
			{
				throw new Exception(String.Format("Failed to perform component " +
				      "audit for installation at [{0}]", instInstallation.Path), ex);
			}
		}

		/// <summary>
		/// Save an audit response. This updates information in the database 
		/// according to the components which have been found in the
		/// installation
		/// </summary>
		/// <remarks>If the product is unidentified, it is expected that
		/// its name and details will have been filled out before this method
		/// is called to save it into the database</remarks>
		/// <param name="arResponse">Audit response to save back into the 
		/// database</param>

		public void Save (AuditResponse arResponse)
		{
			try
			{
				// TODO: Seriously need to consider ordering/save conflicts 
				// by using the database's revision identifier. 
				//
				// Shouldn't be an issue for small time use but should fix
				//

				Product prdProduct = arResponse.Product;

				// Save the product
				m_dbDatabase.SaveProduct (arResponse.Product);

				// Get all identified and unidentified components
				IEnumerable<Component> enComponents = 
				arResponse.IdentifiedComponents.Union (arResponse.UnidentifiedComponents);

				foreach (Component compComponent in enComponents)
				{
					// Add the product hash if required
					if (compComponent.Products.Contains (prdProduct.Hash) == false)
					{
						compComponent.Products.Add (prdProduct.Hash);
					}

					// Save (or update) the component
					m_dbDatabase.SaveComponent (compComponent);
				}
			}
			catch (Exception ex)
			{
				throw new Exception("Failed to save audit response details", ex);
			}
		}
	}
}

