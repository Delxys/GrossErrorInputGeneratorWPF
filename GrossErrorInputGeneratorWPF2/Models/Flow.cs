using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrossErrorInputGeneratorWPF2.Models
{
    internal class Flow
    {
        public string Id { get; set; }
        public string SourceId { get; set; }
        public string DestinationId { get; set; }
        public string Name { get; set; }
        public double Measured { get; set; }
        public double UpperBound { get; set; }
        public double LowerBound { get; set; }
        public double Tolerance { get; set; }
        public bool IsMeasured { get; set; }
    }
}
