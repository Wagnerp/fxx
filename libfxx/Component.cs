using System;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace libfxx.persistence
{
	public class Component
	{
		/// <summary>
		/// Name of the product e.g. 'DARWIN'
		/// </summary>
		 
		public string Name 
		{
			get;
			set;
		}

		/// <summary>
		/// Set of product IDs that the component is associated with
		/// </summary>

		public List<string> Products
		{
			get;
			set;
		}

		/// <summary>
		/// Any additional notes about the product
		/// </summary>

		public string Notes 
		{
			get;
			set;
		}

		/// <summary>
		/// Hash of the given component
		/// </summary>

		[JsonIgnore]
		public string Hash
		{
			get;
			set;
		}


		public Component()
		{
			Products = new List<string>();
		}
	}
}

