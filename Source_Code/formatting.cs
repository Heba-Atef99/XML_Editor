using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XML_Editor
{
    class Formatting
    {
        public int first;
        public string filename;

        public Formatting(string fname, int first)
        {
            filename = fname;
            this.first = first;
        }
//function used for writing in output file
        public void write(string s, bool add)
        {
            if (first == 0)//if this is the first write after submitting formatting button  
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
            //if this is not  the first write after submitting formatting button 
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

        public void Format(Node root)
        {
            Node r = root;
            List<Node> children = new List<Node>();
            children = root.getChildren();
            //if node is null will return 
            if (r == null)
                return;
            //loop to the depth of each node and write space for indentation
            for (int loop = 0; loop < r.getDepth(); loop++)
            {
                write("    ", false);
            }
            //if node not has attribute
            if (r.getTagAttributes() == null)
            {//if the tag name is closing tag < id/>
                if (r.getIsClosingTag())
                    write("<" + r.getTagName() + "/" + ">", true);
                //if the tag name isnot closing tag < id>
                else
                    write("<" + r.getTagName() + ">", true);
            }
            //if node has attribute
            else
            {//if the tag name is closing tag < id/>
                if (r.getIsClosingTag())
                    write("<" + r.getTagName() + " " + r.getTagAttributes() + "/" + ">", true);
                  //if the tag name isnot closing tag < id>
                else
                    write("<" + r.getTagName() + " " + r.getTagAttributes() + ">", true);
            }
              //if node has tag value
            if (r.getTagValue() != null)
            {    //loop for the depth of node+1 to make indentation
                for (int loop = 0; loop < r.getDepth() + 1; loop++)
                {
                    write("    ", false);
                }
                //write tag value 
                write(r.getTagValue(), true);
            }
            if (r != null)
            { //loop for each child in the node 
                foreach (Node child in children)
                {
                    Format(child);
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
    }
}


