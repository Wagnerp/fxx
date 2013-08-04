using System;
using libfxx.persistence;
using System.Collections.Generic;

namespace libfxx.core
{
	/// <summary>
	/// Evaluates the response given by the InstallationAuditor to determine
	/// the state of the installer - what the likely product is, whether
	/// there are patches in use etc.
	/// </summary>

	public class AuditResponseEvaluator
	{
		public AuditResponseEvaluator()
		{
		}

		public Product Evaluate (AuditResponse arResponse)
		{
			try 
			{
				// If there is a definitive product attached to the response
				// then that is the product in use
	
				if ((arResponse.Product is UnidentifiedProduct) == false) 
				{
					return arResponse.Product;
				}

				// Otherwise, we iterate each component and try to tally up 
				// exactly what the product is (i.e. if it isn't definitive we
				// determine based on votes)
				//
				// This dictionary is product hash -> votes

				Dictionary<string, int> dicVotes = new Dictionary<string, int> ();

				// Iterate the known components and tally up the votes 

				foreach (Component compComponent in arResponse.IdentifiedComponents) 
				{
					foreach (string strProductHash in compComponent.Products) 
					{
						if (dicVotes.ContainsKey (strProductHash) == true) 
						{
							dicVotes [strProductHash]++;
						} 
						else 
						{
							dicVotes.Add (strProductHash, 0);
						}
					}
				}

				return new UnidentifiedProduct ();
			} 
			catch (Exception ex) 
			{
				throw new Exception("Failed to evaluate audit response", ex);
			}
		}
	}
}

