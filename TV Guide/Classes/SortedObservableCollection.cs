﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace TV_Guide.Classes
{
    public class SortedObservableCollection<T> : ObservableCollection<T>
    where T : IComparable
    {
        protected override void InsertItem(int index, T item)
        {
            for (int i = 0; i < this.Count; i++)
            {
                switch (Math.Sign(this[i].CompareTo(item)))
                {
                    case 0:
                        throw new InvalidOperationException("Cannot insert duplicated items");
                    case 1:
                        base.InsertItem(i, item);
                        return;
                    case -1:
                        break;
                }
            }
            base.InsertItem(this.Count, item);
        }
    }
}
