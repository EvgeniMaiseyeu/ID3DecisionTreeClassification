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
        public int depth = 0;

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

        private TreeNode ID3(List<Individual> examples, List<Feature> attributes, Feature best)
        {
            string category = sameCategory(examples);
            if (category != null)
            {
                return new TreeNode(category);
            }
            bool allLocked = checkLocks(attributes);
            if (allLocked)
            {
                return new TreeNode(commonCategory(examples));
            }
            Feature bestAttribute = chooseAttribute(examples, attributes);
            bestAttribute.locked = true;
            TreeNode tree = new TreeNode(bestAttribute.name);
            for (int i = 0; i < bestAttribute.attributeList.Length; i++)
            {
                if(best == null && i != 0)
                {
                    for (int j = 0; j < attributes.Count; j++)
                    {
                        if(attributes[j] != bestAttribute)
                            attributes[j].locked = false;
                    }
                }
                List<Individual> subsetExamples = subset(examples, bestAttribute.attributeList[i]);
                TreeNode connector = new TreeNode(bestAttribute.attributeList[i]);
                TreeNode subtree = ID3(subsetExamples, attributes, bestAttribute);
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
                    /*
                    for(int j = 0; j < examples[i].attributes.Length; j++)
                    {
                        if(examples[i].attributes[j] == attributeSubset)
                        {
                          // examples[i].attributes[j] = null;
                        }
                    }*/
                    sub.Add(examples[i]);
                }
            }
            //return list of individuals who are in that attribute subset and set it to null
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
                Console.Write("    ");
            }
            Console.Write(node.value + "\n");
            node.discovered = true;
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
