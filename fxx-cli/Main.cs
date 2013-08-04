using System;
using libfxx.core;
using System.Collections.Generic;
using System.IO;
using libfxx.persistence;
using libfxx;
using System.Reflection;
using libfxx.iface;

namespace fxxcli
{
	class MainClass
	{
		/// <summary>
		/// Command line interface to fxx
		/// </summary>
		/// <remarks>
		/// 
		/// Usage:
		/// fxxcli <installation directory> <database server> 
		/// 
		/// </remarks>
		/// <param name="args">Command line arguments</param>

		public static void Main (string[] args)
		{
			IDatabase dbConnection = null;

			if (args.Length != 2 && args.Length != 3)
			{
				ShowUsage ();
				return;
			}

			// Attempt database connection
			Console.WriteLine ("Connecting to {0}", args [1]);

			try
			{
				CouchDatabase cdCouch = new CouchDatabase ();
				cdCouch.Connect (args [1]);

				// Use the couch connection for further database comms
				dbConnection = cdCouch;
			}
			catch (Exception ex)
			{
				// Cannot connect: stop
				Console.WriteLine ("Failed to connect to {0}: aborting", 
				                  args [1]);

				Console.WriteLine();
				Console.WriteLine (ex);

				return;
			}

			Console.WriteLine ("Successfully connected. Analysing...");
			Console.WriteLine ();

			try
			{
				// Create an installation of the specified location
				Installation instProduct = new Installation (args [0]);

				SimpleProductIdentifier spiIdentifier = new SimpleProductIdentifier();

				IdentificationResults irResults = 
					spiIdentifier.Identify(instProduct, dbConnection);

				// Iterate each file in the results
				foreach(FileDisplayDetails fddDetails in irResults.Files)
				{
					// Determine text colour by state
					switch(fddDetails.Flag)
					{
					case FileDisplayFlag.Recognised:
						Console.ForegroundColor = ConsoleColor.Green;
						break;
					case FileDisplayFlag.RecognisedShared:
						Console.ForegroundColor = ConsoleColor.DarkGreen;
						break;
					case FileDisplayFlag.Unrecognised:
						Console.ForegroundColor = ConsoleColor.Red;
						break;
					}

					Console.WriteLine(fddDetails.ToString());
				}

				Console.WriteLine();

				// Change text colour depending on whether total hash is recognised
				if (irResults.Product is UnidentifiedProduct)
				{
					Console.ForegroundColor = ConsoleColor.Red;
				}
				else
				{
					Console.ForegroundColor = ConsoleColor.Green;
				}

				Console.WriteLine("{0}", irResults.Product.ToLongString());
				Console.WriteLine("({0})", instProduct.Hash);
				Console.ForegroundColor = ConsoleColor.White;
				Console.WriteLine();

				// If we have three arguments and the third one is to submit
				// details, and we have an unidentified product, then prompt
				// for product details

				if (args.Length == 3 && args[2] == "-submit")
				{
					if (irResults.Product is UnidentifiedProduct)
					{
						Product prdNewProduct = irResults.Response.Product;
						prdNewProduct.Name = PromptForText("Product name");
						prdNewProduct.Version = PromptForText("Product version");
						prdNewProduct.Build = PromptForText("Product build");
						prdNewProduct.Platform = PromptForText("Product platform");
						prdNewProduct.Architecture = PromptForText("Product architecture");
						prdNewProduct.Type = PromptForText("Product type (e.g. beta)");
						spiIdentifier.SaveResults(irResults, dbConnection);

					}
				}

			}
			catch (Exception ex)
			{
				
				Console.WriteLine ("Failed to analyse {0}: ensure the " +
				                   "path is valid and read permissions are " +
				                   "available. Aborting", args [0]);

				Console.WriteLine();
				Console.WriteLine (ex);
			}


		}

		private static string PromptForText (string strProperty)
		{
			Console.Write("{0}> ", strProperty);
			return Console.ReadLine();
		}

		/// <summary>
		/// Show the usage text for fxx-cli
		/// </summary>

		private static void ShowUsage ()
		{
			try
			{
				using (Stream stream = 
			       Assembly.GetExecutingAssembly().
			       GetManifestResourceStream("fxxcli.Usage.txt"))
				{
					using (StreamReader reader = new StreamReader(stream))
					{
						string strUsage = reader.ReadToEnd ();
						Console.Write (strUsage);
					}
				}
			}
			catch
			{
				Console.WriteLine("Usage text unavailable. Please consult " +
				                  "standalone documentation for assistance");
			}
		}
	}
}
