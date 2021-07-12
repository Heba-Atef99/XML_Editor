using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace XML_Editor
{
    class Program
    {
        public static object FilleMode { get; private set; }

        static void Main(string[] args)
        {

            //string s = "";
            //string[] lines = File.ReadAllLines(path);
            //foreach (string line in lines)
            //{
            //    byte[] arr = Encoding.ASCII.GetBytes(line);

            //    //foreach(byte e in arr)
            //    //{
            //    //    Console.WriteLine((char)Convert.ToByte(Convert.ToString(e, 2),2));
            //    //    s += Convert.ToString(e, 2);
            //    //    Console.WriteLine(Convert.ToString(e, 2));

            //    //}
            //}

            ////Console.WriteLine(s+"\n");
            ////Console.WriteLine(s.Length+"\n");
            string path = @"E:\DS project\C#\XML_Editor\test.txt";
            string path2 = @"E:\DS project\C#\XML_Editor\test2.bin";
            File.WriteAllText(path2, String.Empty);

            string[] lines = File.ReadAllLines(path);
            string text = "";
            foreach (string line in lines)
            {
                text += line;
            }
            //File.WriteAllText(path2, text);
           // string s = "WYS*WYGWYS*WYSWYSG";
            List<short> outCode = new List<short>();
            outCode = encoding(text);
            //Console.WriteLine("Output Codes are: ");
            string s = "";
            for(int i = 0; i < outCode.Count; i++)
            {
                Console.WriteLine(outCode[i] + " " );
                s += (short)outCode[i];
                //BitArray b = new BitArray(new int[] { outCode[i] });
                //byte[] bytes = new byte[b.Length / 8 + (b.Length % 8 == 0 ? 0 : 1)];
                //b.CopyTo(bytes, 0);
                //File.WriteAllBytes(path2, bytes);
                //string binary = Convert.ToString(outCode[i], 2);
                //byte [] arr =new byte[binary.Length]
            }
            var  arr = Encoding.ASCII.GetBytes(s);
            BinaryWriter writer = new BinaryWriter(new FileStream(path2, FileMode.Create));

            writer.Write(arr);
                //foreach(var b in arr)
                //{
                //    writer.Write(b);
                //}
                writer.Flush();
                writer.Close();
            
            Console.WriteLine("\n");
            File.WriteAllText(path2, s);
            decoding(outCode);
            Console.WriteLine(outCode.Count);
            Console.WriteLine(text.Length);
        }

        public static List<short> encoding(string file)
        {

            //Console.WriteLine("Encoding:\n");
            Dictionary<string, short> table = new Dictionary<string, short>();
            //prepare a table of ASCII codes for all  characters
            for(short i = 0; i <=255; i++)
            {
                string key = "";
                key += (char) i;
                table.Add(key, i);
            }

            string first = "", second = "";
            first += file[0];
            short code = 256;
            List<short> outCode = new List<short>();
           // Console.WriteLine("String\tOutput_Code\tAddition\n");
            for(int j = 0; j < file.Length; j++)
            {
                if(j != file.Length - 1)
                {
                    second += file[j + 1];
                }
                if (table.ContainsKey(first + second))
                {
                    first += second;
                }
                else
                {
                    //Console.WriteLine(first + "\t" + table[first] + "\t\t" + 
                        //first+second + "\t" + code + "\n");
                    outCode.Add(table[first]);
                    table.Add(first + second, code);
                    code++;
                    first = second;
                }
                second = "";
            }
            //Console.WriteLine(first + "\t" + table[first] + "\n");
            outCode.Add(table[first]);
            return outCode;
        }

        public static void decoding(List<short> outCode) 
        {
            //Console.WriteLine("Decoding: ");
            Dictionary<short, string> table = new Dictionary<short, string>();
            for(short i = 0; i <=255; i++)
            {
                string val = "";
                val += (char)i;
                table.Add(i, val);
            }
            short old = outCode[0], n;
            string str1 = table[old];
            string str2 = "";
            str2 += str1[0];
            //Console.WriteLine(str1);
            short count = 256;
            for(int i = 0; i < outCode.Count - 1; i++)
            {
                n = outCode[i + 1];
                if (!table.ContainsKey(n))
                {
                    str1 = table[old];
                    str1 += str2;
                }
                else
                {
                    str1 = table[n];
                }
               // Console.WriteLine(str1);
                str2 = "";
                str2 += str1[0];
                table[count] = table[old] + str2;
                count++;
                old = n;
            }
        }
    }
}
