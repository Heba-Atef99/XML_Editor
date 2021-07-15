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

        public void Format(Node root)
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


