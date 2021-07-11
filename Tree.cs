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
        private int depth;
        private List<Node> children = new List<Node>();

        public Node(string tagName, string tagValue, int depth)
        {
            this.tagName = tagName;
            this.tagValue = tagValue;
            this.depth = depth;
        }
        public Node()
        {
            tagName = null;
            tagValue = null;
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

        private void readTwoletters(StreamReader reader, char[] letters)
        {
            letters[0] = (char)reader.Read();
            while(letters[0] == '\n' || letters[0] == '\t') letters[0] = (char)reader.Read();
            letters[1] = (char)reader.Read();
            while (letters[1] == '\n' || letters[1] == '\t') letters[1] = (char)reader.Read();
        }

        public Node getTreeRoot()
        {
            return this.root;
        }

        private void appendChild(StreamReader reader, Node parent)
        {
            char[] letter = new char[2];
            string data;
            //Node parent = new Node();

            while (reader.Peek() >= 0)
            {
                //read two char.s from the file
                readTwoletters(reader, letter);

                //check if we are reading an opening tag
                if (letter[0] == '<' && letter[1] != '/')
                {
                    Node child = new Node();
                    child.setDepth(parent.getDepth() + 1);
                    parent.getChildren().Add(child);
                    data = letter[1].ToString();
                    letter[1] = (char)reader.Read();

                    //save the tagName in data
                    while (letter[1] != '>')
                    {
                        data += letter[1].ToString();
                        letter[1] = (char)reader.Read();
                    }
                    //****save the tagname in node****
                    child.setTagName(data);

                    //data = ((char)reader.Peek()).ToString();
                    while(reader.Peek() == (int)('\n') || reader.Peek() == (int)('\t'))
                    {
                        letter[1] = (char)reader.Read();
                    }
                    //save the data inside the tag if there is data
                    if (reader.Peek() != (int)('<'))
                    {
                        letter[0] = (char)reader.Read();
                        data = letter[0].ToString();

                        letter[0] = (char)reader.Read();

                        //save the data until reaching the closing tag
                        while (letter[0] != '<')
                        {
                            data += letter[0].ToString();
                            letter[0] = (char)reader.Read();
                        }
                        //****save the data in node****
                        child.setTagValue(data);
                    }
                    else
                    {
                        appendChild(reader, child);
                    }
                }

                //check if we are reading a closing tag
                else if (letter[0] == '<' && letter[1] == '/')
                {
                    return;
                }
            }
        }

        public void insertFile(StreamReader reader)
        {
            Node parent = new Node();
            root = parent;
            parent.setDepth(-1);
            appendChild(reader, parent);
        }
    }
}

