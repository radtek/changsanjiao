using System;
using System.Collections.Generic;
using System.Text;

namespace MMShareBLL.Model
{
    public interface IGroup
    {
        int[] ItemIdentities { get; }

        string Name { get; }
    }
}
