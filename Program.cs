using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using XML_Editor;

namespace XML_Editor
{
    class Program
    {
        static void Main(string[] args)
        {
            string path1 = @"E:\DS project\C#\XML_Editor\test.txt";

            string path2 = @"E:\DS project\C#\XML_Editor\test2.txt";
            File.WriteAllText(path2, String.Empty);
            string input = File.ReadAllText(path1);
          
            HuffmanTree huffmanTree = new HuffmanTree();
            huffmanTree.Build(input);

            //encode
            BitArray encoded = huffmanTree.Encode(input);
            //Console.WriteLine("encoded length :"  + encoded.Length);
            //Console.WriteLine("round number of bytes: " + Math.Ceiling((double)encoded.Length / 8));

            //convert bits to bytes
            byte[] bytes = new byte[(int)Math.Ceiling((double)encoded.Length / 8)];
            encoded.CopyTo(bytes, 0);

            //extra byte for bits which is not divisible by 8
            int Addextra = 8 - ((encoded.Length) - ((encoded.Length / 8) * 8));
            string binary = Addextra.ToString();
           
            //Console.WriteLine("extra bits at the end: "+binary);
            byte[] add = Encoding.ASCII.GetBytes(binary);//1 byte
           
            //store symbols of the tree at the begining of the file with its number
            string arr = "" ;
            arr = huffmanTree.storeSymbol(huffmanTree.Root,arr);
            //Console.WriteLine("symbols stored: " + arr + " Length: " + arr.Length);
          
            byte[] dict = Encoding.ASCII.GetBytes(arr);

            byte[] arrLength = BitConverter.GetBytes((short)arr.Length);
            //Console.WriteLine("dict: " + dict.Length);
            //Console.WriteLine("bytes encoded: " + bytes.Length);
            //Console.WriteLine("extra byte" + add.Length);

            //store compressed bytes
            BinaryWriter writer = new BinaryWriter(File.OpenWrite(path2));
            writer.Write(arrLength);
            writer.Write(dict);
            writer.Write(bytes);
            writer.Write(add);
            writer.Flush();
            writer.Close();


            /*
                    ------  DECODING -----
             
             */

            //read compressed file to decompress
            byte[] read = File.ReadAllBytes(path2);
            //Console.WriteLine("size readed: " + read.Length);

            //get back Huffman tree of the compressed file
            byte[] firstBytes = { read[0],read[1]};
            //Console.WriteLine(Encoding.ASCII.GetString(firstBytes));
            //Console.WriteLine("first byte:" + read[0]);
            short TreeSize = BitConverter.ToInt16(firstBytes,0);
            //Console.WriteLine("Tree size: " + TreeSize);

            
            string TreeNodes = "";
            firstBytes = new byte[TreeSize];
            for (int i = 0; i < TreeSize; i++)
            {
                firstBytes[i] = read[2+i];
            }
            
            TreeNodes = Encoding.ASCII.GetString(firstBytes);
            //Console.WriteLine("TreeNodes:" + TreeNodes);

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
            
            string pathD = @"E:\DS project\C#\XML_Editor\decoded.txt";
            StreamWriter sw = File.CreateText(pathD);
            sw.Write(decoded);
            sw.Flush();
            sw.Close();
            Console.WriteLine("Decoded: " + decoded);
            if (input == decoded)
                Console.WriteLine("TRUE");
        }

      
    }
}
