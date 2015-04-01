using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Collections.Specialized;
using System.Text.RegularExpressions;

namespace mailinglistscrape {
	class Login {
		public String logincookie { get; set; }
		public List<String> pageletters { get; set; }
		private String listpw;
		public String listname;
		public Login(String listname, String password) {
			this.listname = listname;
			this.listpw = password;
		}
		public void run() {
			setcookie();
			setletters();
		}
		private void setcookie() {
			using (var client = new WebClient()) {
				NameValueCollection values = new NameValueCollection();
				values["adminpw"] = this.listpw;
				values["admlogin"] = "Let+me+in...";
				var response = client.UploadValues("http://lists.otakuthon.com/admin/" + this.listname.ToLower() + "/", values);
				//var responseString = Encoding.Default.GetString(response);
				this.logincookie = client.ResponseHeaders["Set-Cookie"];
			}
		}
		private void setletters() {
			String startpage = Download.dlpage("http://lists.otakuthon.com/admin/" + this.listname.ToLower() + "/members", this.logincookie);
			Match match = Regex.Match(startpage, "(?<=href=\")http://lists\\.otakuthon\\.com/admin/" + this.listname.ToLower() + "/members\\?letter=[a-z]");
			pageletters = new List<string>();
			while(match.Success){
				pageletters.Add(match.Value);
				match = match.NextMatch();
			}
		}
	}
}
