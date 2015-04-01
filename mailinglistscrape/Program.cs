using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.Collections.Specialized;
using System.Text.RegularExpressions;
namespace mailinglistscrape {
	class Program {
		static void Main(string[] args) {
			//dunno what this is but important
			System.Net.ServicePointManager.Expect100Continue = false;

			//parse
			if (!Options.silent) {
				Console.WriteLine("[Parsing arguments]");
			}
			Options.parse(args);
			if (Options.help) {
				Options.printhelp();
				return;
			}

			if (Options.haserrors) {
				Console.WriteLine(Options.parseerrors);
				return;
			}

			//for every list in lists login
			if (!Options.silent) {
				Console.WriteLine("[Logging In]");
			}
			List<Login> logins = new List<Login>();
			for (int i = 0; i < Options.mailinglists.Count; i++) {
				if (!Options.silent) {
					Console.WriteLine("\tList: "+ Options.mailinglists[i].name);
				}
				Login listlogin = new Login(Options.mailinglists[i].name, Options.mailinglists[i].password);
				listlogin.run();
				logins.Add(listlogin);
			}
			
			//for each list in logins
			if (!Options.silent) {
				Console.WriteLine("[Scrapping]");
			}
			StringBuilder combo;
			for (int i = 0; i < logins.Count; i++) {
				if (!Options.silent) {
					Console.WriteLine("List: " + logins[i].listname);
				}
				switch (Options.imexmode) {
					case Options.imex.notset:
						break;
					case Options.imex.import:
						break;
					case Options.imex.export:
						combo = new StringBuilder();
						foreach (String link in logins[i].pageletters) {
							if (!Options.silent) {
								Console.WriteLine("\tDownloading: " + link.Replace("http://lists.otakuthon.com/admin/",""));
							}
							combo.Append(Download.dlpage(link, logins[i].logincookie));
						}
						if (!Options.silent) {
							Console.WriteLine("[Matching]");
						}
						Match match = Regex.Match(combo.ToString(), "([^<>]+?@[^<>]+?)<.+?value=\"(.*?)\"");
						StringBuilder members = new StringBuilder();
						while (match.Success) {
							members.AppendLine(match.Groups[1].Value + "," + WebUtility.HtmlDecode(match.Groups[2].Value));
							match = match.NextMatch();
						}
						if (!Options.silent) {
							Console.WriteLine("Exporting: " + logins[i].listname);
						}
						Fileio.overwritefile(members.ToString(),logins[i].listname +"-export.csv");
						combo.Clear();
						break;
					case Options.imex.delete:
						break;
				}
			}
			if (!Options.silent) {
				Console.WriteLine("[Complete]");
			}
		}
	}
}
