using System;
using libfxx.persistence;

namespace libfxx.iface
{
	/// <summary>
	/// Interface for defining product and component storage in a database
	/// </summary>

	public interface IDatabase
	{
		/// <summary>
		/// Save the details of a specified product
		/// </summary>
		/// <param name="prdProduct">Product details to save</param>
		
		void SaveProduct (Product prdProduct);
		
		/// <summary>
		/// Load the details of a product with the given hash
		/// </summary>
		/// <returns>Product details or null if no such product</returns>
		/// <param name="strHash">Hash ID of the product to load</param>
		
		Product LoadProduct (string strHash);
		
		/// <summary>
		/// Save the details of the specified component
		/// </summary>
		/// <param name="compComponent">Component details to save</param>
		
		void SaveComponent(Component compComponent);
		
		/// <summary>
		/// Load the details of a component with the given hash 
		/// </summary>
		/// <returns>Component details or null if no such component</returns>
		/// <param name="strHash">Hash associated with the component to load</param>
		
		Component LoadComponent(string strHash);
	}
}

