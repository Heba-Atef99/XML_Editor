public static void Formating(Node root)
        {
            bool flag = false;//to determine if child has value or not 
            Node r = root;
            List<Node> children = new List<Node>();
            children = root.getChildren();
            if(r==null)
                return ;
  //for formatting
            for (int loop = 0; loop < r.getDepth(); loop++)
               Console.Write("    ");
            Console.Write( "<" + r.getTagName() + ">");
            if (r.getTagValue() == "")//no value write in a new line  
                Console.WriteLine(r.getTagValue());
            else 
            {
                Console.Write(r.getTagValue()); flag = true;
            }
            if (r != null)
            {

                foreach (Node child in children)
                {
                     Formating(child);
                }
            }
            if (flag == true)
            { Console.WriteLine("</" + r.getTagName() + ">");flag = false; return; }
            if (r.children == null)
                Console.WriteLine("</" + r.getTagName() + ">");
            else
            {
                for (int loop = 0; loop < r.getDepth(); loop++)
                    Console.Write("    ");
                Console.WriteLine("</" + r.getTagName() + ">");
            }
        }
