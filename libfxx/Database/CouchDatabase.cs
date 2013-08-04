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

		public void Connect (string strHost)
		{
			try
			{
				m_connConnection = new Connection (new Uri (strHost));

				// Create the database if necessary
				if (!m_connConnection.ListDatabases ().Contains (DATABASE_NAME))
				{
					m_connConnection.CreateDatabase (DATABASE_NAME);
				}

				// Attach our session to the fxx database
				m_sesSession = m_connConnection.CreateSession (DATABASE_NAME);
			}
			catch (Exception ex)
			{
				throw new Exception(
					String.Format("Failed to connect to database [{0}] on " +
				              "host [{1}]", DATABASE_NAME, strHost), ex);
			}
		}

		/// <summary>
		/// Save the details of a specified product
		/// </summary>
		/// <param name="prdProduct">Product details to save</param>

		public void SaveProduct (Product prdProduct)
		{
			try
			{
				m_sesSession.Save (prdProduct, prdProduct.Hash);
			}
			catch (Exception ex)
			{
				throw new Exception(
					String.Format("Failed to save product [{0}]",
				              prdProduct.Name), ex);
			}
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
			catch
			{
				// If for any reason we can't retrieve, assume product doesn't
				// exist
				return null;
			}
		}

		/// <summary>
		/// Save the details of the specified component
		/// </summary>
		/// <param name="compComponent">Component details to save</param>

		public void SaveComponent (Component compComponent)
		{
			try
			{
				m_sesSession.Save (compComponent, compComponent.Hash);
			}
			catch (Exception ex)
			{
				throw new Exception (
					String.Format ("Failed to save component [{0}]",
				              compComponent.Name), ex);
			}
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
			catch
			{
				// If for any reason we can't retrieve, assume component doesn't
				// exist
			
				return null;
			}
		}
	}
}

