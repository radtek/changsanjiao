using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Windows.Forms;

namespace MMShareBLL.DAL
{
    public class HashComparerByValue : IComparer
    {
        private SortOrder sortDir = SortOrder.None;
        public HashComparerByValue(SortOrder sd)
        {
            sortDir = sd;
        }

        public int Compare(object x, object y)
        {
            DictionaryEntry de_x = (DictionaryEntry)x;
            DictionaryEntry de_y = (DictionaryEntry)y;
            Decimal diff = (Decimal.Parse(de_x.Value.ToString())) - (Decimal.Parse(de_y.Value.ToString()));
            if (sortDir == SortOrder.Descending)
            {
                return (diff == 0) ? 0 : (diff > 0 ? -1 : 1);
            }
            else
            {
                return (diff == 0) ? 0 : (diff > 0 ? 1 : -1);
            }
        }
    }
}
