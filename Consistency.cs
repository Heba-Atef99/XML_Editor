 public int Consistency(StreamReader reader)
            {
                //you will need to read from reader & write in filename 
                //you need to write in filename after every read from reader
                int errors_num = 0;
                Stack<string> tags = new Stack<string>();
                char characteres;
                string tagName;
                string tagAttributes;
                string prev ="";//to save preveous tag name
                Boolean flag = false;//to know if the saved tabg name has value or not
                //opening tag / closed tag
                //read from the file char by char
                while (reader.Peek() >= 0)
                {
                    //read charachters until you reach <
                    characteres = skipSpaces(reader);

                    skipchars(reader);

                    //check if it is an opentag
                    if (reader.Peek() != (int)'/')
                    {
                        //get the tagname
                        tagName = readTagName(reader);
                        if (reader.Peek() == (int)' ')
                        {
                            //means that there are attributes to be read
                            characteres = (char)reader.Read();
                            tagAttributes = readTagAttributes(reader);
                        }
                        //but check that it is not a selfclosing tag (dont push self closing tags in stack)
                        if (tagName != null)
                        {
                            //push in stack
                            //if has value and not closed 
                            if (((tagName != tags.Peek() && (!flag)) || (tags.Count == 0)))
                            {if (prev == tags.Peek())
                                { tags.Pop();//<id></id> after edit <id><name>
                                    errors_num++;
                                }
                                tags.Push(tagName);
                                flag = false;

                                prev = tagName;
                            }
                            else if ((tagName == tags.Peek() && (flag)))
                            {
                                write_closing_tagname(tags.Peek());
                                tags.Pop();
                                tags.Push(tagName);
                                flag = false;
                                errors_num++;

                                prev = tagName;
                            
                            }
                            //not has a value and not closed and the next is tha same
                            else if ((tagName==tags.Peek())&&(!flag))
                            {
                                tags.Push(tagName);
                                flag = false;
                                prev = tagName;
                            }
                           

                        }
                        characteres = (char)reader.Read();//read '>'
                        if (reader.Peek() != '<')
                        { flag = true;//has value
                        }

                    }

                    //if it is a closing tag
                    else if (reader.Peek() == (int)'/')
                    {
                        characteres = (char)reader.Read();
                        //get the tag name of the closing tag
                        tagName = readTagName(reader);
                        //if matching 
                        if (tags.Peek() == tagName)
                        {
                            tags.Pop();
                        }
                        else
                        { errors_num += 1;
                         write_closing_tagname(tags.Peek());
                            tags.Pop();
                        }
                        //compare it with the top of stack
                    }
                    //if file is finished and stack not empty
                    while(tags.Count != 0)
                    { errors_num++;
                        tags.Pop();
                    }
                return errors_num;
            }
