using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace ConsoleApp12
{
    class Program
    {
        const string filename = "xmll.txt";
        public static void write(string s,bool add)
        {
            StreamWriter sw = new StreamWriter(filename,true);
            if (add == true)
            {
                sw.WriteLine(s);
                sw.Close();
            }
            else
            {
                sw.Write(s);
                sw.Close();
            }  
        }


        public static void Formating(Node root)
        {
            
           
            bool flag = false;
            Node r = root;
            List<Node> children = new List<Node>();
            children = root.getChildren();
            if(r==null)
                return ;
            for (int loop = 0; loop < r.getDepth(); loop++)
            {
                write("    ",false);
                Console.Write("    ");   
            }

            write("<" + r.getTagName() + ">",false);
           
             Console.Write( "<" + r.getTagName() + ">");
            if (r.getTagValue() == "")
            {
               write(r.getTagValue(),true);
                Console.WriteLine(r.getTagValue());
            }

            else
            {
                write(r.getTagValue(),false);
               
                flag = true;
                   Console.Write(r.getTagValue()); flag = true;
            }
            if (r != null)
            {

                foreach (Node child in children)
                {
                     Formating(child);
                }
            }
            if (flag == true)
            {
                write("</" + r.getTagName() + ">",true);
            
                flag = false; 
                Console.WriteLine("</" + r.getTagName() + ">");flag = false; return;
                
            }
            
            if (r.getChildren() == null)
            {
                write("</" + r.getTagName() + ">",true);
            

                Console.WriteLine("</" + r.getTagName() + ">"); }
            
            else
            {
                for (int loop = 0; loop < r.getDepth(); loop++)

                {
                    write("    ",false);
                  
                    Console.Write("    "); }
                
                write("</" + r.getTagName() + ">",true);

                
                 Console.WriteLine("</" + r.getTagName() + ">");
            }
            
        }


        static void Main(string[] args)
        {
            int i;
            Node root = new Node("users", "", 0);
            Node node1 = new Node("user", "", 1);
            Node node2 = new Node("id", "1", 2);
            Node node3 = new Node("name", "nana", 2);
            Node node4 = new Node("posts", "", 2);
            Node node5 = new Node("post", "11111111111111111111111111111111111111111", 3);
            Node node6 = new Node("post", "22222222222", 3);
            Node node7 = new Node("fllowers", "", 2);
            Node node8 = new Node("fllower", "", 3);
            Node node9 = new Node("id", "2", 4);
            Node node10 = new Node("follower", "", 3);
            Node node11 = new Node("id", "3", 4);
            Tree tree = new Tree(root);
            tree.addChild(node1, root);
            tree.addChild(node2, node1);
            tree.addChild(node3, node1);
            tree.addChild(node4, node1);
            tree.addChild(node7, node1);
            tree.addChild(node5, node4);
            tree.addChild(node6, node4);
            tree.addChild(node8, node4);
            tree.addChild(node10, node7);
            tree.addChild(node9, node8);
            tree.addChild(node11, node10);
            Node r = root;
            int count = r.getChildren().Count;
            Formating(root);
      

        }
    }

    class Node
    {
        private string tagName;
        private string tagValue;
        private int depth;
        private List<Node> children;

        public Node(string tagName, string tagValue, int depth)
        {
            this.tagName = tagName;
            this.tagValue = tagValue;
            this.depth = depth;
            children = new List<Node>();

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




    };

    class Tree
    {
        private Node root;
        public Tree(Node root)
        {
            this.root = root;
        }
        public void addChild(Node child, Node parent)
        {

            parent.getChildren().Add(child);
        }

        public Node getTreeRoot()
        {
            return this.root;
        }
    };
}
