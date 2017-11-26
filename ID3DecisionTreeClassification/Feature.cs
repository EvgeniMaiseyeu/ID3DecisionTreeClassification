using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ID3DecisionTreeClassification
{
    class Feature
    {
        public string name;
        public string[] attributeList;
        public bool locked = false;

        //Constructor parses file line
        public Feature(string feature)
        {
            string[] splitString = feature.Split();

            //this removes null characters from split
            List<string> temp = new List<string>();
            for (int i = 0; i < splitString.Length; i++)
            {
                if (splitString[i] != "")
                {
                    temp.Add(splitString[i]);
                }
            }
            splitString = temp.ToArray();

            this.name = splitString[0];
            attributeList = new string[splitString.Length - 1];
            for (int i = 0; i < attributeList.Length; i++)
            {
                attributeList[i] = splitString[i+1];
            }
        }
    }
}
