using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class Path
    {
        public Academy ConnectedAcademia { get; set; } 
        public decimal Distance { get; set; }
        public override string ToString()
        {
            return "-> " + ConnectedAcademia.ToString() + $"({Distance})";
        }
    }
}
