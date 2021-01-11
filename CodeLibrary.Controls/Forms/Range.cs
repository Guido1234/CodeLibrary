using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeLibrary.Controls
{
    public class Range
    {
        public int Start { get; set; }
        public int Length { get; set; }

        public int End => Start + Length;
    }
}
