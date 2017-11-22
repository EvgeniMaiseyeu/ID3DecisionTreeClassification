using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIAssignment5
{
    class Feature
    {
        public string name;
        public string[] attributeList;

        //In our examples attribute count should be size 2 always
        Feature(string name, int attributeCount)
        {
            this.name = name;
            attributeList = new string[attributeCount];
        }
    }
}
