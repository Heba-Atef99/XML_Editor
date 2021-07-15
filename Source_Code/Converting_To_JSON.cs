using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XML_Editor
{
    class Converting_To_JSON
    {
        public int first;
        public string filename;

        public Converting_To_JSON(string fname, int first)
        {
            filename = fname;
            this.first = first;
        }

        public void write(string s, bool add)
        {
            if (first == 0)
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
                first = 1;
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

        public void Convert(Node root, Node parent)
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
                    for (int i = 0; i < r.getDepth() * 4; i++)
                    {
                        write(" ", false);
                    }
                }
                else
                {
                    for (int i = 0; i < r.getDepth() * 4; i++)
                    {
                        write(" ", false);
                    }

                }
                write('"' + r.getTagName() + '"' + ":" + "{", true);
            }

            if (r.getChildren().Count == 0
                && ((r.getTagName() + "s") != parent.getTagName()) && root == children_parent[parent.getChildren().Count - 1])
            {
                for (int i = 0; i < r.getDepth() * 4; i++)
                {
                    write(" ", false);
                }
                write('"' + r.getTagName() + '"' + ":" + '"' + r.getTagValue() + '"', true);
            }

            if (r.getChildren().Count == 0
                && ((r.getTagName() + "s") != parent.getTagName()) && root != children_parent[parent.getChildren().Count - 1])
            {
                for (int i = 0; i < r.getDepth() * 4; i++)
                {
                    write(" ", false);

                }

                write('"' + r.getTagName() + '"' + ":" + '"' + r.getTagValue() + '"' + ",", true);
            }

            if (r != null && ((r.getTagName() + "s") == parent.getTagName())
                && root == children_parent[0] && children_parent.Count > 1)
            {
                for (int i = 0; i < r.getDepth() * 4; i++)
                {
                    write(" ", false);
                }
                write('"' + r.getTagName() + '"' + ":" + "[", true);
            }

            if (r != null && ((r.getTagName() + "s") == parent.getTagName())
                && root == children_parent[0] && children_parent.Count == 1)
            {
                for (int i = 0; i < r.getDepth() * 4; i++)
                {
                    write(" ", false);
                }
                write('"' + r.getTagName() + '"' + ":" + "{", true);
                flag = 0;
            }

            if (r.getChildren().Count == 0
                && ((r.getTagName() + "s") == parent.getTagName()) && root == children_parent[parent.getChildren().Count - 1])
            {
                for (int i = 0; i < r.getDepth() * 4; i++)
                {
                    write(" ", false);
                }
                write('"' + r.getTagValue() + '"', true);
            }
            if (r.getChildren().Count == 0
                && ((r.getTagName() + "s") == parent.getTagName()) && root != children_parent[parent.getChildren().Count - 1])
            {
                for (int i = 0; i < r.getDepth() * 4; i++)
                {
                    write(" ", false);
                }
                write('"' + r.getTagValue() + '"' + ",", true);
            }
            if (r != null && ((r.getTagName() + "s") == parent.getTagName())
                && children.Count > 0 && flag == 1)
            {
                for (int i = 0; i < r.getDepth() * 4 + 2; i++)
                {
                    write(" ", false);
                }

                write("{", true);
            }
            foreach (Node child in children)
            {
                Convert(child, root);
            }
            if (root.getChildren().Count != 0 && root.getTagName() == (children[0].getTagName() + "s")
                && children.Count > 1)
            {
                for (int i = 0; i < r.getDepth() * 4 + 4; i++)
                {
                    write(" ", false);
                }
                write("]", true);
            }
            if (r != null && r.getChildren().Count != 0
                && ((root.getTagName() + "s") != parent.getTagName()) && root == children_parent[parent.getChildren().Count - 1] || root.getDepth() == 0)
            {
                if (root.getDepth() == 0)
                {
                    for (int i = 0; i < r.getDepth() * 4; i++)
                    {
                        write(" ", false);
                    }
                }
                else
                {
                    for (int i = 0; i < r.getDepth() * 4; i++)
                    {
                        write(" ", false);
                    }
                }
                write("}", true);
            }
            if (r != null && r.getChildren().Count != 0
                && ((root.getTagName() + "s") != parent.getTagName()) && root != children_parent[parent.getChildren().Count - 1] && root.getDepth() != 0)
            {
                for (int i = 0; i < r.getDepth() * 4; i++)
                {
                    write(" ", false);
                }
                write("},", true);
            }
            if (r != null && ((r.getTagName() + "s") == parent.getTagName())
                && children.Count > 0 && root == children_parent[parent.getChildren().Count - 1])
            {
                for (int i = 0; i < r.getDepth() * 4 + 2; i++)
                {
                    write(" ", false);
                }
                write("}", true);
            }
            if (r != null && ((r.getTagName() + "s") == parent.getTagName())
                && children.Count > 0
                && root != children_parent[parent.getChildren().Count - 1])
            {
                for (int i = 0; i < r.getDepth() * 4 + 2; i++)
                {
                    write(" ", false);
                }
                write("},", true);
            }

        }
    }
}
