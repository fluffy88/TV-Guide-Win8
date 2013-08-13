using System;
using System.IO;
using System.Net;
using Windows.UI.Popups;

namespace TV_Guide
{
    public class HttpUtils
    {
        public static HttpWebRequest GetHttpRequest(String url)
        {
            System.Uri targetUri = new System.Uri(url);
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(targetUri);

            return request;
        }

        public static String GetResponse(IAsyncResult callbackResult)
        {
            String results = null;
            try
            {
                HttpWebRequest myRequest = (HttpWebRequest)callbackResult.AsyncState;
                HttpWebResponse myResponse = (HttpWebResponse)myRequest.EndGetResponse(callbackResult);
                using (StreamReader httpwebStreamReader = new StreamReader(myResponse.GetResponseStream()))
                {
                    results = WebUtility.HtmlDecode(httpwebStreamReader.ReadToEnd());
                }
            }
            catch
            {
                new MessageDialog("No internet connection!").ShowAsync();
            }
            return results;
        }
    }
}