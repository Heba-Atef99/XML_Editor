using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace XML_Editor
{
    class Operations
    {
        public int first;
        public string filename;

        public Operations(string fname, int first)
        {
            filename = fname;
            this.first = first;
        }

        private void skipchars(StreamReader reader)
        {
            char letter = (char)reader.Read();
            while (letter != '<')
            {
                letter = (char)reader.Read();
            }
        }

        private char skipSpaces(StreamReader reader)
        {
            char letter = (char)reader.Read();
            while (letter == '\n' || letter == '\t') letter = (char)reader.Read();
            return letter;
        }

        private string readTagName(StreamReader reader)
        {
            char letter = (char)reader.Read();
            
            //eliminate comments & weird tags
            if (char.IsLetter(letter))
            {
                string data = letter.ToString();

                while (letter != '>' && reader.Peek() != (int)' ')
                {
                    data += letter.ToString();
                    letter = (char)reader.Read();

                    //if it is a self closing tag
                    if (letter == '/') return null;
                }
                return data;
            }
            return null;
        }

        private string readTagAttributes(StreamReader reader)
        {
            char letter = (char)reader.Read();
            string data = letter.ToString();

            while (letter != '>')
            {
                data += letter.ToString();
                letter = (char)reader.Read();

                //if it is a self closing tag
                if (letter == '/') return null;
            }
            return data;
        }


        public int Consistency(StreamReader reader)
        {
            //you will need to read from reader & write in filename 
            //you need to write in filename after every read from reader
            int errors_num = 0;
            Stack<string> tags = new Stack<string>();
            char characteres;
            string tagName;
            string tagAttributes;

            //opening tag / closed tag

            //read from the file char by char
            while (reader.Peek() >= 0)
            {
                //read charachters until you reach <
                characteres = skipSpaces(reader);
                //skipchars(reader);
                
                //check if it is an opentag
                if (reader.Peek() != (int)'/')
                {
                    //get the tagname
                    tagName = readTagName(reader);

                    if(reader.Peek() == (int)' ')
                    {
                        //means that there are attributes to be read
                        characteres = (char)reader.Read();
                        tagAttributes = readTagAttributes(reader);
                    }
                    //but check that it is not a selfclosing tag (dont push self closing tags in stack)
                    if(tagName != null)
                    {
                        //push in stack
                    }
                }

                //if it is a closing tag
                else if(reader.Peek() == (int)'/')
                {
                    characteres = (char)reader.Read();
                    //get the tag name of the closing tag
                    tagName = readTagName(reader);

                    //compare it with the top of stack
                }
            }
            return errors_num;
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

        public void Formating(Node root)
        {
            Node r = root;
            List<Node> children = new List<Node>();
            children = root.getChildren();
            if (r == null)
                return;
            for (int loop = 0; loop < r.getDepth(); loop++)
            {
                write("    ", false);
            }
            if (r.getTagAttributes() == null)
            {
                if (r.getIsClosingTag())
                    write("<" + r.getTagName() + "/" + ">", true);
                
                else
                    write("<" + r.getTagName() + ">", true);
            }
            else
            {
                if (r.getIsClosingTag())
                    write("<" + r.getTagName() + " " + r.getTagAttributes() + "/" + ">", true);

                else
                    write("<" + r.getTagName() + " " + r.getTagAttributes() + ">", true);
            }
            
            if (r.getTagValue() != null)
            {
                for (int loop = 0; loop < r.getDepth() + 1; loop++)
                {
                    write("    ", false);
                }

                write(r.getTagValue(), true);
            }
            if (r != null)
            {
                foreach (Node child in children)
                {
                    Formating(child);
                }
            }

            if (r.getIsClosingTag() == false)
            {
                for (int loop = 0; loop < r.getDepth(); loop++)
                {
                    write("    ", false);
                }
                write("</" + r.getTagName() + ">", true);
            }
        }


        public string MinifyingAUX(Node root)
        {
            string xml = "";
            Node r = root;
            List<Node> children = new List<Node>();
            children = root.getChildren();
            if (r == null)
                return "";
            if (r.getTagAttributes() == null)
            {
                if (r.getIsClosingTag())
                { xml += "<" + r.getTagName() + "/" + ">"; }

                else
                { xml += "<" + r.getTagName() + ">"; }
            }
            else
            {
                if (r.getIsClosingTag())
                { xml += "<" + r.getTagName() + " " + r.getTagAttributes() + "/" + ">"; }

                else
                {
                    xml += "<" + r.getTagName() + " " + r.getTagAttributes() + ">";
                }
            }

            if (r.getTagValue() == null) { }
            else
            {
                string newStr = r.getTagValue().Trim();

                xml += newStr;
            }

            if (r != null)
            {

                foreach (Node child in children)
                {
                    xml += MinifyingAUX(child);
                }
            }
            if (!(r.getIsClosingTag()))
            {
                xml += "</" + r.getTagName() + ">";
            }
            return xml;
            
        }

        public void Minifying(Node root)
        {
            string s = MinifyingAUX(root);
            File.WriteAllText(filename, s);
        }

        public void Converting(Node root, Node parent)
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
                for (int i = 0; i < r.getDepth() * 4 + 2; i++)
                {
                    write(" ", false);
                }
                write('"' + r.getTagValue() + '"', true);
            }
            if (r.getChildren().Count == 0
                && ((r.getTagName() + "s") == parent.getTagName()) && root != children_parent[parent.getChildren().Count - 1])
            {
                for (int i = 0; i < r.getDepth() * 4 + 2; i++)
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
                Converting(child, root);
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
                    for (int i = 0; i < r.getDepth() * 4 + 2; i++)
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
