using System;
using Newtonsoft.Json;

namespace libfxx.persistence
{
	public class Product
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
		/// Version of the product e.g. 4.1.5 
		/// </summary>
		
		public string Version 
		{
			get;
			set;
		}
		
		/// <summary>
		/// Build of the product e.g. 1321
		/// </summary>
		
		public string Build
		{
			get;
			set;
		}
		
		/// <summary>
		/// Platform of the product e.g Windows
		/// </summary>
		
		public string Platform 
		{
			get;
			set;
		}
		
		/// <summary>
		/// Architecture of the product e.g. x64
		/// </summary>
		
		public string Architecture 
		{
			get;
			set;
		}
		
		/// <summary>
		/// Type of product, e.g. released, beta, internal build etc.
		/// </summary>
		
		public string Type 
		{
			get;
			set;
		}

		/// <summary>
		/// Hash of product (snowball)
		/// </summary>
		
		[JsonIgnore]
		public string Hash
		{
			get;
			set;
		}

		public Product ()
		{
		}
	}
}

