using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrossErrorInputGeneratorWPF2
{
    internal class Settings
    {
        public Settings()
        {
            this.Multiplier = 1;
            this.Borders = false;
            this.Tolerance = false;
            this.TestSchemes = false;
            this.EmptySchemes = false;
        }
        public int Multiplier { get; set; }
        public bool Borders { get; set; }
        public bool Tolerance { get; set; }
        public bool TestSchemes { get; set; }
        public bool EmptySchemes { get; set; }
    }
}
