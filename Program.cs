using System;

namespace XML_Editor
{
    class Program
    {
        static void Main(string[] args)
        {
            Node root = new Node("users", "", 0);
            Node node1 = new Node("user", "", 1);
            Node node2 = new Node("id", "1", 2);
            Node node3 = new Node("name", "nana", 2);
            Node node4 = new Node("posts", "", 2);
            Node node5 = new Node("post", "11111111111", 3);
            Node node6 = new Node("post", "2222222222", 3);
            Node node7 = new Node("followers", "", 2);
            Node node8 = new Node("follower", "", 3);
            Node node9 = new Node("id", "2", 4);
            Node node10 = new Node("follower", "", 3);
            Node node11 = new Node("id", "3", 4);
            Tree tree = new Tree(root);
            tree.addChild(node1, root);
            tree.addChild(node2, node1);
            tree.addChild(node3, node1);
            tree.addChild(node4, node1);
            tree.addChild(node7, node1);
            tree.addChild(node5, node4);
            tree.addChild(node6, node4);
            tree.addChild(node8, node7);
            tree.addChild(node10, node7);
            tree.addChild(node9, node8);
            tree.addChild(node11, node10);
            Node r = root;




        }
    }
}
