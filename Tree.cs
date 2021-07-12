using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace XML_Editor
{
    class Node
    {
        private string tagName;
        private string tagValue;
        private string tagAttributes;
        private int depth;
        private List<Node> children = new List<Node>();

        public Node(string tagName, string tagValue, string tagAttributes, int depth)
        {
            this.tagName = tagName;
            this.tagValue = tagValue;
            this.depth = depth;
            this.tagAttributes = tagAttributes;
        }
        public Node()
        {
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

        public int getDepth()
        {
            return this.depth;
        }

        public string getTagAttributes()
        {
            return this.tagAttributes;
        }

        public void setTagName(string tn)
        {
            tagName = tn;
        }
        public void setTagValue(string tv)
        {
            tagValue = tv;
        }
        public void setDepth(int d)
        {
            depth = d;
        }

        public void setTagAttributes(string ta)
        {
            tagAttributes = ta;
        }
    }

    class Tree
    {
        private Node root;
        public Tree(Node root)
        {
            this.root = root;
        }

        public Tree()
        {
            root = null;
        }

        private char skipSpaces(StreamReader reader)
        {
            char letter = (char)reader.Read();
            while (letter == '\n' || letter == '\t') letter = (char)reader.Read();
            return letter;
            //letters[1] = (char)reader.Read();
            //while (letters[1] == '\n' || letters[1] == '\t') letters[1] = (char)reader.Read();
        }

        private char skipSpaces2(StreamReader reader)
        {
            char letter = (char)reader.Peek();
            while (reader.Peek() == (int)('\n') || reader.Peek() == (int)('\t') || reader.Peek() == (int)(' '))
            {
                letter = (char)reader.Read();
            }
            return letter;
            //letters[1] = (char)reader.Read();
            //while (letters[1] == '\n' || letters[1] == '\t') letters[1] = (char)reader.Read();
        }


        public Node getTreeRoot()
        {
            return this.root.getChildren()[0];
        }

        private void insertFileAUX(StreamReader reader, Node parent)
        {
            //char[] letter = new char[2];
            char letter;
            string data;
            string name;
            //Node parent = new Node();

            while (reader.Peek() >= 0)
            {
                //read one char skipping spaces & new lines
                letter = skipSpaces(reader);

                //check if we are reading an opening tag
                if (letter == '<')
                {
                    letter = skipSpaces(reader);

                    if (Char.IsLetter(letter))
                    { 
                        //add new child to current parent and update child depth
                        Node child = new Node(null, null, null, parent.getDepth() + 1);
                        //child.setDepth(parent.getDepth() + 1);
                        parent.getChildren().Add(child);

                        //save the tagName in name
                        name = letter.ToString();
                        letter = (char)reader.Read();
                        while (letter != '>')
                        {
                            name += letter.ToString();
                            letter = (char)reader.Read();

                            //save attributes
                            if(letter == ' ')
                            {
                                data = reader.Read().ToString();
                                letter = (char)reader.Read();
                                while (letter != '>')
                                {
                                    data += letter.ToString();
                                    letter = (char)reader.Read();
                                }

                                child.setTagAttributes(data);
                            }
                        }

                        //****save the tagname in node****
                        child.setTagName(name);

                        //skip spaces & new lines without consuming the letter
                        while (reader.Peek() == (int)('\n') || reader.Peek() == (int)('\t') || reader.Peek() == (int)(' '))
                        {
                            letter = (char)reader.Read();
                        }

                        //save the data inside the tag if there is data
                        if (reader.Peek() != (int)('<'))
                        {
                            letter = (char)reader.Read();
                            data = letter.ToString();

                            //save the data until reaching the closing tag
                            while (reader.Peek() != (int)('<'))
                            {
                                letter = (char)reader.Read();
                                data += letter.ToString();
                            }
                            //****save the data in node****
                            child.setTagValue(data);
                        }
                        else
                        {
                            insertFileAUX(reader, child);
                        }
                    }
                }

                //check if we are reading a closing tag
                else if (letter == '<' && reader.Peek() == (int)('/'))
                {
                    return;
                }

                //else if (letter == '<')
                //{
                //    letter = skipSpaces(reader);
                //    if(letter == '/') return;
                //}

            }
        }

        public void insertFile(StreamReader reader)
        {
            Node parent = new Node();
            root = parent;
            parent.setDepth(-1);
            insertFileAUX(reader, parent);
        }
    }
}

