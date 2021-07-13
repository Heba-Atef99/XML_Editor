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
            Stack<string> myStack = new Stack<string>();
            string text = System.IO.File.ReadAllText(@"C:\Users\hp\Desktop\project_data\new.xml");

            // Display the file contents to the console. Variable text is a string.
            //System.Console.WriteLine("Contents of WriteText.txt = {0}", text);
            string[] lines = System.IO.File.ReadAllLines
                (@"C:\Users\hp\Desktop\project_data\new.xml");
           
            string n = lines[0];
            int num = lines.Length;
            Console.WriteLine(n);
            Console.WriteLine(num);
            consistency( lines, num);
        }
        public static void consistency(string[] lines,int n)
        {
            Stack<string> myStack = new Stack<string>();
            for(int i = 0; i < n; i++)
            {
                //line=line in xml
                string tag;
                string line = lines[i];
                if (line[0] == '<' && line[1] != '/')
                {
                    int endindex = line.IndexOf('>');
                    int length = endindex - 1 ;
                    tag = line.Substring(1, length);
                    
                }
            }

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
                        Console.Write(" ");
                        write(" ", false);
                    }


                }
                else
                {
                    for (int i = 0; i < r.getDepth() * 4; i++)
                    {
                       Console.Write(" ");
                        write(" ", false);

                    }

                }
                Console.WriteLine('"' + r.getTagName() + '"' + ":" + "{");
                write('"' + r.getTagName() + '"' + ":" + "{", true);
            }

            if (r.getChildren().Count == 0
                && ((r.getTagName() + "s") != parent.getTagName()) && root == children_parent[parent.getChildren().Count - 1])
            {
                for (int i = 0; i < r.getDepth() * 4; i++)
                {
                    Console.Write(" ");
                    write(" ", false);
                }
                Console.WriteLine('"' + r.getTagName() + '"' + ":" + '"' + r.getTagValue() + '"');
                write('"' + r.getTagName() + '"' + ":" + '"' + r.getTagValue() + '"', true);
            }

            if (r.getChildren().Count == 0
                && ((r.getTagName() + "s") != parent.getTagName()) && root != children_parent[parent.getChildren().Count - 1])
            {
                for (int i = 0; i < r.getDepth() * 4; i++)
                {
                    Console.Write(" ");
                    write(" ", false);

                }

               Console.WriteLine('"' + r.getTagName() + '"' + ":" + '"' + r.getTagValue() + '"' + ",");
                write('"' + r.getTagName() + '"' + ":" + '"' + r.getTagValue() + '"' + ",", true);
            }

            if (r != null && ((r.getTagName() + "s") == parent.getTagName())
                && root == children_parent[0] && children_parent.Count > 1)
            {
                for (int i = 0; i < r.getDepth() * 4; i++)
                {
                    Console.Write(" ");
                    write(" ", false);
                }
                Console.WriteLine('"' + r.getTagName() + '"' + ":" + "[");
                write('"' + r.getTagName() + '"' + ":" + "[", true);
            }

            if (r != null && ((r.getTagName() + "s") == parent.getTagName())
                && root == children_parent[0] && children_parent.Count == 1)
            {
                for (int i = 0; i < r.getDepth() * 4; i++)
                {
                    Console.Write(" ");
                    write(" ", false);
                }
                Console.WriteLine('"' + r.getTagName() + '"' + ":" + "{");
                write('"' + r.getTagName() + '"' + ":" + "{", true);
                flag = 0;
            }

            if (r.getChildren().Count == 0
                && ((r.getTagName() + "s") == parent.getTagName()) && root == children_parent[parent.getChildren().Count - 1])
            {
                for (int i = 0; i < r.getDepth() * 4 + 2; i++)
                {
                   Console.Write(" ");
                    write(" ", false);
                }
                Console.WriteLine('"' + r.getTagValue() + '"');
                write('"' + r.getTagValue() + '"', true);
            }
            if (r.getChildren().Count == 0
                && ((r.getTagName() + "s") == parent.getTagName()) && root != children_parent[parent.getChildren().Count - 1])
            {
                for (int i = 0; i < r.getDepth() * 4 + 2; i++)
                {
                    Console.Write(" ");
                    write(" ", false);
                }
                Console.WriteLine('"' + r.getTagValue() + '"' + ",");
                write('"' + r.getTagValue() + '"' + ",", true);
            }
            if (r != null && ((r.getTagName() + "s") == parent.getTagName())
                && children.Count > 0 && flag == 1)
            {
                for (int i = 0; i < r.getDepth() * 4 + 2; i++)
                {
                    Console.Write(" ");
                    write(" ", false);
                }
                Console.WriteLine("{");
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
                    Console.Write(" ");
                    write(" ", false);
                }
                Console.WriteLine("]");
                write("]", true);
            }
            if (r != null && r.getChildren().Count != 0
                && ((root.getTagName() + "s") != parent.getTagName()) && root == children_parent[parent.getChildren().Count - 1] || root.getDepth() == 0)
            {
                if (root.getDepth() == 0)
                {
                    for (int i = 0; i < r.getDepth() * 4 + 2; i++)
                    {
                        Console.Write(" ");
                        write(" ", false);

                    }

                }
                else
                {
                    for (int i = 0; i < r.getDepth() * 4; i++)
                    {
                        Console.Write(" ");
                        write(" ", false);
                    }
                }
                Console.WriteLine("}");
                write("}", true);
            }
            if (r != null && r.getChildren().Count != 0
                && ((root.getTagName() + "s") != parent.getTagName()) && root != children_parent[parent.getChildren().Count - 1] && root.getDepth() != 0)
            {
                for (int i = 0; i < r.getDepth() * 4; i++)
                {
                    Console.Write(" ");
                    write(" ", false);
                }
                Console.WriteLine("},");
                write("},", true);
            }
            //follower
            if (r != null && ((r.getTagName() + "s") == parent.getTagName())
                && children.Count > 0 && root == children_parent[parent.getChildren().Count - 1])
            {
                for (int i = 0; i < r.getDepth() * 4 + 2; i++)
                {
                    Console.Write(" ");
                    write(" ", false);
                }
                Console.WriteLine("}");
                write("}", true);
            }
            if (r != null && ((r.getTagName() + "s") == parent.getTagName())
                && children.Count > 0
                && root != children_parent[parent.getChildren().Count - 1])
            {
                for (int i = 0; i < r.getDepth() * 4 + 2; i++)
                {
                    Console.Write(" ");
                    write(" ", false);
                }
                Console.WriteLine("},");
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

