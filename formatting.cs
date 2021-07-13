using System;
using System.Collections.Generic;
using System.IO;

namespace ConsoleApp14
{
    public static class Global
    {
        public static int first = 0;
    }
    class Program
    {
        const string filename = "xmll.txt";
        public static void write(string s, bool add)
        {
            if (Global.first == 0)
            {
                StreamWriter sw = new StreamWriter(filename, false);
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
                Global.first = 1;
            }
            else
            {
                StreamWriter sw = new StreamWriter(filename, true);
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
        }

        public static void Formating(Node root)
        {
            Node r = root;
            List<Node> children = new List<Node>();
            children = root.getChildren();
            if (r == null)
                return;
            for (int loop = 0; loop < r.getDepth(); loop++)
            {
                write("    ", false);
                //Console.Write("    ");
            }
            if (r.getTagAttributes() == null)//no attributes
            {if (r.getIsClosingTag())
             {
                    write("<" + r.getTagName() + "/" + ">", true);
            }
             else 
             {
                write("<" + r.getTagName() + ">", true);

             }  //Console.WriteLine("<" + r.getTagName() + ">");
            }
            else
            {if (r.getIsClosingTag())
                {
                    write("<" + r.getTagName() + " " + r.getTagAttributes() + "/" + ">", true);

                    
                }
             else 
             {
                write("<" + r.getTagName() +" "+r.getTagAttributes()+ ">", true);

                //Console.WriteLine("<" + r.getTagName() + " " + r.getTagAttributes() + ">");
            }}
          
              if (!(r.getTagValue() == null))
            {
                for (int loop = 0; loop < r.getDepth() + 1; loop++)
                {
                    write("    ", false);
                  //  Console.Write("    ");
                }

                write(r.getTagValue(), true);
                //Console.WriteLine(r.getTagValue());
            }
            if (r != null)
            {

                foreach (Node child in children)
                {
                    Formating(child);
                }
            }
         
           
                if (!(r.getIsClosingTag())) {
                   for (int loop = 0; loop < r.getDepth(); loop++)

                {
                    write("    ", false);

                  //  Console.Write("    ");
                }

                write("</" + r.getTagName() + ">", true);


                //Console.WriteLine("</" + r.getTagName() + ">");
               }

        }

        static void Main(string[] args)
        {
            
             int i;
            Node root = new Node("users", null,null,false, 0);
            Node node1 = new Node("user", null,null,false, 1);
            Node node2 = new Node("id", "1",null,false, 2);
            Node node3 = new Node("name", "nana",null,false, 2);
            Node node4 = new Node("posts", null,"attribute",false, 2);
            Node node5 = new Node("post", "11111111111111111111111111111111111111111",null,false,3);
            Node node6 = new Node("post", "22222222222",null,false, 3);
            Node node7 = new Node("fllowers", null,null,false, 2);
            Node node8 = new Node("fllower", null,null,false, 3);
            Node node9 = new Node("id", "2",null,false, 4);
            Node node10 = new Node("follower", null,null,false, 3);
            Node node11 = new Node("id", "3",null,false, 4);
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
        private string tagAttributes;
         private bool isClosingTag;
        private int depth;
        private List<Node> children;

         public Node(string tagName, string tagValue, string tagAttributes, bool isClosingTag, int depth)
        {
            this.tagName = tagName;
            this.tagValue = tagValue;
            this.depth = depth;
            this.tagAttributes = tagAttributes;
              this.isClosingTag = isClosingTag;
            children = new List<Node>();

        }
        public Node()
        {     isClosingTag = false;
            tagName = null;
            tagValue = null;
            tagAttributes = null;
            depth = 0;
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
        public bool getIsClosingTag()
        {
            return isClosingTag;
        }
        public int getDepth()
        {
            return this.depth;
        }

        public string getTagAttributes()
        {
            return this.tagAttributes;
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

