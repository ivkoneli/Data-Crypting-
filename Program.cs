using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zastita_Informacija
{
    internal class Program
    {   
        static void CFB_playfair()
        {
            Console.WriteLine("############################");
            Console.WriteLine("         CFB-PLAYFAIR       ");
            string key = "MONARCHY";
            string input = "text";
            string iv = "vect";
            CFB playfair = new CFB(key, iv, 4);

            string encryptedText = playfair.EncryptPlayFair(input);
            Console.WriteLine(encryptedText);
            string decryptedText = playfair.DecryptPlayFair(encryptedText);
            Console.WriteLine(decryptedText);

        }
        static void CFB()
        {
            Console.WriteLine("############################");
            Console.WriteLine("             CFB            ");
            int[] key = { 0, 1, 0, 0, 1, 0, 1, 1, 0, 1, 0, 0, 0, 1, 1, 1,
                          1, 0, 0, 0, 1, 1, 0, 1, 0, 1, 0, 0, 0, 1, 0, 0,
                          0, 1, 0, 1, 0, 0, 1, 1, 0, 0, 0, 0, 0, 1, 1, 0,
                          0, 1, 0, 0, 1, 0, 0, 1, 0, 1, 0, 0, 1, 0, 0, 0 };

            int[] input =      { 1, 0, 1, 0, 0, 1, 1, 1 };
            int[] initVector = { 0, 1, 1, 1 ,0, 1, 0 ,1 };


            Console.Write("CFB A5-1 Text  :");
            for (int i = 0; i < input.Length; i++)
            {
                Console.Write(input[i]);
            }
            Console.WriteLine();

            CFB a5 = new CFB(key, initVector, 4);

            int[] encryptedText = a5.Encrypt(input, key);
            Console.Write("CFB A5-1 Encrypted Text  :");
            for ( int i = 0; i < encryptedText.Length; i++)
            {
                Console.Write(encryptedText[i]);
            }


            int[] decryptedText = a5.Decrypt(encryptedText, key);
            Console.WriteLine("");
            Console.Write("CFB A5-1 Decrypted Text  :");
            for ( int i = 0; i < decryptedText.Length; i++)
            {
                Console.Write(decryptedText[i]);
            }
            Console.WriteLine("");
            Console.WriteLine("############################");
        }
        static void playfair()
        {
            Console.WriteLine("############################");
            Console.WriteLine("           PLAYFAIR         ");
            string key = "MONARCHY";
            playfair pf = new playfair(key);
            string input = "secret message";
            Console.Write("Text to encrypt :");
            Console.WriteLine(input);
            Console.WriteLine("");
            string encryptedText = pf.Crypt(input);
            Console.Write("Encrypted text :");
            Console.WriteLine(encryptedText);
            Console.WriteLine("Decrypting the new message...");
            Console.WriteLine();
            string output = pf.Decrypt(encryptedText);
            //Console.WriteLine("Decrypted text : ", output);
            Console.WriteLine("");
            Console.WriteLine("Citanje iz fajla...");
            string inputFile = "playfairInput.txt";
            string encryptedFile = "playfairEncrypted.txt";
            string outputFile = "playfairOutput.txt";

            pf.EncryptFile(inputFile, encryptedFile);
            pf.DecryptFile(encryptedFile, outputFile);

            Console.WriteLine("############################");


        }
        static void a5_1()
        {
            Console.WriteLine("############################");
            Console.WriteLine("             A5-1           ");
            int[] key = { 0, 1, 0, 0, 1, 0, 1, 1, 0, 1, 0, 0, 0, 1, 1, 1,
                          1, 0, 0, 0, 1, 1, 0, 1, 0, 1, 0, 0, 0, 1, 0, 0,
                          0, 1, 0, 1, 0, 0, 1, 1, 0, 0, 0, 0, 0, 1, 1, 0,
                          0, 1, 0, 0, 1, 0, 0, 1, 0, 1, 0, 0, 1, 0, 0, 0 };

            int[] input =       { 1, 1, 1, 1, 1, 1, 1 ,0 };
            int[] inputzaFajl = { 1, 1, 1, 1, 1, 1, 1, 0 };
            int[] initVector =  { 0, 1, 1, 1, 0, 1, 0, 1 };
            A5_1 a5 = new A5_1(key);


            int[] kodiranInput = a5.Crypt(initVector);
            int[] dekodiranText = a5.Decrypt(kodiranInput,key);

            Console.WriteLine("A5/1 - Text:");
            for (int i = 0; i < input.Length; i++)
            {
                Console.Write(input[i]);
            }
            Console.WriteLine("");

            Console.WriteLine("A5/1 - Cipher:");
            for (int i = 0; i < kodiranInput.Length; i++)
            {
                Console.Write(kodiranInput[i]);
            }
            Console.WriteLine("");


            Console.WriteLine("A5/1 - Dekodiran Text:");
            for (int i = 0; i < dekodiranText.Length; i++)
            {
                Console.Write(dekodiranText[i]);
            }
            Console.WriteLine("");

            // Writing test input to a binary file
            string inputFile = "A5_1input.bin";
            BinaryWriter bw = new BinaryWriter(new FileStream(inputFile, FileMode.Create));
            for ( int i = 0; i < 8; i++)
            {
                bw.Write(inputzaFajl[i]);
            }         
            bw.Close();

            // Encrypt our test file using A5/1 
            string outputFile = "a5_1encrypted.bin";
            a5.EncryptFile(inputFile,outputFile,key);

            // Decrypt our encrypted file using the same key
            inputFile = "a5_1encrypted.bin";
            outputFile = "a5_1decrypted.bin";
            a5.DecryptFile(inputFile,outputFile,key);

            // Read from the decrypted file
            int[] output = new int[8];
            BinaryReader br = new BinaryReader(new FileStream(outputFile, FileMode.Open));
            for (int i = 0; i < 8; i++)
            {
                output[i] = br.ReadInt32();
            }
            br.Close();

            Console.WriteLine("Dekriptovani text :");
            for (int i = 0; i < 8; i++)
            {
                Console.Write(output[i]);
            }
            Console.WriteLine("");


        }
        static void Main(string[] args)
        {


            //a5_1();
            //playfair();
            //CFB();
            CFB_playfair();
            Console.ReadLine();
        }
    }
}
