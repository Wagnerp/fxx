using System;
using System.Collections.Generic;
using libfxx.persistence;

namespace libfxx.core
{
	/// <summary>
	/// Results of the overall identification process: details of each individual
	/// file and the product that has been determined as relevant
	/// </summary>

	public class IdentificationResults
	{
		private List<FileDisplayDetails> m_lstFiles;
	
		/// <summary>
		/// Set of files and their associated hashes and states
		/// </summary>

		public List<FileDisplayDetails> Files
		{
			get
			{
				return m_lstFiles;
			}
		}

		/// <summary>
		/// Overall product result (what the installation is - if anything)
		/// </summary>

		public Product Product
		{
			get;
			set;
		}

		/// <summary>
		/// The auditing response the results are based upon
		/// </summary>

		public AuditResponse Response
		{
			get;
			set;
		}

		/// <summary>
		/// Constructor
		/// </summary>

		public IdentificationResults()
		{
			m_lstFiles = new List<FileDisplayDetails>();
			Product = new UnidentifiedProduct();
			Response = new AuditResponse();
		}
	}
}

