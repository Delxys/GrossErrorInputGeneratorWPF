using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrossErrorInputGeneratorWPF2.Models
{
    internal class FlowDouble
    {
        public double Id { get; set; }
        public double? SourceId { get; set; }
        public double? DestinationId { get; set; }
        public double Name { get; set; }
        public double Measured { get; set; }
        public double UpperBound { get; set; }
        public double LowerBound { get; set; }
        public double Tolerance { get; set; }
        public bool IsMeasured { get; set; }
    }
}
