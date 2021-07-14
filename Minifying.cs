 public static string Minifying(Node root)
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

            if (!(r.getTagValue() == null)) 
            
            {
                string newStr = r.getTagValue().Trim();

                xml += newStr;
            }

            if (r != null)
            {

                foreach (Node child in children)
                {
                    xml += Minifying(child);
                }
            }
            if (!(r.getIsClosingTag()))
           
            { 
            xml += "</" + r.getTagName() + ">"; }
            return xml;
        } 
