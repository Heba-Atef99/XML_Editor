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
                write(letter.ToString(), false);
                letter = (char)reader.Read();
            }
        }

        private char skipSpaces(StreamReader reader)
        {
            char letter;
            while (reader.Peek() == (int)'\n' || reader.Peek() == (int)'\t' || reader.Peek() == (int)' ')
            {
                letter = (char)reader.Read();
            }
            return (char)reader.Peek();
        }

        private List<string> readTagData(StreamReader reader)
        {
            char letter = (char)reader.Read();
            List<string> data_letter = new List<string>();
            data_letter.Add(null);
            data_letter.Add(letter.ToString());
            data_letter.Add(null);

            string data = "";

            //<frame/>
            while (letter != '>')
            {
                data += letter.ToString();
                letter = (char)reader.Read();

                //if there are attributes
                if (letter == ' ')
                {
                    string attr = "";
                    while(letter != '>')
                    {
                        attr += letter;
                        letter = (char)reader.Read();

                        if (letter == '/' && reader.Peek() == (int)'>')
                        {
                            break;
                        }

                    }
                    data_letter[2] = attr;
                }
                
                if (letter == '/' && reader.Peek() == (int)'>')
                {
                    break;
                }
            }
            data_letter[0] = (data == "") ? null : data;
            data_letter[1] = letter.ToString();
            return data_letter;
        }

        private bool hasData(StreamReader reader)
        {
            char characteres = skipSpaces(reader);//read '<'
            if (characteres != '<' && characteres != '\0' && characteres != '>')
            {
                return true;//has value
            }
            return false;
        }

        public int Consistency(StreamReader reader)
        {
            int errors_num = 0;
            Stack<string> tags = new Stack<string>();
            char characteres;
            string tagName = null;
            string tagAttributes = null;
            bool prevHadData = false;//to know if the saved tag name has value or not
            bool currentHasData = false;//to know if the saved tag name has value or not
            List<string> data_letter = new List<string>();

            //read from the file char by char
            while (reader.Peek() >= 0)
            {
                //read charachters until you reach <
                skipchars(reader);

                //check if it is an opening tag
                if (Char.IsLetter((char)reader.Peek()))
                {                    
                    //get the tagname
                    data_letter = readTagData(reader);
                    tagName = data_letter[0];

                    //get tag attributes
                    tagAttributes = data_letter[2];

                    //but check that it is not a selfclosing tag (dont push self closing tags in stack)
                    if (data_letter[1] != "/" && tagName != null)
                    {
                        //read tag data 
                        currentHasData = hasData(reader);

                        //if it is the first opening tag then push it in stack
                        if (tags.Count == 0)
                        {
                            tags.Push(tagName);
                            write("<" + tagName + tagAttributes + ">", false);
                        }

                        //if closing tag is opened !!
                        else if (tagName == tags.Peek() && prevHadData)
                        {
                            //check if current tag has data or not
                            write("</" + tags.Peek() + ">", true);
                            tags.Pop();
                            if (currentHasData)
                            {
                                tags.Push(tagName);
                                write("<" + tagName + tagAttributes + ">", false);
                            }
                            errors_num++;
                        }

                        else
                        {
                            //normal case with no errors
                            tags.Push(tagName);
                            write("<" + tagName + tagAttributes + ">", false);
                        }
                    }
                    else if (data_letter[1] == "/")
                    {
                        //selfclosing tag
                        write("<" + tagName + tagAttributes + "/>", false);
                        characteres = (char)reader.Read();
                    }
                    else
                    {
                        write("<" + data_letter[1], false);
                    }

                    prevHadData = currentHasData;
                }

                //if it is a closing tag
                else if (reader.Peek() == (int)'/')
                {
                    characteres = (char)reader.Read();

                    //get the tag name of the closing tag
                    tagName = readTagData(reader)[0];

                    //if matching 
                    //compare it with the top of stack
                    if (tags.Count > 0 && tags.Peek() == tagName)
                    {
                        write("</" + tagName + ">", false);
                        tags.Pop();
                    }

                    else if(tags.Count > 0)
                    {
                        errors_num += 1;
                        write("</" + tags.Peek() + ">", false);
                        tags.Pop();
                    }
                }

                //if it is a comment or anything else , skip it
                else
                {
                    characteres = (char)reader.Read();
                    while (characteres != '>')
                    {
                        characteres = (char)reader.Read();
                    }
                }
            }

            //if file is finished and stack not empty
            while (tags.Count != 0)
            {
                errors_num++;
                write("</" + tags.Peek() + ">", false);
                tags.Pop();
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
