using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mailinglistscrape {
	class Options {
		public enum imex {
			notset,
			import,
			export,
			delete
		}
		public static imex imexmode { get; set; }
		public static bool silent { get; set; }
		public static List<MailingList> mailinglists { get; set; }
		public static string parseerrors { get; set; }
		public static bool  haserrors { get; set; }
		public static bool help { get; set; }
		public static void parse(String[] args) {
			silent = false;
			imexmode = imex.notset;
			mailinglists = new List<MailingList>();
			help = false;
			for (int i = 0; i < args.Length; i++) {
				switch (args[i]) {
					case("--list"):
					case("-l"):
						MailingList templist;
						String password;
						for (int j = i+1; !args[j].Contains("-") ; j++) {
							args[j] = args[j].Replace(".txt","");
							if (!System.IO.File.Exists(args[j] + ".txt")) {
								haserrors = true;
								parseerrors += args[j] + ".txt file does not exists!\r\n";
								if (j + 1 >= args.Length) {
									break;
								} else {
									continue;
								}
							}
							password = "";
							password = Fileio.readallfilestring(args[j] + ".txt");
							if (password.Length < 4) {
								haserrors = true;
								parseerrors += args[j] + " password is really short!\r\n";
								continue;
							}
							templist = new MailingList();
							templist.name = args[j];
							templist.password = password;
							mailinglists.Add(templist);
							i++;
							if (j+1 >= args.Length) {
								break;
							}
						}
						break;
					case("import"):
					case("-i"):
						if (imexmode != imex.notset) {
							haserrors = true;
							parseerrors += "Do not use import, export and delete at the same time!\r\n";
						} else {
							imexmode = imex.import;
						}
						break;
					case("--export"):
					case("-e"):
						if (imexmode != imex.notset) {
							haserrors = true;
							parseerrors += "Do not use import, export and delete at the same time!\r\n";
						} else {
							imexmode = imex.export;
						}
						break;
					case("--delete"):
					case ("-d"):
						if (imexmode != imex.notset) {
							haserrors = true;
							parseerrors += "Do not use import, export and delete at the same time!\r\n";
						} else {
							imexmode = imex.delete;
						}
						break;
					case("-s"):
						silent = true;
						break;
					case ("--help"):
					case ("-h"):
						help = true;
						break;
					default:
						break;
				}
			}
			if (imexmode == imex.notset) {
				haserrors = true;
				parseerrors += "No mode selected! Use -h of --help for help!\r\n";
			}
			if (mailinglists.Count == 0) {
				haserrors = true;
				parseerrors += "No list selected! Use -h of --help for help!\r\n";
			}
		}
		public static void printhelp() {
			Console.WriteLine("--import -i --export -e --delete -d  --list list1 list2 list3 -s --help -h");
		}
	}
}
