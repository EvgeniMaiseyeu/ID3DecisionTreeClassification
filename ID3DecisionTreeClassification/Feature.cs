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

        //Constructor parses file line
        public Feature(string feature)
        {
            string[] splitString = feature.Split();
            this.name = splitString[0];
            attributeList = new string[splitString.Length - 1];
            for (int i = 1; i < attributeList.Length; i++)
            {
                attributeList[i] = splitString[i];
            }
        }
    }
}
