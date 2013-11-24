using System.Collections.Generic;
using System.Net;
using System.Text;
using Core.Documents;

namespace Tests
{
    public class RouterDescriptotLoader
    {

        public static IEnumerable<RouterDescriptor> Load()
        {
            var url = "http://86.59.21.38:80/tor/server/all";
            var webClient = new WebClient();
            var result = webClient.DownloadData(url);
            var str = Encoding.ASCII.GetString(result);
            return RouterDescriptorParser.GetDescriptors(str.Replace("\r\n", "\n"));
        }
    }
}
