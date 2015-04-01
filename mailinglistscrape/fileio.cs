using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
namespace mailinglistscrape {
	class Fileio {
		public static void overwritefile(String towrite, String filename){
			using (StreamWriter sw = File.CreateText(filename)) {
				sw.WriteLine(towrite);
			}	
		}
		public static void appendfile(String towrite, String filename) {
			using (StreamWriter sw = File.AppendText(filename)) {
				sw.WriteLine(towrite);
			}	
		}
		public static String readallfilestring(String filename) {
			return File.ReadAllText(filename);
		}
		public static int readallfileint(String filename) {
			return Convert.ToInt32(File.ReadAllText(filename));
		}
		public static List<String> getlinelist(String file) {
			char[] delimiters = new char[] { '\r', '\n' };
			String[] linear = file.Split(delimiters,StringSplitOptions.RemoveEmptyEntries);
			List<String> linelist = new List<String>();
			for (int i = 0; i < linear.Length ; i++) {
				if (String.IsNullOrWhiteSpace(linear[i]) == false) {
					linelist.Add(linear[i]);	
				}
			}
			return linelist;
		}
	}
}