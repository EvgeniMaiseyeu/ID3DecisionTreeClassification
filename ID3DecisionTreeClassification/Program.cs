using ID3DecisionTreeClassification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ID3DecisionTreeClassification
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 2)
            {
                Console.WriteLine("ID3 Decision Tree Program\n");

                List<Feature> features = new List<Feature>();
                List<Individual> individuals = new List<Individual>();
                List<Feature> featuresTest = new List<Feature>();
                List<Individual> individualsTest = new List<Individual>();
                int lineCounter = 0;
                int featureCounter = 0;
                int exampleCounter = 0;
                string line;
                string value1 = "";
                string value2 = "";

                //open file and parse input
                System.IO.StreamReader file = new System.IO.StreamReader(args[0]);
                while ((line = file.ReadLine()) != null)
                {
                    if (line.Length != 0 && line[0] != '/')
                    {
                        if (lineCounter == 0)
                        {
                            value1 = line;
                        } else if (lineCounter == 1)
                        {
                            value2 = line;
                        } else if (lineCounter == 2)
                        {
                            //feature count
                            featureCounter = Convert.ToInt32(line);
                        } else if (lineCounter == 3 + featureCounter)
                        {
                            //example count
                            exampleCounter = Convert.ToInt32(line);
                        } else if (lineCounter < 3 + featureCounter)
                        {
                            //parse features i.e. (type passenger crew)
                            features.Add(new Feature(line));    

                        } else if (lineCounter < 4 + featureCounter + exampleCounter)
                        {
                            //parse individuals i.e. (testEx1		survived passenger regular child female)
                            individuals.Add(new Individual(line));
                        }

                        lineCounter++;
                    }
                }

                lineCounter = 0;
                featureCounter = 0;
                exampleCounter = 0;
                value1 = "";
                value2 = "";
                file = new System.IO.StreamReader(args[1]);
                while ((line = file.ReadLine()) != null)
                {
                    if (line.Length != 0 && line[0] != '/')
                    {
                        if (lineCounter == 0)
                        {
                            value1 = line;
                        }
                        else if (lineCounter == 1)
                        {
                            value2 = line;
                        }
                        else if (lineCounter == 2)
                        {
                            //feature count
                            featureCounter = Convert.ToInt32(line);
                        }
                        else if (lineCounter == 3 + featureCounter)
                        {
                            //example count
                            exampleCounter = Convert.ToInt32(line);
                        }
                        else if (lineCounter < 3 + featureCounter)
                        {
                            //parse features i.e. (type passenger crew)
                            featuresTest.Add(new Feature(line));

                        }
                        else if (lineCounter < 4 + featureCounter + exampleCounter)
                        {
                            //parse individuals i.e. (testEx1		survived passenger regular child female)
                            individualsTest.Add(new Individual(line));
                        }

                        lineCounter++;
                    }
                }
                /*
                //DEBUGGING FOR FILE PARSING ------------------------------------
                foreach (Individual i in individuals)
                {
                    Console.WriteLine(i.value);
                }

                foreach (Feature f in features)
                {
                    Console.WriteLine(f.name);
                }
                //---------------------------------------------------------------
                */

                //ID3 Decision Tree Generation
                Console.WriteLine("ID3 Tree: ");
                Tree tree = new Tree(individuals, features, value1, value2);

                Console.WriteLine("\nNames of Individuals Missclassified: ");
                tree.classify(individualsTest, featuresTest);
                file.Close();
            }
            else
            {
                Console.WriteLine("Invalid Command Line Arguments");
            }
            //Pause termination
            Console.ReadLine();
        }
    }
}
