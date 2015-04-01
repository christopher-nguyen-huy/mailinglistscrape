using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;

namespace mailinglistscrape {
	class Download {
		public static string dlpage(String url, String cookieheader) {
			string pageSource;
			WebRequest getRequest = WebRequest.Create(url);
			getRequest.Headers.Add("Cookie", cookieheader);
			WebResponse getResponse = getRequest.GetResponse();
			using (StreamReader sr = new StreamReader(getResponse.GetResponseStream())) {
				pageSource = sr.ReadToEnd();
			}
			return pageSource;
		}
	}
}
