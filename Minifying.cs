using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XML_Editor
{
    class Minifying
    {
        public int first;
        public string filename;

        public Minifying(string fname, int first)
        {
            filename = fname;
            this.first = first;
        }

        public string MinifyingAUX(Node root)
        {
            string xml = ""; //used to save data and will be return from this function 
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

            if (r.getTagValue() != null)
            {
                string newStr = r.getTagValue().Trim();

                xml += newStr;
            }

            if (r != null)
            {
                     // loop to each child in the node 
                foreach (Node child in children)
                {
                    xml += MinifyingAUX(child);//call function for each node 
                }
            }
            if (!(r.getIsClosingTag()))
            {
                xml += "</" + r.getTagName() + ">";
            }
            return xml;

        }

        public void Minify(Node root)
        {
            string s = MinifyingAUX(root);
            File.WriteAllText(filename, s);
        }

    }
}
