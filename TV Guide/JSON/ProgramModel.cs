using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TV_Guide.Common;

namespace TV_Guide.JSONModels
{
    public class ProgramModel
    {
        public string bannerad { get; set; }
        public DetailedProgram program { get; set; }
    }

    public class ProgramDetails : BindableBase, IComparable
    {
        public string channel { get; set; }
        public int channelid { get; set; }
        public string title { get; set; }
        private string _category;
        private string _duration;
        private DateTime _programtime;
        public int subtitles { get; set; }
        private string _description;
        public string channelurl { get; set; }
        private string _image;

        public string category
        {
            get { return _category; }
            set
            {
                if (_category != value)
                {
                    this.SetProperty(ref this._category, value);
                }
            }
        }

        public string duration
        {
            get { return _duration; }
            set
            {
                if (_duration != value)
                {
                    this.SetProperty(ref this._duration, value + "mins");
                }
            }
        }

        public DateTime programtime
        {
            get { return _programtime; }
            set
            {
                if (_programtime != value)
                {
                    this.SetProperty(ref this._programtime, Convert.ToDateTime(value));
                    if (!_programtime.IsDaylightSavingTime())
                    {
                        this.SetProperty(ref this._programtime, _programtime.AddHours(1));
                    }
                }
            }
        }

        public string starttime
        {
            get
            {
                return String.Format("{0:D2}:{1:D2}", _programtime.Hour, _programtime.Minute);
            }
        }

        public string description
        {
            get { return _description; }
            set
            {
                if (_description != value)
                {
                    this.SetProperty(ref this._description, value);
                }
            }
        }

        public string image
        {
            get
            {
                if (_image == null)
                {
                    return "/Assets/Default_Program.png";
                }
                return _image;
            }
            set
            {
                this.SetProperty(ref this._image, value);
            }
        }

        public int CompareTo(object obj)
        {
            return programtime.CompareTo(((ProgramDetails)obj).programtime);
        }
    }

    public class Link
    {
        public string title { get; set; }
        public string url { get; set; }
        public string type { get; set; }
    }

    public class DetailedProgram
    {
        public ProgramDetails details { get; set; }
        public List<Link> links { get; set; }
    }
}