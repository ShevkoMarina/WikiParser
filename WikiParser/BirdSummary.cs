using System;
using System.Collections.Generic;
using System.Text;

namespace WikiParser
{
    class BirdSummary
    {
        public string Name { get; set; }
        public string Picture { get; set; }

        public override string ToString()
        {
            return Name + " " + Picture;
        }
    }
}
