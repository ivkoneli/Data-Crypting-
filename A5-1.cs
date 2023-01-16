using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zastita_Informacija
{
    internal class A5_1
    {
        int[] key = null;

        int SIZEX = 19; int SIZEY = 22; int SIZEZ = 23;
        int regXCLK = 8; int regYCLK = 10; int regZCLK = 10;

        int[] regX = null; int[] regY = null; int[] regZ = null;


        // Konstruktor - inicijalizujemo vrednosti registra i stavljamo nas kljuc u njih
        public A5_1(int[] key)
        {
            regX = new int[SIZEX]; regY = new int[SIZEY]; regZ = new int[SIZEZ];

            this.PlaceKey(key);


           /* Console.Write("A5/1 - Kljuc     :");
            for (int i = 0; i < key.Length; i++)
            {
                Console.Write(key[i]);
            }
            Console.WriteLine("");

            Console.Write("A5/1 - Registar X:");
            for (int i = 0; i < regX.Length; i++)
            {
                Console.Write(regX[i]);
            }
            Console.WriteLine("");

            Console.Write("A5/1 - Registar Y:");
            for (int i = 0; i < regY.Length; i++)
            {
                Console.Write(regY[i]);
            }
            Console.WriteLine("");

            Console.Write("A5/1 - Registar Z:");
            for (int i = 0; i < regZ.Length; i++)
            {
                Console.Write(regZ[i]);
            }
            Console.WriteLine("");*/
        }

        // Majority funkcija koja proverava clock bitove sva tri registra da bi proverila koji trenutno radi 
        public int majority(int x, int y, int z)
        {
            if (x == y)
            {
                return x;
            }
            else if (y == z)
            {
                return y;
            }
            else if (x == z)
            {
                return z;
            }
            return 0;
        }
        public void PlaceKey(int[] key)
        {
            // Prazan kljuc 
            if (key == null)
            {
                return;
            }


            // Stavljamo 64-bitni kljuc u nase registre redom pocevsi od LFSR  X 
            int j = 0;
            int k = 0;
            for (int i = 0; i < key.Length; i++)
            {

                if (i < 19)
                {
                    regX[i] = key[i];
                }
                else if (i >= 19 && i < 41)
                {
                    regY[j++] = key[i];
                }
                else if (i >= 41 && i < 64)
                {
                    regZ[k++] = key[i];
                }
            }
        }


        // Shiftovanje prosledjenog registra
        public void shiftRegister(ref int[] register)
        {
            int swap = 0;

            if (register.Length == SIZEX)
            {
                swap = register[18] ^ register[17] ^ register[16] ^ register[13];

            }
            else if (register.Length == SIZEY)
            {
                swap = register[20] ^ register[21];
            }
            else if (register.Length == SIZEZ)
            {
                swap = register[22] ^ register[21] ^ register[20] ^ register[7];
            }

            for (int size = (register.Length - 1); size >= 0; size--)
            {
                if (register.Length == SIZEX)
                {
                    if (size == 0)
                    {
                        register[size] = swap;
                    }
                    else
                    {
                        register[size] = register[size - 1];
                    }
                }
                else if (register.Length == SIZEY)
                {
                    if (size == 0)
                    {
                        register[size] = swap;
                    }
                    else
                    {
                        register[size] = register[size - 1];
                    }
                }
                else if (register.Length == SIZEZ)
                {
                    if (size == 0)
                    {
                        register[size] = swap;
                    }
                    else
                    {
                        register[size] = register[size - 1];
                    }
                }

            }

        }

        // Vracamo poslednji element iz registra 
        public int Output(int[] register)
        {
            return register[register.Length - 1];
        }

        public int[] Crypt ( int[] input)
        {
            int [] result = new int[input.Length];
            int s = 0;

            PlaceKey(key);

            for (int i = 0; i < input.Length; i++)
            {
                int m = majority(this.regX[regXCLK], this.regY[regYCLK], this.regZ[regZCLK]);

                if (this.regX[regXCLK] == m)
                {
                    shiftRegister(ref this.regX);
                }else if (this.regY[regYCLK] == m)
                {
                    shiftRegister(ref this.regY);
                }else if (this.regZ[regZCLK] == m)
                {
                    shiftRegister(ref this.regZ);
                }
                s = (Output(regX) ^ Output(regY) ^ Output(regZ));
                result[i] = s ^ input[i];
            }

            

            return result;
        }

        public int[] Decrypt(int[] input, int[] key)
        {
            PlaceKey(key);
            return this.Crypt(input);
        }

        public void EncryptFile(string inputFile , string outputFile,int[] key)
        {
            //BinaryReader br = new BinaryReader(new FileStream(inputFile, FileMode.Open));
            //BinaryWriter bw = new BinaryWriter(new FileStream(outputFile, FileMode.Create));

            /*int[] input = new int[8];
            int[] output = new int[8];

            while (br.BaseStream.Position != br.BaseStream.Length)
            {
                for (int i = 0; i < 8; i++)
                {
                    if (br.BaseStream.Position == br.BaseStream.Length)
                        break;
                    else
                        input[i] = br.ReadInt32();
                }

                Console.WriteLine("Procitano : ");
                for (int i = 0; i < 8; i++)
                    {
                        Console.Write(input[i]);
                    }
                Console.WriteLine("");
                output = this.Crypt(input);


                for (int i = 0; i < 8; i++)
                {
                    bw.Write(output[i]);
                }
            }*/

            int[] input = new int[8];
            BinaryReader br = new BinaryReader(new FileStream(inputFile, FileMode.Open));
            for (int i = 0; i < 8; i++)
            {
                input[i] = br.ReadInt32();
            }
            br.Close();
   
            Console.WriteLine("Text iz fajla : ");
            for (int i = 0; i < input.Length; i++)
            {
                Console.Write(input[i]);
            }
            Console.WriteLine("");

            int[] output = new int[8];
            output = this.Decrypt(input,key);

            /*Console.WriteLine("Cipher iz fajla : ");
            for (int i = 0; i < output.Length; i++)
            {
                Console.Write(output[i]);
            }
            Console.WriteLine("");*/
      

            BinaryWriter bw = new BinaryWriter(new FileStream(outputFile, FileMode.Create));
            for (int i = 0; i < 8; i++)
            {
                bw.Write(output[i]);
            }
            bw.Close();





        }
        
        public void DecryptFile(string inputFile, string outputFile,int[] key)
        {
            EncryptFile(inputFile, outputFile,key);
        }
    }
}
