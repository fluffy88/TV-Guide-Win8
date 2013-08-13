using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;
using System.Net;
using System.Text.RegularExpressions;
using TV_Guide.Classes;
using TV_Guide.Common;
using TV_Guide.JSONModels;
using TV_Guide.Models;
using Windows.ApplicationModel.Core;
namespace TV_Guide.ViewModels
{
    public class OnNowViewModel : BindableBase
    {
        public ObservableCollection<ChannelModel> ProgramCollection { get; private set; }

        public OnNowViewModel()
        {
            this.ProgramCollection = new ObservableCollection<ChannelModel>();

            HttpWebRequest request = HttpUtils.GetHttpRequest("http://api.entertainment.ie/tvguide/tvnow.asp");
            request.BeginGetResponse(new AsyncCallback(ReadCallback), request);
        }

        private void ReadCallback(IAsyncResult ar)
        {
            string rawHtml = HttpUtils.GetResponse(ar);
            if (rawHtml != null)
            {
                var jsonObject = JsonConvert.DeserializeObject<OnNowModel>(rawHtml);
                foreach (OnNowChannel c in jsonObject.channels)
                {
                    foreach (OnNowProgram p in c.programs)
                    {
                        HttpWebRequest request = HttpUtils.GetHttpRequest(Uri.UnescapeDataString(p.url));
                        request.BeginGetResponse(new AsyncCallback(ImageCallBack), request);
                    }

                    var dispatcher = CoreApplication.MainView.CoreWindow.Dispatcher;
                    ChannelModel model = ChannelModel.adaptJsonModel(c);
                    model.Content = c.convert();
                    lock (ProgramCollection)
                    {
                        dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => ProgramCollection.Add(model));
                    }
                }
            }
        }

        private void ImageCallBack(IAsyncResult ar)
        {
            string rawHtml = HttpUtils.GetResponse(ar);
            if (rawHtml != null)
            {
                var jsonObject = JsonConvert.DeserializeObject<ProgramModel>(rawHtml);
                if (jsonObject.program != null)
                {
                    ProgramDetails details = jsonObject.program.details;

                    lock (ProgramCollection)
                    {
                        foreach (ChannelModel cm in ProgramCollection)
                        {
                            foreach (ProgramDetails pd in cm.Content)
                            {
                                if (pd.title == details.title && pd.starttime == details.starttime)
                                {
                                    CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                                    {
                                        pd.image = details.image;
                                        pd.category = details.category;
                                        pd.duration = Regex.Match(details.duration, @"\d+").Value;
                                        pd.description = details.description;
                                    });
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}