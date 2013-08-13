using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using TV_Guide.Classes;
using TV_Guide.Models;

namespace TV_Guide.JSONModels
{
    public class OnNowModel
    {
        public string bannerad { get; set; }
        public List<OnNowChannel> channels { get; set; }
    }

    public class OnNowProgram
    {
        public string programtitle { get; set; }
        public string programtime { get; set; }
        public string url { get; set; }
        public int repeat { get; set; }
        public int subtitles { get; set; }

        public string starttime
        {
            get
            {
                DateTime dt = Convert.ToDateTime(programtime);
                return String.Format("{0:D2}:{1:D2}", dt.Hour, dt.Minute);
            }
        }
    }

    public class OnNowChannel : Channel
    {
        public List<OnNowProgram> programs { get; set; }

        public ObservableCollection<ProgramDetails> convert()
        {
            ObservableCollection<ProgramDetails> convertedItems = new ObservableCollection<ProgramDetails>();
            foreach (OnNowProgram p in this.programs)
            {
                convertedItems.Add(new ProgramDetails()
                {
                    title = p.programtitle,
                    programtime = Convert.ToDateTime(p.programtime)
                });
            }
            return convertedItems;
        }
    }
}