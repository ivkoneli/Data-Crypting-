using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Zastita_Informacija
{
    internal class playfair
    {

        int size = 5;
        char[,] table;

        // Initialization 
        public playfair(string key)
        {
            table = new char[size, size];
            CreateTable(key);
        }

        // Create table and fill it with our rulesets 
        public void CreateTable(string key)
        {
            // List for storing table elements
            List<char> elements = new List<char>();

            key = key.ToLower();

            char[] newkey = key.ToCharArray();

            


            // Add only unique elements to list and remove 'j' 
            foreach (char c in newkey)
            {
                if ( c != 'j' && !elements.Contains(c))
                {
                    elements.Add(c);
                }             
            }


            // Add all the remaining alphabet letters 
            for ( char c ='a'; c<='z';c++)
            {
                if(c !='j' && !elements.Contains(c))
                {
                    elements.Add(c);
                }
            }

            /*foreach ( char c in elements)
            {
                Console.Write(c);
            }
            Console.WriteLine("");*/

            // example key = MONARCHY  = > M O N A R    REMOVED letters of the alpfabet A C H M N O R Y J 
            //                             C H Y B D
            //                             E F G I K
            //                             L P Q S T
            //                             U V W X Z 

            // Add elements into our table 

            int i = 0, j = 0;
            foreach (char c in elements)
            {
                table[i, j] = c;
                j++;
                if ( j == size)
                {
                    i++;
                    j = 0;
                }
            }
        }

        public List<int> GetPos(char c)
        {
            List<int> pos = new List<int>();

            //Console.WriteLine(c);


            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    if (table[i , j] == c )
                    {
                        pos.Add(i);
                        pos.Add(j);
                        return pos;
                    }
                }
            }

            return pos;
        }
        public string Crypt(string text)
        {
            Console.WriteLine("Play fair Keytable : ");
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    Console.Write(table[i, j] + "\t");
                }
                Console.WriteLine();
            }
            Console.WriteLine();




            // Transform to lower cap and remove spaces 
            text = text.ToLower().Replace(" ", "");
            /*Console.Write("Starting text to lower cap :");
            foreach(char c in text)
            {
                Console.Write(c);
            }
            Console.WriteLine("");*/

            // New array to store text 
            List<char> newText = new List<char>();

            // Adding an 'x' if two chararacters are the same 
            for (int i = 0; i < text.Length; i++)
            {
                if ( i < text.Length  - 1 && text[i] == text[i+1])
                {
                    newText.Add(text[i]); newText.Add('x');
                }else{
                    newText.Add(text[i]);
                }
            }

            // example HELLO WORLD = helxloworld 

            if (newText.Count % 2 == 1)
            {
                newText.Add('z');
            }

            /*Console.Write("new text :");
            foreach (char c in newText)
            {
                Console.Write(c);
            }
            Console.WriteLine("");*/


            // helxloworldz he lx lo wo rl dz
            //              cf su mp nv tm kr

            List<char> encryptedtext = new List<char>();
            for ( int i = 0; i < newText.Count; i+=2 )
            {
                List<int> x = GetPos(newText[i]);
                List<int> y = GetPos(newText[i + 1]);
                /*Console.WriteLine("Pozicija");
                Console.Write(x[0]); Console.Write(x[1]);
                Console.Write(y[0]); Console.Write(y[1]);
                Console.WriteLine("");*/

                // same row
                if (x[0] == y[0])
                {
                    encryptedtext.Add(table[x[0], (x[1]+ 1 ) % size]);
                    encryptedtext.Add(table[y[0], (y[1] + 1) % size]);

                }
                // same column
                else if (x[1] == y[1])
                {
                    encryptedtext.Add(table[(x[0] + 1) % size, x[1]]);
                    encryptedtext.Add(table[(y[0] + 1) % size, y[1]]);

                }
                // both different
                else
                {   //  add the opposite diagonals 
                    //  he - > cf 
                    encryptedtext.Add(table[x[0], y[1]]);              
                    encryptedtext.Add(table[y[0], x[1]]);                                            
                } 
            }



            // now our text looks like this 
            // helxloworldz  = he lx lo wo rl dz
            // encryptedtext = cf su mp nv tm kr


            return new string(encryptedtext.ToArray());
        }

        public string Decrypt(string encryptedtext)
        {

            // encryptedtext = cf su mp nv tm kr 
            List<char> text = new List<char>();

 


            // Decrypt into modified plain text
            for (int i = 0;i < encryptedtext.Length; i+=2)
            {
                List<int> x = GetPos(encryptedtext[i]);
                List<int> y = GetPos(encryptedtext[i + 1]);

                if (x[0] == y[0])
                {
                    // Same row 
                    if ( x[1] == 0)
                    {

                        text.Add(table[x[0],     size-1]);
                        text.Add(table[y[0], y[1] - 1]);
                    }
                    else if( y[1] == 0)
                    {
                        text.Add(table[x[0], x[1] - 1]);
                        text.Add(table[y[0],     size-1]);
                    }
                    else
                    {
                        text.Add(table[x[0], x[1] - 1]);
                        text.Add(table[y[0], y[1] - 1]);
                    }

                }
                else if(x[1] == y[1])
                {
                    // Same column
                    // if its on the top edge replace with the bottom element
                    if ( x[0] == 0)
                    {
                        text.Add(table[size-1  , x[1]]);
                        text.Add(table[y[0] - 1, y[1]]);

                    }
                    else if ( y[0] == 0)
                    {
                        text.Add(table[x[0] - 1, x[1]]);
                        text.Add(table[size-1  , y[1]]);

                    }
                    // else replace with up + 1
                    else
                    {
                        text.Add(table[x[0] - 1, x[1]]);
                        text.Add(table[y[0] - 1, y[1]]);
                    }

                }
                else
                {
                    // Both different 
                    text.Add(table[x[0], y[1]]);
                    text.Add(table[y[0], x[1]]);
                }
            }

            // text = helxloworldz
            /*Console.WriteLine("");
            foreach( char c in text)
            {
                Console.Write(c);
            }
            Console.WriteLine("");*/


            // remove x and z 
            //string newtext = text.ToString();
            text.Remove('x');
            if ( text.Last() == 'z')
            {
                text.Remove('z');
            }
            

            /*foreach( char c in text)
            {
                Console.Write(c);
            }*/

            // new text = helloworld
            Console.Write("Decrypted Text : ");
            Console.WriteLine(text.ToArray());

            return new string(text.ToArray());

           
        }
        public string EncryptFile(string inputFilePath, string outputFilePath)
        {
            // Read the plaintext from the input file
            string plaintext = File.ReadAllText(inputFilePath);

            // Encrypt the plaintext
            string ciphertext = Crypt(plaintext);

            // Write the ciphertext to the output file
            File.WriteAllText(outputFilePath, ciphertext);

            return plaintext;
        }

        public void DecryptFile(string inputFilePath, string outputFilePath)
        {
            // Read the ciphertext from the input file
           string ciphertext = File.ReadAllText(inputFilePath);

            // Decrypt the ciphertext
           string plaintext = Decrypt(ciphertext);

            // Write the plaintext to the output file
            File.WriteAllText(outputFilePath, plaintext);
        }
    }

}
