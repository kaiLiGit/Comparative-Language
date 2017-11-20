using System;
using System.Linq;
using System.IO;
using System.Diagnostics;
using System.Collections.Generic; 
using System.Text.RegularExpressions; 

namespace HW4 {
	public class MailMerge {
	public static void mergingFile(string tsvFileName, string tmpFileName) {
		// dlist for mapping column names with corresponding column values 
		Dictionary<string, string> dlist = new Dictionary<string, string>();
		string line = ""; 
		try 
		{ 
		// pass the file path and file name to the StreamReader constructor 
	        //StreamReader sr = new StreamReader(tsvFileName);
	        using (var sr = new StreamReader(tsvFileName)) {	
		// read the first line of text 
		line = sr.ReadLine();
		
		// return immediately if first line is an empty string
	        if (line == null) return;
	        // clist, column name list 
		List<string> clist = new List<string>();
		if (!string.IsNullOrEmpty(line)) {
			// using split to delimit the line string into a list 
			clist = line.ToString().Split('\t').ToList();	
		}	
		foreach (string ele in clist) 
		{
			// populate the dlist with only "keys" 
			dlist[ele] = ""; 
		}
		// string dstring = string.Join(";", dlist);
		//Console.WriteLine("dlist: " + dstring);
 		List<string> vlist = new List<string>(); // column value list 
		// finish reading the first line of text
		// continue to read until the EOF
		while (line != null) 
		{
		// Read the next line 
		line = sr.ReadLine();
	        //Console.WriteLine("line from tsv:" + line);
		if (!string.IsNullOrEmpty(line)) {
		// delimit the value list by tab 
		vlist = line.ToString().Split('\t').ToList();
		}
	        int cIter = 0; // counter for clist
		// populate dlist with each individual record (1 line per account) 	
		foreach (string val in vlist) {
		   if (cIter < clist.Count()) {    // bound checking 
			dlist[clist[cIter]] = val; 
		   }
		// finish populating dlist
		cIter++; 
		}

		// pass dlist and tmpFileName to tmpMerge
		processTmpFile(ref dlist, tmpFileName);
		
		// string dstring = string.Join(";", dlist);
		// Console.WriteLine("dlist: " + dstring);
		}
		//sr.Close();
		}
		} catch(Exception e)
		{
		//int i = Convert.ToInt32(e.StackTrace.Substring(e.StackTrace.LastIndexOf(' ')));	
		Console.WriteLine("Exception: " + e);
		}
	}

	public static void processTmpFile(ref Dictionary<string, string> dlist, string tmpFileName) {
		string wFileName = dlist["ID"]+ ".txt"; // ouput file name
		String line = ""; 
		try {
			// processing tmp file content 
			StreamReader tmpReader = new StreamReader(tmpFileName);
			// Get a list of values within tags
			List<string> tvalueList = new List<string>();
			//Console.WriteLine("value from tags: " + string.Join("\t", tvalueList));	
			// Read the first line 
			line = tmpReader.ReadLine();
		        // regex class for pattern finding 
			Regex pattern = null; 
			string re = ""; // regular expression 	
			// use using to auto open and close file writing 
			using (var writer = new StreamWriter(File.Create(wFileName))) {
			while (line != null) 
			{
			// using regex.match to find all values in one line record with regex "<<(.*?)>>"
			tvalueList = Regex.Matches(line, @"<<(.*?)>>").OfType<Match>().
						  Select(m => m.Groups[1].Value).ToList();
			// subtitute each tag with value from dlist 
			foreach(string val in tvalueList) {
				re = "<<" + val + ">>"; 
				pattern = new Regex(re);
				line = pattern.Replace(line, dlist[val]);
			}
			// end of subtituion
			// write to new file begins 
			writer.WriteLine(line);
			// end of writing for one record 
		        // Console.WriteLine("value from tags: " + string.Join("\t", tvalueList));	
			// reading new lines 
			line = tmpReader.ReadLine();
			}
			Console.WriteLine("finish writing to :" + wFileName);
			}
			//close tmpReader
			tmpReader.Close(); 
		} catch(Exception e) {
		Console.WriteLine("Exception: " + e.Message);
		}
	}

	static void Main(string[] args) 
	{ 
		if (args.Length != 2) {
		Console.WriteLine(String.Format("Missing file name arguments, Your arguments: {0}", string.Join(", ", args)));		      }
	        if (args.Length == 2){
		Console.WriteLine("args 1:" + args[0] + " args 2:" + args[1]);
		mergingFile(args[0], args[1]);
		}
	}
	}
}
