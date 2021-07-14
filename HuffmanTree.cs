using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace XML_Editor
{
    class HuffmanTree
    {
        private List<BNode> nodes = new List<BNode>();
        public BNode Root { get; set; }
        public Dictionary<char, int> Frequencies = new Dictionary<char, int>();
        

        public void Build(string source)
        {
            for (int i = 0; i < source.Length; i++)
            {
                if (!Frequencies.ContainsKey(source[i]))
                {
                    Frequencies.Add(source[i], 0);
                }

                Frequencies[source[i]]++;
            }

            foreach (KeyValuePair<char, int> symbol in Frequencies)
            {
                nodes.Add(new BNode() { Symbol = symbol.Key, Frequency = symbol.Value });
            }

            while (nodes.Count > 1)
            {
                //custom comparer(ascending)
                List<BNode> orderedNodes = nodes.OrderBy(node => node.Frequency).ToList<BNode>();

                if (orderedNodes.Count >= 2)
                {
                    // Take first two items
                    List<BNode> taken = orderedNodes.Take(2).ToList<BNode>();

                    // Create a parent node by combining the frequencies
                    BNode parent = new BNode()
                    {
                        Symbol = '\0',
                        Frequency = taken[0].Frequency + taken[1].Frequency,
                        Left = taken[0],
                        Right = taken[1]
                    };

                    nodes.Remove(taken[0]);
                    nodes.Remove(taken[1]);
                    nodes.Add(parent);
                }

                this.Root = nodes.FirstOrDefault();

            }

        }

        public BitArray Encode(string source)
        {
            List<bool> encodedSource = new List<bool>();

            for (int i = 0; i < source.Length; i++)
            {
                List<bool> encodedSymbol = this.Root.Traverse(source[i], new List<bool>());
                encodedSource.AddRange(encodedSymbol);//add code of each symbol
            }

            BitArray bits = new BitArray(encodedSource.ToArray());

            return bits;
        }

        public string Decode(BitArray bits)
        {
            BNode current = this.Root;
            string decoded = "";

            foreach (bool bit in bits)
            {
                if (bit)
                {
                    if (current.Right != null)
                    {
                        current = current.Right;
                    }
                }
                else
                {
                    if (current.Left != null)
                    {
                        current = current.Left;
                    }
                }

                if (IsLeaf(current))
                {
                    decoded += current.Symbol;
                    current = this.Root;
                }
            }

            return decoded;
        }

        public bool IsLeaf(BNode node)
        {
            return (node.Left == null && node.Right == null);
        }

        public string storeSymbol(BNode root,string arr)
        {
            if(root == null)
            {
                return arr;
            }
            arr += root.Symbol;
             arr = storeSymbol(root.Left,arr);
             arr = storeSymbol(root.Right,arr);
            return arr;  
        }

        public BNode PreOrderBuild(string s)
        {
            Stack<BNode> myStack = new Stack<BNode>();
            if (s.Length == 0)
                return null;
            BNode root = new BNode();
            root.Symbol = s[0];
            myStack.Push(root);
            myStack.Push(root);
            
            for (int i = 1; i < s.Length; i++)
            {
                BNode current = new BNode();
                current.Symbol = s[i];
                if (root.Left == null)
                    root.Left = current;
                else
                    root.Right = current;

                root = current;

                if (root.Symbol == '\0')
                    myStack.Push(root);
                else
                {
                    root = myStack.Peek();
                    myStack.Pop();
                }
            }
            return root;

        }
    }
}
