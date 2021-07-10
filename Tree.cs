using System;
using System.Collections.Generic;
using System.Text;

namespace XML_Editor
{
    class Node 
    {
        private string tagName;
        private string tagValue;
        private int depth;
        private List<Node> children = new List<Node>();

        public Node(string tagName, string tagValue, int depth)
        {
            this.tagName = tagName;
            this.tagValue = tagValue;
            this.depth = depth;

        }
       
        public List<Node> getChildren()
        {
            return this.children;
        }
        public string getTagName()
        {
            return this.tagName;
        }
        public string getTagValue()
        {
            return this.tagValue;
        }
        public int getDepth()
        {
            return this.depth;
        }

       

      
    }
    
    class Tree
    {
        private Node root;
        public Tree(Node root)
        {
            this.root = root;
        }
        public void addChild(Node child, Node parent)
        {

            parent.getChildren().Add(new Node(child.getTagName(), child.getTagValue(), child.getDepth()));
        }

        public Node getTreeRoot()
        {
            return this.root;
        }
    }

    
    
}
