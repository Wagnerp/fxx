using System;
using libfxx.persistence;
using System.Collections.Generic;

namespace libfxx.core
{
	/// <summary>
	/// The response from having audited a given installation
	/// </summary>

	public class AuditResponse
	{
		private Product m_prdProduct;
		private List<Component> m_lstIdentified;
		private List<Component> m_lstUnidentified;

		/// <summary>
		/// The product the installation represents
		/// </summary>

		public Product Product 
		{
			get 
			{
				return m_prdProduct;
			}

			set 
			{
				m_prdProduct = value;
			}
		}

		/// <summary>
		/// The list of known (identified) components in the installation
		/// </summary>

		public List<Component> IdentifiedComponents 
		{
			get 
			{
				return m_lstIdentified;
			}
		}

		/// <summary>
		/// The list of unknown (unidentified) components in the installation
		/// </summary>

		public List<Component> UnidentifiedComponents 
		{
			get 
			{
				return m_lstUnidentified;
			}
		}

		/// <summary>
		/// Constructor
		/// </summary>

		public AuditResponse ()
		{
			m_prdProduct = new UnidentifiedProduct();
			m_lstIdentified = new List<Component>();
			m_lstUnidentified = new List<Component>();
		}


	}
}

