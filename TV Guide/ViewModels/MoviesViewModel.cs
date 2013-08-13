using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using TV_Guide.Classes;
using TV_Guide.Common;
using TV_Guide.JSONModels;
using TV_Guide.Models;
using Windows.ApplicationModel.Core;

namespace TV_Guide.ViewModels
{
    public class MoviesViewModel : BindableBase
    {
        private Dictionary<string, ChannelModel> channels = new Dictionary<string, ChannelModel>();

        public ObservableCollection<ChannelModel> MoviesCollection { get; private set; }

        public MoviesViewModel()
        {
            this.MoviesCollection = new ObservableCollection<ChannelModel>();

            String url = String.Format("http://api.entertainment.ie/tvguide/categories.asp?cat=Movies&date={0}", DateTime.Today.ToString("dd/MM/yyyy"));
            HttpWebRequest request = HttpUtils.GetHttpRequest(url);
            request.BeginGetResponse(new AsyncCallback(ReadCallback), request);
        }

        private void ReadCallback(IAsyncResult ar)
        {
            string rawHtml = HttpUtils.GetResponse(ar);
            if (rawHtml != null)
            {
                var jsonObject = JsonConvert.DeserializeObject<MoviesModel>(rawHtml);
                foreach (Movie m in jsonObject.programs)
                {
                    HttpWebRequest request = HttpUtils.GetHttpRequest(Uri.UnescapeDataString(m.url));
                    request.BeginGetResponse(new AsyncCallback(ImageCallBack), request);

                    if (!channels.ContainsKey(m.channel))
                    {
                        channels[m.channel] = ChannelModel.adaptJsonModel(m);
                    }
                    channels[m.channel].Content.Add(new ProgramDetails()
                    {
                        title = m.title,
                        image = m.logo,
                        programtime = Convert.ToDateTime(m.time)
                    });
                }

                var sortedChannels = channels.Values.OrderBy((m) => { return m.Title; });
                foreach (ChannelModel mc in sortedChannels)
                {
                    var dispatcher = CoreApplication.MainView.CoreWindow.Dispatcher;
                    lock (MoviesCollection)
                    {
                        dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => MoviesCollection.Add(mc));
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

                    lock (MoviesCollection)
                    {
                        foreach (ChannelModel cm in MoviesCollection)
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