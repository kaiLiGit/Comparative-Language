/*
	Date: October 28, 2017
	HW4, MergingMail, retrieving column name and corresponding column values	from .tsv files and substitute all the "tagged" values in .tmp values 
	with the relevant values being retrived from .tsv file. 

	Note the code solution is all my personal work and does not guaranttee
	completely error-free solution, but it does fulfil the requirement of 
	this programming language comparison assignment. The same implementation 	of MailMerge is being implemented in both python and c#. The first 
	langugage promotes simplist approach (i.e. no semicolon and braces but 
	indentation) whereas the second promotes a combination of Java and C++
	features. 
	This program shall remain as is but only for viewing purpose not 
	for reference of any other use. Anyone attemptted to use or copy 
	this program by any means should be completely responsible for the 
	consequences that follow along that demeanor. Disclaimer: I myself 
	will not be responsible for any demage and violation of academic 
	instition policy if such program is copied or reused.   
*/
using System;
using System.Linq;
using System.IO;
using System.Diagnostics;
using System.Collections.Generic; 
using System.Text.RegularExpressions; 

namespace HW4 {
	public class MailMerge {

	public static void mergingFile(string tsvFileName, string tmpFileName) {
		Dictionary<string, string> dlist = new Dictionary<string, string>();
		string line = "      "; 
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
		while (line != null) {
		// Read the next line 
		line = sr.ReadLine();
	        //Console.WriteLine("line from tsv:" + line);
		vlist = line.ToString().Split('\t').ToList();
	        int cIter = 0; // counter for clist
		// populate dlist with each individual record (1 line per account) 	
		foreach (string val in vlist) {
		   if (cIter < clist.Count()) { 
			dlist[clist[cIter]] = val; 
		   }
		// finish populating dlist
		// pass dlist and tmpfilename to tmpMerge
		//Console.WriteLine("from merge:Process:" + dlist["ID"]); 
		//processTmpFile(ref dlist, tmpFileName);
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
		Console.WriteLine("Exception: " + e.Message + " exception ln : " + e.ToString());
		}
	}

	public static void processTmpFile(ref Dictionary<string, string> dlist, string tmpFileName) {
		string wFileName = dlist["ID"]+ ".txt"; // ouput file name
		//StreamWriter sw = null; // streamWriter for writing to new file for each individual 
		//Console.WriteLine("dist: " + string.Join("; ", dlist));
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
			
			using (var writer = new StreamWriter(File.Create(wFileName))) {
			while (line != null) 
			{
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
			
			//sw = new StreamWriter(wFileName);
			//Console.WriteLine("newline: " + line);
			//sw.WriteLine(line);	
			// end of writing
		        // Console.WriteLine("value from tags: " + string.Join("\t", tvalueList));	
			// reading new lines 
			line = tmpReader.ReadLine();
			
			}
			}
			//sw.Close(); 
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
