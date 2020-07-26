using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.CodeDom.Compiler;
using System.Drawing.Drawing2D;

namespace Pokemon_Fusion_Calculator
{
    class Generatefile
    {

        public string[][] writeLines = new string[351][];
        public void fetchData(string pokemon)
        {
            var mySortedList = DexData.pokemon.OrderBy(d => d.Value).ToList();
            string bakenumber = "";
            int index = Convert.ToInt32(pokemon);

            if (pokemon.Length < 3)
            {
                for (int i = 0; i < 3 - pokemon.Length; i++)
                {
                    bakenumber += "0";
                }
                bakenumber += pokemon;
            }
            else
            {
                bakenumber = pokemon;
            }

            string bakedUrl = "https://serebii.net/pokedex-swsh/" + mySortedList[Convert.ToInt32(pokemon)-1].Key.ToLower();
            Console.WriteLine("Trying \"" + bakedUrl + "\"");
            HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(bakedUrl);
            myRequest.Method = "GET";
            WebResponse myResponse = myRequest.GetResponse();
            StreamReader sr = new StreamReader(myResponse.GetResponseStream(), System.Text.Encoding.UTF8);
            
            string temp;

            while ((temp = sr.ReadLine()) != null)
            {
                if (temp.Length == 76 && temp.Substring(48,19) == "Base Stats - Total:")
                {
                    foreach (var item in DexData.pokemon)
                    {
                        if (item.Value == index)
                        {
                            writeLines[index][0] = item.Key;
                            Console.WriteLine("Added name "+item.Key);
                            break;
                        }
                    }

                    for (int i = 1; i < 7; i++)
                    {
                        string f = sr.ReadLine().Substring(35, 2);
                        writeLines[index][i] = f;
                    }
                    Console.WriteLine();
                    break;
                }
            }

            sr.Close();
            myResponse.Close();
        }

        public void writeFile(int size)
        {
            using (System.IO.StreamWriter file =
            new System.IO.StreamWriter(@"C:\Users\as_so\Desktop\basestats.txt"))
            {
                file.Write("{ ");
                for (int i = 1; i < size; i++)
                {
                    if (writeLines[i] != null)
                    {
                        file.Write("{ \"" + writeLines[i][0] + "\", new int[6] { ");
                        for (int x = 1; x < 6; x++)
                        {
                            file.Write(writeLines[i][x] + ", ");
                        }
                        file.WriteLine(writeLines[i][6] + " } }, ");
                    }
                }
                file.Write("{ \"" + writeLines[size][0] + "\", new int[6] { ");

                for (int x = 1; x < 6; x++)
                {
                    file.Write(writeLines[size][x] + ", ");
                }
                file.WriteLine(writeLines[size][6] + " } } ");

                file.Write(" }");
            }
        }

        public void dothing()
        {
            int currentSize = 350;

            for (int i = 0; i <= currentSize; i++)
            {
                writeLines[i] = new string[7];
            }

            if (currentSize > 251)
            {
                for (int i = 1; i <= 251; i++)
                {
                    fetchData(i.ToString());
                }

                for (int i = 251; i < currentSize; i++)
                {

                }
            }
            writeFile(currentSize);
        }
    }
}
