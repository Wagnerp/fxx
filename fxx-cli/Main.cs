using System;
using libfxx.core;
using System.Collections.Generic;
using System.IO;
using libfxx.persistence;
using libfxx;

namespace fxxcli
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			CouchDatabase db = new CouchDatabase();
			db.Connect("http://localhost:5984");

			Installation inst = new Installation(@"/Users/David/Dev/varcars/build/env");

			InstallationAuditor iaAuditor = 
				new InstallationAuditor(db, new Sha1HashCalculator());
		
			iaAuditor.Audit(inst);



//			FileHashCalculator fhcFileHasher = new FileHashCalculator(new Sha1HashCalculator());
//
//			IDictionary<FileInfo, string> dicResults = 
//				fhcFileHasher.Calculate(Directory.EnumerateFiles(@"/Users/David/Dev/varcars/build/env"));
//
//			string strSnowball = fhcFileHasher.CalculateSnowball(dicResults.Values);
//
//			CouchDatabase db = new CouchDatabase();
//			db.Connect("http://localhost:5984");
//
//			Component dipbus = db.LoadComponent("ca23");
//			dipbus.Products.Add("foobar9000");
//			db.SaveComponent(dipbus);


//			Product Darwin = new Product();
//			Darwin.Name = "DARWIN";
//			Darwin.Architecture = "x64";
//			Darwin.Build = "1321";
//			Darwin.Platform = "Windows";
//			Darwin.Type = "Released";
//			Darwin.Version = "4.1.5";
//			Darwin.Hash = "abc123";
//
//			db.SaveProduct(Darwin);
//
//
//
//			Component comp = new Component();
//			comp.Name = "DIPBUS00.dll";
//			comp.Hash = "ca23";
//			comp.Notes = "n/a";
//			comp.Products.Add(Darwin.Hash);
//
//			db.SaveComponent(comp);


		}
	}
}
