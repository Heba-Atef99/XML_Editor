using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XML_Editor
{

    class Compressing
    {
        public string path1;
        public string path2;

        public Compressing(string input, string dest_file)
        {
            path1 = input;
            path2 = dest_file;
        }

        public void encoding()
        {
            File.WriteAllText(path2, String.Empty);
            string input = File.ReadAllText(path1);

            HuffmanTree huffmanTree = new HuffmanTree();
            huffmanTree.Build(input);

            //encode
            BitArray encoded = huffmanTree.Encode(input);

            //convert bits to bytes
            byte[] bytes = new byte[(int)Math.Ceiling((double)encoded.Length / 8)];
            encoded.CopyTo(bytes, 0);

            //extra byte for bits which is not divisible by 8
            int Addextra = 8 - ((encoded.Length) - ((encoded.Length / 8) * 8));
            string binary = Addextra.ToString();

            byte[] add = Encoding.ASCII.GetBytes(binary);//1 byte

            //store symbols of the tree at the begining of the file with its number
            string arr = "";
            arr = huffmanTree.storeSymbol(huffmanTree.Root, arr);

            byte[] dict = Encoding.ASCII.GetBytes(arr);

            byte[] arrLength = BitConverter.GetBytes((short)arr.Length);

            //store compressed bytes
            BinaryWriter writer = new BinaryWriter(File.OpenWrite(path2));
            writer.Write(arrLength);
            writer.Write(dict);
            writer.Write(bytes);
            writer.Write(add);
            writer.Flush();
            writer.Close();
        }

        public void decoding()
        {
            //read compressed file to decompress
            byte[] read = File.ReadAllBytes(path1);

            //get back Huffman tree of the compressed file
            byte[] firstBytes = { read[0], read[1] };
            short TreeSize = BitConverter.ToInt16(firstBytes, 0);


            string TreeNodes = "";
            firstBytes = new byte[TreeSize];
            for (int i = 0; i < TreeSize; i++)
            {
                firstBytes[i] = read[2 + i];
            }

            TreeNodes = Encoding.ASCII.GetString(firstBytes);

            byte[] cut = new byte[read.Length - (TreeSize + 2)];
            for (int i = 0; i < cut.Length; i++)
            {
                cut[i] = read[i + TreeSize + 2];
            }

            /*Construct Tree*/
            HuffmanTree huffmanTree1 = new HuffmanTree();
            huffmanTree1.Root = huffmanTree1.PreOrderBuild(TreeNodes);

            //get the last byte
            byte[] b = { cut[cut.Length - 1] };
            int getBack = 8 + Convert.ToInt32(Encoding.ASCII.GetString(b));

            var e = new BitArray(cut);
            string s = "";
            BitArray f = new BitArray(e.Length - getBack);
            for (var i = 0; i < e.Length - getBack; i++)
            {
                f[i] = e[i];
            }

            for (var i = 0; i < f.Length; i++)
            {
                s += (f[i] ? 1 : 0);

            }

            string decoded = huffmanTree1.Decode(f);

            string pathD = path2;
            StreamWriter sw = File.CreateText(pathD);
            sw.Write(decoded);
            sw.Flush();
            sw.Close();
        }
    }
}
