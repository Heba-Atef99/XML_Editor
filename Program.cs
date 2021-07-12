using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
namespace XML_Editor
{
    public static class Global
    {
        public static int first = 0;
    }
    class Program
    {
        static void Main(string[] args)
        {


            Node root = new Node("users", null, 0);
            //n = & root;
            Node node20 = new Node("user", null, 1);
            Node node21 = new Node("id", "5", 2);
            Node node22 = new Node("user", null, 1);
            Node node23 = new Node("id", "8", 2);
            Node node1 = new Node("user", null, 1);
            Node node2 = new Node("id", "1", 2);
            Node node3 = new Node("name", "nana", 2);
            Node node4 = new Node("posts", null, 2);
            Node node5 = new Node("post", "11111111111", 3);
            Node node6 = new Node("post", "2222222222", 3);
            Node node7 = new Node("followers", null, 2);
            Node node8 = new Node("follower", null, 3);
            Node node9 = new Node("id", "2", 4);
            Node node10 = new Node("follower", null, 3);
            Node node11 = new Node("id", "3", 4);
            Tree tree = new Tree(root);
            tree.addChild(node1, root);
            tree.addChild(node20, root);
            tree.addChild(node21, node20);
            //tree.addChild(node22, root);
            //tree.addChild(node23, node22);
            tree.addChild(node2, node1);
            tree.addChild(node3, node1);
            tree.addChild(node4, node1);
            tree.addChild(node7, node1);
            tree.addChild(node5, node4);
            tree.addChild(node6, node4);
            tree.addChild(node8, node7);
            tree.addChild(node10, node7);
            tree.addChild(node9, node8);
            tree.addChild(node11, node10);
            //node1.setTagName("dddd");
            Node r = root;

            int count = r.getChildren().Count;
            List<Node> children = new List<Node>();
            children = r.getChildren();
            count = children[0].getChildren().Count;
            //Console.WriteLine("{");
            write("{", true);
            convert(root, root);
            //Console.WriteLine("}");
            write("}", true);
        }
        public static void convert(Node root, Node parent)
        {
            Node r = root;
            int flag = 1;
            List<Node> children = new List<Node>();
            children = root.getChildren();
            List<Node> children_parent = new List<Node>();
            children_parent = parent.getChildren();
            if (r != null && r.getChildren().Count != 0 && ((r.getTagName() + "s") != parent.getTagName()))
            {
                if (root.getDepth() == 0)
                {
                    for (int i = 0; i < r.getDepth() * 4 + 2; i++)
                    {
                        //Console.Write(" ");
                        write(" ", false);
                    }


                }
                else
                {
                    for (int i = 0; i < r.getDepth() * 4; i++)
                    {
                       // Console.Write(" ");
                        write(" ", false);

                    }

                }
                //Console.WriteLine('"' + r.getTagName() + '"' + ":" + "{");
                write('"' + r.getTagName() + '"' + ":" + "{", true);
            }

            if (r.getChildren().Count == 0
                && ((r.getTagName() + "s") != parent.getTagName()) && root == children_parent[parent.getChildren().Count - 1])
            {
                for (int i = 0; i < r.getDepth() * 4; i++)
                {
                    //Console.Write(" ");
                    write(" ", false);
                }
               // Console.WriteLine('"' + r.getTagName() + '"' + ":" + '"' + r.getTagValue() + '"');
                write('"' + r.getTagName() + '"' + ":" + '"' + r.getTagValue() + '"', true);
            }

            if (r.getChildren().Count == 0
                && ((r.getTagName() + "s") != parent.getTagName()) && root != children_parent[parent.getChildren().Count - 1])
            {
                for (int i = 0; i < r.getDepth() * 4; i++)
                {
                    //Console.Write(" ");
                    write(" ", false);

                }

               // Console.WriteLine('"' + r.getTagName() + '"' + ":" + '"' + r.getTagValue() + '"' + ",");
                write('"' + r.getTagName() + '"' + ":" + '"' + r.getTagValue() + '"' + ",", true);
            }

            if (r != null && ((r.getTagName() + "s") == parent.getTagName())
                && root == children_parent[0] && children_parent.Count > 1)
            {
                for (int i = 0; i < r.getDepth() * 4; i++)
                {
                    //Console.Write(" ");
                    write(" ", false);
                }
                //Console.WriteLine('"' + r.getTagName() + '"' + ":" + "[");
                write('"' + r.getTagName() + '"' + ":" + "[", true);
            }

            if (r != null && ((r.getTagName() + "s") == parent.getTagName())
                && root == children_parent[0] && children_parent.Count == 1)
            {
                for (int i = 0; i < r.getDepth() * 4; i++)
                {
                    //Console.Write(" ");
                    write(" ", false);
                }
                //Console.WriteLine('"' + r.getTagName() + '"' + ":" + "{");
                write('"' + r.getTagName() + '"' + ":" + "{", true);
                flag = 0;
            }

            if (r.getChildren().Count == 0
                && ((r.getTagName() + "s") == parent.getTagName()) && root == children_parent[parent.getChildren().Count - 1])
            {
                for (int i = 0; i < r.getDepth() * 4 + 2; i++)
                {
                   //Console.Write(" ");
                    write(" ", false);
                }
                //Console.WriteLine('"' + r.getTagValue() + '"');
                write('"' + r.getTagValue() + '"', true);
            }
            if (r.getChildren().Count == 0
                && ((r.getTagName() + "s") == parent.getTagName()) && root != children_parent[parent.getChildren().Count - 1])
            {
                for (int i = 0; i < r.getDepth() * 4 + 2; i++)
                {
                    //Console.Write(" ");
                    write(" ", false);
                }
                //Console.WriteLine('"' + r.getTagValue() + '"' + ",");
                write('"' + r.getTagValue() + '"' + ",", true);
            }
            if (r != null && ((r.getTagName() + "s") == parent.getTagName())
                && children.Count > 0 && flag == 1)
            {
                for (int i = 0; i < r.getDepth() * 4 + 2; i++)
                {
                    //Console.Write(" ");
                    write(" ", false);
                }
                //Console.WriteLine("{");
                write("{", true);
            }
            foreach (Node child in children)
            {
                convert(child, root);
            }
            if (root.getChildren().Count != 0 && root.getTagName() == (children[0].getTagName() + "s")
                && children.Count > 1)
            {
                for (int i = 0; i < r.getDepth() * 4 + 4; i++)
                {
                    //Console.Write(" ");
                    write(" ", false);
                }
                //Console.WriteLine("]");
                write("]", true);
            }
            if (r != null && r.getChildren().Count != 0
                && ((root.getTagName() + "s") != parent.getTagName()) && root == children_parent[parent.getChildren().Count - 1] || root.getDepth() == 0)
            {
                if (root.getDepth() == 0)
                {
                    for (int i = 0; i < r.getDepth() * 4 + 2; i++)
                    {
                        //Console.Write(" ");
                        write(" ", false);

                    }

                }
                else
                {
                    for (int i = 0; i < r.getDepth() * 4; i++)
                    {
                        //Console.Write(" ");
                        write(" ", false);
                    }
                }
                //Console.WriteLine("}");
                write("}", true);
            }
            if (r != null && r.getChildren().Count != 0
                && ((root.getTagName() + "s") != parent.getTagName()) && root != children_parent[parent.getChildren().Count - 1] && root.getDepth() != 0)
            {
                for (int i = 0; i < r.getDepth() * 4; i++)
                {
                    //Console.Write(" ");
                    write(" ", false);
                }
                //Console.WriteLine("},");
                write("},", true);
            }
            //follower
            if (r != null && ((r.getTagName() + "s") == parent.getTagName())
                && children.Count > 0 && root == children_parent[parent.getChildren().Count - 1])
            {
                for (int i = 0; i < r.getDepth() * 4 + 2; i++)
                {
                    //Console.Write(" ");
                    write(" ", false);
                }
                //Console.WriteLine("}");
                write("}", true);
            }
            if (r != null && ((r.getTagName() + "s") == parent.getTagName())
                && children.Count > 0
                && root != children_parent[parent.getChildren().Count - 1])
            {
                for (int i = 0; i < r.getDepth() * 4 + 2; i++)
                {
                    //Console.Write(" ");
                    write(" ", false);
                }
                //Console.WriteLine("},");
                write("},", true);
            }

        }
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
    }
}

