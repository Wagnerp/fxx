using System;
using libfxx.persistence;

namespace libfxx.persistence
{
	/// <summary>
	/// Represents a product of unknown identification
	/// </summary>

	public class UnidentifiedProduct : Product 
	{
		public UnidentifiedProduct()
		{
			Name = "?";
			Architecture = "?";
			Build = "?";
			Platform = "?";
			Type = "?";
			Version = "?";
			Hash = "?";
		}
	}
}

