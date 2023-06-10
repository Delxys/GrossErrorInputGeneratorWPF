using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrossErrorInputGeneratorWPF2
{
    internal interface IGenerator
    {
        public void Generate(Settings settings);
    }
}
