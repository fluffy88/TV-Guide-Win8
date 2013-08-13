using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using TV_Guide.Common;
using TV_Guide.JSONModels;
using TV_Guide.Models;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using Windows.UI.Xaml;

namespace TV_Guide.ViewModels
{
    public class ChannelViewModel
    {
        public ObservableCollection<ChannelModel> ChannelCollection { get; private set; }

        public ChannelViewModel()
        {
            this.ChannelCollection = new ObservableCollection<ChannelModel>();
        
            HttpWebRequest request = HttpUtils.GetHttpRequest("http://api.entertainment.ie/tvguide/listings.asp");
            request.BeginGetResponse(new AsyncCallback(ReadCallback), request);
        }

        private void ReadCallback(IAsyncResult ar)
        {
            string rawHtml = HttpUtils.GetResponse(ar);
            if (rawHtml != null)
            {
                var jsonObject = JsonConvert.DeserializeObject<ChannelListModel>(rawHtml);
                foreach (Channel c in jsonObject.channels)
                {
                    var dispatcher = CoreApplication.MainView.CoreWindow.Dispatcher;
                    ChannelModel model = ChannelModel.adaptJsonModel(c);
                    dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => ChannelCollection.Add(model));
                }
            }
        }
    }
}