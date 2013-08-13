using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TV_Guide.Common;
using TV_Guide.JSONModels;

namespace TV_Guide.Models
{
    public class ChannelModel : BindableBase
    {
        public string Title { get; set; }
        public string Subtitle { get; set; }
        public string Image { get; set; }
        public string Description { get; set; }
        private ObservableCollection<ProgramDetails> _content;
        public string Url { get; set; }

        public ObservableCollection<ProgramDetails> Content
        {
            get { return this._content; }
            set { this.SetProperty(ref this._content, value); }
        }

        public static ChannelModel adaptJsonModel(Channel channel)
        {
            return new ChannelModel()
            {
                Title = channel.channel,
                Image = channel.logo,
                Url = channel.url
            };
        }

        public static ChannelModel adaptJsonModel(Movie m)
        {
            return new ChannelModel()
            {
                Title = m.channel,
                Image = m.logo,
                Url = m.url,
                Content = new ObservableCollection<ProgramDetails>(),
            };
        }
    }
}