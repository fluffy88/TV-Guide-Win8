using System.Collections.ObjectModel;
using TV_Guide.Classes;
using TV_Guide.Common;
using TV_Guide.JSONModels;
using TV_Guide.ViewModels;

namespace TV_Guide.Models
{
    public class MainListModel : BindableBase
    {
        public string Title { get; set; }
        public string Subtitle { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }

        private ObservableCollection<ChannelModel> _items;
        public ObservableCollection<ChannelModel> Items
        {
            get { return this._items; }
            set { this.SetProperty(ref this._items, value); }
        }
    }
}