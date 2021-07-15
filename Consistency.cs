using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XML_Editor
{
    class Consistency
    {
        public int first;
        public string filename;

        public Consistency(string fname, int first)
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
                    while (letter != '>')
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
            // \n ==> new line ,, \t ==> tap 
            if (characteres != '<' && characteres != '\r' && characteres != '>')
            {
                return true;//has value
            }
            return false;
        }

        public int Consistent(StreamReader reader)
        {
            int errors_num = 0;
            Stack<string> tags = new Stack<string>();
            char characteres;
            string tagName = null;
            string tagAttributes = null;
            bool prevHadData = false;//to know if the saved tag name has value or not
            bool currentHasData = false;//to know if the saved tag name has value or not
            List<string> data_letter = new List<string>();
            bool prevIsClosed = false;

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

                        //case that the closing tag is missing
                        else if (prevHadData && !prevIsClosed)
                        {
                            //check if current tag has data or not
                            write("</" + tags.Peek() + ">", true);
                            prevIsClosed = true;
                            tags.Pop();
                            tags.Push(tagName);
                            write("<" + tagName + tagAttributes + ">", false);
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
                        if (prevHadData && !prevIsClosed)
                        {
                            //check if current tag has data or not
                            write("</" + tags.Peek() + ">", true);
                            prevIsClosed = true;
                            tags.Pop();
                            errors_num++;
                        }

                        //selfclosing tag
                        write("<" + tagName + tagAttributes + "/>", false);
                        characteres = (char)reader.Read();
                    }
                    else
                    {
                        write("<" + data_letter[1], false);
                    }

                    prevHadData = (data_letter[1] == "/") ? false : currentHasData;
                    prevIsClosed = (data_letter[1] == "/") ? true : false;
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
                        prevIsClosed = true;
                    }

                    else if (tags.Count > 0)
                    {
                        errors_num += 1;
                        write("</" + tags.Peek() + ">", false);
                        tags.Pop();
                        prevIsClosed = true;

                        if (tags.Count > 0 && tagName == tags.Peek())
                        {
                            write("</" + tagName + ">", false);
                            tags.Pop();
                        }
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
    }
}
