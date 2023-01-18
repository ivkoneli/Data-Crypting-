using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Zastita_Informacija
{
    internal class Program
    {   static void SHA256()
        {
            Console.WriteLine("############################");
            Console.WriteLine("            SHA256          ");

            string inputFile1 = "RSAinput.txt";
            string inputFile2 = "RSAdecrypted.txt";

            SHA256 sha256_1 = new SHA256();
            SHA256 sha256_2 = new SHA256();

            string Filehash1 = sha256_1.ComputeFileHash(inputFile1);
            string Filehash2 = sha256_2.ComputeFileHash(inputFile2);

            string hash1 = sha256_2.ComputeHash("poruka");
            string hash2 = sha256_1.ComputeHash("poruka");

            Console.Write("Hash vrednost prvog  fajla : ");
            Console.WriteLine(Filehash1);

            Console.Write("Hash vrednost drugog fajla : ");
            Console.WriteLine(Filehash2);



            Console.WriteLine("############################");
        }
        static void RSA()
        {
            Console.WriteLine("############################");
            Console.WriteLine("         CFB-PLAYFAIR       ");

            RSA rsa = new RSA();
            Console.Write("Vrednost p : ");
            Console.WriteLine(rsa.p);
            Console.Write("Vrednost q : "); 
            Console.WriteLine(rsa.q);
            Console.Write("Vrednost n : ");
            Console.WriteLine(rsa.n);
            Console.Write("Vrednost phi(n) : ");
            Console.WriteLine(rsa.phi);
            Console.Write("Vrednost e : ");
            Console.WriteLine(rsa.e);
            Console.Write("Vrednost d : ");
            Console.WriteLine(rsa.d);


            rsa.BMPEncrypt();
            rsa.BMPDecrypt();
            /*BigInteger input = 89;

            for ( int i = 0; i < 20; i ++)
            {
                Console.Write("Input : ");
                Console.WriteLine(input);
                BigInteger encrypted = rsa.Crypt(input);
                Console.Write("Encrypted : ");
                Console.WriteLine(encrypted);
                BigInteger decrypted = rsa.Decrypt(encrypted);
                Console.Write("Decrypted : ");
                Console.WriteLine(decrypted);
                Console.WriteLine();
                input++;
            }*/

            /*Console.WriteLine("Unestie vrednost koju zelite da enkriptujete ");
            BigInteger fileInput = BigInteger.Parse(Console.ReadLine());

            Console.WriteLine("Citanje iz fajla....");

            string inputFile = "RSAinput.txt";
            string outputFile = "RSAencrypted.txt";
            string destFile = "RSAdecrypted.txt";

            File.WriteAllText(inputFile, fileInput.ToString());

            string encryptedText = rsa.CryptFile(inputFile, outputFile);
            string decryptedText = rsa.DecryptFile(outputFile, destFile);

            Console.Write("Input iz fajla je   : ");
            Console.WriteLine(fileInput);

            Console.Write("Kodirani input je   : ");
            Console.WriteLine(encryptedText);
            Console.WriteLine("I nalazi se u fajlu RSAencrypted.txt");


            Console.WriteLine("Da li zelite da dekodirate ovu vrednost ? (da / ne )");
            string answer = Console.ReadLine();
            if (answer == "da")
            {
                Console.Write("Dekodirani input je : ");
                Console.WriteLine(decryptedText);
                Console.WriteLine("I nalazi se u fajlu RSAdecrypted.txt");
            }
            else if (answer == "ne")
                Console.WriteLine("Kraj");*/
            

            Console.WriteLine("############################");


        }
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

            a5.BMP3("BMPinput.bmp", "BMPencrypted.bmp", "BMPdecrypted.bmp" , key);

            Console.WriteLine("############################");

        }
        static void Main(string[] args)
        {


            Console.WriteLine("########################################################");
            Console.WriteLine("-----------------Welcome to my crypto app---------------");
            Console.WriteLine("Which algorithm would u like to try ? ");

            Console.WriteLine("1. A5-1 ");
            Console.WriteLine("2. Playfair");
            Console.WriteLine("3. CFB");
            Console.WriteLine("4. RSA");
            Console.WriteLine("5. SHA256");

            Console.WriteLine("########################################################");

            string selection = Console.ReadLine();

            if (selection == "1")
                a5_1();
            else if (selection == "2")
                playfair();
            else if (selection == "3")
                CFB();
            else if (selection == "4")
                RSA();
            else if (selection == "5")
                SHA256();
            else
                Console.WriteLine("Invalid");




            Console.ReadLine();
        }
    }
}
