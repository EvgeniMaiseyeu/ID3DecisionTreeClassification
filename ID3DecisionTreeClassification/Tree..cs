using ID3DecisionTreeClassification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ID3DecisionTreeClassification
{
    class Tree
    {
        public TreeNode root;
        public string category1;
        public string category2;
        public string errorMSG = "no training data";
        public int depth = 0;
        public List<TreeNode> treeList = new List<TreeNode>();

        /* ID3 (examples, attributes)
        *  if all examples in same category then
        *      return a leaf node with that category
        *  if attributes is empty then
        *      return a leaf node with the most common category in examples
        *  best = ChooseAttribute(examples, attributes) 
        *  tree = new tree with best as root attribute test 
        *  foreach value v(i) of best 
        *      examples(i) = subset of examples with best == v(i)
        *      subtree = ID3(examples(i), attributes - best)
        *      add a branch to tree with best == v(i) and subtree beneath
        *  return tree
        */

        public Tree(List<Individual> individuals, List<Feature> features, string cat1, string cat2)
        {
            category1 = cat1;
            category2 = cat2;
            root = ID3(individuals, features, null);
            printTree(root);
        }


        public void classify(List<Individual> individualsTest, List<Feature> featuresTest)
        {
            int correctCount = 0;
            int miss = 0;
            foreach (Individual i in individualsTest)
            {
                string classify = findValue(i, featuresTest, root);
                
                if (classify == i.value)
                {
                    correctCount++;
                } else
                {
                    miss++;
                    Console.WriteLine(i.name);
                }
            }
            Console.WriteLine("\nCorrectly Classified Number: " + correctCount);
            Console.WriteLine("Missclassified Number: " + miss);
            Console.WriteLine("Percent Missclassified: " + 100*((double)miss / (double)(correctCount + miss)));
        }

        private string findValue(Individual i, List<Feature> featuresTest, TreeNode node)
        {
            string value = "";
            if(node.value == category1)
            {
                return category1;
            } else if (node.value == category2)
            {
                return category2;
            } else if(node.value == errorMSG)
            {
                return errorMSG;
            }
            for(int j = 0; j < node.connections.Count; j++)
            {
                if (i.attributes.Contains<string>(node.connections[j].value))
                {
                    value = findValue(i, featuresTest, node.connections[j].connections[0]);
                }
            }
            return value;
        }

        private TreeNode ID3(List<Individual> examples, List<Feature> attributes, Feature best)
        {
            if (examples.Count == 0)
                return new TreeNode(errorMSG);
            string category = sameCategory(examples);
            if (category != null)
            {
                return new TreeNode(category);
            }
            if (attributes.Count == 0)
            {
                return new TreeNode(commonCategory(examples));
            }
            Feature bestAttribute = chooseAttribute(examples, attributes);
            List<Feature> copyList = new List<Feature>(attributes);
            copyList.Remove(bestAttribute);
            TreeNode tree = new TreeNode(bestAttribute.name);
            for (int i = 0; i < bestAttribute.attributeList.Length; i++)
            {
                List<Individual> subsetExamples = subset(examples, bestAttribute.attributeList[i]);
                TreeNode connector = new TreeNode(bestAttribute.attributeList[i]);
                TreeNode subtree = ID3(subsetExamples, copyList, bestAttribute);
                subtree.parent = connector;
                tree.connections.Add(connector);
                connector.parent = tree;
                connector.connections.Add(subtree);
            }
            return tree;
        }

        private bool checkLocks(List<Feature> attributes)
        {
            for (int i = 0; i < attributes.Count; i++)
            {
                if (attributes[i].locked == false)
                    return false;
            }
            return true;
        }

        private List<Individual> subset(List<Individual> examples, string attributeSubset)
        {
            List<Individual> sub = new List<Individual>();
            for(int i = 0; i < examples.Count; i++)
            {
                if (Array.Exists<string>(examples[i].attributes, element => element == attributeSubset))
                {
                    sub.Add(examples[i]);
                }
            }
            return sub;
        }

        private Feature chooseAttribute(List<Individual> examples, List<Feature> attributes)
        {
            double bestEntropy = 10;
            Feature bestFeature = null;
            foreach (Feature f in attributes){
                //calculate entropy
                if (f.locked == false)
                {
                    double entropy = 0;
                    foreach (string s in f.attributeList)
                    {
                        double frequency = 0;
                        double count1 = 0;
                        double count2 = 0;
                        for (int i = 0; i < examples.Count; i++)
                        {
                            if (examples[i].attributes.Contains<string>(s))
                            {
                                frequency++;
                                if (examples[i].value == category1)
                                {
                                    count1++;
                                }
                                else if (examples[i].value == category2)
                                {
                                    count2++;
                                }
                            }
                        }
                        //calculate 
                        entropy += (frequency / (double)examples.Count) * D(count1, count2);
                    }
                    if (entropy > 1)
                    {
                        Console.WriteLine("entropy calculation incorrect");
                    }
                    if (entropy < bestEntropy)
                    {
                        bestEntropy = entropy;
                        bestFeature = f;
                    }
                }
            }
            return bestFeature;
        }


        private double D(double count1, double count2)
        {
            if(count1 == 0 || count2 == 0)
            {
                return 0;
            } else if (count1 == count2)
            {
                return 1;
            }
            double num1 = ((-count1 / (count1 + count2))*Math.Log((count1 / (count1 + count2)), 2)); 
            double num2 = ((-count2 / (count1 + count2)) * Math.Log((count2 / (count1 + count2)), 2));
            return num1 + num2;
        }

        private string commonCategory(List<Individual> examples)
        {
            int count1 = 0;
            int count2 = 0;
            for (int i = 0; i < examples.Count; i++)
            {
                if (examples[i].value == category1)
                {
                    count1++;
                } else if(examples[i].value == category2)
                {
                    count2++;
                }
            }
            if (count1 > count2)
                return category1;
            else
                return category2;
        }

        private string sameCategory(List<Individual> examples)
        {
            for (int i = 1; i < examples.Count; i++)
            {
                if (examples[i].value != examples[i - 1].value)
                {
                    return null;
                }
            }
            return examples[0].value;
        }

        public void printTree(TreeNode node)
        {
            for(int i = 0; i < depth/2; i++)
            {
                Console.Write("      ");
            }
            Console.Write(node.value + "\n");
            node.discovered = true;
            treeList.Add(node);
            foreach (TreeNode t in node.connections)
            {
                if (!t.discovered)
                {
                    depth++;
                    printTree(t);
                }
            }
            depth--;
        }
    }
}
