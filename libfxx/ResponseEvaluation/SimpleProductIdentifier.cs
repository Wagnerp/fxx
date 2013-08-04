using System;
using libfxx.iface;

namespace libfxx.core
{
	/// <summary>
	/// Performs a basic identification using SHA-1 hashes and the provided 
	/// database
	/// </summary>

	public class SimpleProductIdentifier : IProductIdentifier
	{
		/// <summary>
		/// Constructor
		/// </summary>

		public SimpleProductIdentifier ()
		{
		}

		/// <summary>
		/// Identify the given installation using the provided database
		/// </summary>
		/// <param name="instInstallation">Product installation</param>
		/// <param name="dbDatabase">Database connection</param>

		public IdentificationResults Identify (Installation instInstallation, 
		                                       IDatabase dbDatabase)
		{
			try
			{
				// Create the auditor and use SHA-1
				InstallationAuditor iaAuditor = 
					new InstallationAuditor(dbDatabase, new Sha1HashCalculator());
				
				// Determine what files we know and which ones we don't
				AuditResponse arResponse = iaAuditor.Audit(instInstallation);
			
				// Evaluate the response: this turns the listing produced by
				// the audit into something we could display on a UI
				//
				ResponseEvaluator reEval = new ResponseEvaluator(dbDatabase);
				return reEval.Evaluate(arResponse);
			}
			catch (Exception ex)
			{
				throw new Exception("Failed to identify installation", ex);
			}
		}

		/// <summary>
		/// Save details associated with the provided results back to the database
		/// </summary>
		/// <param name="irResults">Results in question</param>
		/// <param name="dbDatabase">Database connection</param>
		
		public void SaveResults (IdentificationResults irResults, IDatabase dbDatabase)
		{
			try
			{
				// Create an auditor
				InstallationAuditor iaAuditor = 
					new InstallationAuditor(dbDatabase, new Sha1HashCalculator());

				// Save the auditing response associated with the results
				// back to the database
				//
				iaAuditor.Save(irResults.Response);
			}
			catch (Exception ex)
			{
				throw new Exception("Failed to save identification results", ex);
			}
		}
	}
}

