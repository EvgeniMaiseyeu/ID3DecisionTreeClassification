using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIAssignment5
{
    class Individual
    {
        //name of the individual i.e. (trainEx1)
        public string name;

        //value of the individual in our example (survived or died)
        public string value;

        public string[] attributes;

        public Individual(string individual)
        {
            string[] splitString = individual.Split();

            //this removes null characters from split
            List<string> temp = new List<string>();
            for(int i = 0; i < splitString.Length; i++)
            {
                if(splitString[i] != "")
                {
                    temp.Add(splitString[i]);
                }
            }
            splitString = temp.ToArray();


            this.name = splitString[0];
            this.value = splitString[1];
            attributes = new string[splitString.Length - 2];
            for (int i = 2; i < attributes.Length; i++)
            {
                attributes[i] = splitString[i];
            }
        }
    }
}
