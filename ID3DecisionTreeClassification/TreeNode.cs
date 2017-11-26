using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ID3DecisionTreeClassification
{
    class TreeNode
    {

        public string value;
        public bool discovered = false;
        public TreeNode parent;
        public List<TreeNode> connections = new List<TreeNode>();
        public TreeNode(string val)
        {
            value = val;
        }
        
    }
}
