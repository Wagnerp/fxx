using System;
using libfxx.persistence;
using System.Collections.Generic;
using libfxx.iface;
using System.Linq;

namespace libfxx.core
{
	/// <summary>
	/// Evaluates the response given by the InstallationAuditor to determine
	/// the state of the installer - what the likely product is, whether
	/// there are patches in use etc.
	/// </summary>

	public class ResponseEvaluator
	{
		private const string UNIDENTIFIED = "Unknown component or version";
		private const string PARENT_MISSING = "Associated product details missing";
		private const string MULTI_IDENT = "Recognised shared component";
		private const string PATCHED_VERSION = "Patched version of {0}";
		private const string GUESS_WARNING = "HEURISTIC GUESS";

		private IDatabase m_dbDatabase;

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="dbDatabase">Database to acquire product details from</param>

		public ResponseEvaluator (IDatabase dbDatabase)
		{
			m_dbDatabase = dbDatabase;
		}

		/// <summary>
		/// Evaluate the audit response and create identification results
		/// fit for display or reporting
		/// </summary>
		/// <param name="arResponse">Audit response to evaluate</param>

		public IdentificationResults Evaluate (AuditResponse arResponse)
		{
			try 
			{
				IdentificationResults irResults = new IdentificationResults();
				irResults.Response = arResponse;

				// Determine product 
				irResults.Product = DetermineProduct(arResponse);

				// Iterate identified files
				foreach(Component compComponent in arResponse.IdentifiedComponents)
				{
					FileDisplayDetails fddDetails = new FileDisplayDetails();
					fddDetails.Name = compComponent.Name;
					fddDetails.TruncatedHash = compComponent.Hash.Substring(0, 7);

					// Is the overall product not known?
					if ((arResponse.Product is UnidentifiedProduct) == true) 
					{
						// Is the component linked only against a single 
						// product?

						if (compComponent.Products.Count == 1)
						{
							// Obtain the product
							Product prdProduct = 
								m_dbDatabase.LoadProduct(compComponent.Products[0]);

							// If the product can't be obtained we have an issue
							if (prdProduct == null)
							{
								fddDetails.Status = PARENT_MISSING;
								fddDetails.Flag = FileDisplayFlag.Unrecognised;
							}
							else
							{
								// Otherwise use the version text
								fddDetails.Status = prdProduct.ToShortString();
								fddDetails.Flag = FileDisplayFlag.Recognised;
							}
						}
						else
						{
							// If it is linked against multiple products it is
							// a recognised shared component
							//
							fddDetails.Status = MULTI_IDENT;
							fddDetails.Flag = FileDisplayFlag.RecognisedShared;
						}
					}
					else
					{
						// If the product is known, use the response product
						// name and flag as recognised

						fddDetails.Status = arResponse.Product.ToShortString();
						fddDetails.Flag = FileDisplayFlag.Recognised;
					}

					irResults.Files.Add(fddDetails);
				}

				// Iterate unknown files
				foreach(Component compComponent in arResponse.UnidentifiedComponents)
				{
					// Create details for each unknown file
					FileDisplayDetails fddDetails = new FileDisplayDetails();

					fddDetails.Name = compComponent.Name;
					fddDetails.TruncatedHash = compComponent.Hash.Substring(0, 7);
					fddDetails.Status = UNIDENTIFIED;
					fddDetails.Flag = FileDisplayFlag.Unrecognised;

					// Add to results
					irResults.Files.Add(fddDetails);
				}

				return irResults;
			} 
			catch (Exception ex) 
			{
				throw new Exception("Failed to evaluate audit response", ex);
			}
		}

		/// <summary>
		/// Determines the overall product in use. Either this is just the
		/// overall product determined by total hash, or it is a guess based
		/// upon the products which the components are associated with (e.g.
		/// guessing that it is a modified build of version XYZ)
		/// </summary>
		/// <returns>The most sensible guess at a product name</returns>
		/// <param name="arResponse">The audit response to determine the
		/// product from</param>

		private Product DetermineProduct(AuditResponse arResponse)
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

				// Order the dictionary by votes

				Dictionary<string, int> dicOrdered = dicVotes.OrderBy(x => x.Value).
					ToDictionary(pair => pair.Key, pair => pair.Value);

				// Load the most likely product
				Product prdLikely = 
					m_dbDatabase.LoadProduct(dicOrdered.Keys.First()); 

				// Create a new unidentified product and make it use the name
				// and version of the likely product - but make it clear this
				// is a guess

				Product prdModified = new UnidentifiedProduct();
				prdModified.Name = String.Format(PATCHED_VERSION, prdLikely.Name);
				prdModified.Architecture = prdLikely.Architecture;
				prdModified.Build = prdLikely.Build;
				prdModified.Platform = prdLikely.Platform;
				prdModified.State = ModificationState.Modified;
				prdModified.Type = GUESS_WARNING;
				prdModified.Version = prdLikely.Version;

				return prdModified;
			} 
			catch (Exception ex) 
			{
				throw new Exception("Failed to determine product", ex);
			}
		}
	}
}

