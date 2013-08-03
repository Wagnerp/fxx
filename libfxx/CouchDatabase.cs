using System;
using RedBranch.Hammock;
using System.Linq;
using libfxx.persistence;
using libfxx.iface;

namespace libfxx.persistence
{
	public class CouchDatabase : IDatabase
	{
		private const string DATABASE_NAME = "fxx-db";
		private Connection m_connConnection;
		private Session m_sesSession;

		/// <summary>
		/// Connect to a couchdb host at the specified address
		/// </summary>
		/// <param name="strHost">Protocol hostname and port to connect to</param>

		public void Connect(string strHost)
		{
			m_connConnection = new Connection(new Uri(strHost));

			// Create the database if necessary
			if (!m_connConnection.ListDatabases().Contains(DATABASE_NAME))
			{
				m_connConnection.CreateDatabase(DATABASE_NAME);
			}

			// Attach our session to the fxx database
			m_sesSession = m_connConnection.CreateSession(DATABASE_NAME);
		}

		/// <summary>
		/// Save the details of a specified product
		/// </summary>
		/// <param name="prdProduct">Product details to save</param>

		public void SaveProduct (Product prdProduct)
		{
			m_sesSession.Save(prdProduct, prdProduct.Hash);
		}

		/// <summary>
		/// Load the details of a product with the given hash
		/// </summary>
		/// <returns>Product details or null if no such product</returns>
		/// <param name="strHash">Hash ID of the product to load</param>

		public Product LoadProduct (string strHash)
		{
			try 
			{
				Product prdProduct = m_sesSession.Load<Product> (strHash);
				prdProduct.Hash = strHash;
				
				return prdProduct;
			} 
			catch (CouchException cex) 
			{
				return null;
			}
		}

		/// <summary>
		/// Save the details of the specified component
		/// </summary>
		/// <param name="compComponent">Component details to save</param>

		public void SaveComponent(Component compComponent)
		{
			m_sesSession.Save(compComponent, compComponent.Hash);
		}

		/// <summary>
		/// Load the details of a component with the given hash 
		/// </summary>
		/// <returns>Component details or null if no such component</returns>
		/// <param name="strHash">Hash associated with the component to load</param>

		public Component LoadComponent(string strHash)
		{
			try 
			{
				Component compComponent = m_sesSession.Load<Component> (strHash);
				compComponent.Hash = strHash;

				return compComponent;

			} 
			catch (CouchException cex) 
			{
				return null;
			}
		}
	}
}

