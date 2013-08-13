using Newtonsoft.Json;
using System;
using System.Net;
using TV_Guide.Classes;
using TV_Guide.Common;
using TV_Guide.JSONModels;
using Windows.ApplicationModel.Core;

namespace TV_Guide.ViewModels
{
    public class TVListingViewModel
    {
        public SortedObservableCollection<ProgramDetails> TVListingCollection { get; private set; }
        private string Page { get; set; }

        public TVListingViewModel(string page)
        {
            this.TVListingCollection = new SortedObservableCollection<ProgramDetails>();
            this.Page = page;

            HttpWebRequest request = HttpUtils.GetHttpRequest(this.Page);
            request.BeginGetResponse(new AsyncCallback(ReadCallback), request);
        }

        private void ReadCallback(IAsyncResult ar)
        {
            string rawHtml = HttpUtils.GetResponse(ar);
            if (rawHtml != null)
            {
                var jsonObject = JsonConvert.DeserializeObject<ProgramListModel>(rawHtml);
                foreach (TV_Guide.JSONModels.Program p in jsonObject.programs)
                {
                    HttpWebRequest request = HttpUtils.GetHttpRequest(Uri.UnescapeDataString(p.url));
                    request.BeginGetResponse(new AsyncCallback(DescCallBack), request);
                }
            }
        }

        private void DescCallBack(IAsyncResult ar)
        {
            string rawHtml = HttpUtils.GetResponse(ar);
            if (rawHtml != null)
            {
                var jsonObject = JsonConvert.DeserializeObject<ProgramModel>(rawHtml);
                if (jsonObject.program != null)
                {
                    ProgramDetails details = jsonObject.program.details;
                    lock (TVListingCollection)
                    {
                        var dispatcher = CoreApplication.MainView.CoreWindow.Dispatcher;
                        dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => TVListingCollection.Add(details));
                    }
                }
            }
        }
    }
}